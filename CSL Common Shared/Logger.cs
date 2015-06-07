using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ColossalFramework.Plugins;
using CommonShared.Utils;

namespace CommonShared
{
    /// <summary>
    /// A class that can be instantiated to provide logging.
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Creates a new logger and sets the prefix to the assembly name of the mod.
        /// </summary>
        public Logger()
        {
            Assembly assembly;
            PluginUtils.GetPluginInfo(out assembly);
            this.Prefix = string.Format("[{0}]", assembly.GetName().Name);
        }

        /// <summary>
        /// Gets or sets the prefix.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Gets or sets whether the debug logging is enabled or not.
        /// </summary>
        public bool EnableDebugLogging { get; set; }

        /// <summary>
        /// Logs to the Unity Engine.
        /// </summary>
        /// <param name="logFunc">The Unity Engine log method to use.</param>
        /// <param name="message">The log message.</param>
        protected virtual void LogUE(Action<object> logFunc, string message)
        {
            logFunc(string.Format("{0} {1}", this.Prefix, message));
        }

        /// <summary>
        /// Logs to the default output panel of the game.
        /// </summary>
        /// <param name="messageType">The message type to use.</param>
        /// <param name="message">The log message.</param>
        protected virtual void LogOP(PluginManager.MessageType messageType, string message)
        {
            DebugOutputPanel.AddMessage(messageType, string.Format("{0} {1}", this.Prefix, message));
        }

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The log message.</param>
        public void Debug(string message)
        {
            if (this.EnableDebugLogging)
            {
                message = "[DEBUG] " + message;
                LogUE(UnityEngine.Debug.Log, message);
                LogOP(PluginManager.MessageType.Message, message);
            }
        }

        /// <summary>
        /// Logs a debug message through the string formatter.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public void Debug(string format, params object[] args)
        {
            Debug(string.Format(format, args));
        }

        /// <summary>
        /// Logs an info message.
        /// </summary>
        /// <param name="message">The log message.</param>
        public void Info(string message)
        {
            LogUE(UnityEngine.Debug.Log, message);
            LogOP(PluginManager.MessageType.Message, message);
        }

        /// <summary>
        /// Logs an info message through the string formatter.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public void Info(string format, params object[] args)
        {
            Info(string.Format(format, args));
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The log message.</param>
        public void Warning(string message)
        {
            LogUE(UnityEngine.Debug.LogWarning, message);
            LogOP(PluginManager.MessageType.Warning, message);
        }

        /// <summary>
        /// Logs a warning message through the string formatter.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public void Warning(string format, params object[] args)
        {
            Warning(string.Format(format, args));
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The log message.</param>
        public void Error(string message)
        {
            LogUE(UnityEngine.Debug.LogError, message);
            LogOP(PluginManager.MessageType.Error, message);
        }

        /// <summary>
        /// Logs an error message through the string formatter.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public void Error(string format, params object[] args)
        {
            Error(string.Format(format, args));
        }
    }
}
