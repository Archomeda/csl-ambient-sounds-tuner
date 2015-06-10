using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ColossalFramework.IO;
using ICities;

namespace CommonShared.Utils
{
    /// <summary>
    /// Contains various utilities regarding files.
    /// </summary>
    public class FileUtils
    {
        /// <summary>
        /// Gets the assembly folder of the calling mod. This is where the DLL and every other static file are located.
        /// </summary>
        /// <param name="modInstance">The mod instance.</param>
        /// <returns>The assembly folder.</returns>
        public static string GetAssemblyFolder(IUserMod modInstance)
        {
            var pluginInfo = PluginUtils.GetPluginInfo(modInstance);
            return pluginInfo != null ? pluginInfo.modPath : null;
        }

        /// <summary>
        /// Gets the storage folder of the calling mod. This is where dynamic files are located.
        /// </summary>
        /// <param name="modInstance">The mod instance.</param>
        /// <returns>The storage folder.</returns>
        public static string GetStorageFolder(IUserMod modInstance)
        {
            Assembly assembly;
            var pluginInfo = PluginUtils.GetPluginInfo(modInstance, out assembly);
            return Path.Combine(DataLocation.modsPath, assembly.GetName().Name);
        }
    }
}
