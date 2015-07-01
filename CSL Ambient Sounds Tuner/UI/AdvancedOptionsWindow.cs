using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            { SoundsCollection.AnimalSounds.ID_COW, "Cows" },
            { SoundsCollection.AnimalSounds.ID_PIG, "Pigs" },
            { SoundsCollection.AnimalSounds.ID_SEAGULL, "Seagulls" },

            { SoundsCollection.BuildingSounds.ID_ADVANCED_WIND_TURBINE, "Advanced Wind Turbine" },
            { SoundsCollection.BuildingSounds.ID_AIRPORT, "Airport" },
            { SoundsCollection.BuildingSounds.ID_BUS_DEPOT, "Bus Depot" },
            { SoundsCollection.BuildingSounds.ID_CEMETERY, "Cemetery" },
            { SoundsCollection.BuildingSounds.ID_COAL_POWER_PLANT, "Coal/Oil Power Plant" },
            { SoundsCollection.BuildingSounds.ID_COMMERCIAL, "Commercial" },
            { SoundsCollection.BuildingSounds.ID_CREMATORY, "Crematory" },
            { SoundsCollection.BuildingSounds.ID_ELEMENTARY_SCHOOL, "Elementary School" },
            { SoundsCollection.BuildingSounds.ID_FIRE_STATION, "Fire Station" },
            { SoundsCollection.BuildingSounds.ID_FUSION_POWER_PLANT, "Fusion Power Plant" },
            { SoundsCollection.BuildingSounds.ID_HARBOR, "Harbor" },
            { SoundsCollection.BuildingSounds.ID_HIGH_SCHOOL, "High School" },
            { SoundsCollection.BuildingSounds.ID_HOSPITAL, "Hospital" },
            { SoundsCollection.BuildingSounds.ID_HYDRO_POWER_PLANT, "Hydro Power Plant" },
            { SoundsCollection.BuildingSounds.ID_INCINERATION_PLANT, "Incineration Plant" },
            { SoundsCollection.BuildingSounds.ID_INDUSTRIAL, "Industrial" },
            { SoundsCollection.BuildingSounds.ID_METRO_STATION, "Metro Station" },
            { SoundsCollection.BuildingSounds.ID_NUCLEAR_POWER_PLANT, "Nuclear Power Plant" },
            { SoundsCollection.BuildingSounds.ID_POLICE_STATION, "Police Station" },
            { SoundsCollection.BuildingSounds.ID_POWER_PLANT_SMALL, "Small Power Plant" },
            { SoundsCollection.BuildingSounds.ID_SOLAR_POWER_PLANT, "Solar Power Plant" },
            { SoundsCollection.BuildingSounds.ID_TRAIN_STATION, "Train Station" },
            { SoundsCollection.BuildingSounds.ID_UNIVERSITY, "University" },
            { SoundsCollection.BuildingSounds.ID_WATER_DRAIN_PIPE, "Water Drain/Treatment Plant" },
            { SoundsCollection.BuildingSounds.ID_WATER_PUMPING_STATION, "Water Pumping Station" },
            { SoundsCollection.BuildingSounds.ID_WIND_TURBINE, "Wind Turbine" },
            { SoundsCollection.BuildingSounds.ID_ON_FIRE, "On Fire" },
            { SoundsCollection.BuildingSounds.ID_ON_UPGRADE, "On Upgrade" },

            { SoundsCollection.VehicleSounds.ID_AIRCRAFT_MOVEMENT, "Aircrafts" },
            { SoundsCollection.VehicleSounds.ID_SMALL_CAR_MOVEMENT, "Cars (Small)" },
            { SoundsCollection.VehicleSounds.ID_LARGE_CAR_MOVEMENT, "Cars (Large)" },
            { SoundsCollection.VehicleSounds.ID_AMBULANCE_SIREN, "Sirens (Ambulances)" },
            { SoundsCollection.VehicleSounds.ID_FIRE_TRUCK_SIREN, "Sirens (Fire Trucks)" },
            { SoundsCollection.VehicleSounds.ID_POLICE_CAR_SIREN, "Sirens (Police Cars)" },
            { SoundsCollection.VehicleSounds.ID_METRO_MOVEMENT, "Metros" },
            { SoundsCollection.VehicleSounds.ID_TRAIN_MOVEMENT, "Trains" },
            { SoundsCollection.VehicleSounds.ID_TRANSPORT_ARRIVE, "Transportation Arrivals" },

            { SoundsCollection.MiscSounds.ID_BUILDING_BULLDOZE, "Bulldozer (Buildings)" },
            { SoundsCollection.MiscSounds.ID_PROP_BULLDOZE, "Bulldozer (Props)" },
            { SoundsCollection.MiscSounds.ID_ROAD_BULLDOZE, "Bulldozer (Roads)" },
            { SoundsCollection.MiscSounds.ID_BUILDING_PLACEMENT, "Placement (Buildings)" },
            { SoundsCollection.MiscSounds.ID_PROP_PLACEMENT, "Placement (Props)" },
            { SoundsCollection.MiscSounds.ID_ROAD_PLACEMENT, "Placement (Roads)" },
            { SoundsCollection.MiscSounds.ID_ROAD_DRAW, "Road Drawer" },
            { MiscPatcher.ID_CLICK_SOUND, "UI Clicks" },
            { MiscPatcher.ID_DISABLED_CLICK_SOUND, "UI Clicks (Disabled)" },
            { SoundsCollection.MiscSounds.ID_ZONE_FILL, "Zone Filler" },
     };

        protected List<GameObject> AmbientSettingObjects = new List<GameObject>();
        protected List<GameObject> AnimalSettingObjects = new List<GameObject>();
        protected List<GameObject> BuildingSettingObjects = new List<GameObject>();
        protected List<GameObject> VehicleSettingObjects = new List<GameObject>();
        protected List<GameObject> MiscSettingObjects = new List<GameObject>();

        protected UITabstrip Tabstrip;
        protected UIButton AmbientsTabButton;
        protected UIButton AnimalsTabButton;
        protected UIButton BuildingsTabButton;
        protected UIButton VehiclesTabButton;
        protected UIButton MiscTabButton;

        protected UITabContainer TabContainer;
        protected UIPanel AmbientsPanel;
        protected UIPanel AnimalsPanel;
        protected UIPanel BuildingsPanel;
        protected UIPanel VehiclesPanel;
        protected UIPanel MiscPanel;

        private bool slidersSorted = false;

        public float ambientVolumeAgricultural;
        public float ambientVolumeCity;
        public float ambientVolumeForest;
        public float ambientVolumeIndustrial;
        public float ambientVolumePlaza;
        public float ambientVolumeSea;
        public float ambientVolumeStream;
        public float ambientVolumeSuburban;
        public float ambientVolumeWorld;
        public float animalVolumeCow;
        public float animalVolumePig;
        public float animalVolumeSeagull;
        public float buildingVolumeAdvancedWindTurbine;
        public float buildingVolumeAirport;
        public float buildingVolumeBusDepot;
        public float buildingVolumeCemetary;
        public float buildingVolumeCoalOilPowerPlant;
        public float buildingVolumeCommercial;
        public float buildingVolumeCrematory;
        public float buildingVolumeElementarySchool;
        public float buildingVolumeFireStation;
        public float buildingVolumeFusionPowerPlant;
        public float buildingVolumeHarbor;
        public float buildingVolumeHighSchool;
        public float buildingVolumeHospital;
        public float buildingVolumeHydroPowerPlant;
        public float buildingVolumeIncinerationPlant;
        public float buildingVolumeIndustrial;
        public float buildingVolumeMetroStation;
        public float buildingVolumeNuclearPowerPlant;
        public float buildingVolumePoliceStation;
        public float buildingVolumePowerPlantSmall;
        public float buildingVolumeSolarPowerPlant;
        public float buildingVolumeTrainStation;
        public float buildingVolumeUniversity;
        public float buildingVolumeWaterDrainPipe;
        public float buildingVolumeWaterPumpingStation;
        public float buildingVolumeWindTurbine;
        public float buildingVolumeOnFire;
        public float buildingVolumeOnLevelUp;
        public float vehicleVolumeAircraftMovement;
        public float vehicleVolumeAmbulanceSiren;
        public float vehicleVolumeFireTruckSiren;
        public float vehicleVolumeLargeCarMovement;
        public float vehicleVolumeMetroMovement;
        public float vehicleVolumePoliceCarSiren;
        public float vehicleVolumeSmallCarMovement;
        public float vehicleVolumeTrainMovement;
        public float vehicleVolumeTransportArrive;
        public float miscVolumeBuildingBulldoze;
        public float miscVolumeBuildingPlacement;
        public float miscVolumePropBulldoze;
        public float miscVolumePropPlacement;
        public float miscVolumeRoadBulldoze;
        public float miscVolumeRoadDraw;
        public float miscVolumeRoadPlacement;
        public float miscVolumeClick;
        public float miscVolumeDisabledClick;
        public float miscVolumeZoneFill;

        public override void Start()
        {
            this.width = 750;
            this.height = 630;
            this.Title = "SOUNDS TUNER";
            base.Start();

            this.Tabstrip = this.ContentPanel.AddUIComponent<UITabstrip>();
            this.Tabstrip.relativePosition = new Vector3(20, 20);
            this.Tabstrip.width = 700;
            this.Tabstrip.anchor = UIAnchorStyle.Top | UIAnchorStyle.Left | UIAnchorStyle.Right;

            this.TabContainer = this.ContentPanel.AddUIComponent<UITabContainer>();
            this.TabContainer.size = new Vector2(700, 530);
            this.TabContainer.relativePosition = new Vector3(20, 20 + this.Tabstrip.height);
            this.TabContainer.anchor = UIAnchorStyle.All;
            this.Tabstrip.tabPages = this.TabContainer;

            // Get template button from the options panel tabstrip
            UITabstrip keyMappingTabStrip = GameObject.Find(GameObjectDefs.ID_KEYMAPPING_TABSTRIP).GetComponent<UITabstrip>();
            UIButton tabStripButtonTemplate = keyMappingTabStrip.GetComponentInChildren<UIButton>();

            this.AmbientsTabButton = this.Tabstrip.AddTab("AMBIENTS", tabStripButtonTemplate, true);
            this.AmbientsTabButton.playAudioEvents = true;
            this.AmbientsTabButton.focusedTextColor = tabStripButtonTemplate.focusedTextColor;
            this.AmbientsTabButton.width = 140;
            this.AnimalsTabButton = this.Tabstrip.AddTab("ANIMALS", this.AmbientsTabButton, true);
            this.AnimalsTabButton.playAudioEvents = true;
            this.AnimalsTabButton.focusedTextColor = tabStripButtonTemplate.focusedTextColor;
            this.BuildingsTabButton = this.Tabstrip.AddTab("BUILDINGS", this.AmbientsTabButton, true);
            this.BuildingsTabButton.playAudioEvents = true;
            this.BuildingsTabButton.focusedTextColor = tabStripButtonTemplate.focusedTextColor;
            this.VehiclesTabButton = this.Tabstrip.AddTab("VEHICLES", this.AmbientsTabButton, true);
            this.VehiclesTabButton.playAudioEvents = true;
            this.VehiclesTabButton.focusedTextColor = tabStripButtonTemplate.focusedTextColor;
            this.MiscTabButton = this.Tabstrip.AddTab("MISC", this.AmbientsTabButton, true);
            this.MiscTabButton.playAudioEvents = true;
            this.MiscTabButton.focusedTextColor = tabStripButtonTemplate.focusedTextColor;

            // Tabs layout
            UIPanel[] tabs = this.TabContainer.GetComponentsInChildren<UIPanel>();
            this.AmbientsPanel = tabs[0];
            this.AnimalsPanel = tabs[1];
            this.BuildingsPanel = tabs[2];
            this.VehiclesPanel = tabs[3];
            this.MiscPanel = tabs[4];

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

            this.MiscPanel.Hide();
            this.MiscPanel.padding = new RectOffset(0, 0, 10, 10);
            this.MiscPanel.autoLayout = true;
            this.MiscPanel.autoLayoutDirection = LayoutDirection.Vertical;
            this.MiscPanel.autoLayoutPadding = new RectOffset(15, 15, 5, 5);
            this.MiscPanel.wrapLayout = true;

            // Sliders
            this.AddAmbientSlider(AudioManager.AmbientType.Agricultural, new { ambientVolumeAgricultural });
            this.AddAmbientSlider(AudioManager.AmbientType.City, new { ambientVolumeCity });
            this.AddAmbientSlider(AudioManager.AmbientType.Forest, new { ambientVolumeForest });
            this.AddAmbientSlider(AudioManager.AmbientType.Industrial, new { ambientVolumeIndustrial });
            this.AddAmbientSlider(AudioManager.AmbientType.Plaza, new { ambientVolumePlaza });
            this.AddAmbientSlider(AudioManager.AmbientType.Sea, new { ambientVolumeSea });
            this.AddAmbientSlider(AudioManager.AmbientType.Stream, new { ambientVolumeStream });
            this.AddAmbientSlider(AudioManager.AmbientType.Suburban, new { ambientVolumeSuburban });
            this.AddAmbientSlider(AudioManager.AmbientType.World, new { ambientVolumeWorld });

            this.AddAnimalSlider(SoundsCollection.AnimalSounds.ID_COW, new { animalVolumeCow });
            this.AddAnimalSlider(SoundsCollection.AnimalSounds.ID_PIG, new { animalVolumePig });
            this.AddAnimalSlider(SoundsCollection.AnimalSounds.ID_SEAGULL, new { animalVolumeSeagull });

            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_ADVANCED_WIND_TURBINE, new { buildingVolumeAdvancedWindTurbine });
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_AIRPORT, new { buildingVolumeAirport });
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_BUS_DEPOT, new { buildingVolumeBusDepot });
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_CEMETERY, new { buildingVolumeCemetary });
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_COAL_POWER_PLANT, new { buildingVolumeCoalOilPowerPlant });
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_COMMERCIAL, new { buildingVolumeCommercial });
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_CREMATORY, new { buildingVolumeCrematory });
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_ELEMENTARY_SCHOOL, new { buildingVolumeElementarySchool });
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_FIRE_STATION, new { buildingVolumeFireStation });
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_FUSION_POWER_PLANT, new { buildingVolumeFusionPowerPlant }, 0, 4);
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_HARBOR, new { buildingVolumeHarbor });
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_HIGH_SCHOOL, new { buildingVolumeHighSchool });
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_HOSPITAL, new { buildingVolumeHospital });
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_HYDRO_POWER_PLANT, new { buildingVolumeHydroPowerPlant });
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_INCINERATION_PLANT, new { buildingVolumeIncinerationPlant });
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_INDUSTRIAL, new { buildingVolumeIndustrial }, 0, 0.5f);
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_METRO_STATION, new { buildingVolumeMetroStation });
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_NUCLEAR_POWER_PLANT, new { buildingVolumeNuclearPowerPlant });
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_POLICE_STATION, new { buildingVolumePoliceStation });
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_POWER_PLANT_SMALL, new { buildingVolumePowerPlantSmall });
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_SOLAR_POWER_PLANT, new { buildingVolumeSolarPowerPlant });
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_TRAIN_STATION, new { buildingVolumeTrainStation });
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_UNIVERSITY, new { buildingVolumeUniversity });
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_WATER_DRAIN_PIPE, new { buildingVolumeWaterDrainPipe });
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_WATER_PUMPING_STATION, new { buildingVolumeWaterPumpingStation });
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_WIND_TURBINE, new { buildingVolumeWindTurbine });
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_ON_FIRE, new { buildingVolumeOnFire });
            this.AddBuildingSlider(SoundsCollection.BuildingSounds.ID_ON_UPGRADE, new { buildingVolumeOnLevelUp }, 0, 0.25f);

            this.AddVehicleSlider(SoundsCollection.VehicleSounds.ID_AIRCRAFT_MOVEMENT, new { vehicleVolumeAircraftMovement });
            this.AddVehicleSlider(SoundsCollection.VehicleSounds.ID_AMBULANCE_SIREN, new { vehicleVolumeAmbulanceSiren });
            this.AddVehicleSlider(SoundsCollection.VehicleSounds.ID_FIRE_TRUCK_SIREN, new { vehicleVolumeFireTruckSiren }, 0, 3);
            this.AddVehicleSlider(SoundsCollection.VehicleSounds.ID_LARGE_CAR_MOVEMENT, new { vehicleVolumeLargeCarMovement }, 0, 1.5f);
            this.AddVehicleSlider(SoundsCollection.VehicleSounds.ID_METRO_MOVEMENT, new { vehicleVolumeMetroMovement });
            this.AddVehicleSlider(SoundsCollection.VehicleSounds.ID_POLICE_CAR_SIREN, new { vehicleVolumePoliceCarSiren });
            this.AddVehicleSlider(SoundsCollection.VehicleSounds.ID_SMALL_CAR_MOVEMENT, new { vehicleVolumeSmallCarMovement }, 0, 1.5f);
            this.AddVehicleSlider(SoundsCollection.VehicleSounds.ID_TRAIN_MOVEMENT, new { vehicleVolumeTrainMovement });
            this.AddVehicleSlider(SoundsCollection.VehicleSounds.ID_TRANSPORT_ARRIVE, new { vehicleVolumeTransportArrive });

            this.AddMiscSlider(SoundsCollection.MiscSounds.ID_BUILDING_BULLDOZE, new { miscVolumeBuildingBulldoze });
            this.AddMiscSlider(SoundsCollection.MiscSounds.ID_BUILDING_PLACEMENT, new { miscVolumeBuildingPlacement });
            this.AddMiscSlider(SoundsCollection.MiscSounds.ID_PROP_BULLDOZE, new { miscVolumePropBulldoze });
            this.AddMiscSlider(SoundsCollection.MiscSounds.ID_PROP_PLACEMENT, new { miscVolumePropPlacement });
            this.AddMiscSlider(SoundsCollection.MiscSounds.ID_ROAD_BULLDOZE, new { miscVolumeRoadBulldoze });
            this.AddMiscSlider(SoundsCollection.MiscSounds.ID_ROAD_DRAW, new { miscVolumeRoadDraw });
            this.AddMiscSlider(SoundsCollection.MiscSounds.ID_ROAD_PLACEMENT, new { miscVolumeRoadPlacement });
            this.AddMiscSlider(MiscPatcher.ID_CLICK_SOUND, new { miscVolumeClick });
            this.AddMiscSlider(MiscPatcher.ID_DISABLED_CLICK_SOUND, new { miscVolumeDisabledClick });
            this.AddMiscSlider(SoundsCollection.MiscSounds.ID_ZONE_FILL, new { miscVolumeZoneFill });

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
                var animalsPanelChildComponents = ReflectionUtils.GetPrivateField<PoolList<UIComponent>>(this.AnimalsPanel, "m_ChildComponents");
                animalsPanelChildComponents.Sort(new SortSlidersByTextComparer());
                var buildingsPanelChildComponents = ReflectionUtils.GetPrivateField<PoolList<UIComponent>>(this.BuildingsPanel, "m_ChildComponents");
                buildingsPanelChildComponents.Sort(new SortSlidersByTextComparer());
                var vehiclesPanelChildComponents = ReflectionUtils.GetPrivateField<PoolList<UIComponent>>(this.VehiclesPanel, "m_ChildComponents");
                vehiclesPanelChildComponents.Sort(new SortSlidersByTextComparer());
                var miscPanelChildComponents = ReflectionUtils.GetPrivateField<PoolList<UIComponent>>(this.MiscPanel, "m_ChildComponents");
                miscPanelChildComponents.Sort(new SortSlidersByTextComparer());

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

            if (!this.isVisible)
            {
                // Reset the slider sorting, since otherwise the sliders are not correctly sorted anymore after closing the game menu for some reason.
                // This is still partly a workaround, as the sliders might not be sorted sometimes, but it should fix itself.
                this.slidersSorted = false;
            }
        }

        public override void Close()
        {
            Mod.Settings.SaveConfig(Mod.SettingsFilename);
            base.Close();
        }


        #region Slider Helpers

        protected void AddAmbientSlider<F>(AudioManager.AmbientType id, F volumeField, float minValue = 0, float maxValue = 1) where F : class
        {
            this.AddSlider(Mod.Settings.AmbientVolumes, this.AmbientSettingObjects, this.CreateAmbientVolumeSetting, Mod.Instance.AmbientsPatcher, id, volumeField, minValue, maxValue);
        }

        protected void AddAnimalSlider<F>(string id, F volumeField, float minValue = 0, float maxValue = 1) where F : class
        {
            this.AddSlider(Mod.Settings.AnimalVolumes, this.AnimalSettingObjects, this.CreateAnimalVolumeSetting, Mod.Instance.AnimalsPatcher, id, volumeField, minValue, maxValue);
        }

        protected void AddBuildingSlider<F>(string id, F volumeField, float minValue = 0, float maxValue = 1) where F : class
        {
            this.AddSlider(Mod.Settings.BuildingVolumes, this.BuildingSettingObjects, this.CreateBuildingVolumeSetting, Mod.Instance.BuildingsPatcher, id, volumeField, minValue, maxValue);
        }

        protected void AddVehicleSlider<F>(string id, F volumeField, float minValue = 0, float maxValue = 1) where F : class
        {
            this.AddSlider(Mod.Settings.VehicleVolumes, this.VehicleSettingObjects, this.CreateVehicleVolumeSetting, Mod.Instance.VehiclesPatcher, id, volumeField, minValue, maxValue);
        }

        protected void AddMiscSlider<F>(string id, F volumeField, float minValue = 0, float maxValue = 1) where F : class
        {
            this.AddSlider(Mod.Settings.MiscVolumes, this.MiscSettingObjects, this.CreateMiscVolumeSetting, Mod.Instance.MiscPatcher, id, volumeField, minValue, maxValue);
        }

        protected void AddSlider<T, F>(IDictionary<T, float> volumeDictionary, IList<GameObject> settingObjectsList, Func<T, string, float, float, GameObject> volumeSettingCreator, SoundsInstancePatcher<T> patcher, T id, F volumeField, float minValue = 0, float maxValue = 1) where F : class
        {
            float volume;
            volumeDictionary.TryGetValueOrDefault(id, patcher.DefaultVolumes[id], out volume);
            string volumeFieldName = typeof(F).GetProperties()[0].Name;
            FieldInfo volumeFieldInfo = this.GetType().GetField(volumeFieldName);
            volumeFieldInfo.SetValue(this, volume);
            settingObjectsList.Add(volumeSettingCreator(id, volumeFieldName, minValue, maxValue));
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

        protected GameObject CreateMiscVolumeSetting(string id, string memberName, float minValue = 0, float maxValue = 1)
        {
            return CreateVolumeSetting(Mod.Settings.MiscVolumes, Mod.Instance.MiscPatcher, this.MiscPanel, "MiscVolumeSetting", id, memberName, minValue, maxValue);
        }

        protected GameObject CreateVolumeSetting<T>(IDictionary<T, float> volumeDictionary, SoundsInstancePatcher<T> patcher, UIComponent parent, string gameObjectName, T id, string memberName, float minValue = 0, float maxValue = 1)
        {
            PropertyChangedEventHandler<float> valueChangedCallback = new PropertyChangedEventHandler<float>((c, v) =>
            {
                volumeDictionary[id] = v;
                patcher.PatchVolume(id, v);
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
            label.text = name;
            while (label.width > sliderX) label.textScale -= 0.01f;
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

        #endregion


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
