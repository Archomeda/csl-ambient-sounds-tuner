using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.Defs;
using ColossalFramework.UI;
using UnityEngine;

namespace AmbientSoundsTuner.Utils
{
    internal static class OptionsPanelUtils
    {
        public static UIPanel GetAudioOptionsPanel()
        {
            GameObject optionsContainer = GameObject.Find(GameObjectDefs.ID_OPTIONSCONTAINER);
            if (optionsContainer != null)
            {
                return optionsContainer.GetComponentsInChildren<UIPanel>().FirstOrDefault(p => p.name == "Audio");
            }
            return null;
        }
    }
}
