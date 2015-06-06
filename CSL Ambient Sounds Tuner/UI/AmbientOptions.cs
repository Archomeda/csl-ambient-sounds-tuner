using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.Utils;
using ColossalFramework.UI;
using UnityEngine;

namespace AmbientSoundsTuner.UI
{
    internal static class AmbientOptions
    {
        private static GameObject ambientOptions;
        private static UIButton ambientOptionsButton;
        private static AmbientOptionsWindow ambientOptionsWindow;

        public static void CreateAmbientOptions()
        {
            if (ambientOptions != null)
            {
                Logger.Debug("Trying to recreate the Ambient Sounds Tuner button and window while they already exist");
                return;
            }

            // Create button and attach it
            UIPanel audioOptionsPanel = OptionsPanelUtils.GetAudioOptionsPanel();
            ambientOptionsButton = audioOptionsPanel.AddUIComponent<UIButton>();
            ambientOptionsButton.name = "AmbientSoundsTunerButton";
            ambientOptionsButton.text = "Ambient Sounds Tuner";
            ambientOptionsButton.isLocalized = false;
            ambientOptionsButton.playAudioEvents = true;
            ambientOptionsButton.normalBgSprite = "ButtonMenu";
            ambientOptionsButton.hoveredTextColor = new Color32(7, 132, 255, 255);
            ambientOptionsButton.disabledTextColor = new Color32(46, 46, 46, 255);
            ambientOptionsButton.autoSize = true;
            ambientOptionsButton.textPadding = new RectOffset(8, 8, 8, 8);
            ambientOptionsButton.textScale = 1.3f;
            ambientOptionsButton.eventClick += ambientOptionsButton_eventClick;

            ambientOptions = new GameObject("AmbientSoundsTuner");
            ambientOptionsWindow = ambientOptions.AddComponent<AmbientOptionsWindow>();
            Logger.Info("Created Ambient Sounds Tuner button and window");
        }

        public static void DestroyAmbientOptions()
        {
            if (ambientOptions == null)
            {
                return;
            }

            GameObject.DestroyObject(ambientOptionsButton);
            GameObject.DestroyObject(ambientOptions);
            ambientOptions = null;
            ambientOptionsButton = null;
            ambientOptionsWindow = null;
        }

        private static void ambientOptionsButton_eventClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            Logger.Debug("Opening Ambient Sounds Tuner window");

            UIView.GetAView().AttachUIComponent(ambientOptions);
            ambientOptionsWindow.transform.SetParent(OptionsPanelUtils.GetNativeOptionsPanel().GetComponent<OptionsPanel>().component.transform);
            ambientOptionsWindow.BringToFront();

            Window.ShowWindow(ambientOptions.GetComponent<AmbientOptionsWindow>());
        }
    }
}
