using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalFramework;

namespace AmbientSoundsTuner.SoundPatchers
{
    public class SoundPatchersManager : Singleton<SoundPatchersManager>
    {
        public SoundPatchersManager()
        {
            this.AmbientsPatcher = new AmbientsPatcher();
            this.AnimalsPatcher = new AnimalsPatcher();
            this.BuildingsPatcher = new BuildingsPatcher();
            this.VehiclesPatcher = new VehiclesPatcher();
            this.MiscPatcher = new MiscPatcher();
        }

        public AmbientsPatcher AmbientsPatcher { get; private set; }

        public AnimalsPatcher AnimalsPatcher { get; private set; }

        public BuildingsPatcher BuildingsPatcher { get; private set; }

        public VehiclesPatcher VehiclesPatcher { get; private set; }

        public MiscPatcher MiscPatcher { get; private set; }

        public SoundsInstancePatcher<T> GetPatcherById<T>(string id)
        {
            switch (id)
            {
                case "Ambient": return this.AmbientsPatcher as SoundsInstancePatcher<T>;
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
    }
}
