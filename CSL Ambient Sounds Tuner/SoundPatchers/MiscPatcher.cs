using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmbientSoundsTuner.SoundPatchers
{
    /// <summary>
    /// A class that can patch instances of miscellaneous sounds.
    /// </summary>
    public class MiscPatcher : MiscellaneousSoundsInstancePatcher<string>
    {
        public const string ID_CLICK_SOUND = "UI Clicks";
        public const string ID_DISABLED_CLICK_SOUND = "UI Clicks (Disabled)";

        public MiscPatcher()
            : base()
        {
            this.DefaultVolumes.Add(SoundsCollection.MiscSounds.ID_BUILDING_BULLDOZE, 1);
            this.DefaultVolumes.Add(SoundsCollection.MiscSounds.ID_PROP_BULLDOZE, 1);
            this.DefaultVolumes.Add(SoundsCollection.MiscSounds.ID_ROAD_BULLDOZE, 1);
            this.DefaultVolumes.Add(SoundsCollection.MiscSounds.ID_BUILDING_PLACEMENT, 1);
            this.DefaultVolumes.Add(SoundsCollection.MiscSounds.ID_PROP_PLACEMENT, 1);
            this.DefaultVolumes.Add(SoundsCollection.MiscSounds.ID_ROAD_PLACEMENT, 1);
            this.DefaultVolumes.Add(SoundsCollection.MiscSounds.ID_ROAD_DRAW, 1);
            this.DefaultVolumes.Add(ID_CLICK_SOUND, 1);
            this.DefaultVolumes.Add(ID_DISABLED_CLICK_SOUND, 1);
            this.DefaultVolumes.Add(SoundsCollection.MiscSounds.ID_ZONE_FILL, 1);
        }

        protected override AudioInfo GetAudioInfoById(string id)
        {
            SoundContainer sound = SoundsCollection.Misc[id];

            if (sound.HasSound)
            {
                return sound.AudioInfo;
            }

            return null;
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
                    SoundsCollection.Misc.UIClickSoundVolume = newVolume;
                    return true;
                case ID_DISABLED_CLICK_SOUND:
                    SoundsCollection.Misc.UIDisabledClickSoundVolume = newVolume;
                    return true;
                default:
                    return base.PatchVolume(id, newVolume);
            }
        }
    }
}
