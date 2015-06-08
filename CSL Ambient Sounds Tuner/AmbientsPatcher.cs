using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonShared.Extensions;

namespace AmbientSoundsTuner
{
    /// <summary>
    /// This internal class contains various methods related to patching the ambient sounds.
    /// </summary>
    internal static class AmbientsPatcher
    {
        private static Dictionary<AudioManager.AmbientType, float> originalVolumes = new Dictionary<AudioManager.AmbientType, float>()
        {
            // These are the volumes as of 6/6/2015
            { AudioManager.AmbientType.World, 1 },
            { AudioManager.AmbientType.Forest, 1 },
            { AudioManager.AmbientType.Sea, 1 },
            { AudioManager.AmbientType.Stream, 1 },
            { AudioManager.AmbientType.Industrial, 1 },
            { AudioManager.AmbientType.Plaza, 1 },
            { AudioManager.AmbientType.Suburban, 1 },
            { AudioManager.AmbientType.City, 1 },
            { AudioManager.AmbientType.Agricultural, 1 }
        };

        /// <summary>
        /// Gets the original or backed up ambient volumes.
        /// </summary>
        public static Dictionary<AudioManager.AmbientType, float> OriginalVolumes { get { return originalVolumes; } }

        /// <summary>
        /// Backs up the original ambient volumes.
        /// </summary>
        /// <returns>The amount of ambient volumes that have been backed up.</returns>
        public static int BackupAmbientVolumes()
        {
            List<AudioManager.AmbientType> ambientTypes = new List<AudioManager.AmbientType>(originalVolumes.Keys);
            int success = 0;
            foreach (AudioManager.AmbientType ambientType in ambientTypes)
            {
                if (AudioManager.instance.m_properties.m_ambients.Length > (int)ambientType)
                {
                    originalVolumes[ambientType] = AudioManager.instance.m_properties.m_ambients[(int)ambientType].m_volume;
                    success++;
                }
                else
                {
                    Mod.Log.Info("Ambient sound '{0}' has not been found", ambientType);
                }
            }
            return success;
        }

        /// <summary>
        /// Patches the ambient volumes with the values defined by <see cref="Configuration"/>.
        /// </summary>
        /// <returns>The amount of ambient volumes that have been patched.</returns>
        public static int PatchAmbientVolumes()
        {
            List<AudioManager.AmbientType> ambientTypes = new List<AudioManager.AmbientType>(originalVolumes.Keys);
            int success = 0;
            foreach (AudioManager.AmbientType ambientType in ambientTypes)
            {
                if (AudioManager.instance.m_properties.m_ambients.Length > (int)ambientType)
                {
                    Mod.Settings.State.AmbientVolumes.TryGetValueOrDefault(ambientType, originalVolumes[ambientType], out AudioManager.instance.m_properties.m_ambients[(int)ambientType].m_volume);
                    success++;
                }
                else
                {
                    Mod.Log.Info("Ambient sound '{0}' has not been found", ambientType);
                }
            }
            return success;
        }

        /// <summary>
        /// Patches a specific ambient volume with a specified value.
        /// </summary>
        /// <param name="type">The ambient type to patch.</param>
        /// <param name="value">The volume value to set.</param>
        /// <returns>True if successful; false otherwise.</returns>
        public static bool PatchAmbientVolumeFor(AudioManager.AmbientType type, float value)
        {
            if (AudioManager.instance != null && AudioManager.instance.m_properties != null && AudioManager.instance.m_properties.m_ambients != null)
            {
                if (AudioManager.instance.m_properties.m_ambients.Length > (int)type && AudioManager.instance.m_properties.m_ambients[(int)type] != null)
                {
                    AudioManager.instance.m_properties.m_ambients[(int)type].m_volume = value;
                    return true;
                }
            }
            Logger.Info("Ambient sound '{0}' has not been found", type);
            return false;
        }

        /// <summary>
        /// Resets the ambient volumes to their original values.
        /// </summary>
        /// <returns>The amount of ambient volumes that have been reset.</returns>
        public static int ResetAmbientVolumes()
        {
            List<AudioManager.AmbientType> ambientTypes = new List<AudioManager.AmbientType>(originalVolumes.Keys);
            int success = 0;
            foreach (AudioManager.AmbientType ambientType in ambientTypes)
            {
                if (AudioManager.instance.m_properties.m_ambients.Length > (int)ambientType)
                {
                    originalVolumes.TryGetValue(ambientType, out AudioManager.instance.m_properties.m_ambients[(int)ambientType].m_volume);
                    success++;
                }
                else
                {
                    Mod.Log.Info("Ambient sound '{0}' has not been found", ambientType);
                }
            }
            return success;
        }
    }
}
