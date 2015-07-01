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
                { 0, this.MigrateFromVersion0 },
                { 1, this.MigrateFromVersion1 }
            };

            this.VersionTypes = new Dictionary<uint, Type>()
            {
                { 0, typeof(ConfigurationV0) },
                { 1, typeof(ConfigurationV1) }
            };
        }

        protected object MigrateFromVersion0(object oldConfig)
        {
            ConfigurationV0 config = (ConfigurationV0)oldConfig;
            ConfigurationV1 newConfig = new ConfigurationV1();

            newConfig.ExtraDebugLogging = config.ExtraDebugLogging;
            foreach (var kvp in config.State.AmbientVolumes)
                newConfig.AmbientVolumes.Add(kvp.Key, kvp.Value);
            foreach (var kvp in config.State.EffectVolumes)
                newConfig.VehicleVolumes.Add(kvp.Key, kvp.Value);

            return newConfig;
        }

        protected object MigrateFromVersion1(object oldConfig)
        {
            ConfigurationV1 config = (ConfigurationV1)oldConfig;
            Configuration newConfig = new Configuration();

            newConfig.ExtraDebugLogging = config.ExtraDebugLogging;
            foreach (var kvp in config.AmbientVolumes)
                newConfig.AmbientVolumes.Add(kvp.Key, kvp.Value);
            foreach (var kvp in config.AnimalVolumes)
                newConfig.AnimalVolumes.Add(kvp.Key, kvp.Value);
            foreach (var kvp in config.BuildingVolumes)
                newConfig.BuildingVolumes.Add(kvp.Key, kvp.Value);
            foreach (var kvp in config.VehicleVolumes)
            {
                switch (kvp.Key)
                {
                    case "Small Car Movement":
                        newConfig.VehicleVolumes.Add("Small Car Sound", kvp.Value);
                        break;
                    case "Large Car Movement":
                        newConfig.VehicleVolumes.Add("Large Car Sound", kvp.Value);
                        break;
                    default:
                        newConfig.VehicleVolumes.Add(kvp.Key, kvp.Value);
                        break;
                }
            }
            foreach (var kvp in config.MiscVolumes)
                newConfig.MiscVolumes.Add(kvp.Key, kvp.Value);

            return newConfig;
        }
    }
}
