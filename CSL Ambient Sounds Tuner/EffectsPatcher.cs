using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonShared.Extensions;

namespace AmbientSoundsTuner
{
    /// <summary>
    /// This internal class contains various methods related to patching the effect sounds.
    /// </summary>
    internal static class EffectsPatcher
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

        private static Dictionary<string, float> originalVolumes = new Dictionary<string, float>()
        {
            // These are the volumes as of 6/6/2015
            { ID_AIRCRAFT_MOVEMENT, 0.5f },
            { ID_AMBULANCE_SIREN, 1 },
            { ID_FIRE_TRUCK_SIREN, 3 },
            { ID_LARGE_CAR_MOVEMENT, 1.5f },
            { ID_METRO_MOVEMENT, 0.5f },
            { ID_POLICE_CAR_SIREN, 1 },
            { ID_SMALL_CAR_MOVEMENT, 1.5f },
            { ID_TRAIN_MOVEMENT, 0.5f },
            { ID_TRANSPORT_ARRIVE, 1 }
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
                    Mod.Log.Info("Effect sound '{0}' has not been found", effectName);
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
                    Mod.Settings.State.EffectVolumes.TryGetValueOrDefault(effectName, originalVolumes[effectName], out soundEffect.m_audioInfo.m_volume);
                    success++;
                }
                else
                {
                    Mod.Log.Info("Effect sound '{0}' has not been found", effectName);
                }
            }
            return success;
        }

        /// <summary>
        /// Patches a specific effect volume with a specified value.
        /// </summary>
        /// <param name="name">The effect name to patch.</param>
        /// <param name="value">The volume value to set.</param>
        /// <returns>True if successful; false otherwise.</returns>
        public static bool PatchEffectVolumeFor(string name, float value)
        {
            EffectInfo effectInfo = EffectCollection.FindEffect(name);
            SoundEffect soundEffect = effectInfo as SoundEffect;
            if (soundEffect != null)
            {
                soundEffect.m_audioInfo.m_volume = value;
                return true;
            }
            Logger.Info("Effect sound '{0}' has not been found", name);
            return false;
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
                    Mod.Log.Info("Effect sound '{0}' has not been found", effectName);
                }
            }
            return success;
        }
    }
}
