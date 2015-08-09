/*
The MIT License (MIT)

Copyright (c) 2015 Dimitri Slappendel

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AmbientSoundsTuner.Compatibility
{
    /// <summary>
    /// This static class contains patches for sirens that maintain compatibility with other mods as much as possible,
    /// while still patching and improving functionality of those sirens.
    /// </summary>
    /// 
    /// <remarks>
    /// Patches created by Archomeda, released under MIT license.
    /// 
    /// If you're looking for applying the same patch, please use my methods below.
    /// This will ensure that you don't break other mods when trying to create your own patch.
    /// </remarks>
    public static class SoundDuplicator
    {
        public const string EFFECT_POLICE_CAR_SIREN = "Police Car Siren";
        public const string EFFECT_SCOOTER_MOVEMENT = "Scooter Movement";
        public const string EFFECT_SCOOTER_SOUND = "Scooter Sound";
        public const string BUILDING_OIL_POWER_PLANT = "Oil Power Plant";
        public const string BUILDING_WATER_TREATMENT_PLANT = "Water Treatment Plant";

        public const string AUDIOINFO_POLICE_CAR_SIREN = "Siren Police Car";
        public const string AUDIOINFO_SCOOTER_ENGINE = "Scooter Engine";
        public const string AUDIOINFO_OIL_POWER_PLANT = "Building Oil Power Plant";
        public const string AUDIOINFO_WATER_TREATMENT_PLANT = "Building Water Treatment Plant";

        public enum PatchResult
        {
            Success,
            AlreadyPatched,
            NotFound,
        }

        /// <summary>
        /// This method has to be called before changing the police sirens.
        /// Currently, the game treats the police sirens the same as the ambulance sirens.
        /// This means that, when the ambulance siren gets changed, the police siren gets changed too, and vice versa.
        /// Here we patch it so they are both treated differently.
        /// </summary>
        public static PatchResult PatchPoliceSiren()
        {
            return DuplicateEffectAudioInfo(EFFECT_POLICE_CAR_SIREN, AUDIOINFO_POLICE_CAR_SIREN);
        }


        /// <summary>
        /// This method has to be called before changing the scooter sounds.
        /// Currently, the game treats the scooter sounds the same as the small car engine sounds.
        /// This means that, when the small car engine sound gets changed, the scooter sound gets changed too, and vice versa.
        /// Here we patch it so they are both treated differently.
        /// </summary>
        public static PatchResult PatchScooterSound()
        {
            MultiEffect scooterMovementEffect = EffectCollection.FindEffect(EFFECT_SCOOTER_MOVEMENT) as MultiEffect;
            if (scooterMovementEffect == null || scooterMovementEffect.m_effects == null)
            {
                return PatchResult.NotFound;
            }

            return DuplicateEffectAudioInfo(EFFECT_SCOOTER_SOUND, AUDIOINFO_SCOOTER_ENGINE, scooterMovementEffect);
        }

        /// <summary>
        /// This method has to be called before changing the oil power plant sounds.
        /// Currently, the game treats the oil power plant sounds the same as the coal power plant sounds.
        /// This means that, when the oil power plant sound gets changed, the coal power plant sound gets changed too, and vice versa.
        /// Here we patch it so they are both treated differently.
        /// </summary>
        public static PatchResult PatchOilPowerPlant()
        {
            return DuplicateBuildingAudioInfo(BUILDING_OIL_POWER_PLANT, AUDIOINFO_OIL_POWER_PLANT);
        }

        /// <summary>
        /// This method has to be called before changing the water treatment plant sounds.
        /// Currently, the game treats the water treatment plant sounds the same as the water drain pipe sounds.
        /// This means that, when the water treatment plant sound gets changed, the water drain pipe sound gets changed too, and vice versa.
        /// Here we patch it so they are both treated differently.
        /// </summary>
        public static PatchResult PatchWaterTreatmentPlant()
        {
            return DuplicateBuildingAudioInfo(BUILDING_WATER_TREATMENT_PLANT, AUDIOINFO_WATER_TREATMENT_PLANT);
        }

        private static PatchResult DuplicateEffectAudioInfo(string effectId, string audioInfoId, MultiEffect effectContainer = null)
        {
            SoundEffect soundEffect = null;
            if (effectContainer != null)
                soundEffect = effectContainer.m_effects.FirstOrDefault(e => e.m_effect.name == effectId).m_effect as SoundEffect;
            else
                soundEffect = EffectCollection.FindEffect(effectId) as SoundEffect;

            if (soundEffect == null || soundEffect.m_audioInfo == null)
                return PatchResult.NotFound;

            // Check if the AudioInfo object has our name, if not, we have to patch it.
            if (soundEffect.m_audioInfo.name != audioInfoId)
            {
                AudioInfo audioInfo = GameObject.Instantiate(soundEffect.m_audioInfo);
                audioInfo.name = audioInfoId;
                soundEffect.m_audioInfo = audioInfo;
                return PatchResult.Success;
            }
            return PatchResult.AlreadyPatched;
        }

        private static PatchResult DuplicateBuildingAudioInfo(string buildingId, string audioInfoId)
        {
            BuildingInfo building = PrefabCollection<BuildingInfo>.FindLoaded(buildingId);
            if (building == null || building.m_customLoopSound == null)
                return PatchResult.NotFound;

            // Check if the AudioInfo object has our name, if not, we have to patch it.
            if (building.m_customLoopSound.name != audioInfoId)
            {
                AudioInfo audioInfo = GameObject.Instantiate(building.m_customLoopSound);
                audioInfo.name = audioInfoId;
                building.m_customLoopSound = audioInfo;
                return PatchResult.Success;
            }
            return PatchResult.AlreadyPatched;
        }
    }
}
