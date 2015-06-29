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
        public const string ID_COW = "Cows";
        public const string ID_PIG = "Pigs";
        public const string ID_SEAGULL = "Seagulls";

        public AnimalsPatcher()
            : base()
        {
            this.DefaultVolumes.Add(ID_COW, 1);
            this.DefaultVolumes.Add(ID_PIG, 1);
            this.DefaultVolumes.Add(ID_SEAGULL, 1);
        }

        protected override AudioInfo GetAudioInfoById(string id)
        {
            SoundEffect soundEffect = null;
            switch (id)
            {
                case ID_COW:
                    soundEffect = SoundsCollection.Cow;
                    break;
                case ID_PIG:
                    soundEffect = SoundsCollection.Pig;
                    break;
                case ID_SEAGULL:
                    soundEffect = SoundsCollection.Seagull;
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
