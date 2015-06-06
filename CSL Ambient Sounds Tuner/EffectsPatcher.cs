using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.Utils;
using UnityEngine;

namespace AmbientSoundsTuner
{
    /// <summary>
    /// This internal class contains various methods related to patching the effect sounds.
    /// </summary>
    internal static class EffectsPatcher
    {
        private static Dictionary<string, float> originalVolumes = new Dictionary<string, float>()
        {
            // These are the volumes as of 6/6/2015
            { "Aircraft Movement", 0.5f },
            { "Ambulance Siren", 1 },
            { "Fire Truck Siren", 3 },
            { "Large Car Movement", 1.5f },
            { "Metro Movement", 0.5f },
            { "Police Car Siren", 1 },
            { "Small Car Movement", 1.5f },
            { "Train Movement", 0.5f },
            { "Transport Arrive", 1 }
        };

        /// <summary>
        /// Gets the original or backed up effect volumes.
        /// </summary>
        public static Dictionary<string, float> OriginalVolumes { get { return originalVolumes; } }

        /// <summary>
        /// Backs up the original effect volumes.
        /// </summary>
        /// <returns>The amount of effect volumes that have been backed up.</returns>
        public static int BackupEffectVolumes()
        {
            List<string> effectNames = new List<string>(originalVolumes.Keys);
            int success = 0;
            foreach (string effectName in effectNames)
            {
                EffectInfo effectInfo = EffectCollection.FindEffect(effectName);
                SoundEffect soundEffect = effectInfo as SoundEffect;
                if (soundEffect != null)
                {
                    originalVolumes[effectName] = soundEffect.m_audioInfo.m_volume;
                    success++;
                }
                else
                {
                    Logger.Info("Effect sound '{0}' has not been found", effectName);
                }
            }
            return success;
        }

        /// <summary>
        /// Patches the effect volumes with the values defined by <see cref="Configuration"/>.
        /// </summary>
        /// <returns>The amount of effect volumes that have been patched.</returns>
        public static int PatchEffectVolumes()
        {
            List<string> effectNames = new List<string>(originalVolumes.Keys);
            int success = 0;
            foreach (string effectName in effectNames)
            {
                EffectInfo effectInfo = EffectCollection.FindEffect(effectName);
                SoundEffect soundEffect = effectInfo as SoundEffect;
                if (soundEffect != null)
                {
                    Configuration.Instance.State.EffectVolumes.TryGetValue(effectName, out soundEffect.m_audioInfo.m_volume);
                    success++;
                }
                else
                {
                    Logger.Info("Effect sound '{0}' has not been found", effectName);
                }
            }
            return success;
        }

        /// <summary>
        /// Resets the effect volumes to their original values.
        /// </summary>
        /// <returns>The amount of effect volumes that have been reset.</returns>
        public static int ResetEffectVolumes()
        {
            List<string> effectNames = new List<string>(originalVolumes.Keys);
            int success = 0;
            foreach (string effectName in effectNames)
            {
                EffectInfo effectInfo = EffectCollection.FindEffect(effectName);
                SoundEffect soundEffect = effectInfo as SoundEffect;
                if (soundEffect != null)
                {
                    originalVolumes.TryGetValue(effectName, out soundEffect.m_audioInfo.m_volume);
                    success++;
                }
                else
                {
                    Logger.Info("Effect sound '{0}' has not been found", effectName);
                }
            }
            return success;
        }


    }
}
