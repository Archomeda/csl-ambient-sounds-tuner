using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AmbientSoundsTuner.Defs;
using AmbientSoundsTuner.Migration;
using AmbientSoundsTuner.SoundPack;
using AmbientSoundsTuner.SoundPack.Migration;
using AmbientSoundsTuner.SoundPatchers;
using ColossalFramework;
using ColossalFramework.DataBinding;
using ColossalFramework.UI;
using CommonShared.Extensions;
using CommonShared.UI;
using CommonShared.UI.Extensions;
using CommonShared.Utils;
using ICities;
using UnityEngine;

namespace AmbientSoundsTuner.UI
{
    /// <summary>
    /// A mod options panel.
    /// </summary>
    public class ModOptionsPanel : ConfigPanelBase
    {
        #region Slider definitions

        protected readonly IDictionary<string, SliderDef<AudioManager.AmbientType>[]> AmbientsDef = new Dictionary<string, SliderDef<AudioManager.AmbientType>[]>()
        {
            { "Ambients", new[]
                {
                    new SliderDef<AudioManager.AmbientType>(AudioManager.AmbientType.Agricultural, "Ambient", "Agricultural"),
                    new SliderDef<AudioManager.AmbientType>(AudioManager.AmbientType.City, "Ambient", "City"),
                    new SliderDef<AudioManager.AmbientType>(AudioManager.AmbientType.Forest, "Ambient", "Forest"),
                    new SliderDef<AudioManager.AmbientType>(AudioManager.AmbientType.Industrial, "Ambient", "Industrial"),
                    new SliderDef<AudioManager.AmbientType>(AudioManager.AmbientType.Plaza, "Ambient", "Plaza"),
                    new SliderDef<AudioManager.AmbientType>(AudioManager.AmbientType.Sea, "Ambient", "Sea"),
                    new SliderDef<AudioManager.AmbientType>(AudioManager.AmbientType.Stream, "Ambient", "Stream"),
                    new SliderDef<AudioManager.AmbientType>(AudioManager.AmbientType.Suburban, "Ambient", "Suburban"),
                    new SliderDef<AudioManager.AmbientType>(AudioManager.AmbientType.World, "Ambient", "World")
                }
            }
        };

        protected readonly IDictionary<string, SliderDef<string>[]> AnimalsDef = new Dictionary<string, SliderDef<string>[]>()
        {
            { "Animals", new[]
                {
                    new SliderDef<string>(AnimalsPatcher.ID_COW, "Animal", "Cows"),
                    new SliderDef<string>(AnimalsPatcher.ID_PIG, "Animal", "Pigs"),
                    new SliderDef<string>(AnimalsPatcher.ID_SEAGULL, "Animal", "Seagulls")
                }
            }
        };

