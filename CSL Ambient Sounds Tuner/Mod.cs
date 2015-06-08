using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.UI;
using AmbientSoundsTuner.Utils;
using ColossalFramework.Plugins;
using ICities;
using UnityEngine;

namespace AmbientSoundsTuner
{
    public class Mod : LoadingExtensionBase, IUserMod
    {
        internal const string FriendlyName = "Ambient Sounds Tuner";
        internal const string AssemblyName = "AmbientSoundsTuner";
        internal const ulong WorkshopId = 455958878;

        private bool isLoaded = false;

        public string Name
        {
            get
            {
                // Here we load our stuff (only if it's enabled, to prevent confusion), hacky, but oh well...
                if (!this.isLoaded)
                {
                    this.isLoaded = true;
                    this.Initialize();
                    PluginManager.instance.eventPluginsStateChanged += this.Initialize;
                }

                return FriendlyName;
            }
        }

        public string Description
        {
            get { return "Tune your ambient sounds volumes individually"; }
        }


        private void Initialize()
        {
            if (PluginUtils.GetPluginInfo().isEnabled)
            {
                Configuration.Load();
                if (Configuration.Instance.ExtraDebugLogging)
                {
                    Logger.Warning("Extra debug logging is enabled, please use this only to get more information while hunting for bugs; don't use this when playing normally!");
                }
                AdvancedOptions.CreateAdvancedOptions();
            }
            else
            {
                AdvancedOptions.DestroyAdvancedOptions();
            }
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
            AdvancedOptions.CreateAdvancedOptions();

            PatchAmbientSounds();
            PatchEffectSounds();
        }

        /// <summary>
        /// Our exit point. Here we unload everything we have loaded.
        /// </summary>
        public override void OnLevelUnloading()
        {
            base.OnLevelUnloading();

            Configuration.Save();

            // Set isLoaded to false again so the mod will load again at the main menu
            this.isLoaded = false;
        }

        internal static void PatchAmbientSounds()
        {
            int ambientSoundsCount = AmbientsPatcher.OriginalVolumes.Count;
            int backedUpAmbientSoundsCount = AmbientsPatcher.BackupAmbientVolumes();
            if (backedUpAmbientSoundsCount < ambientSoundsCount)
            {
                Logger.Warning("{0}/{1} ambient sound volumes have been backed up", backedUpAmbientSoundsCount, ambientSoundsCount);
            }
            int patchedAmbientSoundsCount = AmbientsPatcher.PatchAmbientVolumes();
            if (patchedAmbientSoundsCount < ambientSoundsCount)
            {
                Logger.Warning("{0}/{1} ambient sound volumes have been patched", patchedAmbientSoundsCount, ambientSoundsCount);
            }

            Logger.Info("Ambient sound volumes have been patched");
        }

        internal static void PatchEffectSounds()
        {
            int effectSoundsCount = EffectsPatcher.OriginalVolumes.Count;
            int backedUpEffectSoundsCount = EffectsPatcher.BackupEffectVolumes();
            if (backedUpEffectSoundsCount < effectSoundsCount)
            {
                Logger.Warning("{0}/{1} effect sound volumes have been backed up", backedUpEffectSoundsCount, effectSoundsCount);
            }
            int patchedEffectSoundsCount = EffectsPatcher.PatchEffectVolumes();
            if (patchedEffectSoundsCount < effectSoundsCount)
            {
                Logger.Warning("{0}/{1} effect sound volumes have been patched", patchedEffectSoundsCount, effectSoundsCount);
            }

            Logger.Info("Effect sound volumes have been patched");
        }

    }
}
