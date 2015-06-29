using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmbientSoundsTuner.SoundPatchers
{
    /// <summary>
    /// A static class that holds references to various sounds within the game.
    /// </summary>
    public static class SoundsCollection
    {
        private static AmbientSounds ambients = new AmbientSounds();
        private static AnimalSounds animals = new AnimalSounds();
        private static BuildingSounds buildings = new BuildingSounds();
        private static VehicleSounds vehicles = new VehicleSounds();
        private static MiscSounds misc = new MiscSounds();

        /// <summary>
        /// Gets the ambient sounds.
        /// </summary>
        public static AmbientSounds Ambients
        {
            get
            {
                return ambients;
            }
        }

        /// <summary>
        /// Gets the animal sounds.
        /// </summary>
        public static AnimalSounds Animals
        {
            get
            {
                return animals;
            }
        }

        /// <summary>
        /// Gets the building sounds.
        /// </summary>
        public static BuildingSounds Buildings
        {
            get
            {
                return buildings;
            }
        }

        /// <summary>
        /// Gets the vehicle sounds.
        /// </summary>
        public static VehicleSounds Vehicles
        {
            get
            {
                return vehicles;
            }
        }

        /// <summary>
        /// Gets the miscellaneous sounds.
        /// </summary>
        public static MiscSounds Misc
        {
            get
            {
                return misc;
            }
        }


        private static AudioInfo GetAudioInfoFromBuildingInfo(string id)
        {
            BuildingInfo building = PrefabCollection<BuildingInfo>.FindLoaded(id);
            if (building != null)
            {
                return building.m_customLoopSound;
            }
            return null;
        }

        private static AudioInfo GetFirstAudioInfoFromBuildingInfos(IEnumerable<string> ids)
        {
            foreach (string id in ids)
            {
                AudioInfo result = GetAudioInfoFromBuildingInfo(id);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        private static AudioInfo GetAudioInfoFromArray(AudioInfo[] array, string id)
        {
            if (array != null)
            {
                return array.FirstOrDefault(a => a != null && a.name == id);
            }
            return null;
        }

        private static SoundEffect GetSubEffectFromMultiEffect(MultiEffect multiEffect, string id)
        {
            if (multiEffect != null && multiEffect.m_effects != null)
            {
                var subEffect = multiEffect.m_effects.FirstOrDefault(e => e.m_effect.name == id);
                if (subEffect.m_effect != null)
                {
                    return subEffect.m_effect as SoundEffect;
                }
            }
            return null;
        }


        /// <summary>
        /// A class that contains references to ambient sounds.
        /// </summary>
        public class AmbientSounds
        {
            /// <summary>
            /// Gets an ambient sound.
            /// </summary>
            /// <param name="id">The ambient sound id.</param>
            /// <returns>The ambient sound if it exists; an empty container otherwise.</returns>
            public SoundContainer this[AudioManager.AmbientType id]
            {
                get
                {
                    if (AudioManager.instance.m_properties != null && AudioManager.instance.m_properties.m_ambients != null
                        && AudioManager.instance.m_properties.m_ambients.Length > (int)id)
                    {
                        return new SoundContainer(AudioManager.instance.m_properties.m_ambients[(int)id]);
                    }
                    return new SoundContainer();
                }
            }
        }

        /// <summary>
        /// A class that contains references to animal sounds.
        /// </summary>
        public class AnimalSounds
        {
            public const string ID_COW = "Cow";
            public const string ID_PIG = "Pig";
            public const string ID_SEAGULL = "Seagull";

            private const string ID_SEAGULL_SCREAM = "Seagull Scream";

            /// <summary>
            /// Gets an animal sound.
            /// </summary>
            /// <param name="id">The animal sound id.</param>
            /// <returns>The animal sound if it exists; an empty container otherwise.</returns>
            public SoundContainer this[string id]
            {
                get
                {
                    CitizenInfo info = null;

                    // Get whitelisted prefabs only
                    switch (id)
                    {
                        case ID_COW:
                        case ID_PIG:
                        case ID_SEAGULL:
                            info = PrefabCollection<CitizenInfo>.FindLoaded(id);
                            break;
                    }

                    // Get sound from prefab
                    if (info != null)
                    {
                        switch (id)
                        {
                            case ID_COW:
                            case ID_PIG:
                                return new SoundContainer(((LivestockAI)info.m_citizenAI).m_randomEffect as SoundEffect);

                            case ID_SEAGULL:
                                MultiEffect effect = ((BirdAI)info.m_citizenAI).m_randomEffect as MultiEffect;
                                if (effect != null)
                                {
                                    return new SoundContainer(effect.m_effects.FirstOrDefault(e => e.m_effect.name == ID_SEAGULL_SCREAM).m_effect as SoundEffect);
                                }
                                break;
                        }
                    }

                    return new SoundContainer();
                }
            }
        }

        /// <summary>
        /// A class that contains references to building sounds.
        /// </summary>
        public class BuildingSounds
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
            public const string ID_POLICE_STATION = "Building Police Station";
            public const string ID_POWER_PLANT_SMALL = "Building Power Plant Small";
            public const string ID_SOLAR_POWER_PLANT = "Solar Power Plant";
            public const string ID_TRAIN_STATION = "Building Train Station";
            public const string ID_UNIVERSITY = "University";
            private const string ID_UNIVERSITY_EU = "University_EU";
            public const string ID_WATER_DRAIN_PIPE = "Water Outlet";
            public const string ID_WATER_PUMPING_STATION = "Water Intake";
            public const string ID_WIND_TURBINE = "Wind Turbine";

            public const string ID_ON_FIRE = "On Fire";
            public const string ID_ON_UPGRADE = "On Upgrade";
            private const string ID_ON_UPGRADE_SOUND = "Levelup Sound";

            /// <summary>
            /// Gets a building sound.
            /// </summary>
            /// <param name="id">The building sound id.</param>
            /// <returns>The building sound if it exists; an empty container otherwise.</returns>
            public SoundContainer this[string id]
            {
                get
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
                        case ID_SOLAR_POWER_PLANT:
                        case ID_WATER_DRAIN_PIPE:
                        case ID_WATER_PUMPING_STATION:
                        case ID_WIND_TURBINE:
                            return new SoundContainer(GetAudioInfoFromBuildingInfo(id));

                        case ID_ELEMENTARY_SCHOOL:
                            return new SoundContainer(GetFirstAudioInfoFromBuildingInfos(new[] { ID_ELEMENTARY_SCHOOL, ID_ELEMENTARY_SCHOOL_EU }));

                        case ID_HIGH_SCHOOL:
                            return new SoundContainer(GetFirstAudioInfoFromBuildingInfos(new[] { ID_HIGH_SCHOOL, ID_HIGH_SCHOOL_EU }));

                        case ID_UNIVERSITY:
                            return new SoundContainer(GetFirstAudioInfoFromBuildingInfos(new[] { ID_UNIVERSITY, ID_UNIVERSITY_EU }));

                        case ID_FIRE_STATION:
                        case ID_HOSPITAL:
                        case ID_POLICE_STATION:
                        case ID_POWER_PLANT_SMALL:
                            if (BuildingManager.instance.m_properties != null)
                            {
                                return new SoundContainer(GetAudioInfoFromArray(BuildingManager.instance.m_properties.m_serviceSounds, id));
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
                                return new SoundContainer(GetAudioInfoFromArray(BuildingManager.instance.m_properties.m_subServiceSounds, id));
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
                                return new SoundContainer(GetSubEffectFromMultiEffect(BuildingManager.instance.m_properties.m_levelupEffect as MultiEffect, ID_ON_UPGRADE_SOUND));
                            }
                            break;
                    }

                    return new SoundContainer();
                }
            }
        }

        /// <summary>
        /// A class that contains references to vehicle sounds.
        /// </summary>
        public class VehicleSounds
        {
            public const string ID_AIRCRAFT_MOVEMENT = "Aircraft Movement";
            public const string ID_AMBULANCE_SIREN = "Ambulance Siren";
            public const string ID_FIRE_TRUCK_SIREN = "Fire Truck Siren";
            public const string ID_LARGE_CAR_MOVEMENT = "Large Car Movement";
            public const string ID_METRO_MOVEMENT = "Metro Movement";
            public const string ID_POLICE_CAR_SIREN = "Police Car Siren";
            public const string ID_SMALL_CAR_MOVEMENT = "Small Car Movement";
            public const string ID_TRAIN_MOVEMENT = "Train Movement";
            public const string ID_TRANSPORT_ARRIVE = "Transport Arrive";

            /// <summary>
            /// Gets a vehicle sound.
            /// </summary>
            /// <param name="id">The vehicle sound id.</param>
            /// <returns>The vehicle sound if it exists; an empty container otherwise.</returns>
            public SoundContainer this[string id]
            {
                get
                {
                    switch (id)
                    {
                        case ID_AIRCRAFT_MOVEMENT:
                        case ID_AMBULANCE_SIREN:
                        case ID_FIRE_TRUCK_SIREN:
                        case ID_LARGE_CAR_MOVEMENT:
                        case ID_METRO_MOVEMENT:
                        case ID_POLICE_CAR_SIREN:
                        case ID_SMALL_CAR_MOVEMENT:
                        case ID_TRAIN_MOVEMENT:
                        case ID_TRANSPORT_ARRIVE:
                            EffectInfo effectInfo = EffectCollection.FindEffect(id);
                            SoundEffect soundEffect = effectInfo as SoundEffect;
                            if (soundEffect != null)
                            {
                                return new SoundContainer(soundEffect);
                            }
                            break;
                    }
                    return new SoundContainer();
                }
            }
        }

        /// <summary>
        /// A class that contains references to miscellaneous sounds.
        /// </summary>
        public class MiscSounds
        {
            public const string ID_BUILDING_BULLDOZE = "Building Bulldoze Sound";
            public const string ID_BUILDING_PLACEMENT = "Building Placement Sound";
            public const string ID_PROP_BULLDOZE = "Prop Bulldoze Sound";
            public const string ID_PROP_PLACEMENT = "Prop Placement Sound";
            public const string ID_ROAD_BULLDOZE = "Road Bulldoze Sound";
            public const string ID_ROAD_DRAW = "Road Draw Sound";
            public const string ID_ROAD_PLACEMENT = "Road Placement Sound";
            public const string ID_ZONE_FILL = "Zone Fill Sound";

            /// <summary>
            /// Gets a miscellaneous sound.
            /// </summary>
            /// <param name="id">The miscellaneous sound id.</param>
            /// <returns>The miscellaneous sound if it exists; an empty container otherwise.</returns>
            public SoundContainer this[string id]
            {
                get
                {
                    switch (id)
                    {
                        case ID_BUILDING_BULLDOZE:
                            if (BuildingManager.instance.m_properties != null)
                            {
                                return new SoundContainer(GetSubEffectFromMultiEffect(BuildingManager.instance.m_properties.m_bulldozeEffect as MultiEffect, id));
                            }
                            break;

                        case ID_BUILDING_PLACEMENT:
                            if (BuildingManager.instance.m_properties != null)
                            {
                                return new SoundContainer(GetSubEffectFromMultiEffect(BuildingManager.instance.m_properties.m_placementEffect as MultiEffect, id));
                            }
                            break;

                        case ID_PROP_BULLDOZE:
                            if (PropManager.instance.m_properties != null)
                            {
                                return new SoundContainer(GetSubEffectFromMultiEffect(PropManager.instance.m_properties.m_bulldozeEffect as MultiEffect, id));
                            }
                            break;

                        case ID_PROP_PLACEMENT:
                            if (PropManager.instance.m_properties != null)
                            {
                                return new SoundContainer(GetSubEffectFromMultiEffect(PropManager.instance.m_properties.m_placementEffect as MultiEffect, id));
                            }
                            break;

                        case ID_ROAD_BULLDOZE:
                            if (NetManager.instance.m_properties != null)
                            {
                                return new SoundContainer(GetSubEffectFromMultiEffect(NetManager.instance.m_properties.m_bulldozeEffect as MultiEffect, id));
                            }
                            break;

                        case ID_ROAD_DRAW:
                            if (NetManager.instance.m_properties != null)
                            {
                                return new SoundContainer(NetManager.instance.m_properties.m_drawSound);
                            }
                            break;

                        case ID_ROAD_PLACEMENT:
                            if (NetManager.instance.m_properties != null)
                            {
                                return new SoundContainer(GetSubEffectFromMultiEffect(NetManager.instance.m_properties.m_placementEffect as MultiEffect, id));
                            }
                            break;

                        case ID_ZONE_FILL:
                            if (ZoneManager.instance.m_properties != null)
                            {
                                return new SoundContainer(ZoneManager.instance.m_properties.m_fillEffect as SoundEffect);
                            }
                            break;
                    }

                    return new SoundContainer();
                }
            }

            private float uiClickSoundVolume = 1;
            /// <summary>
            /// Gets or sets the click sound volume in UI.
            /// </summary>
            public float UIClickSoundVolume
            {
                get { return this.uiClickSoundVolume; }
                set { this.uiClickSoundVolume = value; }
            }

            private float uiDisabledClickSoundVolume = 1;
            /// <summary>
            /// Gets or sets the disabled click sound volume in UI.
            /// </summary>
            public float UIDisabledClickSoundVolume
            {
                get { return this.uiDisabledClickSoundVolume; }
                set { this.uiDisabledClickSoundVolume = value; }
            }
        }
    }
}
