using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AmbientSoundsTuner.Utils;
using CommonShared;
using CommonShared.Utils;

namespace AmbientSoundsTuner
{
    [XmlRoot("Configuration")]
    public class Configuration : Config
    {
        [XmlRoot("State")]
        public class StateConfig
        {
            public StateConfig() { }
        }

        public Configuration()
        {
            this.State = new StateConfig();
            this.ExtraDebugLogging = false;

            this.AmbientVolumes = new SerializableDictionary<AudioManager.AmbientType, float>();
            this.AnimalVolumes = new SerializableDictionary<string, float>();
            this.BuildingVolumes = new SerializableDictionary<string, float>();
            this.VehicleVolumes = new SerializableDictionary<string, float>();
            this.MiscVolumes = new SerializableDictionary<string, float>();
        }

        public StateConfig State { get; set; }

        public bool ExtraDebugLogging { get; set; }

        [XmlElement("AmbientVolumes")]
        public SerializableDictionary<AudioManager.AmbientType, float> AmbientVolumes { get; set; }

        [XmlElement("AnimalVolumes")]
        public SerializableDictionary<string, float> AnimalVolumes { get; set; }

        [XmlElement("BuildingVolumes")]
        public SerializableDictionary<string, float> BuildingVolumes { get; set; }

        [XmlElement("VehicleVolumes")]
        public SerializableDictionary<string, float> VehicleVolumes { get; set; }

        [XmlElement("MiscVolumes")]
        public SerializableDictionary<string, float> MiscVolumes { get; set; }
    }
}
