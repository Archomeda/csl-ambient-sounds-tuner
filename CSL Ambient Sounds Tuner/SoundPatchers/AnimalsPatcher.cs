using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmbientSoundsTuner.SoundPatchers
{
    /// <summary>
    /// A class that can patch instances of animal sounds.
    /// </summary>
    public class AnimalsPatcher : SoundsInstancePatcher<string>
    {
        public const string ID_COW = "Cow";
        public const string ID_PIG = "Pig";
        public const string ID_SEAGULL = "Seagull";

        private const string ID_SEAGULL_SCREAM = "Seagull Scream";


        public override string[] Ids
        {
            get
            {
                return new[] {
                    ID_COW,
                    ID_PIG,
                    ID_SEAGULL
                };
            }
        }

        public override string AudioPrefixId
        {
            get { return "Animal"; }
        }

        public override SoundContainer GetSoundInstance(string id)
        {
            CitizenInfo info = null;

            // Get whitelisted prefabs only
            switch (id)
            {
                case ID_COW:
                case ID_PIG:
                case ID_SEAGULL:
                    info = PrefabCollection<CitizenInfo>.FindLoaded(id);
                    break;
            }

            // Get sound from prefab
            if (info != null)
            {
                switch (id)
                {
                    case ID_COW:
                    case ID_PIG:
                        return new SoundContainer(((LivestockAI)info.m_citizenAI).m_randomEffect as SoundEffect);

                    case ID_SEAGULL:
                        MultiEffect effect = ((BirdAI)info.m_citizenAI).m_randomEffect as MultiEffect;
                        if (effect != null)
                        {
                            return new SoundContainer(effect.m_effects.FirstOrDefault(e => e.m_effect.name == ID_SEAGULL_SCREAM).m_effect as SoundEffect);
                        }
                        break;
                }
            }

            return null;
        }
    }
}