        protected readonly IDictionary<string, SliderDef<string>[]> BuildingsDef = new Dictionary<string, SliderDef<string>[]>()
        {
            { "Electricity and Water", new[]
                {
                    new SliderDef<string>(BuildingsPatcher.ID_ADVANCED_WIND_TURBINE, "Building", "Advanced Wind Turbine"),
                    new SliderDef<string>(BuildingsPatcher.ID_COAL_POWER_PLANT, "Building", "Coal Power Plant"),
                    new SliderDef<string>(BuildingsPatcher.ID_FUSION_POWER_PLANT, "Building", "Fusion Power Plant", 4),
                    new SliderDef<string>(BuildingsPatcher.ID_HYDRO_POWER_PLANT, "Building", "Hydro Power Plant"),
                    new SliderDef<string>(BuildingsPatcher.ID_INCINERATION_PLANT, "Building", "Incineration Plant"),
                    new SliderDef<string>(BuildingsPatcher.ID_NUCLEAR_POWER_PLANT, "Building", "Nuclear Power Plant"),
                    new SliderDef<string>(BuildingsPatcher.ID_OIL_POWER_PLANT, "Building", "Oil Power Plant"),
                    new SliderDef<string>(BuildingsPatcher.ID_POWER_PLANT_SMALL, "Building", "Small Power Plant"),
                    new SliderDef<string>(BuildingsPatcher.ID_SOLAR_POWER_PLANT, "Building", "Solar Power Plant"),
                    new SliderDef<string>(BuildingsPatcher.ID_WATER_DRAIN_PIPE, "Building", "Water Drain Pipe"),
                    new SliderDef<string>(BuildingsPatcher.ID_WATER_PUMPING_STATION, "Building", "Water Pumping Station"),
                    new SliderDef<string>(BuildingsPatcher.ID_WATER_TREATMENT_PLANT, "Building", "Water Treatment Plant"),
                    new SliderDef<string>(BuildingsPatcher.ID_WIND_TURBINE, "Building", "Wind Turbine")
                }
            },
            { "Services", new[]
                {
                    new SliderDef<string>(BuildingsPatcher.ID_CEMETERY, "Building", "Cemetery"),
                    new SliderDef<string>(BuildingsPatcher.ID_CREMATORY, "Building", "Crematory"),
                    new SliderDef<string>(BuildingsPatcher.ID_ELEMENTARY_SCHOOL, "Building", "Elementary School"),
                    new SliderDef<string>(BuildingsPatcher.ID_FIRE_STATION, "Building", "Fire Station"),
                    new SliderDef<string>(BuildingsPatcher.ID_HIGH_SCHOOL, "Building", "High School"),
                    new SliderDef<string>(BuildingsPatcher.ID_HOSPITAL, "Building", "Hospital"),
                    new SliderDef<string>(BuildingsPatcher.ID_POLICE_STATION, "Building", "Police Station"),
                    new SliderDef<string>(BuildingsPatcher.ID_UNIVERSITY, "Building", "University")
                }
            },
            { "Public Transport", new[]
                {
                    new SliderDef<string>(BuildingsPatcher.ID_AIRPORT, "Building", "Airport"),
                    new SliderDef<string>(BuildingsPatcher.ID_BUS_DEPOT, "Building", "Bus Depot"),
                    new SliderDef<string>(BuildingsPatcher.ID_HARBOR, "Building", "Harbor"),
                    new SliderDef<string>(BuildingsPatcher.ID_METRO_STATION, "Building", "Metro Station"),
                    new SliderDef<string>(BuildingsPatcher.ID_TRAIN_STATION, "Building", "Train Station")
                }
            },
            { "Other", new[]
                {
                    new SliderDef<string>(BuildingsPatcher.ID_COMMERCIAL, "Building", "Commercial"),
                    new SliderDef<string>(BuildingsPatcher.ID_INDUSTRIAL, "Building", "Industrial", 0.5f)
                }
            },
            { "Miscellaneous", new[]
                {
                    new SliderDef<string>(BuildingsPatcher.ID_ON_FIRE, "Building", "On Fire"),
                    new SliderDef<string>(BuildingsPatcher.ID_ON_UPGRADE, "Building", "On Upgrade", 0.25f)
                }
            }
        };

