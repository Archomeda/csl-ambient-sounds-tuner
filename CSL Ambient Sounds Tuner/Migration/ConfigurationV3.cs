using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using CommonShared;
using CommonShared.Configuration;
using CommonShared.Utils;

namespace AmbientSoundsTuner.Migration
{
    [XmlRoot("Configuration")]
    public class ConfigurationV3 : VersionedConfig
    {
        public ConfigurationV3()
        {
            this.Version = 3;

            this.SoundPackPreset = "Default";
            this.ExtraDebugLogging = false;
            this.AmbientSounds = new SerializableDictionary<AudioManager.AmbientType, Sound>();
            this.AnimalSounds = new SerializableDictionary<string, Sound>();
            this.BuildingSounds = new SerializableDictionary<string, Sound>();
            this.VehicleSounds = new SerializableDictionary<string, Sound>();
            this.MiscSounds = new SerializableDictionary<string, Sound>();
        }

        public string SoundPackPreset { get; set; }

        public bool ExtraDebugLogging { get; set; }

        public SerializableDictionary<AudioManager.AmbientType, Sound> AmbientSounds { get; set; }

        public SerializableDictionary<string, Sound> AnimalSounds { get; set; }

        public SerializableDictionary<string, Sound> BuildingSounds { get; set; }

        public SerializableDictionary<string, Sound> VehicleSounds { get; set; }

        public SerializableDictionary<string, Sound> MiscSounds { get; set; }

        public IDictionary<T, Sound> GetSoundsByCategoryId<T>(string id)
        {
            switch (id)
            {
                case "Ambient": return this.AmbientSounds as IDictionary<T, Sound>;
                case "Animal": return this.AnimalSounds as IDictionary<T, Sound>;
                case "Building": return this.BuildingSounds as IDictionary<T, Sound>;
                case "Vehicle": return this.VehicleSounds as IDictionary<T, Sound>;
                case "Misc": return this.MiscSounds as IDictionary<T, Sound>;
            }
            return null;
        }

        public class Sound
        {
            public string Active { get; set; }

            public float Volume { get; set; }
        }
    }
}
