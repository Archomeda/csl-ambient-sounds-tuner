using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalFramework.UI;
using UnityEngine;

namespace AmbientSoundsTuner.Utils
{
    internal static class OptionsPanelUtils
    {
        public static UIPanel GetAudioOptionsPanel()
        {
            GameObject optionsContainer = GameObject.Find("OptionsContainer");
            if (optionsContainer != null)
            {
                return optionsContainer.GetComponentsInChildren<UIPanel>().FirstOrDefault(p => p.name == "Audio");
            }
            return null;
        }

        public static GameObject GetNativeOptionsPanel()
        {
            return GameObject.Find("(Library) OptionsPanel");
        }
    }
}
