using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ColossalFramework;
using ColossalFramework.Plugins;
using CommonShared.Utils;
using ICities;

namespace AmbientSoundsTuner.SoundPack
{
    public class SoundPacksManager : SingletonLite<SoundPacksManager>
    {
        public const string SOUNDPACKS_FILENAME = "AmbientSoundsTuner.SoundPacks.xml";

        public SoundPacksManager()
        {
            this.SoundPackMods = new Dictionary<PluginManager.PluginInfo, SoundPacksFile>();
            this.SoundPacks = new Dictionary<string, SoundPacksFile.SoundPack>();
            this.AudioFiles = new Dictionary<string, SoundPacksFile.Audio>();
        }

        public IDictionary<PluginManager.PluginInfo, SoundPacksFile> SoundPackMods { get; private set; }

        public IDictionary<string, SoundPacksFile.SoundPack> SoundPacks { get; private set; }

        public IDictionary<string, SoundPacksFile.Audio> AudioFiles { get; private set; }

        public void InitSoundPacks()
        {
            this.SoundPackMods.Clear();
            this.AudioFiles.Clear();

            foreach (var mod in instance.GetSoundPackMods())
            {
                if (mod.isEnabled)
                    this.AddSoundPackMod(mod);

                PluginUtils.SubscribePluginStateChange(mod, isEnabled =>
                {
                    if (isEnabled)
                        this.AddSoundPackMod(mod);
                    else
                        this.RemoveSoundPackMod(mod);
                });
            }
        }

        private void AddSoundPackMod(PluginManager.PluginInfo mod)
        {
            string modName = mod.userModInstance != null ? ((IUserMod)mod.userModInstance as IUserMod).Name : mod.name;
            string path = Path.Combine(mod.modPath, SOUNDPACKS_FILENAME);
            try
            {
                SoundPacksFile soundPackFile = SoundPacksFile.LoadConfig<SoundPacksFile>(path);
                this.SoundPackMods[mod] = soundPackFile;

                foreach (var soundPack in soundPackFile.SoundPacks)
                {
                    // Patch the paths
                    Action<SoundPacksFile.AudioInfo> patchAudioInfoClipPath = null;
                    patchAudioInfoClipPath = new Action<SoundPacksFile.AudioInfo>(a =>
                    {
                        a.Clip = Path.Combine(mod.modPath, a.Clip);
                        if (a.Variations != null)
                        {
                            for (int i = 0; i < a.Variations.Length; i++)
                            {
                                patchAudioInfoClipPath(a.Variations[i].AudioInfo);
                            }
                        }
                    });
                    foreach (var group in new[] { soundPack.Ambients, soundPack.Animals, soundPack.Buildings, soundPack.Vehicles, soundPack.Miscs })
                    {
                        for (int i = 0; i < group.Length; i++)
                        {
                            patchAudioInfoClipPath(group[i].AudioInfo);
                        }
                    }

                    // Get every audio
                    var audioFiles = new Dictionary<string, SoundPacksFile.Audio>();
                    foreach (var group in new Dictionary<string, SoundPacksFile.Audio[]>() { 
                        { "Ambient", soundPack.Ambients }, 
                        { "Animal", soundPack.Animals }, 
                        { "Building", soundPack.Buildings },
                        { "Vehicle", soundPack.Vehicles },
                        { "Misc", soundPack.Miscs } })
                    {
                        foreach (var audio in group.Value)
                        {
                            string uniqueName = group.Key + "." + audio.Type.ToString() + "." + audio.Name;
                            if (this.AudioFiles.ContainsKey(uniqueName))
                            {
                                Mod.Log.Warning("Audio file {0} loaded from sound pack {1} in mod {2} already exists", uniqueName, soundPack.Name, modName);
                            }
                            else
                            {
                                this.AudioFiles[uniqueName] = audio;
                                Mod.Log.Debug("Loaded audio file {0} from sound pack {1}", uniqueName, soundPack.Name);
                            }
                        }
                    }

                    this.SoundPacks[soundPack.Name] = soundPack;

                    Mod.Log.Debug("Loaded sound pack {0} from mod {1}", soundPack.Name, modName);
                }
            }
            catch (Exception ex)
            {
                Mod.Log.Warning("Could not initialize the sound packs from mod {0}: {1}", modName, ex);
            }

        }

        private void RemoveSoundPackMod(PluginManager.PluginInfo mod)
        {
            foreach (var soundPackFile in this.SoundPackMods[mod].SoundPacks)
            {
                foreach (var group in new Dictionary<string, SoundPacksFile.Audio[]>() { 
                        { "Ambient", soundPackFile.Ambients }, 
                        { "Animal", soundPackFile.Animals }, 
                        { "Building", soundPackFile.Buildings },
                        { "Vehicle", soundPackFile.Vehicles },
                        { "Misc", soundPackFile.Miscs } })
                {
                    foreach (var audio in group.Value)
                    {
                        string uniqueName = group.Key + "." + audio.Type.ToString() + "." + audio.Name;
                        if (this.AudioFiles.Remove(uniqueName))
                        {
                            Mod.Log.Warning("Unloaded audio file {0} loaded from sound pack {1}", uniqueName, soundPackFile.Name);
                        }
                        else
                        {
                            Mod.Log.Debug("Audio file {0} from sound pack {1} was not added", uniqueName, soundPackFile.Name);
                        }
                    }
                }
            }

            this.SoundPackMods.Remove(mod);
            Mod.Log.Debug("Unloaded sound pack mod {0}", mod.userModInstance != null ? ((IUserMod)mod.userModInstance as IUserMod).Name : mod.name);
        }

        private IEnumerable<PluginManager.PluginInfo> GetSoundPackMods()
        {
            return PluginManager.instance.GetPluginsInfo().Where(i => File.Exists(Path.Combine(i.modPath, SOUNDPACKS_FILENAME)));
        }
    }
}
