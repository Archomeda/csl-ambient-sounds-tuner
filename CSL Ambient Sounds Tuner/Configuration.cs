using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AmbientSoundsTuner.Migration;
using CommonShared;
using CommonShared.Configuration;
using CommonShared.Utils;

namespace AmbientSoundsTuner
{
    [XmlRoot("Configuration")]
    public class Configuration : VersionedConfig
    {
        public Configuration()
        {
            this.Version = 5;

            this.SoundPackPreset = "Default";
            this.ExtraDebugLogging = false;
            this.AmbientSounds = new SerializableDictionary<string, ConfigurationV4.Sound>();
            this.AmbientNightSounds = new SerializableDictionary<string, ConfigurationV4.Sound>();
            this.AnimalSounds = new SerializableDictionary<string, ConfigurationV4.Sound>();
            this.BuildingSounds = new SerializableDictionary<string, ConfigurationV4.Sound>();
            this.VehicleSounds = new SerializableDictionary<string, ConfigurationV4.Sound>();
            this.MiscSounds = new SerializableDictionary<string, ConfigurationV4.Sound>();
        }

        public string SoundPackPreset { get; set; }

        public bool ExtraDebugLogging { get; set; }

        public SerializableDictionary<string, ConfigurationV4.Sound> AmbientSounds { get; set; }

        public SerializableDictionary<string, ConfigurationV4.Sound> AmbientNightSounds { get; set; }

        public SerializableDictionary<string, ConfigurationV4.Sound> AnimalSounds { get; set; }

        public SerializableDictionary<string, ConfigurationV4.Sound> BuildingSounds { get; set; }

        public SerializableDictionary<string, ConfigurationV4.Sound> VehicleSounds { get; set; }

        public SerializableDictionary<string, ConfigurationV4.Sound> MiscSounds { get; set; }

        public IDictionary<T, ConfigurationV4.Sound> GetSoundsByCategoryId<T>(string id)
        {
            switch (id)
            {
                case "Ambient": return this.AmbientSounds as IDictionary<T, ConfigurationV4.Sound>;
                case "AmbientNight": return this.AmbientNightSounds as IDictionary<T, ConfigurationV4.Sound>;
                case "Animal": return this.AnimalSounds as IDictionary<T, ConfigurationV4.Sound>;
                case "Building": return this.BuildingSounds as IDictionary<T, ConfigurationV4.Sound>;
                case "Vehicle": return this.VehicleSounds as IDictionary<T, ConfigurationV4.Sound>;
                case "Misc": return this.MiscSounds as IDictionary<T, ConfigurationV4.Sound>;
            }
            return null;
        }
    }
}
