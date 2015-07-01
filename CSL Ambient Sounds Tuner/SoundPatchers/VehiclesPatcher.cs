using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmbientSoundsTuner.SoundPatchers
{
    /// <summary>
    /// A class that can patch instances of vehicle sounds.
    /// </summary>
    public class VehiclesPatcher : SoundsInstancePatcher<string>
    {
        public VehiclesPatcher()
            : base()
        {
            this.DefaultVolumes.Add(SoundsCollection.VehicleSounds.ID_AIRCRAFT_MOVEMENT, 0.5f);
            this.DefaultVolumes.Add(SoundsCollection.VehicleSounds.ID_AMBULANCE_SIREN, 1);
            this.DefaultVolumes.Add(SoundsCollection.VehicleSounds.ID_FIRE_TRUCK_SIREN, 3);
            this.DefaultVolumes.Add(SoundsCollection.VehicleSounds.ID_LARGE_CAR_SOUND, 1.5f);
            this.DefaultVolumes.Add(SoundsCollection.VehicleSounds.ID_METRO_MOVEMENT, 0.5f);
            this.DefaultVolumes.Add(SoundsCollection.VehicleSounds.ID_POLICE_CAR_SIREN, 1);
            this.DefaultVolumes.Add(SoundsCollection.VehicleSounds.ID_SMALL_CAR_SOUND, 1.5f);
            this.DefaultVolumes.Add(SoundsCollection.VehicleSounds.ID_TRAIN_MOVEMENT, 0.5f);
            this.DefaultVolumes.Add(SoundsCollection.VehicleSounds.ID_TRANSPORT_ARRIVE, 1);
        }

        public override bool BackupVolume(string id)
        {
            if (SimulationManager.instance.m_metaData != null && SimulationManager.instance.m_metaData.m_updateMode != SimulationManager.UpdateMode.Undefined)
            {
                SoundContainer sound = SoundsCollection.Vehicles[id];
                if (sound.HasSound)
                {
                    float? volume = SoundsPatcher.GetVolume(sound);
                    if (volume.HasValue)
                    {
                        this.DefaultVolumes[id] = volume.Value;
                        return true;
                    }
                }
            }
            return false;
        }

        public override bool PatchVolume(string id, float newVolume)
        {
            if (SimulationManager.instance.m_metaData != null && SimulationManager.instance.m_metaData.m_updateMode != SimulationManager.UpdateMode.Undefined)
            {
                SoundContainer sound = SoundsCollection.Vehicles[id];
                if (sound.HasSound)
                {
                    return SoundsPatcher.SetVolume(sound, newVolume);
                }
            }
            return false;
        }
    }
}
