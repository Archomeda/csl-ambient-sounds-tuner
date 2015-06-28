using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmbientSoundsTuner
{
    /// <summary>
    /// A static class that holds references to various sounds within the game.
    /// </summary>
    public static class SoundsCollection
    {
        private const string ID_SEAGULL_INFO = "Seagull";
        private const string ID_SEAGULL_SCREAM = "Seagull Scream";

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
