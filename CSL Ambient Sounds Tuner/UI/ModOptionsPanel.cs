using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AmbientSoundsTuner.Defs;
using AmbientSoundsTuner.SoundPatchers;
using AmbientSoundsTuner.UI.Extensions;
using AmbientSoundsTuner.Utils;
using ColossalFramework;
using ColossalFramework.DataBinding;
using ColossalFramework.UI;
using CommonShared.Extensions;
using CommonShared.UI;
using CommonShared.Utils;
using ICities;
using UnityEngine;

namespace AmbientSoundsTuner.UI
{
    public class ModOptionsPanel
    {
        protected readonly Dictionary<string, string> SliderNames = new Dictionary<string, string>()
        {
            { SoundsCollection.AnimalSounds.ID_COW, "Cows" },
            { SoundsCollection.AnimalSounds.ID_PIG, "Pigs" },
            { SoundsCollection.AnimalSounds.ID_SEAGULL, "Seagulls" },

            { SoundsCollection.BuildingSounds.ID_ADVANCED_WIND_TURBINE, "Advanced Wind Turbine" },
            { SoundsCollection.BuildingSounds.ID_COAL_POWER_PLANT, "Coal/Oil Power Plant" },
            { SoundsCollection.BuildingSounds.ID_FUSION_POWER_PLANT, "Fusion Power Plant" },
            { SoundsCollection.BuildingSounds.ID_HYDRO_POWER_PLANT, "Hydro Power Plant" },
            { SoundsCollection.BuildingSounds.ID_INCINERATION_PLANT, "Incineration Plant" },
            { SoundsCollection.BuildingSounds.ID_NUCLEAR_POWER_PLANT, "Nuclear Power Plant" },
            { SoundsCollection.BuildingSounds.ID_POWER_PLANT_SMALL, "Small Power Plant" },
            { SoundsCollection.BuildingSounds.ID_SOLAR_POWER_PLANT, "Solar Power Plant" },
            { SoundsCollection.BuildingSounds.ID_WATER_DRAIN_PIPE, "Water Drain/Treatment Plant" },
            { SoundsCollection.BuildingSounds.ID_WATER_PUMPING_STATION, "Water Pumping Station" },
            { SoundsCollection.BuildingSounds.ID_WIND_TURBINE, "Wind Turbine" },
            { SoundsCollection.BuildingSounds.ID_CEMETERY, "Cemetery" },
            { SoundsCollection.BuildingSounds.ID_CREMATORY, "Crematory" },
            { SoundsCollection.BuildingSounds.ID_ELEMENTARY_SCHOOL, "Elementary School" },
            { SoundsCollection.BuildingSounds.ID_FIRE_STATION, "Fire Station" },
            { SoundsCollection.BuildingSounds.ID_HIGH_SCHOOL, "High School" },
            { SoundsCollection.BuildingSounds.ID_POLICE_STATION, "Police Station" },
            { SoundsCollection.BuildingSounds.ID_UNIVERSITY, "University" },
            { SoundsCollection.BuildingSounds.ID_HOSPITAL, "Hospital" },
            { SoundsCollection.BuildingSounds.ID_AIRPORT, "Airport" },
            { SoundsCollection.BuildingSounds.ID_BUS_DEPOT, "Bus Depot" },
            { SoundsCollection.BuildingSounds.ID_HARBOR, "Harbor" },
            { SoundsCollection.BuildingSounds.ID_METRO_STATION, "Metro Station" },
            { SoundsCollection.BuildingSounds.ID_TRAIN_STATION, "Train Station" },
            { SoundsCollection.BuildingSounds.ID_COMMERCIAL, "Commercial" },
            { SoundsCollection.BuildingSounds.ID_INDUSTRIAL, "Industrial" },
            { SoundsCollection.BuildingSounds.ID_ON_FIRE, "On Fire" },
            { SoundsCollection.BuildingSounds.ID_ON_UPGRADE, "On Upgrade" },

            { SoundsCollection.VehicleSounds.ID_AIRCRAFT_MOVEMENT, "Aircrafts" },
            { SoundsCollection.VehicleSounds.ID_LARGE_CAR_SOUND, "Large Cars" },
            { SoundsCollection.VehicleSounds.ID_METRO_MOVEMENT, "Metros" },
            { SoundsCollection.VehicleSounds.ID_SMALL_CAR_SOUND, "Small Cars/Scooters" },
            { SoundsCollection.VehicleSounds.ID_TRAIN_MOVEMENT, "Trains" },
            { SoundsCollection.VehicleSounds.ID_AMBULANCE_SIREN, "Ambulances" },
            { SoundsCollection.VehicleSounds.ID_FIRE_TRUCK_SIREN, "Fire Trucks" },
            { SoundsCollection.VehicleSounds.ID_POLICE_CAR_SIREN, "Police Cars" },
            { SoundsCollection.VehicleSounds.ID_TRANSPORT_ARRIVE, "Transportation Arrivals" },

            { SoundsCollection.MiscSounds.ID_BUILDING_BULLDOZE, "Buildings" },
            { SoundsCollection.MiscSounds.ID_PROP_BULLDOZE, "Props" },
            { SoundsCollection.MiscSounds.ID_ROAD_BULLDOZE, "Roads" },
            { SoundsCollection.MiscSounds.ID_BUILDING_PLACEMENT, "Buildings" },
            { SoundsCollection.MiscSounds.ID_PROP_PLACEMENT, "Props" },
            { SoundsCollection.MiscSounds.ID_ROAD_PLACEMENT, "Roads" },
            { MiscPatcher.ID_CLICK_SOUND, "Clicks" },
            { MiscPatcher.ID_DISABLED_CLICK_SOUND, "Clicks (disabled components)" },
            { SoundsCollection.MiscSounds.ID_ROAD_DRAW, "Road Drawer" },
            { SoundsCollection.MiscSounds.ID_ZONE_FILL, "Zone Filler" },
     };

