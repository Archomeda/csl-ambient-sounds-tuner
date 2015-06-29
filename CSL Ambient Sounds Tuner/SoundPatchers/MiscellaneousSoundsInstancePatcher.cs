using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonShared.Extensions;

namespace AmbientSoundsTuner.SoundPatchers
{
    /// <summary>
    /// An abstract class that can patch instances of miscellaneous sounds.
    /// </summary>
    public abstract class MiscellaneousSoundsInstancePatcher<T> : SoundsInstancePatcher<T>
    {
        protected abstract AudioInfo GetAudioInfoById(T id);

        public override bool BackupVolume(T id)
        {
            if (SimulationManager.instance.m_metaData != null && SimulationManager.instance.m_metaData.m_updateMode != SimulationManager.UpdateMode.Undefined)
            {
                AudioInfo audioInfo = this.GetAudioInfoById(id);
                if (audioInfo != null)
                {
                    float? volume = SoundsPatcher.GetVolume(audioInfo);
                    if (volume.HasValue)
                    {
                        this.DefaultVolumes[id] = volume.Value;
                        return true;
                    }
                }
            }
            return false;
        }

        public override bool PatchVolume(T id, float newVolume)
        {
            if (SimulationManager.instance.m_metaData != null && SimulationManager.instance.m_metaData.m_updateMode != SimulationManager.UpdateMode.Undefined)
            {
                AudioInfo audioInfo = this.GetAudioInfoById(id);
                if (audioInfo != null)
                {
                    return SoundsPatcher.SetVolume(audioInfo, newVolume);
                }
            }
            return false;
        }
    }
}
