using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.SoundPack;

namespace AmbientSoundsTuner.SoundPatchers
{
    /// <summary>
    /// A class that can patch instances of vehicle sounds.
    /// </summary>
    public class VehiclesPatcher : SoundsInstancePatcher<string>
    {
        public const string ID_AIRCRAFT_MOVEMENT = "Aircraft Movement";
        public const string ID_AMBULANCE_SIREN = "Ambulance Siren";
        public const string ID_FIRE_TRUCK_SIREN = "Fire Truck Siren";
        private const string ID_LARGE_CAR_MOVEMENT = "Large Car Movement";
        public const string ID_LARGE_CAR_SOUND = "Large Car Sound";
        public const string ID_METRO_MOVEMENT = "Metro Movement";
        public const string ID_POLICE_CAR_SIREN = "Police Car Siren";
        private const string ID_SMALL_CAR_MOVEMENT = "Small Car Movement";
        public const string ID_SMALL_CAR_SOUND = "Small Car Sound";
        public const string ID_TRAIN_MOVEMENT = "Train Movement";
        public const string ID_TRANSPORT_ARRIVE = "Transport Arrive";


        public VehiclesPatcher()
            : base()
        {
            this.DefaultVolumes.Add(ID_AIRCRAFT_MOVEMENT, 0.5f);
            this.DefaultVolumes.Add(ID_FIRE_TRUCK_SIREN, 3);
            this.DefaultVolumes.Add(ID_LARGE_CAR_SOUND, 1.5f);
            this.DefaultVolumes.Add(ID_METRO_MOVEMENT, 0.5f);
            this.DefaultVolumes.Add(ID_SMALL_CAR_SOUND, 1.5f);
            this.DefaultVolumes.Add(ID_TRAIN_MOVEMENT, 0.5f);

            this.DefaultMaxVolumes.Add(ID_FIRE_TRUCK_SIREN, 3);
            this.DefaultMaxVolumes.Add(ID_LARGE_CAR_SOUND, 1.5f);
            this.DefaultMaxVolumes.Add(ID_SMALL_CAR_SOUND, 1.5f);

        }

        public override string[] Ids
        {
            get
            {
                return new[] {
                    ID_AIRCRAFT_MOVEMENT,
                    ID_AMBULANCE_SIREN,
                    ID_FIRE_TRUCK_SIREN,
                    ID_LARGE_CAR_SOUND,
                    ID_METRO_MOVEMENT,
                    ID_POLICE_CAR_SIREN,
                    ID_SMALL_CAR_SOUND,
                    ID_TRAIN_MOVEMENT,
                    ID_TRANSPORT_ARRIVE
                };
            }
        }

        public override string AudioPrefixId
        {
            get { return "Vehicle"; }
        }

        public override SoundContainer GetSoundInstance(string id)
        {
            SoundEffect soundEffect = null;
            MultiEffect multiEffect = null;

            switch (id)
            {
                case ID_AIRCRAFT_MOVEMENT:
                case ID_AMBULANCE_SIREN:
                case ID_FIRE_TRUCK_SIREN:
                case ID_METRO_MOVEMENT:
                case ID_POLICE_CAR_SIREN:
                case ID_TRAIN_MOVEMENT:
                case ID_TRANSPORT_ARRIVE:
                    soundEffect = EffectCollection.FindEffect(id) as SoundEffect;
                    if (soundEffect != null)
                    {
                        return new SoundContainer(soundEffect);
                    }
                    break;

                case ID_SMALL_CAR_SOUND:
                    multiEffect = EffectCollection.FindEffect(ID_SMALL_CAR_MOVEMENT) as MultiEffect;
                    break;

                case ID_LARGE_CAR_SOUND:
                    multiEffect = EffectCollection.FindEffect(ID_LARGE_CAR_MOVEMENT) as MultiEffect;
                    break;
            }

            if (multiEffect != null)
            {
                return new SoundContainer(multiEffect.m_effects.FirstOrDefault(e => e.m_effect != null && e.m_effect.name == id).m_effect as SoundEffect);
            }

            return null;
        }
    }
}
