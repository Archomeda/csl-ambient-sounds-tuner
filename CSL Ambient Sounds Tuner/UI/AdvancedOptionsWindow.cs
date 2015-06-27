using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.Defs;
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
            { EffectsPatcher.ID_AIRCRAFT_MOVEMENT, "Aircrafts" },
            { EffectsPatcher.ID_AMBULANCE_SIREN, "Sirens (ambulances)" },
            { EffectsPatcher.ID_FIRE_TRUCK_SIREN, "Sirens (fire trucks)" },
            { EffectsPatcher.ID_LARGE_CAR_MOVEMENT, "Cars (large)" },
            { EffectsPatcher.ID_METRO_MOVEMENT, "Metros" },
            { EffectsPatcher.ID_POLICE_CAR_SIREN, "Sirens (police cars)" },
            { EffectsPatcher.ID_SMALL_CAR_MOVEMENT, "Cars (small)" },
            { EffectsPatcher.ID_TRAIN_MOVEMENT, "Trains" },
            { EffectsPatcher.ID_TRANSPORT_ARRIVE, "Transportation arrivals" },
        };

        protected GameObject[] AmbientVolumeSettingObjects = new GameObject[9];
        protected GameObject[] EffectVolumeSettingObjects = new GameObject[9];

        protected UITabstrip Tabstrip;
        protected UIButton AmbientsTabButton;
        protected UIButton EffectsTabButton;

        protected UITabContainer TabContainer;
        protected UIPanel AmbientsPanel;
        protected UIPanel EffectsPanel;

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
            this.width = 451;
            this.height = 431;
            this.Title = "SOUNDS TUNER";
            base.Start();

            this.Tabstrip = this.ContentPanel.AddUIComponent<UITabstrip>();
            this.Tabstrip.relativePosition = new Vector3(20, 20);
            this.Tabstrip.width = 410;
            this.Tabstrip.anchor = UIAnchorStyle.Top | UIAnchorStyle.Left | UIAnchorStyle.Right;

            this.TabContainer = this.ContentPanel.AddUIComponent<UITabContainer>();
            this.TabContainer.size = new Vector2(410, 330);
            this.TabContainer.relativePosition = new Vector3(20, 20 + this.Tabstrip.height);
            this.TabContainer.anchor = UIAnchorStyle.All;
            this.Tabstrip.tabPages = this.TabContainer;

            // Get template button from the options panel tabstrip
            UITabstrip optionTabStrip = GameObject.Find(GameObjectDefs.ID_OPTIONTABSTRIP).GetComponent<UITabstrip>();
            UIButton tabStripButtonTemplate = optionTabStrip.GetComponentInChildren<UIButton>();

            UIButton ambientsTabButton = this.Tabstrip.AddTab("AMBIENTS", tabStripButtonTemplate, true);
            ambientsTabButton.playAudioEvents = true;
            ambientsTabButton.focusedTextColor = tabStripButtonTemplate.focusedTextColor;
            UIButton effectsTabButton = this.Tabstrip.AddTab("EFFECTS", tabStripButtonTemplate, true);
            effectsTabButton.playAudioEvents = true;
            effectsTabButton.focusedTextColor = tabStripButtonTemplate.focusedTextColor;

            // Tabs layout
            UIPanel[] tabs = this.TabContainer.GetComponentsInChildren<UIPanel>();
            this.AmbientsPanel = tabs[0];
            this.EffectsPanel = tabs[1];

            this.AmbientsPanel.padding = new RectOffset(5, 5, 10, 10);
            this.AmbientsPanel.autoLayout = true;
            this.AmbientsPanel.autoLayoutDirection = LayoutDirection.Vertical;
            this.AmbientsPanel.autoLayoutPadding = new RectOffset(0, 0, 5, 5);
            //this.AmbientsPanel.wrapLayout = true;

            this.EffectsPanel.Hide();
            this.EffectsPanel.padding = new RectOffset(5, 5, 10, 10);
            this.EffectsPanel.autoLayout = true;
            this.EffectsPanel.autoLayoutDirection = LayoutDirection.Vertical;
            this.EffectsPanel.autoLayoutPadding = new RectOffset(0, 0, 5, 5);
            //this.EffectsPanel.wrapLayout = true;

            // Settings
            Mod.Settings.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.World, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.World], out this.ambientVolumeWorld);
            Mod.Settings.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Forest, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.Forest], out this.ambientVolumeForest);
            Mod.Settings.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Sea, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.Sea], out this.ambientVolumeSea);
            Mod.Settings.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Stream, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.Stream], out this.ambientVolumeStream);
            Mod.Settings.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Industrial, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.Industrial], out this.ambientVolumeIndustrial);
            Mod.Settings.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Plaza, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.Plaza], out this.ambientVolumePlaza);
            Mod.Settings.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Suburban, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.Suburban], out this.ambientVolumeSuburban);
            Mod.Settings.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.City, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.City], out this.ambientVolumeCity);
            Mod.Settings.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Agricultural, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.Agricultural], out this.ambientVolumeAgricultural);

            Mod.Settings.State.EffectVolumes.TryGetValueOrDefault(EffectsPatcher.ID_TRANSPORT_ARRIVE, EffectsPatcher.OriginalVolumes[EffectsPatcher.ID_TRANSPORT_ARRIVE], out this.effectVolumeAircraftMovement);
            Mod.Settings.State.EffectVolumes.TryGetValueOrDefault(EffectsPatcher.ID_AMBULANCE_SIREN, EffectsPatcher.OriginalVolumes[EffectsPatcher.ID_AMBULANCE_SIREN], out this.effectVolumeAmbulanceSiren);
            Mod.Settings.State.EffectVolumes.TryGetValueOrDefault(EffectsPatcher.ID_FIRE_TRUCK_SIREN, EffectsPatcher.OriginalVolumes[EffectsPatcher.ID_FIRE_TRUCK_SIREN], out this.effectVolumeFireTruckSiren);
            Mod.Settings.State.EffectVolumes.TryGetValueOrDefault(EffectsPatcher.ID_LARGE_CAR_MOVEMENT, EffectsPatcher.OriginalVolumes[EffectsPatcher.ID_LARGE_CAR_MOVEMENT], out this.effectVolumeLargeCarMovement);
            Mod.Settings.State.EffectVolumes.TryGetValueOrDefault(EffectsPatcher.ID_METRO_MOVEMENT, EffectsPatcher.OriginalVolumes[EffectsPatcher.ID_METRO_MOVEMENT], out this.effectVolumeMetroMovement);
            Mod.Settings.State.EffectVolumes.TryGetValueOrDefault(EffectsPatcher.ID_POLICE_CAR_SIREN, EffectsPatcher.OriginalVolumes[EffectsPatcher.ID_POLICE_CAR_SIREN], out this.effectVolumePoliceCarSiren);
            Mod.Settings.State.EffectVolumes.TryGetValueOrDefault(EffectsPatcher.ID_SMALL_CAR_MOVEMENT, EffectsPatcher.OriginalVolumes[EffectsPatcher.ID_SMALL_CAR_MOVEMENT], out this.effectVolumeSmallCarMovement);
            Mod.Settings.State.EffectVolumes.TryGetValueOrDefault(EffectsPatcher.ID_TRAIN_MOVEMENT, EffectsPatcher.OriginalVolumes[EffectsPatcher.ID_TRAIN_MOVEMENT], out this.effectVolumeTrainMovement);
            Mod.Settings.State.EffectVolumes.TryGetValueOrDefault(EffectsPatcher.ID_TRANSPORT_ARRIVE, EffectsPatcher.OriginalVolumes[EffectsPatcher.ID_TRANSPORT_ARRIVE], out this.effectVolumeTransportArrive);

            // Sliders
            this.AmbientVolumeSettingObjects[0] = this.CreateAmbientVolumeSetting(AudioManager.AmbientType.World, "ambientVolumeWorld");
            this.AmbientVolumeSettingObjects[1] = this.CreateAmbientVolumeSetting(AudioManager.AmbientType.Forest, "ambientVolumeForest");
            this.AmbientVolumeSettingObjects[2] = this.CreateAmbientVolumeSetting(AudioManager.AmbientType.Sea, "ambientVolumeSea");
            this.AmbientVolumeSettingObjects[3] = this.CreateAmbientVolumeSetting(AudioManager.AmbientType.Stream, "ambientVolumeStream");
            this.AmbientVolumeSettingObjects[4] = this.CreateAmbientVolumeSetting(AudioManager.AmbientType.Industrial, "ambientVolumeIndustrial");
            this.AmbientVolumeSettingObjects[5] = this.CreateAmbientVolumeSetting(AudioManager.AmbientType.Plaza, "ambientVolumePlaza");
            this.AmbientVolumeSettingObjects[6] = this.CreateAmbientVolumeSetting(AudioManager.AmbientType.Suburban, "ambientVolumeSuburban");
            this.AmbientVolumeSettingObjects[7] = this.CreateAmbientVolumeSetting(AudioManager.AmbientType.City, "ambientVolumeCity");
            this.AmbientVolumeSettingObjects[8] = this.CreateAmbientVolumeSetting(AudioManager.AmbientType.Agricultural, "ambientVolumeAgricultural");

            this.EffectVolumeSettingObjects[0] = this.CreateEffectVolumeSetting(EffectsPatcher.ID_TRANSPORT_ARRIVE, "effectVolumeAircraftMovement", 0, 0.5f); // Default value = 0.5f
            this.EffectVolumeSettingObjects[1] = this.CreateEffectVolumeSetting(EffectsPatcher.ID_AMBULANCE_SIREN, "effectVolumeAmbulanceSiren");
            this.EffectVolumeSettingObjects[2] = this.CreateEffectVolumeSetting(EffectsPatcher.ID_FIRE_TRUCK_SIREN, "effectVolumeFireTruckSiren", 0, 3); // Default value = 3
            this.EffectVolumeSettingObjects[3] = this.CreateEffectVolumeSetting(EffectsPatcher.ID_LARGE_CAR_MOVEMENT, "effectVolumeLargeCarMovement", 0, 1.5f); // Default value = 1.5
            this.EffectVolumeSettingObjects[4] = this.CreateEffectVolumeSetting(EffectsPatcher.ID_METRO_MOVEMENT, "effectVolumeMetroMovement", 0, 0.5f); // Default value = 0.5f
            this.EffectVolumeSettingObjects[5] = this.CreateEffectVolumeSetting(EffectsPatcher.ID_POLICE_CAR_SIREN, "effectVolumePoliceCarSiren");
            this.EffectVolumeSettingObjects[6] = this.CreateEffectVolumeSetting(EffectsPatcher.ID_SMALL_CAR_MOVEMENT, "effectVolumeSmallCarMovement", 0, 1.5f); // Default value = 1.5
            this.EffectVolumeSettingObjects[7] = this.CreateEffectVolumeSetting(EffectsPatcher.ID_TRAIN_MOVEMENT, "effectVolumeTrainMovement", 0, 0.5f); // Default value = 0.5f
            this.EffectVolumeSettingObjects[8] = this.CreateEffectVolumeSetting(EffectsPatcher.ID_TRANSPORT_ARRIVE, "effectVolumeTransportArrive");

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
                var effectsPanelChildComponents = ReflectionUtils.GetPrivateField<PoolList<UIComponent>>(this.EffectsPanel, "m_ChildComponents");
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
            PropertyChangedEventHandler<float> valueChangedCallback = new PropertyChangedEventHandler<float>((c, v) =>
            {
                Mod.Settings.State.AmbientVolumes[type] = v;
                if (SimulationManager.instance.m_metaData != null && SimulationManager.instance.m_metaData.m_updateMode != SimulationManager.UpdateMode.Undefined)
                {
                    AmbientsPatcher.PatchAmbientVolumeFor(type, v);
                }
            });

            return CreateVolumeSetting(this.AmbientsPanel, "AmbientVolumeSetting", type.ToString(), memberName, valueChangedCallback, minValue, maxValue);
        }

        protected GameObject CreateEffectVolumeSetting(string name, string memberName, float minValue = 0, float maxValue = 1)
        {
            PropertyChangedEventHandler<float> valueChangedCallback = new PropertyChangedEventHandler<float>((c, v) =>
            {
                Mod.Settings.State.EffectVolumes[name] = v;
                if (SimulationManager.instance.m_metaData != null && SimulationManager.instance.m_metaData.m_updateMode != SimulationManager.UpdateMode.Undefined)
                {
                    EffectsPatcher.PatchEffectVolumeFor(name, v);
                }
            });

            return CreateVolumeSetting(this.EffectsPanel, "EffectVolumeSetting", name, memberName, valueChangedCallback, minValue, maxValue);
        }

        protected GameObject CreateVolumeSetting(UIComponent parent, string gameObjectName, string name, string memberName, PropertyChangedEventHandler<float> valueChangedCallback, float minValue = 0, float maxValue = 1)
        {
            this.SliderNames.TryGetValueOrDefault(name, name, out name);

            float panelWidth = parent.width - ((UIPanel)parent).padding.left - ((UIPanel)parent).padding.right;
            float sliderX = panelWidth - 210;

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
            slider.width = 200;
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
