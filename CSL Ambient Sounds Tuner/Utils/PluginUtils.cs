using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalFramework.Plugins;

namespace AmbientSoundsTuner.Utils
{
    internal static class PluginUtils
    {
        public static PluginManager.PluginInfo GetPluginInfo()
        {
            return PluginManager.instance.GetPluginsInfo().FirstOrDefault(p => p.name == Mod.AssemblyName || p.publishedFileID.AsUInt64 == Mod.WorkshopId);
        }
    }
}
