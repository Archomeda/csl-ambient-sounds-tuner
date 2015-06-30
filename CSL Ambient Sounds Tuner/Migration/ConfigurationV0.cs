using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AmbientSoundsTuner.Utils;
using CommonShared;
using CommonShared.Configuration;
using CommonShared.Utils;

namespace AmbientSoundsTuner.Migration
{
    [XmlRoot("Configuration")]
    public class ConfigurationV0 : Config
    {
        [XmlRoot("State")]
        public class StateConfig
        {
            public StateConfig()
            {
                this.AmbientVolumes = new SerializableDictionary<AudioManager.AmbientType, float>();
                this.EffectVolumes = new SerializableDictionary<string, float>();
            }

            [XmlElement("AmbientVolumes")]
            public SerializableDictionary<AudioManager.AmbientType, float> AmbientVolumes { get; set; }

            [XmlElement("EffectVolumes")]
            public SerializableDictionary<string, float> EffectVolumes { get; set; }
        }

        public ConfigurationV0()
        {
            this.State = new StateConfig();
            this.ExtraDebugLogging = false;
        }

        public StateConfig State { get; set; }

        public bool ExtraDebugLogging { get; set; }
    }
}
