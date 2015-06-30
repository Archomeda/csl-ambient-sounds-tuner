using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CommonShared.Configuration
{
    /// <summary>
    /// An abstract class that implements basic functionality of <see cref="IConfigMigrator"/>.
    /// </summary>
    /// <typeparam name="T">The configuration object type.</typeparam>
    public abstract class ConfigMigratorBase<T> : IConfigMigrator<T> where T : VersionedConfig, new()
    {
        /// <summary>
        /// Gets or sets the migration methods that are used for different versions.
        /// </summary>
        protected virtual IDictionary<uint, Func<object, object>> MigrationMethods { get; set; }

        /// <summary>
        /// Gets or sets the configuration object types used for different versions.
        /// </summary>
        protected virtual IDictionary<uint, Type> VersionTypes { get; set; }

        /// <summary>
        /// Migrate a configuration file.
        /// </summary>
        /// <param name="version">The current version of the configuration file.</param>
        /// <param name="stream">The stream of the configuration file.</param>
        /// <returns>An up-to-date configuration object.</returns>
        public T Migrate(uint version, Stream stream)
        {
            T currentConfig = new T();
            if (version == currentConfig.Version)
            {
                // Using latest version
                return (T)new XmlSerializer(typeof(T)).Deserialize(stream);
            }
            else
            {
                // Using an outdated version
                object config = new XmlSerializer(this.VersionTypes[version]).Deserialize(stream);
                while (version < currentConfig.Version)
                {
                    config = this.MigrationMethods[version](config);
                    version++;
                }
                return (T)config;
            }
        }
    }
}
