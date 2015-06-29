using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmbientSoundsTuner.SoundPatchers
{
    /// <summary>
    /// A class that can patch instances of building sounds.
    /// </summary>
    public class BuildingsPatcher : MiscellaneousSoundsInstancePatcher<string>
    {
        public BuildingsPatcher()
            : base()
        {
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_ADVANCED_WIND_TURBINE, 1);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_AIRPORT, 1);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_BUS_DEPOT, 1);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_CEMETERY, 1);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_COAL_POWER_PLANT, 1);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_COMMERCIAL, 1);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_CREMATORY, 1);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_ELEMENTARY_SCHOOL, 1);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_FIRE_STATION, 1);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_FUSION_POWER_PLANT, 4);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_HARBOR, 1);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_HIGH_SCHOOL, 1);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_HOSPITAL, 1);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_HYDRO_POWER_PLANT, 1);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_INCINERATION_PLANT, 1);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_INDUSTRIAL, 0.5f);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_METRO_STATION, 1);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_NUCLEAR_POWER_PLANT, 1);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_POLICE_STATION, 1);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_POWER_PLANT_SMALL, 1);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_SOLAR_POWER_PLANT, 1);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_TRAIN_STATION, 1);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_UNIVERSITY, 1);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_WATER_DRAIN_PIPE, 1);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_WATER_PUMPING_STATION, 1);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_WIND_TURBINE, 1);

            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_ON_FIRE, 1);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_ON_UPGRADE, 0.25f);
        }

        protected override AudioInfo GetAudioInfoById(string id)
        {
            SoundContainer sound = SoundsCollection.Buildings[id];

            if (sound.HasSound)
            {
                return sound.AudioInfo;
            }

            return null;
        }
    }
}
