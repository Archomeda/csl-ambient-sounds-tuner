using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonShared.Extensions;

namespace AmbientSoundsTuner
{
    /// <summary>
    /// A class that can patch instances of miscellaneous sounds.
    /// </summary>
    public class SoundsInstanceMiscellaneousPatcher : SoundsInstancePatcher<string>
    {
        public const string ID_SEAGULL_SCREAM = "Seagull Scream";
        public const string ID_INCINERATION_PLANT = "Incineration Plant";

        public SoundsInstanceMiscellaneousPatcher()
            : base()
        {
            this.DefaultVolumes.Add(ID_SEAGULL_SCREAM, 1);
            this.DefaultVolumes.Add(ID_INCINERATION_PLANT, 1);
        }

        private AudioInfo GetAudioInfoById(string id)
        {
            SoundEffect soundEffect = null;
            switch (id)
            {
                case ID_SEAGULL_SCREAM:
                    soundEffect = SoundsCollection.SeagullScream;
                    break;
            }

            AudioInfo audioInfo = null;
            if (soundEffect != null)
            {
                audioInfo = soundEffect.m_audioInfo;
            }
            else
            {
                switch (id)
                {
                    case ID_INCINERATION_PLANT:
                        audioInfo = SoundsCollection.IncinerationPlant;
                        break;
                }
            }

            return audioInfo;
        }

        public override bool BackupVolume(string id)
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
            return false;
        }

        public override bool PatchVolume(string id, float newVolume)
        {
            AudioInfo audioInfo = this.GetAudioInfoById(id);
            if (audioInfo != null)
            {
                return SoundsPatcher.SetVolume(audioInfo, newVolume);
            }
            return false;
        }
    }
}
