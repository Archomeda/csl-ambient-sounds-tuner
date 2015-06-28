using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonShared.Extensions;

namespace AmbientSoundsTuner.SoundPatchers
{
    /// <summary>
    /// A class that can patch instances of miscellaneous sounds.
    /// </summary>
    public class MiscPatcher : MiscellaneousSoundsInstancePatcher<string>
    {
        public const string ID_BUILDING_BULLDOZE = "Building Bulldoze";
        public const string ID_BUILDING_PLACEMENT = "Building Placement";
        public const string ID_PROP_BULLDOZE = "Prop Bulldoze";
        public const string ID_PROP_PLACEMENT = "Prop Placement";
        public const string ID_ROAD_BULLDOZE = "Road Bulldoze";
        public const string ID_ROAD_DRAW = "Road Draw";
        public const string ID_ROAD_PLACEMENT = "Road Placement";

        public MiscPatcher()
            : base()
        {
            this.DefaultVolumes.Add(ID_BUILDING_BULLDOZE, 1);
            this.DefaultVolumes.Add(ID_BUILDING_PLACEMENT, 1);
            this.DefaultVolumes.Add(ID_PROP_BULLDOZE, 1);
            this.DefaultVolumes.Add(ID_PROP_PLACEMENT, 1);
            this.DefaultVolumes.Add(ID_ROAD_BULLDOZE, 1);
            this.DefaultVolumes.Add(ID_ROAD_DRAW, 1);
            this.DefaultVolumes.Add(ID_ROAD_PLACEMENT, 1);
        }

        protected override AudioInfo GetAudioInfoById(string id)
        {
            SoundEffect soundEffect = null;
            switch (id)
            {
                case ID_BUILDING_BULLDOZE:
                    soundEffect = SoundsCollection.BuildingBulldoze;
                    break;
                case ID_BUILDING_PLACEMENT:
                    soundEffect = SoundsCollection.BuildingPlacement;
                    break;
                case ID_PROP_BULLDOZE:
                    soundEffect = SoundsCollection.PropBulldoze;
                    break;
                case ID_PROP_PLACEMENT:
                    soundEffect = SoundsCollection.PropPlacement;
                    break;
                case ID_ROAD_BULLDOZE:
                    soundEffect = SoundsCollection.RoadBulldoze;
                    break;
                case ID_ROAD_PLACEMENT:
                    soundEffect = SoundsCollection.RoadPlacement;
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
                switch (id)
                {
                    case ID_ROAD_DRAW:
                        audioInfo = SoundsCollection.RoadDraw;
                        break;
                }
            }

            return audioInfo;
        }
    }
}
