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
                { 1, this.MigrateFromVersion1 },
                { 2, this.MigrateFromVersion2 },
                { 3, this.MigrateFromVersion3 },
                { 4, this.MigrateFromVersion4 }
            };

            this.VersionTypes = new Dictionary<uint, Type>()
            {
                { 0, typeof(ConfigurationV0) },
                { 1, typeof(ConfigurationV1) },
                { 2, typeof(ConfigurationV2) },
                { 3, typeof(ConfigurationV3) },
                { 4, typeof(ConfigurationV4) }
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
            ConfigurationV2 newConfig = new ConfigurationV2();

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

        protected object MigrateFromVersion2(object oldConfig)
        {
            ConfigurationV2 config = (ConfigurationV2)oldConfig;
            ConfigurationV3 newConfig = new ConfigurationV3();

            newConfig.ExtraDebugLogging = config.ExtraDebugLogging;
            foreach (var kvp in config.AmbientVolumes)
                newConfig.AmbientSounds.Add(kvp.Key, new ConfigurationV3.Sound() { Volume = kvp.Value });
            foreach (var kvp in config.AnimalVolumes)
                newConfig.AnimalSounds.Add(kvp.Key, new ConfigurationV3.Sound() { Volume = kvp.Value });
            foreach (var kvp in config.BuildingVolumes)
                newConfig.BuildingSounds.Add(kvp.Key, new ConfigurationV3.Sound() { Volume = kvp.Value });
            foreach (var kvp in config.VehicleVolumes)
                newConfig.VehicleSounds.Add(kvp.Key, new ConfigurationV3.Sound() { Volume = kvp.Value });
            foreach (var kvp in config.MiscVolumes)
                newConfig.MiscSounds.Add(kvp.Key, new ConfigurationV3.Sound() { Volume = kvp.Value });

            return newConfig;
        }

        protected object MigrateFromVersion3(object oldConfig)
        {
            ConfigurationV3 config = (ConfigurationV3)oldConfig;
            ConfigurationV4 newConfig = new ConfigurationV4();

            newConfig.SoundPackPreset = config.SoundPackPreset;
            newConfig.ExtraDebugLogging = config.ExtraDebugLogging;
            foreach (var kvp in config.AmbientSounds)
                newConfig.AmbientSounds.Add(kvp.Key, new ConfigurationV4.Sound() { SoundPack = kvp.Value.Active, Volume = kvp.Value.Volume });
            foreach (var kvp in config.AnimalSounds)
                newConfig.AnimalSounds.Add(kvp.Key, new ConfigurationV4.Sound() { SoundPack = kvp.Value.Active, Volume = kvp.Value.Volume });
            foreach (var kvp in config.BuildingSounds)
                newConfig.BuildingSounds.Add(kvp.Key, new ConfigurationV4.Sound() { SoundPack = kvp.Value.Active, Volume = kvp.Value.Volume });
            foreach (var kvp in config.VehicleSounds)
                newConfig.VehicleSounds.Add(kvp.Key, new ConfigurationV4.Sound() { SoundPack = kvp.Value.Active, Volume = kvp.Value.Volume });
            foreach (var kvp in config.MiscSounds)
                newConfig.MiscSounds.Add(kvp.Key, new ConfigurationV4.Sound() { SoundPack = kvp.Value.Active, Volume = kvp.Value.Volume });

            return newConfig;
        }

        protected object MigrateFromVersion4(object oldConfig)
        {
            ConfigurationV4 config = (ConfigurationV4)oldConfig;
            Configuration newConfig = new Configuration();

            newConfig.SoundPackPreset = config.SoundPackPreset;
            newConfig.ExtraDebugLogging = config.ExtraDebugLogging;
            newConfig.AmbientSounds = config.AmbientSounds;
            newConfig.AnimalSounds = config.AnimalSounds;
            newConfig.BuildingSounds = config.BuildingSounds;
            foreach (var kvp in config.VehicleSounds)
            {
                string key;
                switch (kvp.Key)
                {
                    case "Aircraft Movement":
                        key = "Aircraft Sound";
                        break;
                    default:
                        key = kvp.Key;
                        break;
                }
                newConfig.VehicleSounds.Add(key, kvp.Value);
            }
            newConfig.MiscSounds = config.MiscSounds;

            return newConfig;
        }
    }
}
