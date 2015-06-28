using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.Defs;
using AmbientSoundsTuner.SoundPatchers;
using AmbientSoundsTuner.Utils;
using ColossalFramework;
using ColossalFramework.DataBinding;
using ColossalFramework.UI;
using CommonShared.Extensions;
using CommonShared.UI;
using CommonShared.Utils;
using UnityEngine;

namespace AmbientSoundsTuner.UI
{
    public class AdvancedOptionsWindow : Window
    {
        protected readonly Dictionary<string, string> SliderNames = new Dictionary<string, string>()
        {
            { AnimalsPatcher.ID_SEAGULL_SCREAM, "Seagulls" },

            { BuildingsPatcher.ID_INCINERATION_PLANT, "Incineration Plant" },

            { VehiclesPatcher.ID_AIRCRAFT_MOVEMENT, "Aircrafts" },
            { VehiclesPatcher.ID_SMALL_CAR_MOVEMENT, "Cars (small)" },
            { VehiclesPatcher.ID_LARGE_CAR_MOVEMENT, "Cars (large)" },
            { VehiclesPatcher.ID_AMBULANCE_SIREN, "Sirens (ambulances)" },
            { VehiclesPatcher.ID_FIRE_TRUCK_SIREN, "Sirens (fire trucks)" },
            { VehiclesPatcher.ID_POLICE_CAR_SIREN, "Sirens (police cars)" },
            { VehiclesPatcher.ID_METRO_MOVEMENT, "Metros" },
            { VehiclesPatcher.ID_TRAIN_MOVEMENT, "Trains" },
            { VehiclesPatcher.ID_TRANSPORT_ARRIVE, "Transportation arrivals" },
        };

        protected List<GameObject> AmbientSettingObjects = new List<GameObject>();
        protected List<GameObject> AnimalSettingObjects = new List<GameObject>();
        protected List<GameObject> BuildingSettingObjects = new List<GameObject>();
        protected List<GameObject> VehiclesSettingObjects = new List<GameObject>();

        protected UITabstrip Tabstrip;
        protected UIButton AmbientsTabButton;
        protected UIButton AnimalsTabButton;
        protected UIButton BuildingsTabButton;
        protected UIButton VehiclesTabButton;

        protected UITabContainer TabContainer;
        protected UIPanel AmbientsPanel;
        protected UIPanel AnimalsPanel;
        protected UIPanel BuildingsPanel;
        protected UIPanel VehiclesPanel;

        private bool slidersSorted = false;

        public float ambientVolumeWorld;
        public float ambientVolumeForest;
        public float ambientVolumeSea;
        public float ambientVolumeStream;
        public float ambientVolumeIndustrial;
        public float ambientVolumePlaza;
        public float ambientVolumeSuburban;
        public float ambientVolumeCity;
        public float ambientVolumeAgricultural;
        public float animalVolumeSeagullScream;
        public float buildingVolumeIncinerationPlant;
        public float vehicleVolumeAircraftMovement;
        public float vehicleVolumeAmbulanceSiren;
        public float vehicleVolumeFireTruckSiren;
        public float vehicleVolumeLargeCarMovement;
        public float vehicleVolumeMetroMovement;
        public float vehicleVolumePoliceCarSiren;
        public float vehicleVolumeSmallCarMovement;
        public float vehicleVolumeTrainMovement;
        public float vehicleVolumeTransportArrive;

