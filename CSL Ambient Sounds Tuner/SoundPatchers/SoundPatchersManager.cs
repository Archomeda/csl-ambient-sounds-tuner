using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.SoundPack;
using AmbientSoundsTuner.SoundPack.Migration;
using AmbientSoundsTuner.SoundPatchers.Sounds;
using ColossalFramework;
using UnityEngine;

namespace AmbientSoundsTuner.SoundPatchers
{
    public class SoundPatchersManager : SingletonLite<SoundPatchersManager>
    {
        public SoundPatchersManager()
        {
            this.Sounds = new Dictionary<string, ISound>();
        }

        public IDictionary<string, ISound> Sounds { get; protected set; }

        public void InitializeSounds()
        {
            foreach (var soundClass in this.GetType().Assembly.GetTypes().Where(p => typeof(ISound).IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract))
            {
                ISound sound = (ISound)Activator.CreateInstance(soundClass);
                this.Sounds[string.Format("{0}.{1}", sound.CategoryId, sound.Id)] = sound;
                Mod.Instance.Log.Debug("Initialized sound {0}.{1}", sound.CategoryId, sound.Id);
            }
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
            var ambients = new List<SoundPacksFileV1.Audio>();
            var ambientsNight = new List<SoundPacksFileV1.Audio>();
            var animals = new List<SoundPacksFileV1.Audio>();
            var buildings = new List<SoundPacksFileV1.Audio>();
            var vehicles = new List<SoundPacksFileV1.Audio>();
            var miscs = new List<SoundPacksFileV1.Audio>();

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

            foreach (ISound sound in SoundPatchersManager.instance.Sounds.Values)
            {
                var soundInstance = sound.GetSoundInstance();
                if (soundInstance != null && soundInstance.HasSound)
                {
                    var audioFile = convertToFile(sound.Id, soundInstance.AudioInfo);
                    switch (sound.CategoryId)
                    {
                        case "Ambient":
                            ambients.Add(audioFile);
                            break;
                        case "AmbientNight":
                            ambientsNight.Add(audioFile);
                            break;
                        case "Animal":
                            animals.Add(audioFile);
                            break;
                        case "Building":
                            buildings.Add(audioFile);
                            break;
                        case "Vehicle":
                            vehicles.Add(audioFile);
                            break;
                        case "Misc":
                            miscs.Add(audioFile);
                            break;
                    }
                }
            }

            soundPack.Ambients = ambients.ToArray();
            soundPack.AmbientsNight = ambientsNight.ToArray();
            soundPack.Animals = animals.ToArray();
            soundPack.Buildings = buildings.ToArray();
            soundPack.Vehicles = vehicles.ToArray();
            soundPack.Miscs = miscs.ToArray();

            return file;
        }
    }
}
