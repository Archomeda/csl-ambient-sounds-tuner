using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.SoundPack;
using CommonShared.Extensions;

namespace AmbientSoundsTuner.SoundPatchers
{
    /// <summary>
    /// A class that can patch instances of ambient sounds.
    /// </summary>
    public class AmbientsPatcher : SoundsInstancePatcher<AudioManager.AmbientType>
    {
        public override AudioManager.AmbientType[] Ids
        {
            get
            {
                return new[] {
                    AudioManager.AmbientType.Agricultural,
                    AudioManager.AmbientType.City,
                    AudioManager.AmbientType.Forest,
                    AudioManager.AmbientType.Industrial,
                    AudioManager.AmbientType.Plaza,
                    AudioManager.AmbientType.Sea,
                    AudioManager.AmbientType.Stream,
                    AudioManager.AmbientType.Suburban,
                    AudioManager.AmbientType.World
                };
            }
        }

        public override bool BackupVolume(AudioManager.AmbientType id)
        {
            if (SimulationManager.instance.m_metaData != null && SimulationManager.instance.m_metaData.m_updateMode != SimulationManager.UpdateMode.Undefined)
            {
                SoundContainer sound = SoundsCollection.Ambients[id];
                float? volume = SoundsPatcher.GetVolume(sound);
                if (volume.HasValue)
                {
                    this.OldVolumes[id] = volume.Value;
                    return true;
                }
            }
            return false;
        }

        public override bool PatchVolume(AudioManager.AmbientType id, float newVolume)
        {
            if (SimulationManager.instance.m_metaData != null && SimulationManager.instance.m_metaData.m_updateMode != SimulationManager.UpdateMode.Undefined)
            {
                SoundContainer sound = SoundsCollection.Ambients[id];
                return SoundsPatcher.SetVolume(sound, newVolume);
            }
            return false;
        }

        public override bool BackupSound(AudioManager.AmbientType id)
        {
            if (SimulationManager.instance.m_metaData != null && SimulationManager.instance.m_metaData.m_updateMode != SimulationManager.UpdateMode.Undefined)
            {
                SoundContainer sound = SoundsCollection.Ambients[id];
                this.OldSounds[id] = SoundsPatcher.GetAudioInfo(sound);
                return this.OldSounds != null;
            }
            return false;
        }

        public override bool PatchSound(AudioManager.AmbientType id, SoundPackFile.Audio newSound)
        {
            if (SimulationManager.instance.m_metaData != null && SimulationManager.instance.m_metaData.m_updateMode != SimulationManager.UpdateMode.Undefined)
            {
                SoundContainer sound = SoundsCollection.Ambients[id];
                return SoundsPatcher.SetAudioInfo(sound, newSound);
            }
            return false;
        }
    }
}