        public override void Start()
        {
            this.width = 750;
            this.height = 430;
            this.Title = "SOUNDS TUNER";
            base.Start();

            this.Tabstrip = this.ContentPanel.AddUIComponent<UITabstrip>();
            this.Tabstrip.relativePosition = new Vector3(20, 20);
            this.Tabstrip.width = 700;
            this.Tabstrip.anchor = UIAnchorStyle.Top | UIAnchorStyle.Left | UIAnchorStyle.Right;

            this.TabContainer = this.ContentPanel.AddUIComponent<UITabContainer>();
            this.TabContainer.size = new Vector2(700, 330);
            this.TabContainer.relativePosition = new Vector3(20, 20 + this.Tabstrip.height);
            this.TabContainer.anchor = UIAnchorStyle.All;
            this.Tabstrip.tabPages = this.TabContainer;

            // Get template button from the options panel tabstrip
            UITabstrip optionTabStrip = GameObject.Find(GameObjectDefs.ID_OPTIONTABSTRIP).GetComponent<UITabstrip>();
            UIButton tabStripButtonTemplate = optionTabStrip.GetComponentInChildren<UIButton>();

            this.AmbientsTabButton = this.Tabstrip.AddTab("AMBIENTS", tabStripButtonTemplate, true);
            this.AmbientsTabButton.playAudioEvents = true;
            this.AmbientsTabButton.focusedTextColor = tabStripButtonTemplate.focusedTextColor;
            this.AmbientsTabButton.width = 150;
            this.AnimalsTabButton = this.Tabstrip.AddTab("ANIMALS", this.AmbientsTabButton, true);
            this.AnimalsTabButton.playAudioEvents = true;
            this.AnimalsTabButton.focusedTextColor = tabStripButtonTemplate.focusedTextColor;
            this.BuildingsTabButton = this.Tabstrip.AddTab("BUILDINGS", this.AmbientsTabButton, true);
            this.BuildingsTabButton.playAudioEvents = true;
            this.BuildingsTabButton.focusedTextColor = tabStripButtonTemplate.focusedTextColor;
            this.VehiclesTabButton = this.Tabstrip.AddTab("VEHICLES", this.AmbientsTabButton, true);
            this.VehiclesTabButton.playAudioEvents = true;
            this.VehiclesTabButton.focusedTextColor = tabStripButtonTemplate.focusedTextColor;

            // Tabs layout
            UIPanel[] tabs = this.TabContainer.GetComponentsInChildren<UIPanel>();
            this.AmbientsPanel = tabs[0];
            this.AnimalsPanel = tabs[1];
            this.BuildingsPanel = tabs[2];
            this.VehiclesPanel = tabs[3];

            this.AmbientsPanel.padding = new RectOffset(0, 0, 10, 10);
            this.AmbientsPanel.autoLayout = true;
            this.AmbientsPanel.autoLayoutDirection = LayoutDirection.Vertical;
            this.AmbientsPanel.autoLayoutPadding = new RectOffset(15, 15, 5, 5);
            this.AmbientsPanel.wrapLayout = true;

            this.AnimalsPanel.Hide();
            this.AnimalsPanel.padding = new RectOffset(0, 0, 10, 10);
            this.AnimalsPanel.autoLayout = true;
            this.AnimalsPanel.autoLayoutDirection = LayoutDirection.Vertical;
            this.AnimalsPanel.autoLayoutPadding = new RectOffset(15, 15, 5, 5);
            this.AnimalsPanel.wrapLayout = true;

            this.BuildingsPanel.Hide();
            this.BuildingsPanel.padding = new RectOffset(0, 0, 10, 10);
            this.BuildingsPanel.autoLayout = true;
            this.BuildingsPanel.autoLayoutDirection = LayoutDirection.Vertical;
            this.BuildingsPanel.autoLayoutPadding = new RectOffset(15, 15, 5, 5);
            this.BuildingsPanel.wrapLayout = true;

            this.VehiclesPanel.Hide();
            this.VehiclesPanel.padding = new RectOffset(0, 0, 10, 10);
            this.VehiclesPanel.autoLayout = true;
            this.VehiclesPanel.autoLayoutDirection = LayoutDirection.Vertical;
            this.VehiclesPanel.autoLayoutPadding = new RectOffset(15, 15, 5, 5);
            this.VehiclesPanel.wrapLayout = true;

            // Settings
            Mod.Settings.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.World, Mod.Instance.AmbientsPatcher.DefaultVolumes[AudioManager.AmbientType.World], out this.ambientVolumeWorld);
            Mod.Settings.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Forest, Mod.Instance.AmbientsPatcher.DefaultVolumes[AudioManager.AmbientType.Forest], out this.ambientVolumeForest);
            Mod.Settings.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Sea, Mod.Instance.AmbientsPatcher.DefaultVolumes[AudioManager.AmbientType.Sea], out this.ambientVolumeSea);
            Mod.Settings.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Stream, Mod.Instance.AmbientsPatcher.DefaultVolumes[AudioManager.AmbientType.Stream], out this.ambientVolumeStream);
            Mod.Settings.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Industrial, Mod.Instance.AmbientsPatcher.DefaultVolumes[AudioManager.AmbientType.Industrial], out this.ambientVolumeIndustrial);
            Mod.Settings.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Plaza, Mod.Instance.AmbientsPatcher.DefaultVolumes[AudioManager.AmbientType.Plaza], out this.ambientVolumePlaza);
            Mod.Settings.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Suburban, Mod.Instance.AmbientsPatcher.DefaultVolumes[AudioManager.AmbientType.Suburban], out this.ambientVolumeSuburban);
            Mod.Settings.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.City, Mod.Instance.AmbientsPatcher.DefaultVolumes[AudioManager.AmbientType.City], out this.ambientVolumeCity);
            Mod.Settings.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Agricultural, Mod.Instance.AmbientsPatcher.DefaultVolumes[AudioManager.AmbientType.Agricultural], out this.ambientVolumeAgricultural);

            Mod.Settings.AnimalVolumes.TryGetValueOrDefault(AnimalsPatcher.ID_SEAGULL_SCREAM, Mod.Instance.AnimalsPatcher.DefaultVolumes[AnimalsPatcher.ID_SEAGULL_SCREAM], out this.animalVolumeSeagullScream);

            Mod.Settings.BuildingVolumes.TryGetValueOrDefault(BuildingsPatcher.ID_INCINERATION_PLANT, Mod.Instance.BuildingsPatcher.DefaultVolumes[BuildingsPatcher.ID_INCINERATION_PLANT], out this.buildingVolumeIncinerationPlant);

            Mod.Settings.VehicleVolumes.TryGetValueOrDefault(VehiclesPatcher.ID_AIRCRAFT_MOVEMENT, Mod.Instance.VehiclesPatcher.DefaultVolumes[VehiclesPatcher.ID_AIRCRAFT_MOVEMENT], out this.vehicleVolumeAircraftMovement);
            Mod.Settings.VehicleVolumes.TryGetValueOrDefault(VehiclesPatcher.ID_AMBULANCE_SIREN, Mod.Instance.VehiclesPatcher.DefaultVolumes[VehiclesPatcher.ID_AMBULANCE_SIREN], out this.vehicleVolumeAmbulanceSiren);
            Mod.Settings.VehicleVolumes.TryGetValueOrDefault(VehiclesPatcher.ID_FIRE_TRUCK_SIREN, Mod.Instance.VehiclesPatcher.DefaultVolumes[VehiclesPatcher.ID_FIRE_TRUCK_SIREN], out this.vehicleVolumeFireTruckSiren);
            Mod.Settings.VehicleVolumes.TryGetValueOrDefault(VehiclesPatcher.ID_LARGE_CAR_MOVEMENT, Mod.Instance.VehiclesPatcher.DefaultVolumes[VehiclesPatcher.ID_LARGE_CAR_MOVEMENT], out this.vehicleVolumeLargeCarMovement);
            Mod.Settings.VehicleVolumes.TryGetValueOrDefault(VehiclesPatcher.ID_METRO_MOVEMENT, Mod.Instance.VehiclesPatcher.DefaultVolumes[VehiclesPatcher.ID_METRO_MOVEMENT], out this.vehicleVolumeMetroMovement);
            Mod.Settings.VehicleVolumes.TryGetValueOrDefault(VehiclesPatcher.ID_POLICE_CAR_SIREN, Mod.Instance.VehiclesPatcher.DefaultVolumes[VehiclesPatcher.ID_POLICE_CAR_SIREN], out this.vehicleVolumePoliceCarSiren);
            Mod.Settings.VehicleVolumes.TryGetValueOrDefault(VehiclesPatcher.ID_SMALL_CAR_MOVEMENT, Mod.Instance.VehiclesPatcher.DefaultVolumes[VehiclesPatcher.ID_SMALL_CAR_MOVEMENT], out this.vehicleVolumeSmallCarMovement);
            Mod.Settings.VehicleVolumes.TryGetValueOrDefault(VehiclesPatcher.ID_TRAIN_MOVEMENT, Mod.Instance.VehiclesPatcher.DefaultVolumes[VehiclesPatcher.ID_TRAIN_MOVEMENT], out this.vehicleVolumeTrainMovement);
            Mod.Settings.VehicleVolumes.TryGetValueOrDefault(VehiclesPatcher.ID_TRANSPORT_ARRIVE, Mod.Instance.VehiclesPatcher.DefaultVolumes[VehiclesPatcher.ID_TRANSPORT_ARRIVE], out this.vehicleVolumeTransportArrive);

            // Sliders
            this.AmbientSettingObjects.Add(this.CreateAmbientVolumeSetting(AudioManager.AmbientType.World, "ambientVolumeWorld"));
            this.AmbientSettingObjects.Add(this.CreateAmbientVolumeSetting(AudioManager.AmbientType.Forest, "ambientVolumeForest"));
            this.AmbientSettingObjects.Add(this.CreateAmbientVolumeSetting(AudioManager.AmbientType.Sea, "ambientVolumeSea"));
            this.AmbientSettingObjects.Add(this.CreateAmbientVolumeSetting(AudioManager.AmbientType.Stream, "ambientVolumeStream"));
            this.AmbientSettingObjects.Add(this.CreateAmbientVolumeSetting(AudioManager.AmbientType.Industrial, "ambientVolumeIndustrial"));
            this.AmbientSettingObjects.Add(this.CreateAmbientVolumeSetting(AudioManager.AmbientType.Plaza, "ambientVolumePlaza"));
            this.AmbientSettingObjects.Add(this.CreateAmbientVolumeSetting(AudioManager.AmbientType.Suburban, "ambientVolumeSuburban"));
            this.AmbientSettingObjects.Add(this.CreateAmbientVolumeSetting(AudioManager.AmbientType.City, "ambientVolumeCity"));
            this.AmbientSettingObjects.Add(this.CreateAmbientVolumeSetting(AudioManager.AmbientType.Agricultural, "ambientVolumeAgricultural"));

            this.AnimalSettingObjects.Add(this.CreateAnimalVolumeSetting(AnimalsPatcher.ID_SEAGULL_SCREAM, "animalVolumeSeagullScream"));

            this.BuildingSettingObjects.Add(this.CreateBuildingVolumeSetting(BuildingsPatcher.ID_INCINERATION_PLANT, "buildingVolumeIncinerationPlant"));

            this.VehiclesSettingObjects.Add(this.CreateVehicleVolumeSetting(VehiclesPatcher.ID_AIRCRAFT_MOVEMENT, "vehicleVolumeAircraftMovement", 0, 1));
            this.VehiclesSettingObjects.Add(this.CreateVehicleVolumeSetting(VehiclesPatcher.ID_AMBULANCE_SIREN, "vehicleVolumeAmbulanceSiren"));
            this.VehiclesSettingObjects.Add(this.CreateVehicleVolumeSetting(VehiclesPatcher.ID_FIRE_TRUCK_SIREN, "vehicleVolumeFireTruckSiren", 0, 3));
            this.VehiclesSettingObjects.Add(this.CreateVehicleVolumeSetting(VehiclesPatcher.ID_LARGE_CAR_MOVEMENT, "vehicleVolumeLargeCarMovement", 0, 1.5f));
            this.VehiclesSettingObjects.Add(this.CreateVehicleVolumeSetting(VehiclesPatcher.ID_METRO_MOVEMENT, "vehicleVolumeMetroMovement", 0, 1));
            this.VehiclesSettingObjects.Add(this.CreateVehicleVolumeSetting(VehiclesPatcher.ID_POLICE_CAR_SIREN, "vehicleVolumePoliceCarSiren"));
            this.VehiclesSettingObjects.Add(this.CreateVehicleVolumeSetting(VehiclesPatcher.ID_SMALL_CAR_MOVEMENT, "vehicleVolumeSmallCarMovement", 0, 1.5f));
            this.VehiclesSettingObjects.Add(this.CreateVehicleVolumeSetting(VehiclesPatcher.ID_TRAIN_MOVEMENT, "vehicleVolumeTrainMovement", 0, 1));
            this.VehiclesSettingObjects.Add(this.CreateVehicleVolumeSetting(VehiclesPatcher.ID_TRANSPORT_ARRIVE, "vehicleVolumeTransportArrive"));

            // Some extra event listeners
            this.eventVisibilityChanged += AdvancedOptionsWindow_eventVisibilityChanged;
        }

        public override void Update()
        {
            // Order sliders by name if not already sorted.
            // We have to do this here, since the list of child components is empty in Start().
            if (!this.slidersSorted && this.isVisible)
            {
                var ambientsPanelChildComponents = ReflectionUtils.GetPrivateField<PoolList<UIComponent>>(this.AmbientsPanel, "m_ChildComponents");
                ambientsPanelChildComponents.Sort(new SortSlidersByTextComparer());
                var effectsPanelChildComponents = ReflectionUtils.GetPrivateField<PoolList<UIComponent>>(this.VehiclesPanel, "m_ChildComponents");
                effectsPanelChildComponents.Sort(new SortSlidersByTextComparer());

                this.slidersSorted = true;
            }
            base.Update();
        }

        private void AdvancedOptionsWindow_eventVisibilityChanged(UIComponent component, bool value)
        {
            if (this.isVisibleSelf && this.parent != null && !this.parent.isVisible)
            {
                // Here we save when the parent window goes invisible (aka gets closed), this is a workaround to solve bug #2.
                Mod.Settings.SaveConfig(Mod.SettingsFilename);
            }
        }

        public override void Close()
        {
            Mod.Settings.SaveConfig(Mod.SettingsFilename);
            base.Close();
        }

        protected GameObject CreateAmbientVolumeSetting(AudioManager.AmbientType type, string memberName, float minValue = 0, float maxValue = 1)
        {
            return CreateVolumeSetting(Mod.Settings.AmbientVolumes, Mod.Instance.AmbientsPatcher, this.AmbientsPanel, "AmbientVolumeSetting", type, memberName, minValue, maxValue);
        }

        protected GameObject CreateAnimalVolumeSetting(string id, string memberName, float minValue = 0, float maxValue = 1)
        {
            return CreateVolumeSetting(Mod.Settings.AnimalVolumes, Mod.Instance.AnimalsPatcher, this.AnimalsPanel, "AnimalVolumeSetting", id, memberName, minValue, maxValue);
        }

        protected GameObject CreateBuildingVolumeSetting(string id, string memberName, float minValue = 0, float maxValue = 1)
        {
            return CreateVolumeSetting(Mod.Settings.BuildingVolumes, Mod.Instance.BuildingsPatcher, this.BuildingsPanel, "BuildingVolumeSetting", id, memberName, minValue, maxValue);
        }

        protected GameObject CreateVehicleVolumeSetting(string id, string memberName, float minValue = 0, float maxValue = 1)
        {
            return CreateVolumeSetting(Mod.Settings.VehicleVolumes, Mod.Instance.VehiclesPatcher, this.VehiclesPanel, "VehicleVolumeSetting", id, memberName, minValue, maxValue);
        }

        protected GameObject CreateVolumeSetting<T>(IDictionary<T, float> volumeDictionary, SoundsInstancePatcher<T> patcher, UIComponent parent, string gameObjectName, T id, string memberName, float minValue = 0, float maxValue = 1)
        {
            PropertyChangedEventHandler<float> valueChangedCallback = new PropertyChangedEventHandler<float>((c, v) =>
            {
                volumeDictionary[id] = v;
                if (SimulationManager.instance.m_metaData != null && SimulationManager.instance.m_metaData.m_updateMode != SimulationManager.UpdateMode.Undefined)
                {
                    patcher.PatchVolume(id, v);
                }
            });

            string name = id.ToString();
            this.SliderNames.TryGetValueOrDefault(name, name, out name);

            int panelWidth = (int)(parent.width - ((UIPanel)parent).padding.left - ((UIPanel)parent).padding.right) / 2 - ((UIPanel)parent).autoLayoutPadding.left - ((UIPanel)parent).autoLayoutPadding.right;
            int sliderX = panelWidth - 140;

            GameObject setting = new GameObject(gameObjectName);
            UIPanel panel = setting.AddComponent<UIPanel>();
            panel.transform.SetParent(parent.transform);
            panel.width = panelWidth;
            panel.height = 25;

            UILabel label = panel.AddUIComponent<UILabel>();
            label.width = sliderX;
            label.text = name;
            label.isLocalized = false;
            label.position = new Vector3(0, -(panel.height - label.height) / 2).RoundToInt();
            label.verticalAlignment = UIVerticalAlignment.Middle;

            GameObject sliderObject = UnityEngine.Object.Instantiate(GameObject.Find("SliderAmbientVolume"));
            panel.AttachUIComponent(sliderObject);

            UISlider slider = sliderObject.GetComponent<UISlider>();
            slider.width = 130;
            slider.position = new Vector3(sliderX, 0);
            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.eventValueChanged += valueChangedCallback;

            BindProperty binding = sliderObject.GetComponent<BindProperty>();
            binding.dataSource.component = this;
            binding.dataSource.memberName = memberName;
            binding.dataTarget.component = slider;
            binding.dataTarget.memberName = "value";

            return setting;
        }


        private class SortSlidersByTextComparer : Comparer<UIComponent>
        {
            public override int Compare(UIComponent x, UIComponent y)
            {
                UILabel xLabel = x.GetComponentInChildren<UILabel>();
                UILabel yLabel = y.GetComponentInChildren<UILabel>();
                if (xLabel != null && yLabel != null)
                {
                    int result = xLabel.text.CompareTo(yLabel.text);
                    return result;
                }
                return 0;
            }
        }
    }
}
