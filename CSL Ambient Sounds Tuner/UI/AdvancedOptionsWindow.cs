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
            { VehiclesPatcher.ID_AIRCRAFT_MOVEMENT, "Aircrafts" },
            { VehiclesPatcher.ID_SMALL_CAR_MOVEMENT, "Cars (Small)" },
            { VehiclesPatcher.ID_LARGE_CAR_MOVEMENT, "Cars (Large)" },
            { VehiclesPatcher.ID_AMBULANCE_SIREN, "Sirens (Ambulances)" },
            { VehiclesPatcher.ID_FIRE_TRUCK_SIREN, "Sirens (Fire Trucks)" },
            { VehiclesPatcher.ID_POLICE_CAR_SIREN, "Sirens (Police Cars)" },
            { VehiclesPatcher.ID_METRO_MOVEMENT, "Metros" },
            { VehiclesPatcher.ID_TRAIN_MOVEMENT, "Trains" },
            { VehiclesPatcher.ID_TRANSPORT_ARRIVE, "Transportation Arrivals" },
        };

        protected List<GameObject> AmbientSettingObjects = new List<GameObject>();
        protected List<GameObject> AnimalSettingObjects = new List<GameObject>();
        protected List<GameObject> BuildingSettingObjects = new List<GameObject>();
        protected List<GameObject> VehiclesSettingObjects = new List<GameObject>();
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
        public float animalVolumeSeagullScream;
        public float buildingVolumeAdvancedWindTurbine;
        public float buildingVolumeCoalOilPowerPlant;
        public float buildingVolumeFireStation;
        public float buildingVolumeFusionPowerPlant;
        public float buildingVolumeHospital;
        public float buildingVolumeHydroPowerPlant;
        public float buildingVolumeIncinerationPlant;
        public float buildingVolumeNuclearPowerPlant;
        public float buildingVolumePoliceStation;
        public float buildingVolumePowerPlantSmall;
        public float buildingVolumeSolarPowerPlant;
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

            // Settings
            Mod.Settings.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Agricultural, Mod.Instance.AmbientsPatcher.DefaultVolumes[AudioManager.AmbientType.Agricultural], out this.ambientVolumeAgricultural);
            Mod.Settings.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.City, Mod.Instance.AmbientsPatcher.DefaultVolumes[AudioManager.AmbientType.City], out this.ambientVolumeCity);
            Mod.Settings.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Forest, Mod.Instance.AmbientsPatcher.DefaultVolumes[AudioManager.AmbientType.Forest], out this.ambientVolumeForest);
            Mod.Settings.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Industrial, Mod.Instance.AmbientsPatcher.DefaultVolumes[AudioManager.AmbientType.Industrial], out this.ambientVolumeIndustrial);
            Mod.Settings.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Plaza, Mod.Instance.AmbientsPatcher.DefaultVolumes[AudioManager.AmbientType.Plaza], out this.ambientVolumePlaza);
            Mod.Settings.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Sea, Mod.Instance.AmbientsPatcher.DefaultVolumes[AudioManager.AmbientType.Sea], out this.ambientVolumeSea);
            Mod.Settings.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Stream, Mod.Instance.AmbientsPatcher.DefaultVolumes[AudioManager.AmbientType.Stream], out this.ambientVolumeStream);
            Mod.Settings.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Suburban, Mod.Instance.AmbientsPatcher.DefaultVolumes[AudioManager.AmbientType.Suburban], out this.ambientVolumeSuburban);
            Mod.Settings.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.World, Mod.Instance.AmbientsPatcher.DefaultVolumes[AudioManager.AmbientType.World], out this.ambientVolumeWorld);

            Mod.Settings.AnimalVolumes.TryGetValueOrDefault(AnimalsPatcher.ID_COW, Mod.Instance.AnimalsPatcher.DefaultVolumes[AnimalsPatcher.ID_COW], out this.animalVolumeCow);
            Mod.Settings.AnimalVolumes.TryGetValueOrDefault(AnimalsPatcher.ID_PIG, Mod.Instance.AnimalsPatcher.DefaultVolumes[AnimalsPatcher.ID_PIG], out this.animalVolumePig);
            Mod.Settings.AnimalVolumes.TryGetValueOrDefault(AnimalsPatcher.ID_SEAGULL, Mod.Instance.AnimalsPatcher.DefaultVolumes[AnimalsPatcher.ID_SEAGULL], out this.animalVolumeSeagullScream);

            Mod.Settings.BuildingVolumes.TryGetValueOrDefault(BuildingsPatcher.ID_ADVANCED_WIND_TURBINE, Mod.Instance.BuildingsPatcher.DefaultVolumes[BuildingsPatcher.ID_ADVANCED_WIND_TURBINE], out this.buildingVolumeAdvancedWindTurbine);
            Mod.Settings.BuildingVolumes.TryGetValueOrDefault(BuildingsPatcher.ID_COAL_OIL_POWER_PLANT, Mod.Instance.BuildingsPatcher.DefaultVolumes[BuildingsPatcher.ID_COAL_OIL_POWER_PLANT], out this.buildingVolumeCoalOilPowerPlant);
            Mod.Settings.BuildingVolumes.TryGetValueOrDefault(BuildingsPatcher.ID_FIRE_STATION, Mod.Instance.BuildingsPatcher.DefaultVolumes[BuildingsPatcher.ID_FIRE_STATION], out this.buildingVolumeFireStation);
            Mod.Settings.BuildingVolumes.TryGetValueOrDefault(BuildingsPatcher.ID_FUSION_POWER_PLANT, Mod.Instance.BuildingsPatcher.DefaultVolumes[BuildingsPatcher.ID_FUSION_POWER_PLANT], out this.buildingVolumeFusionPowerPlant);
            Mod.Settings.BuildingVolumes.TryGetValueOrDefault(BuildingsPatcher.ID_HOSPITAL, Mod.Instance.BuildingsPatcher.DefaultVolumes[BuildingsPatcher.ID_HOSPITAL], out this.buildingVolumeHospital);
            Mod.Settings.BuildingVolumes.TryGetValueOrDefault(BuildingsPatcher.ID_HYDRO_POWER_PLANT, Mod.Instance.BuildingsPatcher.DefaultVolumes[BuildingsPatcher.ID_HYDRO_POWER_PLANT], out this.buildingVolumeHydroPowerPlant);
            Mod.Settings.BuildingVolumes.TryGetValueOrDefault(BuildingsPatcher.ID_INCINERATION_PLANT, Mod.Instance.BuildingsPatcher.DefaultVolumes[BuildingsPatcher.ID_INCINERATION_PLANT], out this.buildingVolumeIncinerationPlant);
            Mod.Settings.BuildingVolumes.TryGetValueOrDefault(BuildingsPatcher.ID_NUCLEAR_POWER_PLANT, Mod.Instance.BuildingsPatcher.DefaultVolumes[BuildingsPatcher.ID_NUCLEAR_POWER_PLANT], out this.buildingVolumeNuclearPowerPlant);
            Mod.Settings.BuildingVolumes.TryGetValueOrDefault(BuildingsPatcher.ID_POLICE_STATION, Mod.Instance.BuildingsPatcher.DefaultVolumes[BuildingsPatcher.ID_POLICE_STATION], out this.buildingVolumePoliceStation);
            Mod.Settings.BuildingVolumes.TryGetValueOrDefault(BuildingsPatcher.ID_POWER_PLANT_SMALL, Mod.Instance.BuildingsPatcher.DefaultVolumes[BuildingsPatcher.ID_POWER_PLANT_SMALL], out this.buildingVolumePowerPlantSmall);
            Mod.Settings.BuildingVolumes.TryGetValueOrDefault(BuildingsPatcher.ID_SOLAR_POWER_PLANT, Mod.Instance.BuildingsPatcher.DefaultVolumes[BuildingsPatcher.ID_SOLAR_POWER_PLANT], out this.buildingVolumeSolarPowerPlant);
            Mod.Settings.BuildingVolumes.TryGetValueOrDefault(BuildingsPatcher.ID_WATER_DRAIN_PIPE, Mod.Instance.BuildingsPatcher.DefaultVolumes[BuildingsPatcher.ID_WATER_DRAIN_PIPE], out this.buildingVolumeWaterDrainPipe);
            Mod.Settings.BuildingVolumes.TryGetValueOrDefault(BuildingsPatcher.ID_WATER_PUMPING_STATION, Mod.Instance.BuildingsPatcher.DefaultVolumes[BuildingsPatcher.ID_WATER_PUMPING_STATION], out this.buildingVolumeWaterPumpingStation);
            Mod.Settings.BuildingVolumes.TryGetValueOrDefault(BuildingsPatcher.ID_WIND_TURBINE, Mod.Instance.BuildingsPatcher.DefaultVolumes[BuildingsPatcher.ID_WIND_TURBINE], out this.buildingVolumeWindTurbine);
            Mod.Settings.BuildingVolumes.TryGetValueOrDefault(BuildingsPatcher.ID_FIRE, Mod.Instance.BuildingsPatcher.DefaultVolumes[BuildingsPatcher.ID_FIRE], out this.buildingVolumeOnFire);
            Mod.Settings.BuildingVolumes.TryGetValueOrDefault(BuildingsPatcher.ID_LEVELUP, Mod.Instance.BuildingsPatcher.DefaultVolumes[BuildingsPatcher.ID_LEVELUP], out this.buildingVolumeOnLevelUp);

            Mod.Settings.VehicleVolumes.TryGetValueOrDefault(VehiclesPatcher.ID_AIRCRAFT_MOVEMENT, Mod.Instance.VehiclesPatcher.DefaultVolumes[VehiclesPatcher.ID_AIRCRAFT_MOVEMENT], out this.vehicleVolumeAircraftMovement);
            Mod.Settings.VehicleVolumes.TryGetValueOrDefault(VehiclesPatcher.ID_AMBULANCE_SIREN, Mod.Instance.VehiclesPatcher.DefaultVolumes[VehiclesPatcher.ID_AMBULANCE_SIREN], out this.vehicleVolumeAmbulanceSiren);
            Mod.Settings.VehicleVolumes.TryGetValueOrDefault(VehiclesPatcher.ID_FIRE_TRUCK_SIREN, Mod.Instance.VehiclesPatcher.DefaultVolumes[VehiclesPatcher.ID_FIRE_TRUCK_SIREN], out this.vehicleVolumeFireTruckSiren);
            Mod.Settings.VehicleVolumes.TryGetValueOrDefault(VehiclesPatcher.ID_LARGE_CAR_MOVEMENT, Mod.Instance.VehiclesPatcher.DefaultVolumes[VehiclesPatcher.ID_LARGE_CAR_MOVEMENT], out this.vehicleVolumeLargeCarMovement);
            Mod.Settings.VehicleVolumes.TryGetValueOrDefault(VehiclesPatcher.ID_METRO_MOVEMENT, Mod.Instance.VehiclesPatcher.DefaultVolumes[VehiclesPatcher.ID_METRO_MOVEMENT], out this.vehicleVolumeMetroMovement);
            Mod.Settings.VehicleVolumes.TryGetValueOrDefault(VehiclesPatcher.ID_POLICE_CAR_SIREN, Mod.Instance.VehiclesPatcher.DefaultVolumes[VehiclesPatcher.ID_POLICE_CAR_SIREN], out this.vehicleVolumePoliceCarSiren);
            Mod.Settings.VehicleVolumes.TryGetValueOrDefault(VehiclesPatcher.ID_SMALL_CAR_MOVEMENT, Mod.Instance.VehiclesPatcher.DefaultVolumes[VehiclesPatcher.ID_SMALL_CAR_MOVEMENT], out this.vehicleVolumeSmallCarMovement);
            Mod.Settings.VehicleVolumes.TryGetValueOrDefault(VehiclesPatcher.ID_TRAIN_MOVEMENT, Mod.Instance.VehiclesPatcher.DefaultVolumes[VehiclesPatcher.ID_TRAIN_MOVEMENT], out this.vehicleVolumeTrainMovement);
            Mod.Settings.VehicleVolumes.TryGetValueOrDefault(VehiclesPatcher.ID_TRANSPORT_ARRIVE, Mod.Instance.VehiclesPatcher.DefaultVolumes[VehiclesPatcher.ID_TRANSPORT_ARRIVE], out this.vehicleVolumeTransportArrive);

            Mod.Settings.MiscVolumes.TryGetValueOrDefault(MiscPatcher.ID_BUILDING_BULLDOZE, Mod.Instance.MiscPatcher.DefaultVolumes[MiscPatcher.ID_BUILDING_BULLDOZE], out this.miscVolumeBuildingBulldoze);
            Mod.Settings.MiscVolumes.TryGetValueOrDefault(MiscPatcher.ID_BUILDING_PLACEMENT, Mod.Instance.MiscPatcher.DefaultVolumes[MiscPatcher.ID_BUILDING_PLACEMENT], out this.miscVolumeBuildingPlacement);
            Mod.Settings.MiscVolumes.TryGetValueOrDefault(MiscPatcher.ID_PROP_BULLDOZE, Mod.Instance.MiscPatcher.DefaultVolumes[MiscPatcher.ID_PROP_BULLDOZE], out this.miscVolumePropBulldoze);
            Mod.Settings.MiscVolumes.TryGetValueOrDefault(MiscPatcher.ID_PROP_PLACEMENT, Mod.Instance.MiscPatcher.DefaultVolumes[MiscPatcher.ID_PROP_PLACEMENT], out this.miscVolumePropPlacement);
            Mod.Settings.MiscVolumes.TryGetValueOrDefault(MiscPatcher.ID_ROAD_BULLDOZE, Mod.Instance.MiscPatcher.DefaultVolumes[MiscPatcher.ID_ROAD_BULLDOZE], out this.miscVolumeRoadBulldoze);
            Mod.Settings.MiscVolumes.TryGetValueOrDefault(MiscPatcher.ID_ROAD_DRAW, Mod.Instance.MiscPatcher.DefaultVolumes[MiscPatcher.ID_ROAD_DRAW], out this.miscVolumeRoadDraw);
            Mod.Settings.MiscVolumes.TryGetValueOrDefault(MiscPatcher.ID_ROAD_PLACEMENT, Mod.Instance.MiscPatcher.DefaultVolumes[MiscPatcher.ID_ROAD_PLACEMENT], out this.miscVolumeRoadPlacement);
            Mod.Settings.MiscVolumes.TryGetValueOrDefault(MiscPatcher.ID_CLICK_SOUND, Mod.Instance.MiscPatcher.DefaultVolumes[MiscPatcher.ID_CLICK_SOUND], out this.miscVolumeClick);
            Mod.Settings.MiscVolumes.TryGetValueOrDefault(MiscPatcher.ID_DISABLED_CLICK_SOUND, Mod.Instance.MiscPatcher.DefaultVolumes[MiscPatcher.ID_DISABLED_CLICK_SOUND], out this.miscVolumeDisabledClick);
            Mod.Settings.MiscVolumes.TryGetValueOrDefault(MiscPatcher.ID_ZONE_FILL, Mod.Instance.MiscPatcher.DefaultVolumes[MiscPatcher.ID_ZONE_FILL], out this.miscVolumeZoneFill);

            // Sliders
            this.AmbientSettingObjects.Add(this.CreateAmbientVolumeSetting(AudioManager.AmbientType.Agricultural, "ambientVolumeAgricultural"));
            this.AmbientSettingObjects.Add(this.CreateAmbientVolumeSetting(AudioManager.AmbientType.City, "ambientVolumeCity"));
            this.AmbientSettingObjects.Add(this.CreateAmbientVolumeSetting(AudioManager.AmbientType.Forest, "ambientVolumeForest"));
            this.AmbientSettingObjects.Add(this.CreateAmbientVolumeSetting(AudioManager.AmbientType.Industrial, "ambientVolumeIndustrial"));
            this.AmbientSettingObjects.Add(this.CreateAmbientVolumeSetting(AudioManager.AmbientType.Plaza, "ambientVolumePlaza"));
            this.AmbientSettingObjects.Add(this.CreateAmbientVolumeSetting(AudioManager.AmbientType.Sea, "ambientVolumeSea"));
            this.AmbientSettingObjects.Add(this.CreateAmbientVolumeSetting(AudioManager.AmbientType.Stream, "ambientVolumeStream"));
            this.AmbientSettingObjects.Add(this.CreateAmbientVolumeSetting(AudioManager.AmbientType.Suburban, "ambientVolumeSuburban"));
            this.AmbientSettingObjects.Add(this.CreateAmbientVolumeSetting(AudioManager.AmbientType.World, "ambientVolumeWorld"));

            this.AnimalSettingObjects.Add(this.CreateAnimalVolumeSetting(AnimalsPatcher.ID_COW, "animalVolumeCow"));
            this.AnimalSettingObjects.Add(this.CreateAnimalVolumeSetting(AnimalsPatcher.ID_PIG, "animalVolumePig"));
            this.AnimalSettingObjects.Add(this.CreateAnimalVolumeSetting(AnimalsPatcher.ID_SEAGULL, "animalVolumeSeagullScream"));

            this.BuildingSettingObjects.Add(this.CreateBuildingVolumeSetting(BuildingsPatcher.ID_ADVANCED_WIND_TURBINE, "buildingVolumeAdvancedWindTurbine"));
            this.BuildingSettingObjects.Add(this.CreateBuildingVolumeSetting(BuildingsPatcher.ID_COAL_OIL_POWER_PLANT, "buildingVolumeCoalOilPowerPlant"));
            this.BuildingSettingObjects.Add(this.CreateBuildingVolumeSetting(BuildingsPatcher.ID_FIRE_STATION, "buildingVolumeFireStation"));
            this.BuildingSettingObjects.Add(this.CreateBuildingVolumeSetting(BuildingsPatcher.ID_FUSION_POWER_PLANT, "buildingVolumeFusionPowerPlant", 0, 4));
            this.BuildingSettingObjects.Add(this.CreateBuildingVolumeSetting(BuildingsPatcher.ID_HOSPITAL, "buildingVolumeHospital"));
            this.BuildingSettingObjects.Add(this.CreateBuildingVolumeSetting(BuildingsPatcher.ID_HYDRO_POWER_PLANT, "buildingVolumeHydroPowerPlant"));
            this.BuildingSettingObjects.Add(this.CreateBuildingVolumeSetting(BuildingsPatcher.ID_INCINERATION_PLANT, "buildingVolumeIncinerationPlant"));
            this.BuildingSettingObjects.Add(this.CreateBuildingVolumeSetting(BuildingsPatcher.ID_NUCLEAR_POWER_PLANT, "buildingVolumeNuclearPowerPlant"));
            this.BuildingSettingObjects.Add(this.CreateBuildingVolumeSetting(BuildingsPatcher.ID_POLICE_STATION, "buildingVolumePoliceStation"));
            this.BuildingSettingObjects.Add(this.CreateBuildingVolumeSetting(BuildingsPatcher.ID_POWER_PLANT_SMALL, "buildingVolumePowerPlantSmall"));
            this.BuildingSettingObjects.Add(this.CreateBuildingVolumeSetting(BuildingsPatcher.ID_SOLAR_POWER_PLANT, "buildingVolumeSolarPowerPlant"));
            this.BuildingSettingObjects.Add(this.CreateBuildingVolumeSetting(BuildingsPatcher.ID_WATER_DRAIN_PIPE, "buildingVolumeWaterDrainPipe"));
            this.BuildingSettingObjects.Add(this.CreateBuildingVolumeSetting(BuildingsPatcher.ID_WATER_PUMPING_STATION, "buildingVolumeWaterPumpingStation"));
            this.BuildingSettingObjects.Add(this.CreateBuildingVolumeSetting(BuildingsPatcher.ID_WIND_TURBINE, "buildingVolumeWindTurbine"));
            this.BuildingSettingObjects.Add(this.CreateBuildingVolumeSetting(BuildingsPatcher.ID_FIRE, "buildingVolumeOnFire"));
            this.BuildingSettingObjects.Add(this.CreateBuildingVolumeSetting(BuildingsPatcher.ID_LEVELUP, "buildingVolumeOnLevelUp", 0, 0.25f));

            this.VehiclesSettingObjects.Add(this.CreateVehicleVolumeSetting(VehiclesPatcher.ID_AIRCRAFT_MOVEMENT, "vehicleVolumeAircraftMovement"));
            this.VehiclesSettingObjects.Add(this.CreateVehicleVolumeSetting(VehiclesPatcher.ID_AMBULANCE_SIREN, "vehicleVolumeAmbulanceSiren"));
            this.VehiclesSettingObjects.Add(this.CreateVehicleVolumeSetting(VehiclesPatcher.ID_FIRE_TRUCK_SIREN, "vehicleVolumeFireTruckSiren", 0, 3));
            this.VehiclesSettingObjects.Add(this.CreateVehicleVolumeSetting(VehiclesPatcher.ID_LARGE_CAR_MOVEMENT, "vehicleVolumeLargeCarMovement", 0, 1.5f));
            this.VehiclesSettingObjects.Add(this.CreateVehicleVolumeSetting(VehiclesPatcher.ID_METRO_MOVEMENT, "vehicleVolumeMetroMovement"));
            this.VehiclesSettingObjects.Add(this.CreateVehicleVolumeSetting(VehiclesPatcher.ID_POLICE_CAR_SIREN, "vehicleVolumePoliceCarSiren"));
            this.VehiclesSettingObjects.Add(this.CreateVehicleVolumeSetting(VehiclesPatcher.ID_SMALL_CAR_MOVEMENT, "vehicleVolumeSmallCarMovement", 0, 1.5f));
            this.VehiclesSettingObjects.Add(this.CreateVehicleVolumeSetting(VehiclesPatcher.ID_TRAIN_MOVEMENT, "vehicleVolumeTrainMovement"));
            this.VehiclesSettingObjects.Add(this.CreateVehicleVolumeSetting(VehiclesPatcher.ID_TRANSPORT_ARRIVE, "vehicleVolumeTransportArrive"));

            this.MiscSettingObjects.Add(this.CreateMiscVolumeSetting(MiscPatcher.ID_BUILDING_BULLDOZE, "miscVolumeBuildingBulldoze"));
            this.MiscSettingObjects.Add(this.CreateMiscVolumeSetting(MiscPatcher.ID_BUILDING_PLACEMENT, "miscVolumeBuildingPlacement"));
            this.MiscSettingObjects.Add(this.CreateMiscVolumeSetting(MiscPatcher.ID_PROP_BULLDOZE, "miscVolumePropBulldoze"));
            this.MiscSettingObjects.Add(this.CreateMiscVolumeSetting(MiscPatcher.ID_PROP_PLACEMENT, "miscVolumePropPlacement"));
            this.MiscSettingObjects.Add(this.CreateMiscVolumeSetting(MiscPatcher.ID_ROAD_BULLDOZE, "miscVolumeRoadBulldoze"));
            this.MiscSettingObjects.Add(this.CreateMiscVolumeSetting(MiscPatcher.ID_ROAD_DRAW, "miscVolumeRoadDraw"));
            this.MiscSettingObjects.Add(this.CreateMiscVolumeSetting(MiscPatcher.ID_ROAD_PLACEMENT, "miscVolumeRoadPlacement"));
            this.MiscSettingObjects.Add(this.CreateMiscVolumeSetting(MiscPatcher.ID_CLICK_SOUND, "miscVolumeClick"));
            this.MiscSettingObjects.Add(this.CreateMiscVolumeSetting(MiscPatcher.ID_DISABLED_CLICK_SOUND, "miscVolumeDisabledClick"));
            this.MiscSettingObjects.Add(this.CreateMiscVolumeSetting(MiscPatcher.ID_ZONE_FILL, "miscVolumeZoneFill"));

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
