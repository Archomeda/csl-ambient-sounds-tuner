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
            this.SoundPacks = new Dictionary<SoundPackFile, PluginManager.PluginInfo>();
        }

        public Dictionary<SoundPackFile, PluginManager.PluginInfo> SoundPacks { get; private set; }

        public void InitSoundPacks()
        {
            this.SoundPacks.Clear();

            var mods = instance.GetSoundPackMods();
            Mod.Log.Debug("Found {0} sound pack mods", mods.Count);

            foreach (var mod in mods)
            {
                foreach (var file in mod.Value)
                {
                    string path = Path.Combine(mod.Key.modPath, file);
                    try
                    {
                        SoundPackFile soundPackFile = SoundPackFile.LoadConfig<SoundPackFile>(path);
                        this.SoundPacks[soundPackFile] = mod.Key;

                        // Patch the paths
                        Action<SoundPackFile.AudioInfo> patchAudioInfoClipPath = null;
                        patchAudioInfoClipPath = new Action<SoundPackFile.AudioInfo>(a =>
                        {
                            a.Clip = Path.Combine(mod.Key.modPath, a.Clip);
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

                        Mod.Log.Debug("Loaded sound pack {0} from mod {1}", soundPackFile.Name, mod.Key.userModInstance != null ? ((IUserMod)mod.Key.userModInstance as IUserMod).Name : mod.Key.name);
                    }
                    catch (Exception ex)
                    {
                        Mod.Log.Warning("Could not initialize the sound pack from file '{0}' from mod {1}: {2}", path, mod.Key.userModInstance != null ? ((IUserMod)mod.Key.userModInstance as IUserMod).Name : mod.Key.name, ex);
                    }
                }
            }
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
