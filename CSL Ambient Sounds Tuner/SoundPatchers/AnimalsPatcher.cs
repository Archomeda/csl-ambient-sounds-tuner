using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonShared.Extensions;

namespace AmbientSoundsTuner.SoundPatchers
{
    /// <summary>
    /// A class that can patch instances of animal sounds.
    /// </summary>
    public class AnimalsPatcher : MiscellaneousSoundsInstancePatcher<string>
    {
        public const string ID_SEAGULL_SCREAM = "Seagull Scream";

        public AnimalsPatcher()
            : base()
        {
            this.DefaultVolumes.Add(ID_SEAGULL_SCREAM, 1);
        }

        protected override AudioInfo GetAudioInfoById(string id)
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
                // For sounds that don't have a SoundEffect tied to it, but directly an AudioInfo
            }

            return audioInfo;
        }
    }
}
