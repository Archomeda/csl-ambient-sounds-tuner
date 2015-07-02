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
        #region Slider definitions

        protected readonly IDictionary<string, SliderDef<AudioManager.AmbientType>[]> AmbientsDef = new Dictionary<string, SliderDef<AudioManager.AmbientType>[]>()
        {
            { "Ambients", new[]
                {
                    new SliderDef<AudioManager.AmbientType>(AudioManager.AmbientType.Agricultural, "Agricultural"),
                    new SliderDef<AudioManager.AmbientType>(AudioManager.AmbientType.City, "City"),
                    new SliderDef<AudioManager.AmbientType>(AudioManager.AmbientType.Forest, "Forest"),
                    new SliderDef<AudioManager.AmbientType>(AudioManager.AmbientType.Industrial, "Industrial"),
                    new SliderDef<AudioManager.AmbientType>(AudioManager.AmbientType.Plaza, "Plaza"),
                    new SliderDef<AudioManager.AmbientType>(AudioManager.AmbientType.Sea, "Sea"),
                    new SliderDef<AudioManager.AmbientType>(AudioManager.AmbientType.Stream, "Stream"),
                    new SliderDef<AudioManager.AmbientType>(AudioManager.AmbientType.Suburban, "Suburban"),
                    new SliderDef<AudioManager.AmbientType>(AudioManager.AmbientType.World, "World")
                }
            }
        };

        protected readonly IDictionary<string, SliderDef<string>[]> AnimalsDef = new Dictionary<string, SliderDef<string>[]>()
        {
            { "Animals", new[]
                {
                    new SliderDef<string>(SoundsCollection.AnimalSounds.ID_COW, "Cows"),
                    new SliderDef<string>(SoundsCollection.AnimalSounds.ID_PIG, "Pigs"),
                    new SliderDef<string>(SoundsCollection.AnimalSounds.ID_SEAGULL, "Seagulls")
                }
            }
        };

        protected readonly IDictionary<string, SliderDef<string>[]> BuildingsDef = new Dictionary<string, SliderDef<string>[]>()
        {
            { "Electricity and Water", new[]
                {
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_ADVANCED_WIND_TURBINE, "Advanced Wind Turbine"),
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_COAL_POWER_PLANT, "Coal/Oil Power Plant"),
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_FUSION_POWER_PLANT, "Fusion Power Plant", 4),
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_HYDRO_POWER_PLANT, "Hydro Power Plant"),
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_INCINERATION_PLANT, "Incineration Plant"),
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_NUCLEAR_POWER_PLANT, "Nuclear Power Plant"),
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_POWER_PLANT_SMALL, "Small Power Plant"),
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_SOLAR_POWER_PLANT, "Solar Power Plant"),
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_WATER_DRAIN_PIPE, "Water Drain/Treatment Plant"),
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_WATER_PUMPING_STATION, "Water Pumping Station"),
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_WIND_TURBINE, "Wind Turbine")
                }
            },
            { "Services", new[]
                {
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_CEMETERY, "Cemetery"),
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_CREMATORY, "Crematory"),
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_ELEMENTARY_SCHOOL, "Elementary School"),
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_FIRE_STATION, "Fire Station"),
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_HIGH_SCHOOL, "High School"),
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_HOSPITAL, "Police Station"),
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_POLICE_STATION, "University"),
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_UNIVERSITY, "Hospital")
                }
            },
            { "Public Transport", new[]
                {
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_AIRPORT, "Airport"),
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_BUS_DEPOT, "Bus Depot"),
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_HARBOR, "Harbor"),
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_METRO_STATION, "Metro Station"),
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_TRAIN_STATION, "Train Station")
                }
            },
            { "Other", new[]
                {
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_COMMERCIAL, "Commercial"),
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_INDUSTRIAL, "Industrial", 0.5f)
                }
            },
            { "Miscellaneous", new[]
                {
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_ON_FIRE, "On Fire"),
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_ON_UPGRADE, "On Upgrade", 0.25f)
                }
            }
        };

        protected readonly IDictionary<string, SliderDef<string>[]> VehiclesDef = new Dictionary<string, SliderDef<string>[]>()
        {
            { "Engines", new[]
                {
                    new SliderDef<string>(SoundsCollection.VehicleSounds.ID_AIRCRAFT_MOVEMENT, "Aircrafts"),
                    new SliderDef<string>(SoundsCollection.VehicleSounds.ID_LARGE_CAR_SOUND, "Large Cars", 1.5f),
                    new SliderDef<string>(SoundsCollection.VehicleSounds.ID_METRO_MOVEMENT, "Metros"),
                    new SliderDef<string>(SoundsCollection.VehicleSounds.ID_SMALL_CAR_SOUND, "Small Cars/Scooters", 1.5f),
                    new SliderDef<string>(SoundsCollection.VehicleSounds.ID_TRAIN_MOVEMENT, "Trains")
                }
            },
            { "Sirens", new[]
                {
                    new SliderDef<string>(SoundsCollection.VehicleSounds.ID_AMBULANCE_SIREN, "Ambulances"),
                    new SliderDef<string>(SoundsCollection.VehicleSounds.ID_FIRE_TRUCK_SIREN, "Fire Trucks", 3),
                    new SliderDef<string>(SoundsCollection.VehicleSounds.ID_POLICE_CAR_SIREN, "Police Cars")
                }
            },
            { "Miscellaneous", new[]
                {
                    new SliderDef<string>(SoundsCollection.VehicleSounds.ID_TRANSPORT_ARRIVE, "Transport Arrivals")
                }
            }
        };

        protected readonly IDictionary<string, SliderDef<string>[]> MiscDef = new Dictionary<string, SliderDef<string>[]>()
        {
            { "Bulldozer", new[]
                {
                    new SliderDef<string>(SoundsCollection.MiscSounds.ID_BUILDING_BULLDOZE, "Buildings"),
                    new SliderDef<string>(SoundsCollection.MiscSounds.ID_PROP_BULLDOZE, "Props"),
                    new SliderDef<string>(SoundsCollection.MiscSounds.ID_ROAD_BULLDOZE, "Roads"),
                }
            },
            { "Placements", new[]
                {
                    new SliderDef<string>(SoundsCollection.MiscSounds.ID_BUILDING_PLACEMENT, "Buildings"),
                    new SliderDef<string>(SoundsCollection.MiscSounds.ID_PROP_PLACEMENT, "Props"),
                    new SliderDef<string>(SoundsCollection.MiscSounds.ID_ROAD_PLACEMENT, "Roads")
                }
            },
            { "User Interface", new[]
                {
                    new SliderDef<string>(MiscPatcher.ID_CLICK_SOUND, "Clicks"),
                    new SliderDef<string>(MiscPatcher.ID_DISABLED_CLICK_SOUND, "Clicks (disabled components)"),
                    new SliderDef<string>(SoundsCollection.MiscSounds.ID_ROAD_DRAW, "Road Drawer"),
                    new SliderDef<string>(SoundsCollection.MiscSounds.ID_ZONE_FILL, "Zone Filler")
                }
            }
        };

        #endregion

        private UIHelper rootHelper;
        private bool prevVisible = false;

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
            UITabstrip tabstrip = this.RootPanel.AddUIComponent<UITabstrip>();
            tabstrip.relativePosition = new Vector3(0, 0);
            tabstrip.size = new Vector2(panelInnerSize.x, 40);
            tabstrip.anchor = UIAnchorStyle.Top | UIAnchorStyle.Left | UIAnchorStyle.Right;

            // Create tab container
            UITabContainer tabContainer = this.RootPanel.AddUIComponent<UITabContainer>();
            tabContainer.relativePosition = new Vector3(0, tabstrip.height);
            tabContainer.size = new Vector2(panelInnerSize.x, panelInnerSize.y - tabstrip.height);
            tabContainer.anchor = UIAnchorStyle.All;
            tabstrip.tabPages = tabContainer;

            // Create tabs
            UITabstrip keyMappingTabStrip = GameObject.Find(GameObjectDefs.ID_KEYMAPPING_TABSTRIP).GetComponent<UITabstrip>();
            UIButton buttonTemplate = keyMappingTabStrip.GetComponentInChildren<UIButton>();
            int tabWidth = (int)(panelInnerSize.x / 5);
            this.AddTab(tabstrip, buttonTemplate, "Ambients", tabWidth, this.AmbientsDef, Mod.Settings.AmbientVolumes, Mod.Instance.AmbientsPatcher);
            this.AddTab(tabstrip, buttonTemplate, "Animals", tabWidth, this.AnimalsDef, Mod.Settings.AnimalVolumes, Mod.Instance.AnimalsPatcher);
            this.AddTab(tabstrip, buttonTemplate, "Buildings", tabWidth, this.BuildingsDef, Mod.Settings.BuildingVolumes, Mod.Instance.BuildingsPatcher);
            this.AddTab(tabstrip, buttonTemplate, "Vehicles", tabWidth, this.VehiclesDef, Mod.Settings.VehicleVolumes, Mod.Instance.VehiclesPatcher);
            this.AddTab(tabstrip, buttonTemplate, "Misc", tabWidth, this.MiscDef, Mod.Settings.MiscVolumes, Mod.Instance.MiscPatcher);

            tabstrip.selectedIndex = -1;
            tabstrip.selectedIndex = 0;

            // Hook onto visibility event so we can manually save our settings
            this.RootPanel.eventVisibilityChanged += RootPanel_eventVisibilityChanged;
        }

        void RootPanel_eventVisibilityChanged(UIComponent component, bool value)
        {
            if (prevVisible && !value)
            {
                Mod.Settings.SaveConfig(Mod.SettingsFilename);
            }
            prevVisible = value;
        }

        protected void AddTab<T>(UITabstrip tabstrip, UIButton buttonTemplate, string buttonName, float buttonWidth, IDictionary<string, SliderDef<T>[]> content, IDictionary<T, float> volumes, SoundsInstancePatcher<T> patcher)
        {
            UIPanel panelTemplate = (UIPanel)UITemplateManager.Peek("OptionsScrollPanelTemplate");
            UIButton tabButton = tabstrip.AddCustomTab(buttonName, buttonTemplate, panelTemplate, true);
            tabButton.playAudioEvents = buttonTemplate.playAudioEvents;
            tabButton.pressedTextColor = buttonTemplate.pressedTextColor;
            tabButton.focusedTextColor = buttonTemplate.focusedTextColor;
            tabButton.disabledTextColor = buttonTemplate.disabledTextColor;
            tabButton.width = buttonWidth;

            UIScrollablePanel tabPanel = ((UIPanel)tabstrip.tabContainer.components.Last()).GetComponentInChildren<UIScrollablePanel>();
            tabPanel.autoLayout = true;
            tabPanel.autoLayoutDirection = LayoutDirection.Vertical;
            tabPanel.autoLayoutPadding = new RectOffset(0, 0, 2, 0);

            UIHelper helper = new UIHelper(tabPanel);
            foreach (var group in content)
            {
                UIHelper groupHelper = this.AddGroup(helper, group.Key);
                foreach (var sliderDef in group.Value)
                {
                    this.AddVolumeSlider(sliderDef, groupHelper, volumes, patcher);
                }
            }
        }

        protected UIHelper AddGroup(UIHelper helper, string text)
        {
            UIHelper groupHelper = (UIHelper)helper.AddGroup(text);
            ((UIComponent)groupHelper.self).parent.width = ((UIComponent)helper.self).width - 20;
            ((UIComponent)groupHelper.self).width = ((UIComponent)groupHelper.self).parent.width;
            return groupHelper;
        }

        protected void AddVolumeSlider<T>(SliderDef<T> slider, UIHelperBase helper, IDictionary<T, float> volumes, SoundsInstancePatcher<T> patcher)
        {
            OnValueChanged valueChangedCallback = v =>
            {
                volumes[slider.Id] = v;
                patcher.PatchVolume(slider.Id, v);
            };

            UISlider uiSlider = (UISlider)helper.AddSlider(slider.Text, slider.MinValue, slider.MaxValue, 0.01f, volumes[slider.Id], valueChangedCallback);
            UIPanel uiPanel = (UIPanel)uiSlider.parent;
            UILabel uiLabel = uiPanel.Find<UILabel>("Label");
            uiPanel.autoLayout = false;
            uiLabel.anchor = UIAnchorStyle.Left | UIAnchorStyle.CenterVertical;
            uiLabel.width = 300;
            uiSlider.relativePosition = new Vector3(0, 0);
            uiSlider.anchor = UIAnchorStyle.Right | UIAnchorStyle.CenterVertical;
            uiSlider.builtinKeyNavigation = false;
            uiPanel.size = new Vector2(310 + uiSlider.width, 30);
        }

        public struct SliderDef<T>
        {
            public T Id;
            public string Text;
            public float MinValue;
            public float MaxValue;

            public SliderDef(T id, string text) : this(id, text, 0, 1) { }

            public SliderDef(T id, string text, float maxValue) : this(id, text, 0, maxValue) { }

            public SliderDef(T id, string text, float minValue, float maxValue)
            {
                this.Id = id;
                this.Text = text;
                this.MinValue = minValue;
                this.MaxValue = maxValue;
            }
        }
    }
}