        protected readonly IDictionary<string, SliderDef<string>[]> VehiclesDef = new Dictionary<string, SliderDef<string>[]>()
        {
            { "Engines", new[]
                {
                    new SliderDef<string>(VehiclesPatcher.ID_AIRCRAFT_SOUND, "Vehicle", "Aircrafts"),
                    new SliderDef<string>(VehiclesPatcher.ID_LARGE_CAR_SOUND, "Vehicle", "Large Cars", 1.5f),
                    new SliderDef<string>(VehiclesPatcher.ID_METRO_MOVEMENT, "Vehicle", "Metros"),
                    new SliderDef<string>(VehiclesPatcher.ID_SMALL_CAR_SOUND, "Vehicle", "Small Cars", 1.5f),
                    new SliderDef<string>(VehiclesPatcher.ID_SCOOTER_SOUND, "Vehicle", "Scooters", 1.5f),
                    new SliderDef<string>(VehiclesPatcher.ID_TRAIN_MOVEMENT, "Vehicle", "Trains")
                }
            },
            { "Sirens", new[]
                {
                    new SliderDef<string>(VehiclesPatcher.ID_AMBULANCE_SIREN, "Vehicle", "Ambulances"),
                    new SliderDef<string>(VehiclesPatcher.ID_FIRE_TRUCK_SIREN, "Vehicle", "Fire Trucks", 3),
                    new SliderDef<string>(VehiclesPatcher.ID_POLICE_CAR_SIREN, "Vehicle", "Police Cars")
                }
            },
            { "Miscellaneous", new[]
                {
                    new SliderDef<string>(VehiclesPatcher.ID_TRANSPORT_ARRIVE, "Vehicle", "Transport Arrivals")
                }
            }
        };

        protected readonly IDictionary<string, SliderDef<string>[]> MiscDef = new Dictionary<string, SliderDef<string>[]>()
        {
            { "Bulldozer", new[]
                {
                    new SliderDef<string>(MiscPatcher.ID_BUILDING_BULLDOZE, "Misc", "Buildings"),
                    new SliderDef<string>(MiscPatcher.ID_PROP_BULLDOZE, "Misc", "Props"),
                    new SliderDef<string>(MiscPatcher.ID_ROAD_BULLDOZE, "Misc", "Roads"),
                }
            },
            { "Placements", new[]
                {
                    new SliderDef<string>(MiscPatcher.ID_BUILDING_PLACEMENT, "Misc", "Buildings"),
                    new SliderDef<string>(MiscPatcher.ID_PROP_PLACEMENT, "Misc", "Props"),
                    new SliderDef<string>(MiscPatcher.ID_ROAD_PLACEMENT, "Misc", "Roads")
                }
            },
            { "User Interface", new[]
                {
                    new SliderDef<string>(MiscPatcher.ID_CLICK_SOUND, "Misc", "Clicks"),
                    new SliderDef<string>(MiscPatcher.ID_DISABLED_CLICK_SOUND, "Misc", "Clicks (disabled components)"),
                    new SliderDef<string>(MiscPatcher.ID_ROAD_DRAW, "Misc", "Road Drawer"),
                    new SliderDef<string>(MiscPatcher.ID_ZONE_FILL, "Misc", "Zone Filler")
                }
            }
        };

        protected readonly HashSet<string> SoundPackBlacklist = new HashSet<string>()
        {
            MiscPatcher.ID_CLICK_SOUND,
            MiscPatcher.ID_DISABLED_CLICK_SOUND
        };

        #endregion

        private string[] soundPacks;
        private bool isChangingSoundPackPreset = false;
        private UIDropDown soundPackPresetDropDown;
        private Dictionary<string, UIDropDown> soundSelections = new Dictionary<string, UIDropDown>();

        public ModOptionsPanel(UIHelper helper) : base(helper) { }

