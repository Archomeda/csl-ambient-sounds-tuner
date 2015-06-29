using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmbientSoundsTuner.SoundPatchers
{
    /// <summary>
    /// A wrapper class to improve the structure of the different sound instances.
    /// </summary>
    public class SoundContainer
    {
        /// <summary>
        /// Creates a new empty container.
        /// </summary>
        public SoundContainer()
        {
            this.HasSound = false;
        }

        /// <summary>
        /// Creates a new container from a <see cref="SoundEffect"/>.
        /// </summary>
        /// <param name="soundEffect">The sound effect object.</param>
        public SoundContainer(SoundEffect soundEffect)
        {
            if (soundEffect != null)
            {
                this.SoundEffect = soundEffect;
                this.AudioInfo = soundEffect.m_audioInfo;
                this.HasSound = true;
            }
            else
            {
                this.HasSound = false;
            }
        }

        /// <summary>
        /// Creates a new container from an <see cref="AudioInfo"/>.
        /// </summary>
        /// <param name="audioInfo">The audio info object.</param>
        public SoundContainer(AudioInfo audioInfo)
        {
            if (audioInfo != null)
            {
                this.AudioInfo = audioInfo;
                this.HasSound = true;
            }
            else
            {
                this.HasSound = false;
            }
        }

        /// <summary>
        /// Gets the <see cref="SoundEffect"/>.
        /// This value can be null if no SoundEffect is available.
        /// </summary>
        public SoundEffect SoundEffect { get; private set; }

        /// <summary>
        /// Gets the <see cref="AudioInfo"/>.
        /// </summary>
        public AudioInfo AudioInfo { get; private set; }

        /// <summary>
        /// Gets whether this container contains sounds.
        /// </summary>
        public bool HasSound { get; private set; }
    }
}
