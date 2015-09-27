using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.SoundPack;
using CommonShared.Extensions;

namespace AmbientSoundsTuner.SoundPatchers
{
    /// <summary>
    /// A class that can patch instances of ambient sounds at night.
    /// </summary>
    public class AmbientsNightPatcher : SoundsInstancePatcher<AudioManager.AmbientType>
    {
        public override AudioManager.AmbientType[] Ids
        {
            get
            {
                return new[] {
                    AudioManager.AmbientType.Agricultural,
                    AudioManager.AmbientType.City,
                    AudioManager.AmbientType.Forest,
                    AudioManager.AmbientType.Leisure,
                    AudioManager.AmbientType.Suburban,
                    AudioManager.AmbientType.Tourist
                };
            }
        }

        public override string AudioPrefixId
        {
            get { return "AmbientNight"; }
        }

        public override SoundContainer GetSoundInstance(AudioManager.AmbientType id)
        {
            if (AudioManager.instance.m_properties != null && AudioManager.instance.m_properties.m_ambientsNight != null
                && AudioManager.instance.m_properties.m_ambientsNight.Length > (int)id)
            {
                return new SoundContainer(AudioManager.instance.m_properties.m_ambientsNight[(int)id]);
            }
            return null;
        }
    }
}
