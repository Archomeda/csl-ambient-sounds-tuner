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
        public const string ID_SEAGULL_SCREAM = "Seagull Scream 1";

        public SoundsInstanceMiscellaneousPatcher()
            : base()
        {
            this.DefaultVolumes.Add(ID_SEAGULL_SCREAM, 1);
        }

        private SoundEffect GetSoundEffectById(string id)
        {
            SoundEffect soundEffect = null;
            switch (id)
            {
                case ID_SEAGULL_SCREAM:
                    soundEffect = SoundsCollection.SeagullScream;
                    break;
            }
            return soundEffect;
        }

        public override bool BackupVolume(string id)
        {
            SoundEffect soundEffect = this.GetSoundEffectById(id);
            if (soundEffect != null)
            {
                float? volume = SoundsPatcher.GetVolume(soundEffect);
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
            SoundEffect soundEffect = this.GetSoundEffectById(id);
            if (soundEffect != null)
            {
                return SoundsPatcher.SetVolume(soundEffect, newVolume);
            }
            return false;
        }
    }
}
