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
                this.AmbientWorld = 1;
                this.AmbientForest = 1;
                this.AmbientSea = 1;
                this.AmbientStream = 1;
                this.AmbientIndustrial = 1;
                this.AmbientPlaza = 1;
                this.AmbientSuburban = 1;
                this.AmbientCity = 1;
                this.AmbientAgricultural = 1;
            }

            public float AmbientWorld { get; set; }
            public float AmbientForest { get; set; }
            public float AmbientSea { get; set; }
            public float AmbientStream { get; set; }
            public float AmbientIndustrial { get; set; }
            public float AmbientPlaza { get; set; }
            public float AmbientSuburban { get; set; }
            public float AmbientCity { get; set; }
            public float AmbientAgricultural { get; set; }
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
