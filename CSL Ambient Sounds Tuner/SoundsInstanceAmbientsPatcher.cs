using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonShared.Extensions;

namespace AmbientSoundsTuner
{
    /// <summary>
    /// A class that can patch instances of ambient sounds.
    /// </summary>
    public class SoundsInstanceAmbientsPatcher : SoundsInstancePatcher<AudioManager.AmbientType>
    {
        public SoundsInstanceAmbientsPatcher()
            : base()
        {
            this.DefaultVolumes.Add(AudioManager.AmbientType.World, 1);
            this.DefaultVolumes.Add(AudioManager.AmbientType.Forest, 1);
            this.DefaultVolumes.Add(AudioManager.AmbientType.Sea, 1);
            this.DefaultVolumes.Add(AudioManager.AmbientType.Stream, 1);
            this.DefaultVolumes.Add(AudioManager.AmbientType.Industrial, 1);
            this.DefaultVolumes.Add(AudioManager.AmbientType.Plaza, 1);
            this.DefaultVolumes.Add(AudioManager.AmbientType.Suburban, 1);
            this.DefaultVolumes.Add(AudioManager.AmbientType.City, 1);
            this.DefaultVolumes.Add(AudioManager.AmbientType.Agricultural, 1);
        }

        public override bool BackupVolume(AudioManager.AmbientType id)
        {
            if (SoundsCollection.Ambients.Length > (int)id && SoundsCollection.Ambients[(int)id] != null)
            {
                this.DefaultVolumes[id] = SoundsCollection.Ambients[(int)id].m_volume;
                return true;
            }
            return false;
        }

        public override bool PatchVolume(AudioManager.AmbientType id, float newVolume)
        {
            if (SoundsCollection.Ambients.Length > (int)id && SoundsCollection.Ambients[(int)id] != null)
            {
                SoundsCollection.Ambients[(int)id].m_volume = newVolume;
                return true;
            }
            return false;
        }
    }
}
