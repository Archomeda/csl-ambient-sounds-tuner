using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonShared.Extensions;

namespace AmbientSoundsTuner.SoundPatchers
{
    /// <summary>
    /// A class that can patch instances of ambient sounds.
    /// </summary>
    public class AmbientsPatcher : SoundsInstancePatcher<AudioManager.AmbientType>
    {
        public AmbientsPatcher()
            : base()
        {
            this.DefaultVolumes.Add(AudioManager.AmbientType.Agricultural, 1);
            this.DefaultVolumes.Add(AudioManager.AmbientType.City, 1);
            this.DefaultVolumes.Add(AudioManager.AmbientType.Forest, 1);
            this.DefaultVolumes.Add(AudioManager.AmbientType.Industrial, 1);
            this.DefaultVolumes.Add(AudioManager.AmbientType.Plaza, 1);
            this.DefaultVolumes.Add(AudioManager.AmbientType.Sea, 1);
            this.DefaultVolumes.Add(AudioManager.AmbientType.Stream, 1);
            this.DefaultVolumes.Add(AudioManager.AmbientType.Suburban, 1);
            this.DefaultVolumes.Add(AudioManager.AmbientType.World, 1);
        }

        public override bool BackupVolume(AudioManager.AmbientType id)
        {
            if (SimulationManager.instance.m_metaData != null && SimulationManager.instance.m_metaData.m_updateMode != SimulationManager.UpdateMode.Undefined)
            {
                if (SoundsCollection.Ambients.Length > (int)id)
                {
                    float? volume = SoundsPatcher.GetVolume(SoundsCollection.Ambients[(int)id]);
                    if (volume.HasValue)
                    {
                        this.DefaultVolumes[id] = volume.Value;
                        return true;
                    }
                }
            }
            return false;
        }

        public override bool PatchVolume(AudioManager.AmbientType id, float newVolume)
        {
            if (SimulationManager.instance.m_metaData != null && SimulationManager.instance.m_metaData.m_updateMode != SimulationManager.UpdateMode.Undefined)
            {
                if (SoundsCollection.Ambients.Length > (int)id)
                {
                    return SoundsPatcher.SetVolume(SoundsCollection.Ambients[(int)id], newVolume);
                }
            }
            return false;
        }
    }
}
