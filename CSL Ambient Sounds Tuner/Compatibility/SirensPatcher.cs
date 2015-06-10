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
    public static class SirensPatcher
    {
        public const string EFFECT_POLICE_CAR_SIREN = "Police Car Siren";
        public const string AUDIOINFO_POLICE_CAR_SIREN = "Siren Police Car";

        public enum PatchResult
        {
            Success,
            AlreadyPatched,
            NotFound,
        }

        /// <summary>
        /// This method has to be called before changing the police sirens.
        /// Currently, the game treats the police sirens the same as the ambulance sirens.
        /// This means that, when the ambulance siren gets changed, the police siren gets changed to, and vice versa.
        /// Here we patch it so they are both treated differently.
        /// </summary>
        public static PatchResult PatchPoliceSiren()
        {
            SoundEffect policeSirenSoundEffect = EffectCollection.FindEffect(EFFECT_POLICE_CAR_SIREN) as SoundEffect;

            if (policeSirenSoundEffect == null || policeSirenSoundEffect.m_audioInfo == null)
            {
                return PatchResult.NotFound;
            }

            // Check if both AudioInfo objects are the same, if so, we have to patch it.
            if (policeSirenSoundEffect.m_audioInfo.name != AUDIOINFO_POLICE_CAR_SIREN)
            {
                AudioInfo policeSirenAudioInfo = GameObject.Instantiate(policeSirenSoundEffect.m_audioInfo);
                policeSirenAudioInfo.name = AUDIOINFO_POLICE_CAR_SIREN;
                policeSirenSoundEffect.m_audioInfo = policeSirenAudioInfo;
                return PatchResult.Success;
            }
            return PatchResult.AlreadyPatched;
        }
    }
}
