using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonShared.Extensions;

namespace AmbientSoundsTuner.SoundPatchers
{
    /// <summary>
    /// A class that can patch instances of building sounds.
    /// </summary>
    public class BuildingsPatcher : MiscellaneousSoundsInstancePatcher<string>
    {
        public const string ID_INCINERATION_PLANT = "Incineration Plant";

        public BuildingsPatcher()
            : base()
        {
            this.DefaultVolumes.Add(ID_INCINERATION_PLANT, 1);
        }

        protected override AudioInfo GetAudioInfoById(string id)
        {
            AudioInfo audioInfo = null;
            switch (id)
            {
                case ID_INCINERATION_PLANT:
                    audioInfo = SoundsCollection.IncinerationPlant;
                    break;
            }

            return audioInfo;
        }
    }
}