        protected override void PopulateUI()
        {
            UIHelper groupHelper = null;

            // Create global options
            groupHelper = this.RootHelper.AddGroup2("Mod settings");
            this.soundPacks = new[] { "Default", "Custom" }.Union(SoundPacksManager.instance.SoundPacks.Values.OrderBy(p => p.Name).Select(p => p.Name)).ToArray();
            this.soundPackPresetDropDown = (UIDropDown)groupHelper.AddDropdown("Sound pack preset", this.soundPacks, 0, this.SoundPackPresetDropDownSelectionChanged);
            this.soundPackPresetDropDown.selectedValue = Mod.Instance.Settings.SoundPackPreset;
            groupHelper.AddCheckbox("Enable debug logging (don't use this during normal gameplay)", Mod.Instance.Settings.ExtraDebugLogging, v =>
            {
                Mod.Instance.Settings.ExtraDebugLogging = v;
                Mod.Instance.Log.EnableDebugLogging = v;
            });

            // Create tab strip
            groupHelper = this.RootHelper.AddGroup2("Sound settings");
            UIComponent groupParent = ((UIPanel)groupHelper.self).parent;
            UITabstrip tabstrip = groupHelper.AddTabstrip();
            tabstrip.tabPages.size = new Vector2(
                groupParent.width - 20,
                this.RootPanelInnerArea.y - (groupParent.absolutePosition.y - this.RootPanel.absolutePosition.y) - (groupParent.height - tabstrip.tabPages.height)
            );
            tabstrip.tabPages.anchor = UIAnchorStyle.All;

            // Create tabs
            int tabWidth = (int)(tabstrip.tabPages.width / 5);
            this.AddTab(tabstrip, tabWidth, "Ambients", this.AmbientsDef);
            this.AddTab(tabstrip, tabWidth, "Animals", this.AnimalsDef);
            this.AddTab(tabstrip, tabWidth, "Buildings", this.BuildingsDef);
            this.AddTab(tabstrip, tabWidth, "Vehicles", this.VehiclesDef);
            this.AddTab(tabstrip, tabWidth, "Misc", this.MiscDef);

            tabstrip.selectedIndex = -1;
            tabstrip.selectedIndex = 0;

            // Add mod information
            this.RootPanel.autoLayout = false;
            UILabel versionLabel = this.RootPanel.AddUIComponent<UILabel>();
            versionLabel.autoSize = true;
            versionLabel.textScale = 0.8f;
            versionLabel.text = Mod.Instance.BuildVersion;
            versionLabel.relativePosition = new Vector3(this.RootPanel.width - versionLabel.size.x - 10, 0);
        }

        protected override void OnClose()
        {
            Mod.Instance.Log.Debug("Options panel closed, saving config");
            Mod.Instance.Settings.SaveConfig(Mod.Instance.SettingsFilename);
        }

        protected void AddTab<T>(UITabstrip tabstrip, float buttonWidth, string title, IDictionary<string, SliderDef<T>[]> content)
        {
            UIHelper tabHelper = this.RootHelper.AddScrollingTab(tabstrip, buttonWidth, title);
            foreach (var group in content)
            {
                UIHelper groupHelper = tabHelper.AddGroup2(group.Key);
                foreach (var sliderDef in group.Value)
                {
                    this.AddSoundRow(sliderDef, groupHelper);
                }
            }
        }

