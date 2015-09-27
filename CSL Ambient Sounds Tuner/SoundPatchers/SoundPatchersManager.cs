using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.SoundPack;
using AmbientSoundsTuner.SoundPack.Migration;
using ColossalFramework;
using UnityEngine;

namespace AmbientSoundsTuner.SoundPatchers
{
    public class SoundPatchersManager : SingletonLite<SoundPatchersManager>
    {
        public SoundPatchersManager()
        {
            this.AmbientsPatcher = new AmbientsPatcher();
            this.AmbientsNightPatcher = new AmbientsNightPatcher();
            this.AnimalsPatcher = new AnimalsPatcher();
            this.BuildingsPatcher = new BuildingsPatcher();
            this.VehiclesPatcher = new VehiclesPatcher();
            this.MiscPatcher = new MiscPatcher();
        }

        public AmbientsPatcher AmbientsPatcher { get; private set; }

        public AmbientsNightPatcher AmbientsNightPatcher { get; private set; }

        public AnimalsPatcher AnimalsPatcher { get; private set; }

        public BuildingsPatcher BuildingsPatcher { get; private set; }

        public VehiclesPatcher VehiclesPatcher { get; private set; }

        public MiscPatcher MiscPatcher { get; private set; }

        public SoundsInstancePatcher<T> GetPatcherById<T>(string id)
        {
            switch (id)
            {
                case "Ambient": return this.AmbientsPatcher as SoundsInstancePatcher<T>;
                case "AmbientNight": return this.AmbientsNightPatcher as SoundsInstancePatcher<T>;
                case "Animal": return this.AnimalsPatcher as SoundsInstancePatcher<T>;
                case "Building": return this.BuildingsPatcher as SoundsInstancePatcher<T>;
                case "Vehicle": return this.VehiclesPatcher as SoundsInstancePatcher<T>;
                case "Misc": return this.MiscPatcher as SoundsInstancePatcher<T>;
            }
            return null;
        }


        public static AudioInfo GetAudioInfoFromBuildingInfo(string id)
        {
            BuildingInfo building = PrefabCollection<BuildingInfo>.FindLoaded(id);
            if (building != null)
            {
                return building.m_customLoopSound;
            }
            return null;
        }

