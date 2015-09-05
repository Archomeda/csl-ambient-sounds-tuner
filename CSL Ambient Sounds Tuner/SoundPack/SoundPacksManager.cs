﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ColossalFramework;
using ColossalFramework.Plugins;
using CommonShared.Proxies.Plugins;
using CommonShared.Utils;
using ICities;

namespace AmbientSoundsTuner.SoundPack
{
    public class SoundPacksManager : SingletonLite<SoundPacksManager>
    {
        public const string SOUNDPACKS_FILENAME_XML = "AmbientSoundsTuner.SoundPacks.xml";
        public const string SOUNDPACKS_FILENAME_YAML = "AmbientSoundsTuner.SoundPacks.yml";

        public SoundPacksManager()
        {
            this.SoundPackMods = new Dictionary<IPluginInfoInteractor, SoundPacksFile>();
            this.SoundPacks = new Dictionary<string, SoundPacksFile.SoundPack>();
            this.AudioFiles = new Dictionary<string, SoundPacksFile.Audio>();
        }

        public IDictionary<IPluginInfoInteractor, SoundPacksFile> SoundPackMods { get; private set; }

        public IDictionary<string, SoundPacksFile.SoundPack> SoundPacks { get; private set; }

        public IDictionary<string, SoundPacksFile.Audio> AudioFiles { get; private set; }

        public void InitSoundPacks()
        {
            this.SoundPackMods.Clear();
            this.AudioFiles.Clear();

            foreach (var mod in instance.GetSoundPackMods())
            {
                if (mod.IsEnabled)
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

        private void AddSoundPackMod(IPluginInfoInteractor mod)
        {
            string modName = mod.UserModInstance != null ? ((IUserMod)mod.UserModInstance as IUserMod).Name : mod.Name;
            string path = Path.Combine(mod.ModPath, SOUNDPACKS_FILENAME_YAML);
            if (!File.Exists(path))
            {
                // Fallback to XML if YAML does not exist
                path = Path.Combine(mod.ModPath, SOUNDPACKS_FILENAME_XML);
            }

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
                        a.Clip = Path.Combine(mod.ModPath, a.Clip);
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
                            try
                            {
                                patchAudioInfoClipPath(group[i].AudioInfo);
                            }
                            catch (Exception ex)
                            {
                                Mod.Instance.Log.Warning("Failed to load audio file {0} from sound pack {1} in mod {2}. Please check if your XML or YAML syntax is correct. Keep in mind that XML and YAML tags are case-sensitive! Inner exception was: {3}", group[i].Name, soundPack.Name, modName, ex);
                            }
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
                                Mod.Instance.Log.Warning("Audio file {0} loaded from sound pack {1} in mod {2} already exists", uniqueName, soundPack.Name, modName);
                            }
                            else
                            {
                                this.AudioFiles[uniqueName] = audio;
                                Mod.Instance.Log.Debug("Loaded audio file {0} from sound pack {1}", uniqueName, soundPack.Name);
                            }
                        }
                    }

                    this.SoundPacks[soundPack.Name] = soundPack;

                    Mod.Instance.Log.Debug("Loaded sound pack {0} from mod {1}", soundPack.Name, modName);
                }
            }
            catch (Exception ex)
            {
                Mod.Instance.Log.Warning("Could not initialize the sound packs from mod {0}: {1}", modName, ex);
            }

        }

        private void RemoveSoundPackMod(IPluginInfoInteractor mod)
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
                            Mod.Instance.Log.Debug("Unloaded audio file {0} loaded from sound pack {1}", uniqueName, soundPackFile.Name);
                        }
                        else
                        {
                            Mod.Instance.Log.Debug("Audio file {0} from sound pack {1} was not added", uniqueName, soundPackFile.Name);
                        }
                    }
                }
            }

            this.SoundPackMods.Remove(mod);
            Mod.Instance.Log.Debug("Unloaded sound pack mod {0}", mod.UserModInstance != null ? ((IUserMod)mod.UserModInstance as IUserMod).Name : mod.Name);
        }

        private IEnumerable<IPluginInfoInteractor> GetSoundPackMods()
        {
            return PluginUtils.PluginManagerInteractor.GetPluginsInfo().Where(i =>
                File.Exists(Path.Combine(i.ModPath, SOUNDPACKS_FILENAME_XML)) ||
                File.Exists(Path.Combine(i.ModPath, SOUNDPACKS_FILENAME_YAML)));
        }
    }
}