        protected void AddSoundRow<T>(SliderDef<T> slider, UIHelperBase helper)
        {
            // Get all used singletons
            var patcher = SoundPatchersManager.instance.GetPatcherById<T>(slider.Prefix);
            var configuration = Mod.Instance.Settings.GetSoundsByCategoryId<T>(slider.Prefix);

            var customAudioFiles = patcher.GetAvailableAudiosForType(slider.Id.ToString()).Values.ToArray();

            float volume = 0;
            if (configuration.ContainsKey(slider.Id))
            {
                volume = configuration[slider.Id].Volume;
            }
            else
            {
                Mod.Instance.Log.Info("No volume configuration found for {0}, using default value", slider.Id.ToString());
                volume = patcher.DefaultVolumes.ContainsKey(slider.Id) ? patcher.DefaultVolumes[slider.Id] : 1;
            }

            // Add UI components
            UISlider uiSlider = (UISlider)helper.AddSlider(slider.Text, slider.MinValue, slider.MaxValue, 0.01f, volume, v => this.SoundVolumeChanged<T>(slider.Prefix, slider.Id, v));
            UIPanel uiPanel = (UIPanel)uiSlider.parent;
            UILabel uiLabel = uiPanel.Find<UILabel>("Label");
            UIDropDown uiDropDown = null;

            if (customAudioFiles.Length > 0)
            {
                uiDropDown = uiPanel.AttachUIComponent(GameObject.Instantiate((UITemplateManager.Peek(UITemplateDefs.ID_OPTIONS_DROPDOWN_TEMPLATE) as UIPanel).Find<UIDropDown>("Dropdown").gameObject)) as UIDropDown;
                uiDropDown.items = new[] { "Default" }.Union(customAudioFiles.Select(a => a.Name)).ToArray();
                uiDropDown.height = 28;
                uiDropDown.textFieldPadding.top = 4;
                if (configuration.ContainsKey(slider.Id) && !string.IsNullOrEmpty(configuration[slider.Id].SoundPack))
                    uiDropDown.selectedValue = configuration[slider.Id].SoundPack;
                else
                    uiDropDown.selectedIndex = 0;

                uiDropDown.eventSelectedIndexChanged += (c, i) => this.SoundPackChanged(slider.Prefix, slider.Id, i > 0 ? ((UIDropDown)c).items[i] : null, uiSlider);
                this.soundSelections[slider.Prefix + "." + slider.Id.ToString()] = uiDropDown;
            }

            // Configure UI components
            uiPanel.autoLayout = false;
            uiLabel.anchor = UIAnchorStyle.Left | UIAnchorStyle.CenterVertical;
            uiLabel.width = 250;
            uiSlider.anchor = UIAnchorStyle.CenterVertical;
            uiSlider.builtinKeyNavigation = false;
            uiSlider.width = 207;
            uiSlider.relativePosition = new Vector3(uiLabel.relativePosition.x + uiLabel.width + 20, 0);
            if (customAudioFiles.Length > 0)
            {
                uiDropDown.anchor = UIAnchorStyle.CenterVertical;
                uiDropDown.width = 180;
                uiDropDown.relativePosition = new Vector3(uiSlider.relativePosition.x + uiSlider.width + 20, 0);
                uiPanel.size = new Vector2(uiDropDown.relativePosition.x + uiDropDown.width, 32);
            }
            else
            {
                uiPanel.size = new Vector2(uiSlider.relativePosition.x + uiSlider.width, 32);
            }
        }

        private void SoundVolumeChanged<T>(string categoryId, T soundId, float value)
        {
            var patcher = SoundPatchersManager.instance.GetPatcherById<T>(categoryId);
            var configuration = Mod.Instance.Settings.GetSoundsByCategoryId<T>(categoryId);

            if (!configuration.ContainsKey(soundId))
                configuration.Add(soundId, new ConfigurationV4.Sound());

            configuration[soundId].Volume = value;

            if (LoadingManager.instance.m_loadingComplete)
                patcher.PatchVolume(soundId, value);
            else
                Mod.Instance.Log.Debug("Skip patching volume of {0} as there isn't a game active", soundId.ToString());
        }

        private void SoundPackChanged<T>(string categoryId, T soundId, string name, UISlider uiSlider)
        {
            var patcher = SoundPatchersManager.instance.GetPatcherById<T>(categoryId);
            var configuration = Mod.Instance.Settings.GetSoundsByCategoryId<T>(categoryId);

            // Selected audio changed
            if (!configuration.ContainsKey(soundId))
                configuration.Add(soundId, new ConfigurationV4.Sound());

            // Set preset to custom
            if (!this.isChangingSoundPackPreset)
                this.soundPackPresetDropDown.selectedIndex = 1;

            if (!string.IsNullOrEmpty(name))
            {
                // Chosen audio is a custom audio
                configuration[soundId].SoundPack = name;
                var audioFile = patcher.GetAudioByName(soundId.ToString(), name);

                if (LoadingManager.instance.m_loadingComplete)
                    patcher.PatchSound(soundId, audioFile);
                else
                    Mod.Instance.Log.Debug("Skip patching sound of {0} as there isn't a game active", soundId.ToString());

                uiSlider.maxValue = Mathf.Max(audioFile.AudioInfo.MaxVolume, audioFile.AudioInfo.Volume);
                uiSlider.value = audioFile.AudioInfo.Volume;
            }
            else
            {
                // Chosen audio is the default one
                configuration[soundId].SoundPack = "";

                if (LoadingManager.instance.m_loadingComplete)
                    patcher.RevertSound(soundId);
                else
                    Mod.Instance.Log.Debug("Skip reverting sound of {0} as there isn't a game active", soundId.ToString());

                if (patcher.OldSounds.ContainsKey(soundId))
                {
                    uiSlider.maxValue = patcher.OldSounds[soundId].AudioInfo.MaxVolume;
                    uiSlider.value = patcher.OldSounds[soundId].AudioInfo.Volume;
                }
                else
                {
                    uiSlider.maxValue = patcher.DefaultMaxVolumes.ContainsKey(soundId) ? patcher.DefaultMaxVolumes[soundId] : 1;
                    uiSlider.value = patcher.DefaultVolumes.ContainsKey(soundId) ? patcher.DefaultVolumes[soundId] : 1;
                }
            }
        }

