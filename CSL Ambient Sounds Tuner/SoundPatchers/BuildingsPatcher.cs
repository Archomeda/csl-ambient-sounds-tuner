using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmbientSoundsTuner.SoundPatchers
{
    /// <summary>
    /// A class that can patch instances of building sounds.
    /// </summary>
    public class BuildingsPatcher : SoundsInstancePatcher<string>
    {
        public const string ID_ADVANCED_WIND_TURBINE = "Advanced Wind Turbine";
        public const string ID_AIRPORT = "Building Airport";
        public const string ID_BUS_DEPOT = "Building Bus Depot";
        public const string ID_CEMETERY = "Cemetery";
        public const string ID_COAL_POWER_PLANT = "Coal Power Plant";
        public const string ID_COMMERCIAL = "Building Commercial";
        public const string ID_CREMATORY = "Crematory";
        public const string ID_ELEMENTARY_SCHOOL = "Elementary School";
        private const string ID_ELEMENTARY_SCHOOL_EU = "Elementary_School_EU";
        public const string ID_FIRE_STATION = "Building Fire Station";
        public const string ID_FUSION_POWER_PLANT = "Fusion Power Plant";
        public const string ID_HARBOR = "Building Harbor";
        public const string ID_HIGH_SCHOOL = "High School";
        private const string ID_HIGH_SCHOOL_EU = "highschool_EU";
        public const string ID_HOSPITAL = "Building Hospital";
        public const string ID_HYDRO_POWER_PLANT = "Dam Power House";
        public const string ID_INCINERATION_PLANT = "Combustion Plant";
        public const string ID_INDUSTRIAL = "Building Industrial";
        public const string ID_METRO_STATION = "Building Metro Station";
        public const string ID_NUCLEAR_POWER_PLANT = "Nuclear Power Plant";
        public const string ID_OIL_POWER_PLANT = Compatibility.SoundDuplicator.BUILDING_OIL_POWER_PLANT;
        public const string ID_POLICE_STATION = "Building Police Station";
        public const string ID_POWER_PLANT_SMALL = "Building Power Plant Small";
        public const string ID_SOLAR_POWER_PLANT = "Solar Power Plant";
        public const string ID_TRAIN_STATION = "Building Train Station";
        public const string ID_UNIVERSITY = "University";
        private const string ID_UNIVERSITY_EU = "University_EU";
        public const string ID_WATER_DRAIN_PIPE = "Water Outlet";
        public const string ID_WATER_PUMPING_STATION = "Water Intake";
        public const string ID_WATER_TREATMENT_PLANT = Compatibility.SoundDuplicator.BUILDING_WATER_TREATMENT_PLANT;
        public const string ID_WIND_TURBINE = "Wind Turbine";

        public const string ID_ON_FIRE = "On Fire";
        public const string ID_ON_UPGRADE = "On Upgrade";
        private const string ID_ON_UPGRADE_SOUND = "Levelup Sound";


        public BuildingsPatcher()
            : base()
        {
            this.DefaultVolumes.Add(ID_FUSION_POWER_PLANT, 4);
            this.DefaultVolumes.Add(ID_INDUSTRIAL, 0.5f);
            this.DefaultVolumes.Add(ID_ON_UPGRADE, 0.25f);

            this.DefaultMaxVolumes.Add(ID_FUSION_POWER_PLANT, 4);
            this.DefaultMaxVolumes.Add(ID_INDUSTRIAL, 0.5f);
            this.DefaultMaxVolumes.Add(ID_ON_UPGRADE, 0.25f);
        }

        public override string[] Ids
        {
            get
            {
                return new[] {
                    ID_ADVANCED_WIND_TURBINE,
                    ID_AIRPORT,
                    ID_BUS_DEPOT,
                    ID_CEMETERY,
                    ID_COAL_POWER_PLANT,
                    ID_COMMERCIAL,
                    ID_CREMATORY,
                    ID_ELEMENTARY_SCHOOL,
                    ID_FIRE_STATION,
                    ID_FUSION_POWER_PLANT,
                    ID_HARBOR,
                    ID_HIGH_SCHOOL,
                    ID_HOSPITAL,
                    ID_HYDRO_POWER_PLANT,
                    ID_INCINERATION_PLANT,
                    ID_INDUSTRIAL,
                    ID_METRO_STATION,
                    ID_NUCLEAR_POWER_PLANT,
                    ID_OIL_POWER_PLANT,
                    ID_POLICE_STATION,
                    ID_POWER_PLANT_SMALL,
                    ID_SOLAR_POWER_PLANT,
                    ID_TRAIN_STATION,
                    ID_UNIVERSITY,
                    ID_WATER_DRAIN_PIPE,
                    ID_WATER_PUMPING_STATION,
                    ID_WATER_TREATMENT_PLANT,
                    ID_WIND_TURBINE,

                    ID_ON_FIRE,
                    ID_ON_UPGRADE
                };
            }
        }

        public override string AudioPrefixId
        {
            get { return "Building"; }
        }

        public override SoundContainer GetSoundInstance(string id)
        {
            switch (id)
            {
                case ID_ADVANCED_WIND_TURBINE:
                case ID_CEMETERY:
                case ID_COAL_POWER_PLANT:
                case ID_CREMATORY:
                case ID_FUSION_POWER_PLANT:
                case ID_HYDRO_POWER_PLANT:
                case ID_INCINERATION_PLANT:
                case ID_NUCLEAR_POWER_PLANT:
                case ID_OIL_POWER_PLANT:
                case ID_SOLAR_POWER_PLANT:
                case ID_WATER_DRAIN_PIPE:
                case ID_WATER_PUMPING_STATION:
                case ID_WATER_TREATMENT_PLANT:
                case ID_WIND_TURBINE:
                    return new SoundContainer(SoundPatchersManager.GetAudioInfoFromBuildingInfo(id));

                case ID_ELEMENTARY_SCHOOL:
                    return new SoundContainer(SoundPatchersManager.GetFirstAudioInfoFromBuildingInfos(new[] { ID_ELEMENTARY_SCHOOL, ID_ELEMENTARY_SCHOOL_EU }));

                case ID_HIGH_SCHOOL:
                    return new SoundContainer(SoundPatchersManager.GetFirstAudioInfoFromBuildingInfos(new[] { ID_HIGH_SCHOOL, ID_HIGH_SCHOOL_EU }));

                case ID_UNIVERSITY:
                    return new SoundContainer(SoundPatchersManager.GetFirstAudioInfoFromBuildingInfos(new[] { ID_UNIVERSITY, ID_UNIVERSITY_EU }));

                case ID_FIRE_STATION:
                case ID_HOSPITAL:
                case ID_POLICE_STATION:
                case ID_POWER_PLANT_SMALL:
                    if (BuildingManager.instance.m_properties != null)
                    {
                        return new SoundContainer(SoundPatchersManager.GetAudioInfoFromArray(BuildingManager.instance.m_properties.m_serviceSounds, id));
                    }
                    break;

                case ID_AIRPORT:
                case ID_BUS_DEPOT:
                case ID_COMMERCIAL:
                case ID_HARBOR:
                case ID_INDUSTRIAL:
                case ID_METRO_STATION:
                case ID_TRAIN_STATION:
                    if (BuildingManager.instance.m_properties != null)
                    {
                        return new SoundContainer(SoundPatchersManager.GetAudioInfoFromArray(BuildingManager.instance.m_properties.m_subServiceSounds, id));
                    }
                    break;

                case ID_ON_FIRE:
                    if (BuildingManager.instance.m_properties != null)
                    {
                        FireEffect effect = BuildingManager.instance.m_properties.m_fireEffect as FireEffect;
                        if (effect != null)
                        {
                            return new SoundContainer(effect.m_soundEffect);
                        }
                    }
                    break;

                case ID_ON_UPGRADE:
                    if (BuildingManager.instance.m_properties != null)
                    {
                        return new SoundContainer(SoundPatchersManager.GetSubEffectFromMultiEffect(BuildingManager.instance.m_properties.m_levelupEffect as MultiEffect, ID_ON_UPGRADE_SOUND));
                    }
                    break;
            }

            return null;
        }
    }
}
