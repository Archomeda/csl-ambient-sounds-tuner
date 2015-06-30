using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CommonShared.Configuration
{
    /// <summary>
    /// An abstract class that provides basic implementation for loading and saving configuration files.
    /// </summary>
    [XmlRoot("Configuration")]
    public abstract class Config
    {
        /// <summary>
        /// Loads the configuration from a file.
        /// </summary>
        /// <typeparam name="T">The config object type.</typeparam>
        /// <param name="filename">The name of the configuration file.</param>
        public static T LoadConfig<T>(string filename) where T : new()
        {
            if (File.Exists(filename))
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    return (T)new XmlSerializer(typeof(T)).Deserialize(sr);
                }
            }
            return new T();
        }

        /// <summary>
        /// Saves the configuration to a file.
        /// </summary>
        /// <param name="filename">The name of the configuration file.</param>
        public void SaveConfig(string filename)
        {
            string dirname = Path.GetDirectoryName(filename);
            if (!Directory.Exists(dirname))
            {
                Directory.CreateDirectory(dirname);
            }
            using (StreamWriter sw = new StreamWriter(filename))
            {
                new XmlSerializer(this.GetType()).Serialize(sw, this);
            }
        }
    }
}
