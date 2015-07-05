using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.SoundPack;
using CommonShared.Extensions;

namespace AmbientSoundsTuner.SoundPatchers
{
    /// <summary>
    /// A class that can patch instances of ambient sounds.
    /// </summary>
    public class AmbientsPatcher : SoundsInstancePatcher<AudioManager.AmbientType>
    {
        public override AudioManager.AmbientType[] Ids
        {
            get
            {
                return new[] {
                    AudioManager.AmbientType.Agricultural,
                    AudioManager.AmbientType.City,
                    AudioManager.AmbientType.Forest,
                    AudioManager.AmbientType.Industrial,
                    AudioManager.AmbientType.Plaza,
                    AudioManager.AmbientType.Sea,
                    AudioManager.AmbientType.Stream,
                    AudioManager.AmbientType.Suburban,
                    AudioManager.AmbientType.World
                };
            }
        }

        public override string AudioPrefixId
        {
            get { return "Ambient"; }
        }

        public override SoundContainer GetSoundInstance(AudioManager.AmbientType id)
        {
            if (AudioManager.instance.m_properties != null && AudioManager.instance.m_properties.m_ambients != null
                && AudioManager.instance.m_properties.m_ambients.Length > (int)id)
            {
                return new SoundContainer(AudioManager.instance.m_properties.m_ambients[(int)id]);
            }
            return null;
        }
    }
}
