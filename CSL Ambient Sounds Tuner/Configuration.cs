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

        public Configuration()
        {
            this.State = new StateConfig();
            this.ExtraDebugLogging = false;
        }

        public StateConfig State { get; set; }

        public bool ExtraDebugLogging { get; set; }
    }
}
