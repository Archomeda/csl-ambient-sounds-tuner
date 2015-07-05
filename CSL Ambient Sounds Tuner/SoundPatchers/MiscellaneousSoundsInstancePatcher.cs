using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.SoundPack;
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
                float? volume = SoundsPatcher.GetVolume(audioInfo);
                if (volume.HasValue)
                {
                    this.OldVolumes[id] = volume.Value;
                    return true;
                }
            }
            return false;
        }

        public override bool PatchVolume(T id, float newVolume)
        {
            if (SimulationManager.instance.m_metaData != null && SimulationManager.instance.m_metaData.m_updateMode != SimulationManager.UpdateMode.Undefined)
            {
                AudioInfo audioInfo = this.GetAudioInfoById(id);
                return SoundsPatcher.SetVolume(audioInfo, newVolume);
            }
            return false;
        }

        public override bool BackupSound(T id)
        {
            if (SimulationManager.instance.m_metaData != null && SimulationManager.instance.m_metaData.m_updateMode != SimulationManager.UpdateMode.Undefined)
            {
                AudioInfo audioInfo = this.GetAudioInfoById(id);
                this.OldSounds[id] = SoundsPatcher.GetAudioInfo(audioInfo);
                return this.OldSounds[id] != null;
            }
            return false;
        }

        public override bool PatchSound(T id, SoundPackFile.Audio newSound)
        {
            if (SimulationManager.instance.m_metaData != null && SimulationManager.instance.m_metaData.m_updateMode != SimulationManager.UpdateMode.Undefined)
            {
                AudioInfo audioInfo = this.GetAudioInfoById(id);
                return SoundsPatcher.SetAudioInfo(audioInfo, newSound);
            }
            return false;
        }
    }
}
