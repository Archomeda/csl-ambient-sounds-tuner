using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.Utils;
using AmbientSoundsTuner.Extensions;
using ColossalFramework.DataBinding;
using ColossalFramework.UI;
using UnityEngine;

namespace AmbientSoundsTuner.UI
{
    public class AdvancedOptionsWindow : Window
    {
        protected GameObject[] AmbientVolumeSettingObjects = new GameObject[9];
        protected GameObject[] EffectVolumeSettingObjects = new GameObject[9];

        protected UIPanel AmbientsPanel;
        protected UIPanel EffectsPanel;

        public float ambientVolumeWorld;
        public float ambientVolumeForest;
        public float ambientVolumeSea;
        public float ambientVolumeStream;
        public float ambientVolumeIndustrial;
        public float ambientVolumePlaza;
        public float ambientVolumeSuburban;
        public float ambientVolumeCity;
        public float ambientVolumeAgricultural;

        public float effectVolumeAircraftMovement;
        public float effectVolumeAmbulanceSiren;
        public float effectVolumeFireTruckSiren;
        public float effectVolumeLargeCarMovement;
        public float effectVolumeMetroMovement;
        public float effectVolumePoliceCarSiren;
        public float effectVolumeSmallCarMovement;
        public float effectVolumeTrainMovement;
        public float effectVolumeTransportArrive;

