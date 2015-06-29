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
        public AnimalsPatcher()
            : base()
        {
            this.DefaultVolumes.Add(SoundsCollection.AnimalSounds.ID_COW, 1);
            this.DefaultVolumes.Add(SoundsCollection.AnimalSounds.ID_PIG, 1);
            this.DefaultVolumes.Add(SoundsCollection.AnimalSounds.ID_SEAGULL, 1);
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
