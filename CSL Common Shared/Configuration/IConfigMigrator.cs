using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CommonShared.Configuration
{
    /// <summary>
    /// An interface for implementing configuration migrators.
    /// Typically you would want to use <see cref="ConfigMigratorBase"/>.
    /// </summary>
    /// <typeparam name="T">The configuration object type.</typeparam>
    public interface IConfigMigrator<T> where T : VersionedConfig
    {
        /// <summary>
        /// Migrate a configuration file.
        /// </summary>
        /// <param name="version">The current version of the configuration file.</param>
        /// <param name="stream">The stream of the configuration file.</param>
        /// <returns>An up-to-date configuration object.</returns>
        T Migrate(uint version, Stream stream);
    }
}
