using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.SoundPack;
using AmbientSoundsTuner.SoundPack.Migration;
using UnityEngine;

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


        /// <summary>
        /// Gets the sound pack file audio from an audio info.
        /// </summary>
        /// <param name="sound">The sound.</param>
        /// <returns>The sound pack file audio.</returns>
        public static SoundPacksFileV1.Audio GetAudioInfo(SoundContainer sound)
        {
            if (sound.HasSound)
            {
                return GetAudioInfo(sound.AudioInfo);
            }
            return null;
        }

        /// <summary>
        /// Gets the sound pack file audio from an audio info.
        /// </summary>
        /// <param name="audioInfo">The original audio info.</param>
        /// <returns>The sound pack file audio.</returns>
        public static SoundPacksFileV1.Audio GetAudioInfo(AudioInfo audioInfo)
        {
            if (audioInfo != null)
            {
                var spfAudio = new SoundPacksFileV1.Audio()
                {
                    Name = audioInfo.name,
                    AudioInfo = new SoundPacksFileV1.AudioInfo()
                };

                Action<AudioInfo, SoundPacksFileV1.AudioInfo> backupAudioInfo = null;
                backupAudioInfo = new Action<AudioInfo, SoundPacksFileV1.AudioInfo>((ai, spf) =>
                {
                    spf.AudioClip = ai.m_clip;
                    spf.Volume = ai.m_volume;
                    spf.MaxVolume = Mathf.Max(ai.m_volume, 1);
                    spf.Pitch = ai.m_pitch;
                    spf.FadeLength = ai.m_fadeLength;
                    spf.IsLoop = ai.m_loop;
                    spf.Is3D = ai.m_is3D;
                    spf.IsRandomTime = ai.m_randomTime;

                    if (ai.m_variations != null)
                    {
                        spf.Variations = new SoundPacksFileV1.Variation[ai.m_variations.Length];
                        for (int i = 0; i < ai.m_variations.Length; i++)
                        {
                            spf.Variations[i] = new SoundPacksFileV1.Variation()
                            {
                                Probability = ai.m_variations[i].m_probability,
                                AudioInfo = new SoundPacksFileV1.AudioInfo()
                            };
                            backupAudioInfo(ai.m_variations[i].m_sound, spf.Variations[i].AudioInfo);
                        }
                    }
                });
                backupAudioInfo(audioInfo, spfAudio.AudioInfo);
                return spfAudio;
            }
            return null;
        }

        /// <summary>
        /// Sets the audio info to the data of a sound pack file audio.
        /// </summary>
        /// <param name="sound">The sound.</param>
        /// <param name="spfAudio">The sound pack file audio.</param>
        /// <returns>True if successful; false otherwise.</returns>
        public static bool SetAudioInfo(SoundContainer sound, SoundPacksFileV1.Audio spfAudio)
        {
            if (sound.HasSound)
            {
                return SetAudioInfo(sound.AudioInfo, spfAudio);
            }
            return false;
        }

        /// <summary>
        /// Sets the audio info to the data of a sound pack file audio.
        /// </summary>
        /// <param name="audioInfo">The audio info to set.</param>
        /// <param name="spfAudio">The sound pack file audio.</param>
        /// <returns>True if successful; false otherwise.</returns>
        public static bool SetAudioInfo(AudioInfo audioInfo, SoundPacksFileV1.Audio spfAudio)
        {
            if (audioInfo != null && spfAudio != null)
            {
                Action<AudioInfo, SoundPacksFileV1.AudioInfo> patchAudioInfo = null;
                int variation = 0;
                patchAudioInfo = new Action<AudioInfo, SoundPacksFileV1.AudioInfo>((ai, spf) =>
                {
                    if (spf.AudioClip != null)
                        ai.m_clip = spf.AudioClip;
                    else
                    {
                        WWW www = new WWW(new Uri(spf.Clip).AbsoluteUri);
                        ai.m_clip = www.GetAudioClip(true, false);
                    }
                    ai.m_volume = spf.Volume;
                    ai.m_pitch = spf.Pitch;
                    ai.m_fadeLength = spf.FadeLength;
                    ai.m_loop = spf.IsLoop;
                    ai.m_is3D = spf.Is3D;
                    ai.m_randomTime = spf.IsRandomTime;

                    variation++;
                    if (spf.Variations != null)
                    {
                        ai.m_variations = new AudioInfo.Variation[spf.Variations.Length];
                        for (int i = 0; i < spf.Variations.Length; i++)
                        {
                            ai.m_variations[i] = new AudioInfo.Variation()
                            {
                                m_probability = spf.Variations[i].Probability,
                                m_sound = ScriptableObject.CreateInstance<AudioInfo>()
                            };
                            patchAudioInfo(ai.m_variations[i].m_sound, spf.Variations[i].AudioInfo);
                        }
                    }
                    else
                    {
                        ai.m_variations = null;
                    }
                });
                patchAudioInfo(audioInfo, spfAudio.AudioInfo);
                return true;
            }
            return false;
        }
    }
}