        public override void Start()
        {
            this.width = 741;
            this.height = 381;
            this.Title = "SOUNDS TUNER";
            base.Start();

            // Layout
            this.ContentPanel.autoLayout = true;
            this.ContentPanel.autoLayoutDirection = LayoutDirection.Horizontal;
            this.ContentPanel.autoLayoutPadding = new RectOffset(20, 20, 10, 10);

            this.AmbientsPanel = this.ContentPanel.AddUIComponent<UIPanel>();
            this.AmbientsPanel.size = new Vector2(300, 320);
            this.AmbientsPanel.autoLayout = true;
            this.AmbientsPanel.autoLayoutDirection = LayoutDirection.Vertical;
            this.AmbientsPanel.autoLayoutPadding = new RectOffset(0, 0, 5, 5);

            this.EffectsPanel = this.ContentPanel.AddUIComponent<UIPanel>();
            this.EffectsPanel.size = new Vector2(360, 320);
            this.EffectsPanel.autoLayout = true;
            this.EffectsPanel.autoLayoutDirection = LayoutDirection.Vertical;
            this.EffectsPanel.autoLayoutPadding = new RectOffset(0, 0, 5, 5);

            // Settings
            Configuration.Instance.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.World, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.World], out this.ambientVolumeWorld);
            Configuration.Instance.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Forest, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.Forest], out this.ambientVolumeForest);
            Configuration.Instance.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Sea, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.Sea], out this.ambientVolumeSea);
            Configuration.Instance.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Stream, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.Stream], out this.ambientVolumeStream);
            Configuration.Instance.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Industrial, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.Industrial], out this.ambientVolumeIndustrial);
            Configuration.Instance.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Plaza, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.Plaza], out this.ambientVolumePlaza);
            Configuration.Instance.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Suburban, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.Suburban], out this.ambientVolumeSuburban);
            Configuration.Instance.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.City, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.City], out this.ambientVolumeCity);
            Configuration.Instance.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Agricultural, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.Agricultural], out this.ambientVolumeAgricultural);

            Configuration.Instance.State.EffectVolumes.TryGetValueOrDefault("Aircraft Movement", EffectsPatcher.OriginalVolumes["Aircraft Movement"], out this.effectVolumeAircraftMovement);
            Configuration.Instance.State.EffectVolumes.TryGetValueOrDefault("Ambulance Siren", EffectsPatcher.OriginalVolumes["Ambulance Siren"], out this.effectVolumeAmbulanceSiren);
            Configuration.Instance.State.EffectVolumes.TryGetValueOrDefault("Fire Truck Siren", EffectsPatcher.OriginalVolumes["Fire Truck Siren"], out this.effectVolumeFireTruckSiren);
            Configuration.Instance.State.EffectVolumes.TryGetValueOrDefault("Large Car Movement", EffectsPatcher.OriginalVolumes["Large Car Movement"], out this.effectVolumeLargeCarMovement);
            Configuration.Instance.State.EffectVolumes.TryGetValueOrDefault("Metro Movement", EffectsPatcher.OriginalVolumes["Metro Movement"], out this.effectVolumeMetroMovement);
            Configuration.Instance.State.EffectVolumes.TryGetValueOrDefault("Police Car Siren", EffectsPatcher.OriginalVolumes["Police Car Siren"], out this.effectVolumePoliceCarSiren);
            Configuration.Instance.State.EffectVolumes.TryGetValueOrDefault("Small Car Movement", EffectsPatcher.OriginalVolumes["Small Car Movement"], out this.effectVolumeSmallCarMovement);
            Configuration.Instance.State.EffectVolumes.TryGetValueOrDefault("Train Movement", EffectsPatcher.OriginalVolumes["Train Movement"], out this.effectVolumeTrainMovement);
            Configuration.Instance.State.EffectVolumes.TryGetValueOrDefault("Transport Arrive", EffectsPatcher.OriginalVolumes["Transport Arrive"], out this.effectVolumeTransportArrive);

            this.AmbientVolumeSettingObjects[0] = this.CreateVolumeSetting(this.AmbientsPanel, "AmbientVolumeSetting", "World", "ambientVolumeWorld");
            this.AmbientVolumeSettingObjects[1] = this.CreateVolumeSetting(this.AmbientsPanel, "AmbientVolumeSetting", "Forest", "ambientVolumeForest");
            this.AmbientVolumeSettingObjects[2] = this.CreateVolumeSetting(this.AmbientsPanel, "AmbientVolumeSetting", "Sea", "ambientVolumeSea");
            this.AmbientVolumeSettingObjects[3] = this.CreateVolumeSetting(this.AmbientsPanel, "AmbientVolumeSetting", "Stream", "ambientVolumeStream");
            this.AmbientVolumeSettingObjects[4] = this.CreateVolumeSetting(this.AmbientsPanel, "AmbientVolumeSetting", "Industrial", "ambientVolumeIndustrial");
            this.AmbientVolumeSettingObjects[5] = this.CreateVolumeSetting(this.AmbientsPanel, "AmbientVolumeSetting", "Plaza", "ambientVolumePlaza");
            this.AmbientVolumeSettingObjects[6] = this.CreateVolumeSetting(this.AmbientsPanel, "AmbientVolumeSetting", "Suburban", "ambientVolumeSuburban");
            this.AmbientVolumeSettingObjects[7] = this.CreateVolumeSetting(this.AmbientsPanel, "AmbientVolumeSetting", "City", "ambientVolumeCity");
            this.AmbientVolumeSettingObjects[8] = this.CreateVolumeSetting(this.AmbientsPanel, "AmbientVolumeSetting", "Agricultural", "ambientVolumeAgricultural");

            this.EffectVolumeSettingObjects[0] = this.CreateVolumeSetting(this.EffectsPanel, "EffectVolumeSetting", "Aircraft Movement", "effectVolumeAircraftMovement", 0, 0.5f); // Default value = 0.5f
            this.EffectVolumeSettingObjects[1] = this.CreateVolumeSetting(this.EffectsPanel, "EffectVolumeSetting", "Ambulance Siren", "effectVolumeAmbulanceSiren");
            this.EffectVolumeSettingObjects[2] = this.CreateVolumeSetting(this.EffectsPanel, "EffectVolumeSetting", "Fire Truck Siren", "effectVolumeFireTruckSiren", 0, 3); // Default value = 3
            this.EffectVolumeSettingObjects[3] = this.CreateVolumeSetting(this.EffectsPanel, "EffectVolumeSetting", "Large Car Movement", "effectVolumeLargeCarMovement", 0, 1.5f); // Default value = 1.5
            this.EffectVolumeSettingObjects[4] = this.CreateVolumeSetting(this.EffectsPanel, "EffectVolumeSetting", "Metro Movement", "effectVolumeMetroMovement", 0, 0.5f); // Default value = 0.5f
            this.EffectVolumeSettingObjects[5] = this.CreateVolumeSetting(this.EffectsPanel, "EffectVolumeSetting", "Police Car Siren", "effectVolumePoliceCarSiren");
            this.EffectVolumeSettingObjects[6] = this.CreateVolumeSetting(this.EffectsPanel, "EffectVolumeSetting", "Small Car Movement", "effectVolumeSmallCarMovement", 0, 1.5f); // Default value = 1.5
            this.EffectVolumeSettingObjects[7] = this.CreateVolumeSetting(this.EffectsPanel, "EffectVolumeSetting", "Train Movement", "effectVolumeTrainMovement", 0, 0.5f); // Default value = 0.5f
            this.EffectVolumeSettingObjects[8] = this.CreateVolumeSetting(this.EffectsPanel, "EffectVolumeSetting", "Transport Arrive", "effectVolumeTransportArrive");

        }

        public override void Close()
        {
            base.Close();

            Configuration.Instance.State.AmbientVolumes[AudioManager.AmbientType.World] = ambientVolumeWorld;
            Configuration.Instance.State.AmbientVolumes[AudioManager.AmbientType.Forest] = ambientVolumeForest;
            Configuration.Instance.State.AmbientVolumes[AudioManager.AmbientType.Sea] = ambientVolumeSea;
            Configuration.Instance.State.AmbientVolumes[AudioManager.AmbientType.Stream] = ambientVolumeStream;
            Configuration.Instance.State.AmbientVolumes[AudioManager.AmbientType.Industrial] = ambientVolumeIndustrial;
            Configuration.Instance.State.AmbientVolumes[AudioManager.AmbientType.Plaza] = ambientVolumePlaza;
            Configuration.Instance.State.AmbientVolumes[AudioManager.AmbientType.Suburban] = ambientVolumeSuburban;
            Configuration.Instance.State.AmbientVolumes[AudioManager.AmbientType.City] = ambientVolumeCity;
            Configuration.Instance.State.AmbientVolumes[AudioManager.AmbientType.Agricultural] = ambientVolumeAgricultural;

            Configuration.Instance.State.EffectVolumes["Aircraft Movement"] = effectVolumeAircraftMovement;
            Configuration.Instance.State.EffectVolumes["Ambulance Siren"] = effectVolumeAmbulanceSiren;
            Configuration.Instance.State.EffectVolumes["Fire Truck Siren"] = effectVolumeFireTruckSiren;
            Configuration.Instance.State.EffectVolumes["Large Car Movement"] = effectVolumeLargeCarMovement;
            Configuration.Instance.State.EffectVolumes["Metro Movement"] = effectVolumeMetroMovement;
            Configuration.Instance.State.EffectVolumes["Police Car Siren"] = effectVolumePoliceCarSiren;
            Configuration.Instance.State.EffectVolumes["Small Car Movement"] = effectVolumeSmallCarMovement;
            Configuration.Instance.State.EffectVolumes["Train Movement"] = effectVolumeTrainMovement;
            Configuration.Instance.State.EffectVolumes["Transport Arrive"] = effectVolumeTransportArrive;

            Configuration.Save();

            AmbientsPatcher.PatchAmbientVolumes();
            EffectsPatcher.PatchEffectVolumes();
        }

        protected GameObject CreateVolumeSetting(UIComponent parent, string gameObjectName, string name, string memberName, float minValue = 0, float maxValue = 1)
        {
            GameObject setting = new GameObject(gameObjectName);
            UIPanel panel = setting.AddComponent<UIPanel>();
            panel.transform.SetParent(parent.transform);
            panel.width = parent.width;
            panel.height = 25;

            UILabel label = panel.AddUIComponent<UILabel>();
            label.width = parent.width - 200;
            label.text = name;
            label.isLocalized = false;
            label.position = new Vector3(0, 0);

            GameObject sliderObject = UnityEngine.Object.Instantiate(GameObject.Find("SliderAmbientVolume"));
            panel.AttachUIComponent(sliderObject);

            UISlider slider = sliderObject.GetComponent<UISlider>();
            slider.width = 200;
            slider.position = new Vector3(parent.width - 200, 0);
            slider.minValue = minValue;
            slider.maxValue = maxValue;

            BindProperty binding = sliderObject.GetComponent<BindProperty>();
            binding.dataSource.component = this;
            binding.dataSource.memberName = memberName;
            binding.dataTarget.component = slider;
            binding.dataTarget.memberName = "value";

            return setting;
        }
    }
}
