using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmbientSoundsTuner
{
    /// <summary>
    /// A static class that contains various methods related to sounds patching.
    /// </summary>
    public static class SoundsPatcher
    {
        /// <summary>
        /// Gets the volume of a sound effect.
        /// </summary>
        /// <param name="effect">The sound effect.</param>
        /// <returns>The volume of the sound effect if it has one; otherwise null.</returns>
        public static float? GetVolume(SoundEffect effect)
        {
            if (effect != null)
            {
                AudioInfo info = effect.m_audioInfo;
                if (info != null)
                {
                    return info.m_volume;
                }
            }

            return null;
        }

        /// <summary>
        /// Sets the volume of a sound effect.
        /// </summary>
        /// <param name="effect">The sound effect.</param>
        /// <param name="volume">The new volume.</param>
        /// <returns>True if successful; false otherwise.</returns>
        public static bool SetVolume(SoundEffect effect, float volume)
        {
            if (effect != null)
            {
                AudioInfo info = effect.m_audioInfo;
                if (info != null)
                {
                    info.m_volume = volume;
                    for (int i = 0; i < info.m_variations.Length; i++)
                    {
                        info.m_variations[i].m_sound.m_volume = volume;
                    }
                    return true;
                }
            }

            return false;
        }
    }
}
