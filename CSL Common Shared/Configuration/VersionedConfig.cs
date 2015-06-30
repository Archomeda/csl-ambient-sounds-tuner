using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CommonShared.Configuration
{
    /// <summary>
    /// A class that provides basic implementation for loading and saving versioned configuration files.
    /// </summary>
    [XmlRoot("Configuration")]
    public class VersionedConfig : Config
    {
        /// <summary>
        /// Gets or sets the version of this configuration file.
        /// </summary>
        [XmlAttribute("version")]
        public virtual uint Version { get; set; }

        /// <summary>
        /// Loads the configuration from a file.
        /// </summary>
        /// <typeparam name="T">The config object type.</typeparam>
        /// <param name="filename">The name of the configuration file.</param>
        /// <param name="migrator">The config migrator object.</param>
        public static T LoadConfig<T>(string filename, IConfigMigrator<T> migrator) where T : VersionedConfig, new()
        {
            if (File.Exists(filename))
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    VersionedConfig versionedConfig = (VersionedConfig)new XmlSerializer(typeof(VersionedConfig)).Deserialize(fs);
                    fs.Position = 0;
                    return migrator.Migrate(versionedConfig.Version, fs);
                }
            }
            return new T();
        }
    }
}
