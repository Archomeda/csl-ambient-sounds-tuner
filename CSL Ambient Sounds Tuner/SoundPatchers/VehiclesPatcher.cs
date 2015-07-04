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
        public VehiclesPatcher()
        {
            this.DefaultVolumes.Add(SoundsCollection.VehicleSounds.ID_AIRCRAFT_MOVEMENT, 0.5f);
            this.DefaultVolumes.Add(SoundsCollection.VehicleSounds.ID_FIRE_TRUCK_SIREN, 3);
            this.DefaultVolumes.Add(SoundsCollection.VehicleSounds.ID_LARGE_CAR_SOUND, 1.5f);
            this.DefaultVolumes.Add(SoundsCollection.VehicleSounds.ID_METRO_MOVEMENT, 0.5f);
            this.DefaultVolumes.Add(SoundsCollection.VehicleSounds.ID_SMALL_CAR_SOUND, 1.5f);
            this.DefaultVolumes.Add(SoundsCollection.VehicleSounds.ID_TRAIN_MOVEMENT, 0.5f);

            this.DefaultMaxVolumes.Add(SoundsCollection.VehicleSounds.ID_FIRE_TRUCK_SIREN, 3);
            this.DefaultMaxVolumes.Add(SoundsCollection.VehicleSounds.ID_LARGE_CAR_SOUND, 1.5f);
            this.DefaultMaxVolumes.Add(SoundsCollection.VehicleSounds.ID_SMALL_CAR_SOUND, 1.5f);

        }

        public override string[] Ids
        {
            get
            {
                return new[] {
                    SoundsCollection.VehicleSounds.ID_AIRCRAFT_MOVEMENT,
                    SoundsCollection.VehicleSounds.ID_AMBULANCE_SIREN,
                    SoundsCollection.VehicleSounds.ID_FIRE_TRUCK_SIREN,
                    SoundsCollection.VehicleSounds.ID_LARGE_CAR_SOUND,
                    SoundsCollection.VehicleSounds.ID_METRO_MOVEMENT,
                    SoundsCollection.VehicleSounds.ID_POLICE_CAR_SIREN,
                    SoundsCollection.VehicleSounds.ID_SMALL_CAR_SOUND,
                    SoundsCollection.VehicleSounds.ID_TRAIN_MOVEMENT,
                    SoundsCollection.VehicleSounds.ID_TRANSPORT_ARRIVE
                };
            }
        }

        public override bool BackupVolume(string id)
        {
            if (SimulationManager.instance.m_metaData != null && SimulationManager.instance.m_metaData.m_updateMode != SimulationManager.UpdateMode.Undefined)
            {
                SoundContainer sound = SoundsCollection.Vehicles[id];
                float? volume = SoundsPatcher.GetVolume(sound);
                if (volume.HasValue)
                {
                    this.OldVolumes[id] = volume.Value;
                    return true;
                }
            }
            return false;
        }

        public override bool PatchVolume(string id, float newVolume)
        {
            if (SimulationManager.instance.m_metaData != null && SimulationManager.instance.m_metaData.m_updateMode != SimulationManager.UpdateMode.Undefined)
            {
                SoundContainer sound = SoundsCollection.Vehicles[id];
                return SoundsPatcher.SetVolume(sound, newVolume);
            }
            return false;
        }

        public override bool BackupSound(string id)
        {
            if (SimulationManager.instance.m_metaData != null && SimulationManager.instance.m_metaData.m_updateMode != SimulationManager.UpdateMode.Undefined)
            {
                SoundContainer sound = SoundsCollection.Vehicles[id];
                this.OldSounds[id] = SoundsPatcher.GetAudioInfo(sound);
                return this.OldSounds != null;
            }
            return false;
        }

        public override bool PatchSound(string id, SoundPackFile.Audio newSound)
        {
            if (SimulationManager.instance.m_metaData != null && SimulationManager.instance.m_metaData.m_updateMode != SimulationManager.UpdateMode.Undefined)
            {
                SoundContainer sound = SoundsCollection.Vehicles[id];
                return SoundsPatcher.SetAudioInfo(sound, newSound);
            }
            return false;
        }
    }
}
