using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AmbientSoundsTuner.Defs;
using AmbientSoundsTuner.SoundPack;
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
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_HOSPITAL, "Hospital"),
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_POLICE_STATION, "Police Station"),
                    new SliderDef<string>(SoundsCollection.BuildingSounds.ID_UNIVERSITY, "University")
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

        protected readonly HashSet<string> SoundPackBlacklist = new HashSet<string>()
        {
            MiscPatcher.ID_CLICK_SOUND,
            MiscPatcher.ID_DISABLED_CLICK_SOUND
        };

        #endregion

        public ModOptionsPanel(UIHelper helper) : base(helper) { }

        protected override void PopulateUI()
        {
            UIHelper groupHelper = null;

            // Create global options
            groupHelper = this.RootHelper.AddGroup2("Mod settings");
            groupHelper.AddCheckbox("Enable debug logging (don't use this during normal gameplay)", Mod.Settings.ExtraDebugLogging, v =>
            {
                Mod.Settings.ExtraDebugLogging = v;
                Mod.Log.EnableDebugLogging = v;
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
            this.AddTab(tabstrip, tabWidth, "Ambients", this.AmbientsDef, Mod.Instance.AmbientsPatcher, Mod.Settings.AmbientSounds, SoundPacksManager.instance.SoundPacks.Keys.SelectMany(f => f.Ambients).ToArray());
            this.AddTab(tabstrip, tabWidth, "Animals", this.AnimalsDef, Mod.Instance.AnimalsPatcher, Mod.Settings.AnimalSounds, SoundPacksManager.instance.SoundPacks.Keys.SelectMany(f => f.Animals).ToArray());
            this.AddTab(tabstrip, tabWidth, "Buildings", this.BuildingsDef, Mod.Instance.BuildingsPatcher, Mod.Settings.BuildingSounds, SoundPacksManager.instance.SoundPacks.Keys.SelectMany(f => f.Buildings).ToArray());
            this.AddTab(tabstrip, tabWidth, "Vehicles", this.VehiclesDef, Mod.Instance.VehiclesPatcher, Mod.Settings.VehicleSounds, SoundPacksManager.instance.SoundPacks.Keys.SelectMany(f => f.Vehicles).ToArray());
            this.AddTab(tabstrip, tabWidth, "Misc", this.MiscDef, Mod.Instance.MiscPatcher, Mod.Settings.MiscSounds, SoundPacksManager.instance.SoundPacks.Keys.SelectMany(f => f.Miscs).ToArray());

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
            Mod.Log.Debug("Options panel closed, saving config");
            Mod.Settings.SaveConfig(Mod.SettingsFilename);
        }

        protected void AddTab<T>(UITabstrip tabstrip, float buttonWidth, string title, IDictionary<string, SliderDef<T>[]> content, SoundsInstancePatcher<T> patcher, IDictionary<T, Configuration.Sound> configuration, SoundPackFile.Audio[] customAudioFiles)
        {
            UIHelper tabHelper = this.RootHelper.AddScrollingTab(tabstrip, buttonWidth, title);
            foreach (var group in content)
            {
                UIHelper groupHelper = tabHelper.AddGroup2(group.Key);
                foreach (var sliderDef in group.Value)
                {
                    SoundPackFile.Audio[] customAudios = null;
                    if (!this.SoundPackBlacklist.Contains(sliderDef.Id.ToString()))
                        customAudios = customAudioFiles.Where(a => a.Type == sliderDef.Id.ToString()).ToArray();
                    else
                        customAudios = new SoundPackFile.Audio[0];

                    this.AddSoundRow(sliderDef, groupHelper, patcher, configuration, customAudios);
                }
            }
        }

        protected void AddSoundRow<T>(SliderDef<T> slider, UIHelperBase helper, SoundsInstancePatcher<T> patcher, IDictionary<T, Configuration.Sound> configuration, SoundPackFile.Audio[] customAudioFiles)
        {
            OnValueChanged valueChangedCallback = v =>
            {
                if (!configuration.ContainsKey(slider.Id))
                    configuration.Add(slider.Id, new Configuration.Sound());

                configuration[slider.Id].Volume = v;
                patcher.PatchVolume(slider.Id, v);
            };

            UISlider uiSlider = (UISlider)helper.AddSlider(slider.Text, slider.MinValue, slider.MaxValue, 0.01f, configuration[slider.Id].Volume, valueChangedCallback);
            UIPanel uiPanel = (UIPanel)uiSlider.parent;
            UILabel uiLabel = uiPanel.Find<UILabel>("Label");
            UIDropDown uiDropDown = null;
            if (customAudioFiles.Length > 0)
            {
                uiDropDown = uiPanel.AttachUIComponent(GameObject.Instantiate((UITemplateManager.Peek(UITemplateDefs.ID_OPTIONS_DROPDOWN_TEMPLATE) as UIPanel).Find<UIDropDown>("Dropdown").gameObject)) as UIDropDown;
                uiDropDown.items = new[] { "Default" }.Union(customAudioFiles.Select(a => a.Name)).ToArray();
                uiDropDown.height = 28;
                uiDropDown.textFieldPadding.top = 4;
                if (configuration.ContainsKey(slider.Id) && !string.IsNullOrEmpty(configuration[slider.Id].Active))
                    uiDropDown.selectedValue = configuration[slider.Id].Active;
                else
                    uiDropDown.selectedIndex = 0;
                uiDropDown.eventSelectedIndexChanged += (c, i) =>
                {
                    if (!configuration.ContainsKey(slider.Id))
                        configuration.Add(slider.Id, new Configuration.Sound());

                    string name = ((UIDropDown)c).items[i];
                    configuration[slider.Id].Active = i > 0 ? name : "";
                    if (i > 0)
                    {
                        SoundPackFile.Audio audioFile = customAudioFiles.FirstOrDefault(a => a.Name == name);
                        patcher.PatchSound(slider.Id, audioFile);
                        uiSlider.maxValue = Mathf.Max(audioFile.AudioInfo.MaxVolume, audioFile.AudioInfo.Volume);
                        uiSlider.value = audioFile.AudioInfo.Volume;
                    }
                    else
                    {
                        patcher.RevertSound(slider.Id);
                        if (patcher.OldSounds.ContainsKey(slider.Id))
                        {
                            uiSlider.maxValue = patcher.OldSounds.ContainsKey(slider.Id) ? patcher.OldSounds[slider.Id].AudioInfo.MaxVolume : patcher.DefaultMaxVolumes[slider.Id];
                            uiSlider.value = patcher.OldSounds.ContainsKey(slider.Id) ? patcher.OldSounds[slider.Id].AudioInfo.Volume : patcher.DefaultVolumes[slider.Id];
                        }
                        else
                        {
                            uiSlider.maxValue = patcher.DefaultMaxVolumes.ContainsKey(slider.Id) ? patcher.DefaultMaxVolumes[slider.Id] : 1;
                            uiSlider.value = patcher.DefaultVolumes.ContainsKey(slider.Id) ? patcher.DefaultVolumes[slider.Id] : 1;
                        }
                    }
                };
            }

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

        /// <summary>
        /// A struct that represents the definition of a slider.
        /// </summary>
        /// <typeparam name="T">The type of the slider id.</typeparam>
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
