using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using CommonShared.Configuration;
using UnityEngine;

namespace AmbientSoundsTuner.SoundPack
{
    [XmlRoot("SoundPack")]
    public class SoundPackFile : VersionedConfig
    {
        public SoundPackFile()
        {
            this.Version = 1;
            this.Ambients = new Audio[0];
            this.Animals = new Audio[0];
            this.Buildings = new Audio[0];
            this.Vehicles = new Audio[0];
            this.Miscs = new Audio[0];
        }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("author")]
        public string Author { get; set; }

        [XmlElement("Ambients"), XmlArrayItem("Ambient")]
        public Audio[] Ambients { get; set; }

        [XmlElement("Animals"), XmlArrayItem("Animal")]
        public Audio[] Animals { get; set; }

        [XmlElement("Buildings"), XmlArrayItem("Building")]
        public Audio[] Buildings { get; set; }

        [XmlElement("Vehicles"), XmlArrayItem("Vehicle")]
        public Audio[] Vehicles { get; set; }

        [XmlElement("Miscs"), XmlArrayItem("Misc")]
        public Audio[] Miscs { get; set; }


        public class Audio
        {
            public Audio()
            {
                this.AudioInfo = new AudioInfo();
            }

            [XmlAttribute("type")]
            public string Type { get; set; }

            [XmlAttribute("name")]
            public string Name { get; set; }

            public AudioInfo AudioInfo { get; set; }
        }

        public class AudioInfo
        {
            public AudioInfo()
            {
                this.Variations = new Variation[0];
            }

            public string Clip { get; set; }

            public AudioClip AudioClip { get; set; }

            public float Volume { get; set; }

            public float MaxVolume { get; set; }

            public float Pitch { get; set; }

            public float FadeLength { get; set; }

            public bool IsLoop { get; set; }

            public bool Is3D { get; set; }

            public bool IsRandomTime { get; set; }

            [XmlElement("Variations"), XmlArrayItem("Variation")]
            public Variation[] Variations { get; set; }
        }

        public class Variation
        {
            public int Probability { get; set; }

            public AudioInfo AudioInfo { get; set; }
        }
    }
}