        public static AudioInfo GetFirstAudioInfoFromBuildingInfos(IEnumerable<string> ids)
        {
            foreach (string id in ids)
            {
                AudioInfo result = GetAudioInfoFromBuildingInfo(id);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        public static AudioInfo GetAudioInfoFromArray(AudioInfo[] array, string id)
        {
            if (array != null)
            {
                return array.FirstOrDefault(a => a != null && a.name == id);
            }
            return null;
        }

        public static SoundEffect GetSubEffectFromMultiEffect(MultiEffect multiEffect, string id)
        {
            if (multiEffect != null && multiEffect.m_effects != null)
            {
                var subEffect = multiEffect.m_effects.FirstOrDefault(e => e.m_effect.name == id);
                if (subEffect.m_effect != null)
                {
                    return subEffect.m_effect as SoundEffect;
                }
            }
            return null;
        }


        public SoundPacksFile GetCurrentSoundSettingsAsSoundPack()
        {
            SoundPacksFile file = new SoundPacksFile();
            file.SoundPacks = new SoundPacksFileV1.SoundPack[] { new SoundPacksFileV1.SoundPack() };

            var soundPack = file.SoundPacks[0];
            soundPack.Name = "Default";
            soundPack.Ambients = new SoundPacksFileV1.Audio[this.AmbientsPatcher.Ids.Length];
            soundPack.AmbientsNight = new SoundPacksFileV1.Audio[this.AmbientsNightPatcher.Ids.Length];
            soundPack.Animals = new SoundPacksFileV1.Audio[this.AnimalsPatcher.Ids.Length];
            soundPack.Buildings = new SoundPacksFileV1.Audio[this.BuildingsPatcher.Ids.Length];
            soundPack.Vehicles = new SoundPacksFileV1.Audio[this.VehiclesPatcher.Ids.Length];
            soundPack.Miscs = new SoundPacksFileV1.Audio[this.MiscPatcher.Ids.Length];

            Func<AudioInfo, SoundPacksFileV1.AudioInfo> convertAudioInfo = null;
            convertAudioInfo = new Func<AudioInfo, SoundPacksFileV1.AudioInfo>(ai =>
            {
                var audioInfo = new SoundPacksFileV1.AudioInfo()
                {
                    Clip = "NOFILE",
                    Volume = ai.m_volume,
                    MaxVolume = Mathf.Max(ai.m_volume, 1),
                    Pitch = ai.m_pitch,
                    FadeLength = ai.m_fadeLength,
                    IsLoop = ai.m_loop,
                    Is3D = ai.m_is3D,
                    IsRandomTime = ai.m_randomTime,
                };

                if (ai.m_variations.Length > 0)
                {
                    audioInfo.Variations = new SoundPacksFileV1.Variation[ai.m_variations.Length];
                    for (int i = 0; i < audioInfo.Variations.Length; i++)
                    {
                        audioInfo.Variations[i] = new SoundPacksFileV1.Variation()
                        {
                            Probability = ai.m_variations[i].m_probability,
                            AudioInfo = convertAudioInfo(ai.m_variations[i].m_sound)
                        };
                    }
                }

                return audioInfo;
            });

            var convertToFile = new Func<string, AudioInfo, SoundPacksFileV1.Audio>((type, ai) =>
            {
                return new SoundPacksFileV1.Audio()
                {
                    Name = ai.name,
                    Type = type,
                    AudioInfo = convertAudioInfo(ai)
                };
            });

            // Ambients
            for (int i = 0; i < soundPack.Ambients.Length; i++)
            {
                var sound = this.AmbientsPatcher.GetSoundInstance(this.AmbientsPatcher.Ids[i]);
                if (sound != null && sound.HasSound)
                {
                    soundPack.Ambients[i] = convertToFile(this.AmbientsPatcher.Ids[i].ToString(), sound.AudioInfo);
                }
            }

            // Ambients Night
            for (int i = 0; i < soundPack.AmbientsNight.Length; i++)
            {
                var sound = this.AmbientsNightPatcher.GetSoundInstance(this.AmbientsNightPatcher.Ids[i]);
                if (sound != null && sound.HasSound)
                {
                    soundPack.AmbientsNight[i] = convertToFile(this.AmbientsNightPatcher.Ids[i].ToString(), sound.AudioInfo);
                }
            }

            // Animals
            for (int i = 0; i < soundPack.Animals.Length; i++)
            {
                var sound = this.AnimalsPatcher.GetSoundInstance(this.AnimalsPatcher.Ids[i]);
                if (sound != null && sound.HasSound)
                {
                    soundPack.Animals[i] = convertToFile(this.AnimalsPatcher.Ids[i].ToString(), sound.AudioInfo);
                }
            }

            // Buildings
            for (int i = 0; i < soundPack.Buildings.Length; i++)
            {
                var sound = this.BuildingsPatcher.GetSoundInstance(this.BuildingsPatcher.Ids[i]);
                if (sound != null && sound.HasSound)
                {
                    soundPack.Buildings[i] = convertToFile(this.BuildingsPatcher.Ids[i].ToString(), sound.AudioInfo);
                }
            }

            // Vehicles
            for (int i = 0; i < soundPack.Vehicles.Length; i++)
            {
                var sound = this.VehiclesPatcher.GetSoundInstance(this.VehiclesPatcher.Ids[i]);
                if (sound != null && sound.HasSound)
                {
                    soundPack.Vehicles[i] = convertToFile(this.VehiclesPatcher.Ids[i].ToString(), sound.AudioInfo);
                }
            }

            // Miscs
            for (int i = 0; i < soundPack.Miscs.Length; i++)
            {
                var sound = this.MiscPatcher.GetSoundInstance(this.MiscPatcher.Ids[i]);
                if (sound != null && sound.HasSound)
                {
                    soundPack.Miscs[i] = convertToFile(this.MiscPatcher.Ids[i].ToString(), sound.AudioInfo);
                }
            }

            return file;
        }
    }
}
