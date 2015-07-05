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
    public class MiscPatcher : SoundsInstancePatcher<string>
    {
        public const string ID_BUILDING_BULLDOZE = "Building Bulldoze Sound";
        public const string ID_BUILDING_PLACEMENT = "Building Placement Sound";
        public const string ID_PROP_BULLDOZE = "Prop Bulldoze Sound";
        public const string ID_PROP_PLACEMENT = "Prop Placement Sound";
        public const string ID_ROAD_BULLDOZE = "Road Bulldoze Sound";
        public const string ID_ROAD_DRAW = "Road Draw Sound";
        public const string ID_ROAD_PLACEMENT = "Road Placement Sound";
        public const string ID_ZONE_FILL = "Zone Fill Sound";

        public const string ID_CLICK_SOUND = "UI Clicks";
        public const string ID_DISABLED_CLICK_SOUND = "UI Clicks (Disabled)";


        public MiscPatcher()
            : base()
        {
            this.UIClickSoundVolume = 1;
            this.UIDisabledClickSoundVolume = 1;
        }

        /// <summary>
        /// Gets or sets the click sound volume in UI.
        /// </summary>
        public float UIClickSoundVolume { get; set; }

        /// <summary>
        /// Gets or sets the disabled click sound volume in UI.
        /// </summary>
        public float UIDisabledClickSoundVolume { get; set; }


        public override string[] Ids
        {
            get
            {
                return new[] {
                    ID_BUILDING_BULLDOZE,
                    ID_PROP_BULLDOZE,
                    ID_ROAD_BULLDOZE,
                    ID_BUILDING_PLACEMENT,
                    ID_PROP_PLACEMENT,
                    ID_ROAD_PLACEMENT,
                    ID_ROAD_DRAW,
                    ID_CLICK_SOUND,
                    ID_DISABLED_CLICK_SOUND,
                    ID_ZONE_FILL
                };
            }
        }

        public override string AudioPrefixId
        {
            get { return "Misc"; }
        }

        public override SoundContainer GetSoundInstance(string id)
        {
            switch (id)
            {
                case ID_BUILDING_BULLDOZE:
                    if (BuildingManager.instance.m_properties != null)
                        return new SoundContainer(SoundPatchersManager.GetSubEffectFromMultiEffect(BuildingManager.instance.m_properties.m_bulldozeEffect as MultiEffect, id));
                    break;

                case ID_BUILDING_PLACEMENT:
                    if (BuildingManager.instance.m_properties != null)
                        return new SoundContainer(SoundPatchersManager.GetSubEffectFromMultiEffect(BuildingManager.instance.m_properties.m_placementEffect as MultiEffect, id));
                    break;

                case ID_PROP_BULLDOZE:
                    if (PropManager.instance.m_properties != null)
                        return new SoundContainer(SoundPatchersManager.GetSubEffectFromMultiEffect(PropManager.instance.m_properties.m_bulldozeEffect as MultiEffect, id));
                    break;

                case ID_PROP_PLACEMENT:
                    if (PropManager.instance.m_properties != null)
                        return new SoundContainer(SoundPatchersManager.GetSubEffectFromMultiEffect(PropManager.instance.m_properties.m_placementEffect as MultiEffect, id));
                    break;

                case ID_ROAD_BULLDOZE:
                    if (NetManager.instance.m_properties != null)
                        return new SoundContainer(SoundPatchersManager.GetSubEffectFromMultiEffect(NetManager.instance.m_properties.m_bulldozeEffect as MultiEffect, id));
                    break;

                case ID_ROAD_DRAW:
                    if (NetManager.instance.m_properties != null)
                        return new SoundContainer(NetManager.instance.m_properties.m_drawSound);
                    break;

                case ID_ROAD_PLACEMENT:
                    if (NetManager.instance.m_properties != null)
                        return new SoundContainer(SoundPatchersManager.GetSubEffectFromMultiEffect(NetManager.instance.m_properties.m_placementEffect as MultiEffect, id));
                    break;

                case ID_ZONE_FILL:
                    if (ZoneManager.instance.m_properties != null)
                        return new SoundContainer(ZoneManager.instance.m_properties.m_fillEffect as SoundEffect);
                    break;
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
                    this.UIClickSoundVolume = newVolume;
                    return true;
                case ID_DISABLED_CLICK_SOUND:
                    this.UIDisabledClickSoundVolume = newVolume;
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
