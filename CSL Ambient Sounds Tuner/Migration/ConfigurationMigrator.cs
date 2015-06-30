using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AmbientSoundsTuner.Migration;
using CommonShared.Configuration;

namespace AmbientSoundsTuner.Migration
{
    public class ConfigurationMigrator : ConfigMigratorBase<Configuration>
    {
        public ConfigurationMigrator()
        {
            this.MigrationMethods = new Dictionary<uint, Func<object, object>>()
            {
                { 0, this.MigrateFromVersion0 }
            };

            this.VersionTypes = new Dictionary<uint, Type>()
            {
                { 0, typeof(ConfigurationV0) }
            };
        }

        protected object MigrateFromVersion0(object oldConfig)
        {
            ConfigurationV0 config = (ConfigurationV0)oldConfig;
            Configuration newConfig = new Configuration();

            newConfig.ExtraDebugLogging = config.ExtraDebugLogging;
            foreach (var kvp in config.State.AmbientVolumes)
            {
                newConfig.AmbientVolumes.Add(kvp.Key, kvp.Value);
            }
            foreach (var kvp in config.State.EffectVolumes)
            {
                newConfig.VehicleVolumes.Add(kvp.Key, kvp.Value);
            }

            return newConfig;
        }
    }
}
