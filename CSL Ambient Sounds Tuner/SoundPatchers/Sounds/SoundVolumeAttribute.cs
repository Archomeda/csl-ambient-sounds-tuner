using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmbientSoundsTuner.SoundPatchers.Sounds
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SoundVolumeAttribute : Attribute
    {
        public SoundVolumeAttribute() : this(1, 1) { }

        public SoundVolumeAttribute(float defaultVolume, float maxVolume)
        {
            this.DefaultVolume = defaultVolume;
            this.MaxVolume = maxVolume;
        }

        public float DefaultVolume { get; set; }

        public float MaxVolume { get; set; }
    }
}