        private void SoundPackPresetDropDownSelectionChanged(int value)
        {
            this.isChangingSoundPackPreset = true;

            if (value == 0)
            {
                // Default
                Mod.Instance.Log.Debug("Resetting sound pack to default");
                foreach (UIDropDown dropDown in this.soundSelections.Values)
                    dropDown.selectedIndex = 0;
            }
            else if (value == 1)
            {
                // Custom, don't do anything here
            }
            else if (value >= 2)
            {
                // Sound pack
                string soundPackName = this.soundPacks[value];
                SoundPacksFileV1.SoundPack soundPack = null;
                Mod.Instance.Log.Debug("Setting sound pack to {0}", soundPackName);

                if (SoundPacksManager.instance.SoundPacks.TryGetValue(soundPackName, out soundPack))
                {
                    foreach (var dropDown in this.soundSelections)
                    {
                        var prefix = dropDown.Key.Substring(0, dropDown.Key.IndexOf('.'));
                        var id = dropDown.Key.Substring(dropDown.Key.IndexOf('.') + 1);
                        SoundPacksFileV1.Audio[] audios = null;
                        switch (prefix)
                        {
                            case "Ambient":
                                audios = soundPack.Ambients;
                                break;
                            case "Animal":
                                audios = soundPack.Animals;
                                break;
                            case "Building":
                                audios = soundPack.Buildings;
                                break;
                            case "Vehicle":
                                audios = soundPack.Vehicles;
                                break;
                            case "Misc":
                                audios = soundPack.Miscs;
                                break;
                        }
                        if (audios != null)
                        {
                            SoundPacksFileV1.Audio audio = audios.FirstOrDefault(a => a.Type == id);
                            if (audio != null)
                            {
                                Mod.Instance.Log.Debug("Setting sound {0} to {1}", audio.Type, audio.Name);
                                dropDown.Value.selectedValue = audio.Name;
                            }
                        }
                    }
                }
            }

            Mod.Instance.Settings.SoundPackPreset = this.soundPackPresetDropDown.selectedValue;
            this.isChangingSoundPackPreset = false;
        }

        /// <summary>
        /// A struct that represents the definition of a slider.
        /// </summary>
        /// <typeparam name="T">The type of the slider id.</typeparam>
        public struct SliderDef<T>
        {
            public T Id;
            public string Prefix;
            public string Text;
            public float MinValue;
            public float MaxValue;

            public SliderDef(T id, string prefix, string text) : this(id, prefix, text, 0, 1) { }

            public SliderDef(T id, string prefix, string text, float maxValue) : this(id, prefix, text, 0, maxValue) { }

            public SliderDef(T id, string prefix, string text, float minValue, float maxValue)
            {
                this.Id = id;
                this.Prefix = prefix;
                this.Text = text;
                this.MinValue = minValue;
                this.MaxValue = maxValue;
            }
        }
    }
}
