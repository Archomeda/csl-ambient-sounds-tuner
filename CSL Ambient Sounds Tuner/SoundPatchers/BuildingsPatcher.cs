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
        {
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_FUSION_POWER_PLANT, 4);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_INDUSTRIAL, 0.5f);
            this.DefaultVolumes.Add(SoundsCollection.BuildingSounds.ID_ON_UPGRADE, 0.25f);

            this.DefaultMaxVolumes.Add(SoundsCollection.BuildingSounds.ID_FUSION_POWER_PLANT, 4);
            this.DefaultMaxVolumes.Add(SoundsCollection.BuildingSounds.ID_INDUSTRIAL, 0.5f);
            this.DefaultMaxVolumes.Add(SoundsCollection.BuildingSounds.ID_ON_UPGRADE, 0.25f);
        }

        public override string[] Ids
        {
            get
            {
                return new[] {
                    SoundsCollection.BuildingSounds.ID_ADVANCED_WIND_TURBINE,
                    SoundsCollection.BuildingSounds.ID_AIRPORT,
                    SoundsCollection.BuildingSounds.ID_BUS_DEPOT,
                    SoundsCollection.BuildingSounds.ID_CEMETERY,
                    SoundsCollection.BuildingSounds.ID_COAL_POWER_PLANT,
                    SoundsCollection.BuildingSounds.ID_COMMERCIAL,
                    SoundsCollection.BuildingSounds.ID_CREMATORY,
                    SoundsCollection.BuildingSounds.ID_ELEMENTARY_SCHOOL,
                    SoundsCollection.BuildingSounds.ID_FIRE_STATION,
                    SoundsCollection.BuildingSounds.ID_FUSION_POWER_PLANT,
                    SoundsCollection.BuildingSounds.ID_HARBOR,
                    SoundsCollection.BuildingSounds.ID_HIGH_SCHOOL,
                    SoundsCollection.BuildingSounds.ID_HOSPITAL,
                    SoundsCollection.BuildingSounds.ID_HYDRO_POWER_PLANT,
                    SoundsCollection.BuildingSounds.ID_INCINERATION_PLANT,
                    SoundsCollection.BuildingSounds.ID_INDUSTRIAL,
                    SoundsCollection.BuildingSounds.ID_METRO_STATION,
                    SoundsCollection.BuildingSounds.ID_NUCLEAR_POWER_PLANT,
                    SoundsCollection.BuildingSounds.ID_POLICE_STATION,
                    SoundsCollection.BuildingSounds.ID_POWER_PLANT_SMALL,
                    SoundsCollection.BuildingSounds.ID_SOLAR_POWER_PLANT,
                    SoundsCollection.BuildingSounds.ID_TRAIN_STATION,
                    SoundsCollection.BuildingSounds.ID_UNIVERSITY,
                    SoundsCollection.BuildingSounds.ID_WATER_DRAIN_PIPE,
                    SoundsCollection.BuildingSounds.ID_WATER_PUMPING_STATION,
                    SoundsCollection.BuildingSounds.ID_WIND_TURBINE,

                    SoundsCollection.BuildingSounds.ID_ON_FIRE,
                    SoundsCollection.BuildingSounds.ID_ON_UPGRADE
                };
            }
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
