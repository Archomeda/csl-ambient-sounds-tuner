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
        public const string ID_BUILDING_BULLDOZE = "Bulldoze (Buildings)";
        public const string ID_BUILDING_PLACEMENT = "Placement (Buildings)";
        public const string ID_PROP_BULLDOZE = "Bulldoze (Props)";
        public const string ID_PROP_PLACEMENT = "Placement (Props)";
        public const string ID_ROAD_BULLDOZE = "Bulldoze (Roads)";
        public const string ID_ROAD_DRAW = "Road Drawer";
        public const string ID_ROAD_PLACEMENT = "Placement (Roads)";
        public const string ID_CLICK_SOUND = "UI Clicks";
        public const string ID_DISABLED_CLICK_SOUND = "UI Clicks (Disabled)";

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
            this.DefaultVolumes.Add(ID_CLICK_SOUND, 1);
            this.DefaultVolumes.Add(ID_DISABLED_CLICK_SOUND, 1);
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

        public override bool BackupVolume(string id)
        {
            switch (id)
            {
                case ID_CLICK_SOUND:
                case ID_DISABLED_CLICK_SOUND:
                    // Do nothing, since there is nothing to back up
                    return true;
                default:
                    return base.BackupVolume(id);
            }
        }

        public override bool PatchVolume(string id, float newVolume)
        {
            switch (id)
            {
                case ID_CLICK_SOUND:
                    SoundsCollection.MenuClickSoundVolume = newVolume;
                    return true;
                case ID_DISABLED_CLICK_SOUND:
                    SoundsCollection.MenuDisabledClickSoundVolume = newVolume;
                    return true;
                default:
                    return base.PatchVolume(id, newVolume);
            }
        }
    }
}
