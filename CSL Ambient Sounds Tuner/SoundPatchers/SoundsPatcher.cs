using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmbientSoundsTuner.SoundPatchers
{
    /// <summary>
    /// A static class that contains various methods related to sounds patching.
    /// </summary>
    public static class SoundsPatcher
    {
        /// <summary>
        /// Gets the volume of a sound.
        /// </summary>
        /// <param name="sound">The sound.</param>
        /// <returns>The volume of the sound if it has one; otherwise null.</returns>
        public static float? GetVolume(SoundContainer sound)
        {
            if (sound.HasSound)
            {
                return GetVolume(sound.AudioInfo);
            }
            return null;
        }

        /// <summary>
        /// Gets the volume of a sound effect.
        /// </summary>
        /// <param name="effect">The sound effect.</param>
        /// <returns>The volume of the sound effect if it has one; otherwise null.</returns>
        public static float? GetVolume(SoundEffect effect)
        {
            if (effect != null)
            {
                return GetVolume(effect.m_audioInfo);
            }
            return null;
        }

        /// <summary>
        /// Gets the volume of an audio info.
        /// </summary>
        /// <param name="info">The audio info.</param>
        /// <returns>The volume of the audio info if it's not null; otherwise null.</returns>
        public static float? GetVolume(AudioInfo info)
        {
            if (info != null)
            {
                return info.m_volume;
            }
            return null;
        }

        /// <summary>
        /// Sets the volume of a sound.
        /// </summary>
        /// <param name="sound">The sound.</param>
        /// <param name="volume">The new volume.</param>
        /// <returns>True if successful; false otherwise.</returns>
        public static bool SetVolume(SoundContainer sound, float volume)
        {
            if (sound.HasSound)
            {
                return SetVolume(sound.AudioInfo, volume);
            }
            return false;
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
                return SetVolume(effect.m_audioInfo, volume);
            }
            return false;
        }

        /// <summary>
        /// Sets the volume of an audio info and its variations.
        /// </summary>
        /// <param name="info">The audio info.</param>
        /// <param name="volume">The new volume.</param>
        /// <returns>True if successful; false otherwise.</returns>
        public static bool SetVolume(AudioInfo info, float volume)
        {
            if (info != null)
            {
                info.m_volume = volume;
                for (int i = 0; i < info.m_variations.Length; i++)
                {
                    info.m_variations[i].m_sound.m_volume = volume;
                }
                return true;
            }
            return false;
        }
    }
}
