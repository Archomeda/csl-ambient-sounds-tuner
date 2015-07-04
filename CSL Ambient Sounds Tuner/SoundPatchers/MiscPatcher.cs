using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.SoundPack;

namespace AmbientSoundsTuner.SoundPatchers
{
    /// <summary>
    /// A class that can patch instances of miscellaneous sounds.
    /// </summary>
    public class MiscPatcher : MiscellaneousSoundsInstancePatcher<string>
    {
        public const string ID_CLICK_SOUND = "UI Clicks";
        public const string ID_DISABLED_CLICK_SOUND = "UI Clicks (Disabled)";

        public override string[] Ids
        {
            get
            {
                return new[] {
                    SoundsCollection.MiscSounds.ID_BUILDING_BULLDOZE,
                    SoundsCollection.MiscSounds.ID_PROP_BULLDOZE,
                    SoundsCollection.MiscSounds.ID_ROAD_BULLDOZE,
                    SoundsCollection.MiscSounds.ID_BUILDING_PLACEMENT,
                    SoundsCollection.MiscSounds.ID_PROP_PLACEMENT,
                    SoundsCollection.MiscSounds.ID_ROAD_PLACEMENT,
                    SoundsCollection.MiscSounds.ID_ROAD_DRAW,
                    ID_CLICK_SOUND,
                    ID_DISABLED_CLICK_SOUND,
                    SoundsCollection.MiscSounds.ID_ZONE_FILL
                };
            }
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
                    return false;
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

        public override bool BackupSound(string id)
        {
            switch (id)
            {
                case ID_CLICK_SOUND:
                case ID_DISABLED_CLICK_SOUND:
                    // Do nothing, since there is nothing to back up
                    return false;
                default:
                    return base.BackupSound(id);
            }
        }

        public override bool PatchSound(string id, SoundPackFile.Audio newSound)
        {
            switch (id)
            {
                case ID_CLICK_SOUND:
                case ID_DISABLED_CLICK_SOUND:
                    // Do nothing, since there is nothing to patch
                    return false;
                default:
                    return base.PatchSound(id, newSound);
            }
        }
    }
}
