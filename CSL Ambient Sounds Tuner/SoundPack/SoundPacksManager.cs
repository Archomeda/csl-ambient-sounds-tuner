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
    public class SoundPacksManager : Singleton<SoundPacksManager>
    {
        public SoundPacksManager()
        {
            this.SoundPackMods = new Dictionary<PluginManager.PluginInfo, IList<SoundPackFile>>();
            this.AudioFiles = new Dictionary<string, SoundPackFile.Audio>();
        }

        public IDictionary<PluginManager.PluginInfo, IList<SoundPackFile>> SoundPackMods { get; private set; }

        public IDictionary<string, SoundPackFile.Audio> AudioFiles { get; private set; }

        public void InitSoundPacks()
        {
            this.SoundPackMods.Clear();
            this.AudioFiles.Clear();

            var mods = instance.GetSoundPackMods();
            Mod.Log.Debug("Found {0} sound pack mods", mods.Count);

            foreach (var mod in mods)
            {
                if (mod.Key.isEnabled)
                {
                    this.AddSoundPackMod(mod.Key, mod.Value);
                }

                PluginUtils.SubscribePluginStateChange(mod.Key, isEnabled =>
                {
                    if (isEnabled)
                    {
                        this.AddSoundPackMod(mod.Key, mod.Value);
                    }
                    else
                    {
                        this.RemoveSoundPackMod(mod.Key);
                    }
                });
            }
        }

        private void AddSoundPackMod(PluginManager.PluginInfo mod, IEnumerable<string> soundPack)
        {
            string modName = mod.userModInstance != null ? ((IUserMod)mod.userModInstance as IUserMod).Name : mod.name;

            foreach (var file in soundPack)
            {
                if (!this.SoundPackMods.ContainsKey(mod))
                {
                    this.SoundPackMods.Add(mod, new List<SoundPackFile>());
                }

                string path = Path.Combine(mod.modPath, file);
                try
                {
                    SoundPackFile soundPackFile = SoundPackFile.LoadConfig<SoundPackFile>(path);

                    // Patch the paths
                    Action<SoundPackFile.AudioInfo> patchAudioInfoClipPath = null;
                    patchAudioInfoClipPath = new Action<SoundPackFile.AudioInfo>(a =>
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
                    foreach (var group in new[] { soundPackFile.Ambients, soundPackFile.Animals, soundPackFile.Buildings, soundPackFile.Vehicles, soundPackFile.Miscs })
                    {
                        for (int i = 0; i < group.Length; i++)
                        {
                            patchAudioInfoClipPath(group[i].AudioInfo);
                        }
                    }

                    // Get every audio
                    var audioFiles = new Dictionary<string, SoundPackFile.Audio>();
                    foreach (var group in new Dictionary<string, SoundPackFile.Audio[]>() { 
                        { "Ambient", soundPackFile.Ambients }, 
                        { "Animal", soundPackFile.Animals }, 
                        { "Building", soundPackFile.Buildings },
                        { "Vehicle", soundPackFile.Vehicles },
                        { "Misc", soundPackFile.Miscs } })
                    {
                        foreach (var audio in group.Value)
                        {
                            string uniqueName = group.Key + "." + audio.Type.ToString() + "." + audio.Name;
                            if (this.AudioFiles.ContainsKey(uniqueName))
                            {
                                Mod.Log.Warning("Audio file {0} loaded from sound pack {1} in mod {2} already exists", uniqueName, soundPackFile.Name, modName);
                            }
                            else
                            {
                                this.AudioFiles[uniqueName] = audio;
                                Mod.Log.Debug("Loaded audio file {0} from sound pack {1}", uniqueName, soundPackFile.Name);
                            }
                        }
                    }

                    Mod.Log.Debug("Loaded sound pack {0} from mod {1}", soundPackFile.Name, modName);
                }
                catch (Exception ex)
                {
                    Mod.Log.Warning("Could not initialize the sound pack from file '{0}' from mod {1}: {2}", path, modName, ex);
                }
            }
        }

        private void RemoveSoundPackMod(PluginManager.PluginInfo mod)
        {
            foreach (var soundPackFile in this.SoundPackMods[mod])
            {
                foreach (var group in new Dictionary<string, SoundPackFile.Audio[]>() { 
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

        private Dictionary<PluginManager.PluginInfo, IEnumerable<string>> GetSoundPackMods()
        {
            var list = new Dictionary<PluginManager.PluginInfo, IEnumerable<string>>();
            foreach (var plugin in PluginManager.instance.GetPluginsInfo().Where(i => i.userModInstance != null))
            {
                PropertyInfo property = plugin.userModInstance.GetType().GetProperty("SoundPackFiles", typeof(IEnumerable<string>));
                if (property != null)
                {
                    list.Add(plugin, (IEnumerable<string>)property.GetValue(plugin.userModInstance, null));
                }
            }
            return list;
        }
    }
}
