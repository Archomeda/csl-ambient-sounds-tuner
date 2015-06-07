using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using ColossalFramework.Plugins;
using ICities;

namespace CommonShared.Utils
{
    /// <summary>
    /// Contains various utilities regarding plugins.
    /// </summary>
    public static class PluginUtils
    {
        /// <summary>
        /// Gets the plugin info of the calling mod.
        /// </summary>
        /// <returns>The plugin info.</returns>
        public static PluginManager.PluginInfo GetPluginInfo()
        {
            Assembly mainAssembly;
            return GetPluginInfo(out mainAssembly);
        }

        /// <summary>
        /// Gets the plugin info of the calling mod.
        /// </summary>
        /// <param name="mainAssembly">The main assembly of the mod where IUserMod is being used.</param>
        /// <returns>The plugin info.</returns>
        public static PluginManager.PluginInfo GetPluginInfo(out Assembly mainAssembly)
        {
            StackFrame[] frames = new StackTrace().GetFrames();

            // Iterate through the stack to get the mod DLL that called us
            foreach (Assembly assembly in (from f in frames select f.GetMethod().ReflectedType.Assembly).Distinct())
            {
                // Check if this assembly implements IUserMod somewhere
                if (assembly.GetExportedTypes().Count(t => t.GetInterfaces().Contains(typeof(IUserMod))) > 0)
                {
                    // Get the instance
                    foreach (var pluginInfo in PluginManager.instance.GetPluginsInfo())
                    {
                        var assemblies = ReflectionUtils.GetPrivateField<List<Assembly>>(pluginInfo, "m_Assemblies");
                        if (assemblies.Contains(assembly))
                        {
                            mainAssembly = assembly;
                            return pluginInfo;
                        }
                    }
                }
            }

            mainAssembly = null;
            return null;
        }


        private static Dictionary<string, bool> pluginEnabledList = new Dictionary<string, bool>();
        private static Dictionary<string, HashSet<Action<bool>>> pluginStateChangeCallbacks = new Dictionary<string, HashSet<Action<bool>>>();

        /// <summary>
        /// Subscribes to the event when the plugin state changes.
        /// </summary>
        /// <param name="callback">The callback that will be used when the state changes, with a boolean parameter that is true when the plugin is enabled, and false otherwise.</param>
        public static void SubscribePluginStateChange(Action<bool> callback)
        {
            string pluginName = GetPluginInfo().name;
            if (pluginStateChangeCallbacks.Count == 0)
            {
                PluginManager.instance.eventPluginsStateChanged += PluginManager_eventPluginsStateChanged;
                foreach (var pluginInfo in PluginManager.instance.GetPluginsInfo())
                {
                    pluginEnabledList[pluginInfo.name] = pluginInfo.isEnabled;
                }
            }

            if (!pluginStateChangeCallbacks.ContainsKey(pluginName))
            {
                pluginStateChangeCallbacks.Add(pluginName, new HashSet<Action<bool>>());
            }

            pluginStateChangeCallbacks[pluginName].Add(callback);
        }

        private static void PluginManager_eventPluginsStateChanged()
        {
            foreach (var pluginInfo in PluginManager.instance.GetPluginsInfo())
            {
                bool isEnabled;
                if (pluginEnabledList.TryGetValue(pluginInfo.name, out isEnabled) && pluginInfo.isEnabled != isEnabled)
                {
                    pluginEnabledList[pluginInfo.name] = pluginInfo.isEnabled;
                    HashSet<Action<bool>> callbacks;
                    if (pluginStateChangeCallbacks.TryGetValue(pluginInfo.name, out callbacks))
                    {
                        foreach (var callback in callbacks)
                        {
                            callback(pluginInfo.isEnabled);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Unsubscribes from the event when the plugin state changes.
        /// </summary>
        /// <param name="callback">The callback that will be used when the state changes, with a boolean parameter that is true when the plugin is enabled, and false otherwise.</param>
        public static void UnsubscribePluginStateChange(Action<bool> callback)
        {
            string pluginName = GetPluginInfo().name;
            if (pluginStateChangeCallbacks.ContainsKey(pluginName))
            {
                pluginStateChangeCallbacks[pluginName].Remove(callback);
                if (pluginStateChangeCallbacks[pluginName].Count == 0)
                {
                    pluginStateChangeCallbacks.Remove(pluginName);
                }
            }

            if (pluginStateChangeCallbacks.Count == 0)
            {
                PluginManager.instance.eventPluginsStateChanged -= PluginManager_eventPluginsStateChanged;
            }
        }
    }
}
