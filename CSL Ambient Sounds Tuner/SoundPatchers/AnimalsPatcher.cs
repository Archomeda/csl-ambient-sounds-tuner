using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmbientSoundsTuner.SoundPatchers
{
    /// <summary>
    /// A class that can patch instances of animal sounds.
    /// </summary>
    public class AnimalsPatcher : MiscellaneousSoundsInstancePatcher<string>
    {
        public override string[] Ids
        {
            get
            {
                return new[] {
                    SoundsCollection.AnimalSounds.ID_COW,
                    SoundsCollection.AnimalSounds.ID_PIG,
                    SoundsCollection.AnimalSounds.ID_SEAGULL
                };
            }
        }

        public override string AudioPrefixId
        {
            get { return "Animal"; }
        }

        protected override AudioInfo GetAudioInfoById(string id)
        {
            SoundContainer sound = SoundsCollection.Animals[id];

            if (sound.HasSound)
            {
                return sound.AudioInfo;
            }

            return null;
        }
    }
}
