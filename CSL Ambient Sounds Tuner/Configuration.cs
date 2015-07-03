using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
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
            this.Version = 3;

            this.ExtraDebugLogging = false;
            this.AmbientSounds = new SerializableDictionary<AudioManager.AmbientType, Sound>();
            this.AnimalSounds = new SerializableDictionary<string, Sound>();
            this.BuildingSounds = new SerializableDictionary<string, Sound>();
            this.VehicleSounds = new SerializableDictionary<string, Sound>();
            this.MiscSounds = new SerializableDictionary<string, Sound>();
        }

        public bool ExtraDebugLogging { get; set; }

        public SerializableDictionary<AudioManager.AmbientType, Sound> AmbientSounds { get; set; }

        public SerializableDictionary<string, Sound> AnimalSounds { get; set; }

        public SerializableDictionary<string, Sound> BuildingSounds { get; set; }

        public SerializableDictionary<string, Sound> VehicleSounds { get; set; }

        public SerializableDictionary<string, Sound> MiscSounds { get; set; }

        public class Sound
        {
            public string Active { get; set; }

            public float Volume { get; set; }
        }
    }
}
