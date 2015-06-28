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
        public const string ID_NUCLEAR_POWER_PLANT = "Nuclear Power Plant";
        public const string ID_WIND_TURBINE = "Wind Turbine";
        public const string ID_SOLAR_POWER_PLANT = "Solar Power Plant";
        public const string ID_HYDRO_POWER_PLANT = "Hydro Power Plant";
        public const string ID_ADVANCED_WIND_TURBINE = "Advanced Wind Turbine";
        public const string ID_COAL_OIL_POWER_PLANT = "Coal/Oil Power Plant";
        public const string ID_FUSION_POWER_PLANT = "Fusion Power Plant";

        public BuildingsPatcher()
            : base()
        {
            this.DefaultVolumes.Add(ID_INCINERATION_PLANT, 1);
            this.DefaultVolumes.Add(ID_NUCLEAR_POWER_PLANT, 1);
            this.DefaultVolumes.Add(ID_WIND_TURBINE, 1);
            this.DefaultVolumes.Add(ID_SOLAR_POWER_PLANT, 1);
            this.DefaultVolumes.Add(ID_HYDRO_POWER_PLANT, 1);
            this.DefaultVolumes.Add(ID_ADVANCED_WIND_TURBINE, 1);
            this.DefaultVolumes.Add(ID_COAL_OIL_POWER_PLANT, 1);
            this.DefaultVolumes.Add(ID_FUSION_POWER_PLANT, 4);
        }

        protected override AudioInfo GetAudioInfoById(string id)
        {
            AudioInfo audioInfo = null;
            switch (id)
            {
                case ID_INCINERATION_PLANT:
                    audioInfo = SoundsCollection.IncinerationPlant;
                    break;
                case ID_NUCLEAR_POWER_PLANT:
                    audioInfo = SoundsCollection.NuclearPowerPlant;
                    break;
                case ID_WIND_TURBINE:
                    audioInfo = SoundsCollection.WindTurbine;
                    break;
                case ID_SOLAR_POWER_PLANT:
                    audioInfo = SoundsCollection.SolarPowerPlant;
                    break;
                case ID_HYDRO_POWER_PLANT:
                    audioInfo = SoundsCollection.HydroPowerPlant;
                    break;
                case ID_ADVANCED_WIND_TURBINE:
                    audioInfo = SoundsCollection.AdvancedWindTurbine;
                    break;
                case ID_COAL_OIL_POWER_PLANT:
                    audioInfo = SoundsCollection.CoalOilPowerPlant;
                    break;
                case ID_FUSION_POWER_PLANT:
                    audioInfo = SoundsCollection.FusionPowerPlant;
                    break;
            }

            return audioInfo;
        }
    }
}
