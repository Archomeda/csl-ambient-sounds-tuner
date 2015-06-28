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
        private const string ID_SEAGULL_INFO = "Seagull";
        private const string ID_SEAGULL_SCREAM = "Seagull Scream";
        private const string ID_COW_INFO = "Cow";
        private const string ID_PIG_INFO = "Pig";
        private const string ID_INCINERATION_PLANT_INFO = "Combustion Plant";
        private const string ID_NUCLEAR_POWER_PLANT_INFO = "Nuclear Power Plant";
        private const string ID_WIND_TURBINE_INFO = "Wind Turbine";
        private const string ID_SOLAR_POWER_PLANT_INFO = "Solar Power Plant";
        private const string ID_HYDRO_POWER_PLANT_INFO = "Dam Power House";
        private const string ID_ADVANCED_WIND_TURBINE_INFO = "Advanced Wind Turbine";
        private const string ID_COAL_POWER_PLANT_INFO = "Coal Power Plant";
        private const string ID_FUSION_POWER_PLANT_INFO = "Fusion Power Plant";

        /// <summary>
        /// Gets the ambient sounds; or null if they don't exist.
        /// </summary>
        public static AudioInfo[] Ambients
        {
            get
            {
                return AudioManager.instance.m_properties.m_ambients;
            }
        }

        private static EffectSounds effects = new EffectSounds();
        /// <summary>
        /// Gets the effect sounds.
        /// </summary>
        public static EffectSounds Effects
        {
            get
            {
                return effects;
            }
        }

        /// <summary>
        /// Gets the seagull scream sound effect; or null if it doesn't exist.
        /// </summary>
        public static SoundEffect SeagullScream
        {
            get
            {
                CitizenInfo seagullInfo = PrefabCollection<CitizenInfo>.FindLoaded(ID_SEAGULL_INFO);
                if (seagullInfo != null)
                {
                    MultiEffect effect = ((BirdAI)seagullInfo.m_citizenAI).m_randomEffect as MultiEffect;
                    if (effect != null)
                    {
                        return effect.m_effects.FirstOrDefault(e => e.m_effect.name == ID_SEAGULL_SCREAM).m_effect as SoundEffect;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the cow sound effect; or null if it doesn't exist.
        /// </summary>
        public static SoundEffect Cow
        {
            get
            {
                CitizenInfo cowInfo = PrefabCollection<CitizenInfo>.FindLoaded(ID_COW_INFO);
                if (cowInfo != null)
                {
                    return ((LivestockAI)cowInfo.m_citizenAI).m_randomEffect as SoundEffect;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the pig sound effect; or null if it doesn't exist.
        /// </summary>
        public static SoundEffect Pig
        {
            get
            {
                CitizenInfo pigInfo = PrefabCollection<CitizenInfo>.FindLoaded(ID_COW_INFO);
                if (pigInfo != null)
                {
                    return ((LivestockAI)pigInfo.m_citizenAI).m_randomEffect as SoundEffect;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the incineration plant audio info; or null if it doesn't exist.
        /// </summary>
        public static AudioInfo IncinerationPlant
        {
            get
            {
                return GetAudioInfoFromBuildingInfo(ID_INCINERATION_PLANT_INFO);
            }
        }

        /// <summary>
        /// Gets the nuclear power plant audio info; or null if it doesn't exist.
        /// </summary>
        public static AudioInfo NuclearPowerPlant
        {
            get
            {
                return GetAudioInfoFromBuildingInfo(ID_NUCLEAR_POWER_PLANT_INFO);
            }
        }

        /// <summary>
        /// Gets the wind turbine audio info; or null if it doesn't exist.
        /// </summary>
        public static AudioInfo WindTurbine
        {
            get
            {
                return GetAudioInfoFromBuildingInfo(ID_WIND_TURBINE_INFO);
            }
        }

        /// <summary>
        /// Gets the solar power plant audio info; or null if it doesn't exist.
        /// </summary>
        public static AudioInfo SolarPowerPlant
        {
            get
            {
                return GetAudioInfoFromBuildingInfo(ID_SOLAR_POWER_PLANT_INFO);
            }
        }

        /// <summary>
        /// Gets the hydro power plant audio info; or null if it doesn't exist.
        /// </summary>
        public static AudioInfo HydroPowerPlant
        {
            get
            {
                return GetAudioInfoFromBuildingInfo(ID_HYDRO_POWER_PLANT_INFO);
            }
        }

        /// <summary>
        /// Gets the hydro power plant audio info; or null if it doesn't exist.
        /// </summary>
        public static AudioInfo AdvancedWindTurbine
        {
            get
            {
                return GetAudioInfoFromBuildingInfo(ID_ADVANCED_WIND_TURBINE_INFO);
            }
        }

        /// <summary>
        /// Gets the coal/oil power plant audio info; or null if it doesn't exist.
        /// </summary>
        public static AudioInfo CoalOilPowerPlant
        {
            get
            {
                return GetAudioInfoFromBuildingInfo(ID_COAL_POWER_PLANT_INFO);
            }
        }

        /// <summary>
        /// Gets the fusion power plant audio info; or null if it doesn't exist.
        /// </summary>
        public static AudioInfo FusionPowerPlant
        {
            get
            {
                return GetAudioInfoFromBuildingInfo(ID_FUSION_POWER_PLANT_INFO);
            }
        }

        private static AudioInfo GetAudioInfoFromBuildingInfo(string id)
        {
            BuildingInfo info = PrefabCollection<BuildingInfo>.FindLoaded(id);
            if (info != null)
            {
                return info.m_customLoopSound;
            }
            return null;
        }

        /// <summary>
        /// A class that's basically a shortcut to generic effect sounds.
        /// </summary>
        public class EffectSounds
        {
            /// <summary>
            /// Gets an effect sound.
            /// </summary>
            /// <param name="id">The effect sound id.</param>
            /// <returns>The effect sound if it exists; null otherwise.</returns>
            public SoundEffect this[string id]
            {
                get
                {
                    EffectInfo effectInfo = EffectCollection.FindEffect(id);
                    SoundEffect soundEffect = effectInfo as SoundEffect;
                    if (soundEffect != null)
                    {
                        return soundEffect;
                    }
                    return null;
                }
            }
        }
    }
}
