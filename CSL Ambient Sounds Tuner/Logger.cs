using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalFramework.Plugins;

namespace AmbientSoundsTuner
{
    internal static class Logger
    {
        private static void LogUE(Action<object> logFunc, string message)
        {
            logFunc(string.Format("[{0}] {1}", Mod.AssemblyName, message));
        }

        private static void LogOP(PluginManager.MessageType messageType, string message)
        {
            DebugOutputPanel.AddMessage(messageType, string.Format("[{0}] {1}", Mod.AssemblyName, message));
        }

        public static void Debug(string str)
        {
            if (Configuration.Instance.ExtraDebugLogging)
            {
                str = "[DEBUG] " + str;
                LogUE(UnityEngine.Debug.Log, str);
                LogOP(PluginManager.MessageType.Message, str);
            }
        }

        public static void Debug(string str, params object[] args)
        {
            Debug(string.Format(str, args));
        }

        public static void Info(string str)
        {
            LogUE(UnityEngine.Debug.Log, str);
            LogOP(PluginManager.MessageType.Message, str);
        }

        public static void Info(string str, params object[] args)
        {
            Info(string.Format(str, args));
        }

        public static void Warning(string str)
        {
            LogUE(UnityEngine.Debug.LogWarning, str);
            LogOP(PluginManager.MessageType.Warning, str);
        }

        public static void Warning(string str, params object[] args)
        {
            Warning(string.Format(str, args));
        }

        public static void Error(string str)
        {
            LogUE(UnityEngine.Debug.LogError, str);
            LogOP(PluginManager.MessageType.Error, str);
        }

        public static void Error(string str, params object[] args)
        {
            Error(string.Format(str, args));
        }
    }
}
