using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.Utils;
using ColossalFramework.UI;
using UnityEngine;

namespace AmbientSoundsTuner.UI
{
    internal static class AdvancedOptions
    {
        private static GameObject advancedOptions;
        private static UIButton advancedOptionsButton;
        private static AdvancedOptionsWindow advancedOptionsWindow;

        public static void CreateAmbientOptions()
        {
            if (advancedOptions != null)
            {
                Logger.Debug("Trying to recreate the Sounds Tuner button and window while they already exist");
                return;
            }

            // Create button and attach it
            UIPanel audioOptionsPanel = OptionsPanelUtils.GetAudioOptionsPanel();
            advancedOptionsButton = audioOptionsPanel.AddUIComponent<UIButton>();
            advancedOptionsButton.name = "AdvancedSoundsTunerButton";
            advancedOptionsButton.text = "Sounds Tuner";
            advancedOptionsButton.isLocalized = false;
            advancedOptionsButton.playAudioEvents = true;
            advancedOptionsButton.normalBgSprite = "ButtonMenu";
            advancedOptionsButton.hoveredTextColor = new Color32(7, 132, 255, 255);
            advancedOptionsButton.disabledTextColor = new Color32(46, 46, 46, 255);
            advancedOptionsButton.autoSize = true;
            advancedOptionsButton.textPadding = new RectOffset(8, 8, 8, 8);
            advancedOptionsButton.textScale = 1.3f;
            advancedOptionsButton.eventClick += ambientOptionsButton_eventClick;

            advancedOptions = new GameObject("AdvancedSoundsTuner");
            advancedOptionsWindow = advancedOptions.AddComponent<AdvancedOptionsWindow>();
            Logger.Info("Created Sounds Tuner button and window");
        }

        public static void DestroyAmbientOptions()
        {
            if (advancedOptions == null)
            {
                return;
            }

            GameObject.DestroyObject(advancedOptionsButton);
            GameObject.DestroyObject(advancedOptions);
            advancedOptions = null;
            advancedOptionsButton = null;
            advancedOptionsWindow = null;
        }

        private static void ambientOptionsButton_eventClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            Logger.Debug("Opening Sounds Tuner window");

            UIView.GetAView().AttachUIComponent(advancedOptions);
            advancedOptionsWindow.transform.SetParent(GameObject.Find(GameObjectDefs.ID_LIBRARY_OPTIONSPANEL).GetComponent<OptionsPanel>().component.transform);
            advancedOptionsWindow.BringToFront();

            Window.ShowWindow(advancedOptions.GetComponent<AdvancedOptionsWindow>());
        }
    }
}
