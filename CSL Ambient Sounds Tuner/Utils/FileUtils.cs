using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ColossalFramework.IO;
using ColossalFramework.Plugins;

namespace AmbientSoundsTuner.Utils
{
    internal static class FileUtils
    {
        public static string GetModFolder()
        {
            var pluginInfo = PluginUtils.GetPluginInfo();
            return pluginInfo != null ? pluginInfo.modPath : null;
        }

        public static string GetDataFolder()
        {
            return Path.Combine(DataLocation.modsPath, Mod.AssemblyName);
        }
    }
}