        protected UITabstrip Tabstrip;
        protected UIButton AmbientsTabButton;
        protected UIButton AnimalsTabButton;
        protected UIButton BuildingsTabButton;
        protected UIButton VehiclesTabButton;
        protected UIButton MiscTabButton;

        protected UITabContainer TabContainer;
        protected UIScrollablePanel AmbientsPanel;
        protected UIScrollablePanel AnimalsPanel;
        protected UIScrollablePanel BuildingsPanel;
        protected UIScrollablePanel VehiclesPanel;
        protected UIScrollablePanel MiscPanel;

        private UIHelper rootHelper;

        public UIScrollablePanel RootPanel
        {
            get
            {
                return this.rootHelper.self as UIScrollablePanel;
            }
        }

        public ModOptionsPanel(UIHelper helper)
        {
            this.rootHelper = helper;
        }

        public void PopulateUI()
        {
            if (this.RootPanel == null)
            {
                Mod.Log.Warning("Could not populate options panel, panel is null or not a UIScrollablePanel");
                return;
            }

            // Set root panel options
            this.RootPanel.autoLayout = false;
            Vector2 panelInnerSize = new Vector2(this.RootPanel.width - ((UIPanel)this.RootPanel.parent).padding.horizontal - 20, this.RootPanel.height - ((UIPanel)this.RootPanel.parent).padding.vertical - 20);

            // Create tab strip
            this.Tabstrip = this.RootPanel.AddUIComponent<UITabstrip>();
            this.Tabstrip.relativePosition = new Vector3(0, 0);
            this.Tabstrip.size = new Vector2(panelInnerSize.x, 40);
            this.Tabstrip.anchor = UIAnchorStyle.Top | UIAnchorStyle.Left | UIAnchorStyle.Right;

            // Create tab container
            this.TabContainer = this.RootPanel.AddUIComponent<UITabContainer>();
            this.TabContainer.relativePosition = new Vector3(0, this.Tabstrip.height);
            this.TabContainer.size = new Vector2(panelInnerSize.x, panelInnerSize.y - this.Tabstrip.height);
            this.TabContainer.anchor = UIAnchorStyle.All;
            this.Tabstrip.tabPages = this.TabContainer;

            // Create tabs
            UITabstrip keyMappingTabStrip = GameObject.Find(GameObjectDefs.ID_KEYMAPPING_TABSTRIP).GetComponent<UITabstrip>();
            UIButton buttonTemplate = keyMappingTabStrip.GetComponentInChildren<UIButton>();
            int tabWidth = (int)(panelInnerSize.x / 5);
            this.AddTab(this.Tabstrip, buttonTemplate, "Ambients", tabWidth);
            this.AddTab(this.Tabstrip, buttonTemplate, "Animals", tabWidth);
            this.AddTab(this.Tabstrip, buttonTemplate, "Buildings", tabWidth);
            this.AddTab(this.Tabstrip, buttonTemplate, "Vehicles", tabWidth);
            this.AddTab(this.Tabstrip, buttonTemplate, "Misc", tabWidth);

            // Tabs layout
            UIScrollablePanel[] tabs = this.TabContainer.components.Cast<UIPanel>().Select(p => p.GetComponentInChildren<UIScrollablePanel>()).ToArray();
            this.AmbientsPanel = tabs[0];
            this.AnimalsPanel = tabs[1];
            this.BuildingsPanel = tabs[2];
            this.VehiclesPanel = tabs[3];
            this.MiscPanel = tabs[4];

            foreach (UIScrollablePanel tab in tabs)
            {
                tab.autoLayout = true;
                tab.autoLayoutDirection = LayoutDirection.Vertical;
                tab.autoLayoutPadding = new RectOffset(0, 0, 2, 0);
            }

            this.Tabstrip.selectedIndex = -1;
            this.Tabstrip.selectedIndex = 0;

            // Sliders
            UIHelper helper = null;
            UIHelperBase groupHelper = null;

            helper = new UIHelper(this.AmbientsPanel);
            groupHelper = this.AddGroup(helper, "Ambients");
            this.AddAmbientVolumeSlider(groupHelper, AudioManager.AmbientType.Agricultural);
            this.AddAmbientVolumeSlider(groupHelper, AudioManager.AmbientType.City);
            this.AddAmbientVolumeSlider(groupHelper, AudioManager.AmbientType.Forest);
            this.AddAmbientVolumeSlider(groupHelper, AudioManager.AmbientType.Industrial);
            this.AddAmbientVolumeSlider(groupHelper, AudioManager.AmbientType.Plaza);
            this.AddAmbientVolumeSlider(groupHelper, AudioManager.AmbientType.Sea);
            this.AddAmbientVolumeSlider(groupHelper, AudioManager.AmbientType.Stream);
            this.AddAmbientVolumeSlider(groupHelper, AudioManager.AmbientType.Suburban);
            this.AddAmbientVolumeSlider(groupHelper, AudioManager.AmbientType.World);

            helper = new UIHelper(this.AnimalsPanel);
            groupHelper = this.AddGroup(helper, "Animals");
            this.AddAnimalVolumeSlider(groupHelper, SoundsCollection.AnimalSounds.ID_COW);
            this.AddAnimalVolumeSlider(groupHelper, SoundsCollection.AnimalSounds.ID_PIG);
            this.AddAnimalVolumeSlider(groupHelper, SoundsCollection.AnimalSounds.ID_SEAGULL);

            helper = new UIHelper(this.BuildingsPanel);
            groupHelper = this.AddGroup(helper, "Electricity and Water");
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_ADVANCED_WIND_TURBINE);
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_COAL_POWER_PLANT);
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_FUSION_POWER_PLANT, 0, 4);
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_HYDRO_POWER_PLANT);
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_INCINERATION_PLANT);
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_NUCLEAR_POWER_PLANT);
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_POWER_PLANT_SMALL);
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_SOLAR_POWER_PLANT);
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_WATER_DRAIN_PIPE);
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_WATER_PUMPING_STATION);
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_WIND_TURBINE);
            groupHelper = this.AddGroup(helper, "Services");
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_CEMETERY);
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_CREMATORY);
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_ELEMENTARY_SCHOOL);
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_FIRE_STATION);
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_HIGH_SCHOOL);
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_HOSPITAL);
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_POLICE_STATION);
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_UNIVERSITY);
            groupHelper = this.AddGroup(helper, "Public Transport");
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_AIRPORT);
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_BUS_DEPOT);
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_HARBOR);
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_METRO_STATION);
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_TRAIN_STATION);
            groupHelper = this.AddGroup(helper, "Other");
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_COMMERCIAL);
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_INDUSTRIAL, 0, 0.5f);
            groupHelper = this.AddGroup(helper, "Miscellaneous");
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_ON_FIRE);
            this.AddBuildingVolumeSlider(groupHelper, SoundsCollection.BuildingSounds.ID_ON_UPGRADE, 0, 0.25f);

            helper = new UIHelper(this.VehiclesPanel);
            groupHelper = this.AddGroup(helper, "Engines");
            this.AddVehicleVolumeSlider(groupHelper, SoundsCollection.VehicleSounds.ID_AIRCRAFT_MOVEMENT);
            this.AddVehicleVolumeSlider(groupHelper, SoundsCollection.VehicleSounds.ID_LARGE_CAR_SOUND, 0, 1.5f);
            this.AddVehicleVolumeSlider(groupHelper, SoundsCollection.VehicleSounds.ID_METRO_MOVEMENT);
            this.AddVehicleVolumeSlider(groupHelper, SoundsCollection.VehicleSounds.ID_SMALL_CAR_SOUND, 0, 1.5f);
            this.AddVehicleVolumeSlider(groupHelper, SoundsCollection.VehicleSounds.ID_TRAIN_MOVEMENT);
            groupHelper = this.AddGroup(helper, "Sirens");
            this.AddVehicleVolumeSlider(groupHelper, SoundsCollection.VehicleSounds.ID_AMBULANCE_SIREN);
            this.AddVehicleVolumeSlider(groupHelper, SoundsCollection.VehicleSounds.ID_FIRE_TRUCK_SIREN, 0, 3);
            this.AddVehicleVolumeSlider(groupHelper, SoundsCollection.VehicleSounds.ID_POLICE_CAR_SIREN);
            groupHelper = this.AddGroup(helper, "Miscellaneous");
            this.AddVehicleVolumeSlider(groupHelper, SoundsCollection.VehicleSounds.ID_TRANSPORT_ARRIVE);

            helper = new UIHelper(this.MiscPanel);
            groupHelper = this.AddGroup(helper, "Bulldozer");
            this.AddMiscVolumeSlider(groupHelper, SoundsCollection.MiscSounds.ID_BUILDING_BULLDOZE);
            this.AddMiscVolumeSlider(groupHelper, SoundsCollection.MiscSounds.ID_PROP_BULLDOZE);
            this.AddMiscVolumeSlider(groupHelper, SoundsCollection.MiscSounds.ID_ROAD_BULLDOZE);
            groupHelper = this.AddGroup(helper, "Placements");
            this.AddMiscVolumeSlider(groupHelper, SoundsCollection.MiscSounds.ID_BUILDING_PLACEMENT);
            this.AddMiscVolumeSlider(groupHelper, SoundsCollection.MiscSounds.ID_PROP_PLACEMENT);
            this.AddMiscVolumeSlider(groupHelper, SoundsCollection.MiscSounds.ID_ROAD_PLACEMENT);
            groupHelper = this.AddGroup(helper, "User Interface");
            this.AddMiscVolumeSlider(groupHelper, MiscPatcher.ID_CLICK_SOUND);
            this.AddMiscVolumeSlider(groupHelper, MiscPatcher.ID_DISABLED_CLICK_SOUND);
            this.AddMiscVolumeSlider(groupHelper, SoundsCollection.MiscSounds.ID_ROAD_DRAW);
            this.AddMiscVolumeSlider(groupHelper, SoundsCollection.MiscSounds.ID_ZONE_FILL);
        }

        protected void AddTab(UITabstrip tabStrip, UIButton buttonTemplate, string buttonName, float buttonWidth)
        {
            UIPanel panelTemplate = (UIPanel)UITemplateManager.Peek("OptionsScrollPanelTemplate");
            UIButton button = tabStrip.AddScrollableTab(buttonName, buttonTemplate, panelTemplate, true);
            button.playAudioEvents = buttonTemplate.playAudioEvents;
            button.pressedTextColor = buttonTemplate.pressedTextColor;
            button.focusedTextColor = buttonTemplate.focusedTextColor;
            button.disabledTextColor = buttonTemplate.disabledTextColor;
            button.width = buttonWidth;
        }


        #region Slider helpers

        protected void AddAmbientVolumeSlider(UIHelperBase helper, AudioManager.AmbientType type, float minValue = 0, float maxValue = 1)
        {
            AddVolumeSlider(Mod.Settings.AmbientVolumes, Mod.Instance.AmbientsPatcher, helper, type, minValue, maxValue);
        }

        protected void AddAnimalVolumeSlider(UIHelperBase helper, string id, float minValue = 0, float maxValue = 1)
        {
            AddVolumeSlider(Mod.Settings.AnimalVolumes, Mod.Instance.AnimalsPatcher, helper, id, minValue, maxValue);
        }

        protected void AddBuildingVolumeSlider(UIHelperBase helper, string id, float minValue = 0, float maxValue = 1)
        {
            AddVolumeSlider(Mod.Settings.BuildingVolumes, Mod.Instance.BuildingsPatcher, helper, id, minValue, maxValue);
        }

        protected void AddVehicleVolumeSlider(UIHelperBase helper, string id, float minValue = 0, float maxValue = 1)
        {
            AddVolumeSlider(Mod.Settings.VehicleVolumes, Mod.Instance.VehiclesPatcher, helper, id, minValue, maxValue);
        }

        protected void AddMiscVolumeSlider(UIHelperBase helper, string id, float minValue = 0, float maxValue = 1)
        {
            AddVolumeSlider(Mod.Settings.MiscVolumes, Mod.Instance.MiscPatcher, helper, id, minValue, maxValue);
        }

        protected UIHelper AddGroup(UIHelper helper, string text)
        {
            UIHelper groupHelper = (UIHelper)helper.AddGroup(text);
            ((UIComponent)groupHelper.self).parent.width = ((UIComponent)helper.self).width - 20;
            ((UIComponent)groupHelper.self).width = ((UIComponent)groupHelper.self).parent.width;
            return groupHelper;
        }

        protected void AddVolumeSlider<T>(IDictionary<T, float> volumeDictionary, SoundsInstancePatcher<T> patcher, UIHelperBase helper, T id, float minValue = 0, float maxValue = 1)
        {
            OnValueChanged valueChangedCallback = v =>
            {
                volumeDictionary[id] = v;
                patcher.PatchVolume(id, v);
            };

            string name = id.ToString();
            this.SliderNames.TryGetValueOrDefault(name, name, out name);

            UISlider slider = (UISlider)helper.AddSlider(name, minValue, maxValue, 0.01f, volumeDictionary[id], valueChangedCallback);
            UIPanel panel = (UIPanel)slider.parent;
            UILabel label = panel.Find<UILabel>("Label");
            panel.autoLayout = false;
            label.anchor = UIAnchorStyle.Left | UIAnchorStyle.CenterVertical;
            label.width = 300;
            slider.relativePosition = new Vector3(0, 0);
            slider.anchor = UIAnchorStyle.Right | UIAnchorStyle.CenterVertical;
            slider.builtinKeyNavigation = false;
            panel.size = new Vector2(310 + slider.width, 30);
        }

        #endregion

    }
}
