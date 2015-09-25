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
        public const string ID_AIRCRAFT_SOUND = "Aircraft Sound";
        public const string ID_AMBULANCE_SIREN = "Ambulance Siren";
        public const string ID_FIRE_TRUCK_SIREN = "Fire Truck Siren";
        public const string ID_LARGE_CAR_SOUND = "Large Car Sound";
        public const string ID_METRO_MOVEMENT = "Metro Movement";
        public const string ID_POLICE_CAR_SIREN = Compatibility.SoundDuplicator.EFFECT_POLICE_CAR_SIREN;
        public const string ID_SCOOTER_SOUND = Compatibility.SoundDuplicator.EFFECT_SCOOTER_SOUND;
        public const string ID_SMALL_CAR_SOUND = "Small Car Sound";
        public const string ID_TRAIN_MOVEMENT = "Train Movement";
        public const string ID_TRANSPORT_ARRIVE = "Transport Arrive";


        public VehiclesPatcher()
            : base()
        {
            this.DefaultVolumes.Add(ID_AIRCRAFT_SOUND, 0.5f);
            this.DefaultVolumes.Add(ID_FIRE_TRUCK_SIREN, 3);
            this.DefaultVolumes.Add(ID_LARGE_CAR_SOUND, 1.5f);
            this.DefaultVolumes.Add(ID_METRO_MOVEMENT, 0.5f);
            this.DefaultVolumes.Add(ID_SCOOTER_SOUND, 1.5f);
            this.DefaultVolumes.Add(ID_SMALL_CAR_SOUND, 1.5f);
            this.DefaultVolumes.Add(ID_TRAIN_MOVEMENT, 0.5f);

            this.DefaultMaxVolumes.Add(ID_FIRE_TRUCK_SIREN, 3);
            this.DefaultMaxVolumes.Add(ID_LARGE_CAR_SOUND, 1.5f);
            this.DefaultMaxVolumes.Add(ID_SCOOTER_SOUND, 1.5f);
            this.DefaultMaxVolumes.Add(ID_SMALL_CAR_SOUND, 1.5f);

        }

        public override string[] Ids
        {
            get
            {
                return new[] {
                    ID_AIRCRAFT_SOUND,
                    ID_AMBULANCE_SIREN,
                    ID_FIRE_TRUCK_SIREN,
                    ID_LARGE_CAR_SOUND,
                    ID_METRO_MOVEMENT,
                    ID_POLICE_CAR_SIREN,
                    ID_SCOOTER_SOUND,
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

            switch (id)
            {
                case ID_AIRCRAFT_SOUND:
                case ID_AMBULANCE_SIREN:
                case ID_FIRE_TRUCK_SIREN:
                case ID_LARGE_CAR_SOUND:
                case ID_METRO_MOVEMENT:
                case ID_POLICE_CAR_SIREN:
                case ID_SCOOTER_SOUND:
                case ID_SMALL_CAR_SOUND:
                case ID_TRAIN_MOVEMENT:
                case ID_TRANSPORT_ARRIVE:
                    soundEffect = EffectCollection.FindEffect(id) as SoundEffect;
                    if (soundEffect != null)
                        return new SoundContainer(soundEffect);
                    break;
            }

            return null;
        }
    }
}
