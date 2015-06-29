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
                SoundContainer sound = SoundsCollection.Ambients[id];
                if (sound.HasSound)
                {
                    float? volume = SoundsPatcher.GetVolume(sound);
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
                SoundContainer sound = SoundsCollection.Ambients[id];
                if (sound.HasSound)
                {
                    return SoundsPatcher.SetVolume(sound, newVolume);
                }
            }
            return false;
        }
    }
}
