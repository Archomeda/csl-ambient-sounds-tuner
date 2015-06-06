using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.UI;
using AmbientSoundsTuner.Utils;
using ICities;
using UnityEngine;

namespace AmbientSoundsTuner
{
    public class Mod : LoadingExtensionBase, IUserMod
    {
        internal const string FriendlyName = "Ambient Sounds Tuner";
        internal const string AssemblyName = "AmbientSoundsTuner";
        internal const ulong WorkshopId = 455958878;

        public string Name
        {
            get
            {
                // Here we load our stuff (only if it's enabled, to prevent confusion), hacky, but oh well...
                if (PluginUtils.GetPluginInfo().isEnabled)
                {
                    Configuration.Load();
                    if (Configuration.Instance.ExtraDebugLogging)
                    {
                        Logger.Warning("Extra debug logging is enabled, please use this only to get more information while hunting for bugs; don't use this when playing normally!");
                    }
                    AmbientOptions.CreateAmbientOptions();
                }

                return FriendlyName;
            }
        }

        public string Description
        {
            get { return "Tune your ambient sounds volumes individually"; }
        }


        /// <summary>
        /// Our entry point. Here we load the mod.
        /// </summary>
        /// <param name="mode">The game mode.</param>
        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);

            Configuration.Load();
            if (Configuration.Instance.ExtraDebugLogging)
            {
                Logger.Warning("Extra debug logging is enabled, please use this only to get more information while hunting for bugs; don't use this when playing normally!");
            }
            AmbientOptions.CreateAmbientOptions();
            PatchAmbientSounds();
        }

        /// <summary>
        /// Our exit point. Here we unload everything we have loaded.
        /// </summary>
        public override void OnLevelUnloading()
        {
            base.OnLevelUnloading();

            Configuration.Save();
        }

        internal static void PatchAmbientSounds()
        {
            if (AudioManager.instance.m_properties.m_ambients.Length > 8)
            {
                AudioManager.instance.m_properties.m_ambients[0].m_volume = Configuration.Instance.State.AmbientWorld;
                AudioManager.instance.m_properties.m_ambients[1].m_volume = Configuration.Instance.State.AmbientForest;
                AudioManager.instance.m_properties.m_ambients[2].m_volume = Configuration.Instance.State.AmbientSea;
                AudioManager.instance.m_properties.m_ambients[3].m_volume = Configuration.Instance.State.AmbientStream;
                AudioManager.instance.m_properties.m_ambients[4].m_volume = Configuration.Instance.State.AmbientIndustrial;
                AudioManager.instance.m_properties.m_ambients[5].m_volume = Configuration.Instance.State.AmbientPlaza;
                AudioManager.instance.m_properties.m_ambients[6].m_volume = Configuration.Instance.State.AmbientSuburban;
                AudioManager.instance.m_properties.m_ambients[7].m_volume = Configuration.Instance.State.AmbientCity;
                AudioManager.instance.m_properties.m_ambients[8].m_volume = Configuration.Instance.State.AmbientAgricultural;
                Logger.Info("Ambient audio volumes have been patched");
            }
        }

    }
}
