using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AmbientSoundsTuner.Utils;

namespace AmbientSoundsTuner
{
    [XmlRoot("Configuration")]
    public class Configuration
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

        [XmlIgnore]
        public static Configuration Instance { get; private set; }


        /// <summary>
        /// Load configuration.
        /// </summary>
        public static void Load()
        {
            string path = Path.Combine(FileUtils.GetDataFolder(), Mod.AssemblyName + ".xml");
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    Instance = (Configuration)new XmlSerializer(typeof(Configuration)).Deserialize(sr);
                }
                Logger.Info("Loaded configuration");
            }
            else
            {
                Instance = new Configuration();
                Logger.Info("No configuration file found, loaded default");
            }
        }

        /// <summary>
        /// Save configuration.
        /// </summary>
        public static void Save()
        {
            if (Instance == null)
            {
                Logger.Warning("Cannot save configuration when there's no configuration instance!");
                return;
            }

            string path = Path.Combine(FileUtils.GetDataFolder(), Mod.AssemblyName + ".xml");
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            using (StreamWriter sw = new StreamWriter(path))
            {
                new XmlSerializer(typeof(Configuration)).Serialize(sw, Instance);
            }
            Logger.Info("Saved configuration");
        }
    }
}
