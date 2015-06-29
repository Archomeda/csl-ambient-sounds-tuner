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
        public const string ID_ADVANCED_WIND_TURBINE = "Advanced Wind Turbine";
        public const string ID_AIRPORT = "Airport";
        public const string ID_BUS_DEPOT = "Bus Depot";
        public const string ID_COAL_OIL_POWER_PLANT = "Coal/Oil Power Plant";
        public const string ID_COMMERCIAL = "Commercial";
        public const string ID_FIRE_STATION = "Fire Station";
        public const string ID_FUSION_POWER_PLANT = "Fusion Power Plant";
        public const string ID_HARBOR = "Harbor";
        public const string ID_HOSPITAL = "Hospital";
        public const string ID_HYDRO_POWER_PLANT = "Hydro Power Plant";
        public const string ID_INCINERATION_PLANT = "Incineration Plant";
        public const string ID_INDUSTRIAL = "Industrial";
        public const string ID_METRO_STATION = "Metro Station";
        public const string ID_NUCLEAR_POWER_PLANT = "Nuclear Power Plant";
        public const string ID_POLICE_STATION = "Police Station";
        public const string ID_POWER_PLANT_SMALL = "Small Power Plant";
        public const string ID_SOLAR_POWER_PLANT = "Solar Power Plant";
        public const string ID_TRAIN_STATION = "Train Station";
        public const string ID_WATER_DRAIN_PIPE = "Water Drain/Treatment Plant";
        public const string ID_WATER_PUMPING_STATION = "Water Pumping Station";
        public const string ID_WIND_TURBINE = "Wind Turbine";

        public const string ID_FIRE = "On Fire";
        public const string ID_LEVELUP = "On Upgrade";

        public BuildingsPatcher()
            : base()
        {
            this.DefaultVolumes.Add(ID_ADVANCED_WIND_TURBINE, 1);
            this.DefaultVolumes.Add(ID_AIRPORT, 1);
            this.DefaultVolumes.Add(ID_BUS_DEPOT, 1);
            this.DefaultVolumes.Add(ID_COAL_OIL_POWER_PLANT, 1);
            this.DefaultVolumes.Add(ID_COMMERCIAL, 1);
            this.DefaultVolumes.Add(ID_FIRE_STATION, 1);
            this.DefaultVolumes.Add(ID_FUSION_POWER_PLANT, 4);
            this.DefaultVolumes.Add(ID_HARBOR, 1);
            this.DefaultVolumes.Add(ID_HOSPITAL, 1);
            this.DefaultVolumes.Add(ID_HYDRO_POWER_PLANT, 1);
            this.DefaultVolumes.Add(ID_INCINERATION_PLANT, 1);
            this.DefaultVolumes.Add(ID_INDUSTRIAL, 0.5f);
            this.DefaultVolumes.Add(ID_METRO_STATION, 1);
            this.DefaultVolumes.Add(ID_NUCLEAR_POWER_PLANT, 1);
            this.DefaultVolumes.Add(ID_POLICE_STATION, 1);
            this.DefaultVolumes.Add(ID_POWER_PLANT_SMALL, 1);
            this.DefaultVolumes.Add(ID_SOLAR_POWER_PLANT, 1);
            this.DefaultVolumes.Add(ID_TRAIN_STATION, 1);
            this.DefaultVolumes.Add(ID_WATER_DRAIN_PIPE, 1);
            this.DefaultVolumes.Add(ID_WATER_PUMPING_STATION, 1);
            this.DefaultVolumes.Add(ID_WIND_TURBINE, 1);

            this.DefaultVolumes.Add(ID_FIRE, 1);
            this.DefaultVolumes.Add(ID_LEVELUP, 0.25f);
        }

        protected override AudioInfo GetAudioInfoById(string id)
        {
            AudioInfo audioInfo = null;
            switch (id)
            {
                case ID_ADVANCED_WIND_TURBINE:
                    audioInfo = SoundsCollection.AdvancedWindTurbine;
                    break;
                case ID_AIRPORT:
                    audioInfo = SoundsCollection.Airport;
                    break;
                case ID_BUS_DEPOT:
                    audioInfo = SoundsCollection.BusDepot;
                    break;
                case ID_COAL_OIL_POWER_PLANT:
                    audioInfo = SoundsCollection.CoalOilPowerPlant;
                    break;
                case ID_COMMERCIAL:
                    audioInfo = SoundsCollection.Commercial;
                    break;
                case ID_FIRE_STATION:
                    audioInfo = SoundsCollection.FireStation;
                    break;
                case ID_FUSION_POWER_PLANT:
                    audioInfo = SoundsCollection.FusionPowerPlant;
                    break;
                case ID_HARBOR:
                    audioInfo = SoundsCollection.Harbor;
                    break;
                case ID_HOSPITAL:
                    audioInfo = SoundsCollection.Hospital;
                    break;
                case ID_HYDRO_POWER_PLANT:
                    audioInfo = SoundsCollection.HydroPowerPlant;
                    break;
                case ID_INCINERATION_PLANT:
                    audioInfo = SoundsCollection.IncinerationPlant;
                    break;
                case ID_INDUSTRIAL:
                    audioInfo = SoundsCollection.Industrial;
                    break;
                case ID_METRO_STATION:
                    audioInfo = SoundsCollection.MetroStation;
                    break;
                case ID_NUCLEAR_POWER_PLANT:
                    audioInfo = SoundsCollection.NuclearPowerPlant;
                    break;
                case ID_POLICE_STATION:
                    audioInfo = SoundsCollection.PoliceStation;
                    break;
                case ID_POWER_PLANT_SMALL:
                    audioInfo = SoundsCollection.PowerPlantSmall;
                    break;
                case ID_SOLAR_POWER_PLANT:
                    audioInfo = SoundsCollection.SolarPowerPlant;
                    break;
                case ID_TRAIN_STATION:
                    audioInfo = SoundsCollection.TrainStation;
                    break;
                case ID_WATER_DRAIN_PIPE:
                    audioInfo = SoundsCollection.WaterDrainPipe;
                    break;
                case ID_WATER_PUMPING_STATION:
                    audioInfo = SoundsCollection.WaterPumpingStation;
                    break;
                case ID_WIND_TURBINE:
                    audioInfo = SoundsCollection.WindTurbine;
                    break;

                case ID_FIRE:
                    if (SoundsCollection.BuildingFire != null)
                    {
                        audioInfo = SoundsCollection.BuildingFire.m_audioInfo;
                    }
                    break;
                case ID_LEVELUP:
                    if (SoundsCollection.BuildingLevelUp != null)
                    {
                        audioInfo = SoundsCollection.BuildingLevelUp.m_audioInfo;
                    }
                    break;
            }

            return audioInfo;
        }
    }
}
