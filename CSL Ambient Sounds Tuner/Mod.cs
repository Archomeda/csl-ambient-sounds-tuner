using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.Compatibility;
using AmbientSoundsTuner.UI;
using AmbientSoundsTuner.Utils;
using ColossalFramework.Plugins;
using CommonShared;
using CommonShared.Utils;
using ICities;
using UnityEngine;

namespace AmbientSoundsTuner
{
    public class Mod : LoadingExtensionBase, IUserMod
    {
        internal static Configuration Settings;
        internal static string SettingsFilename = Path.Combine(FileUtils.GetStorageFolder(), "AmbientSoundsTuner.xml");
        internal static Logger Log = new Logger();

        internal static HashSet<ulong> IncompatibleMods = new HashSet<ulong>()
        {
            421527612, // SilenceObnoxiousSirens
        };

        private bool isLoaded = false;

        public string Name
        {
            get
            {
                // Here we load our stuff (only if it's enabled, to prevent confusion), hacky, but oh well...
                if (!this.isLoaded)
                {
                    this.isLoaded = true;
                    this.Initialize(PluginUtils.GetPluginInfo().isEnabled);
                    PluginUtils.SubscribePluginStateChange(this.Initialize);
                }

                return "Ambient Sounds Tuner";
            }
        }

        public string Description
        {
            get { return "Tune your ambient sounds volumes individually"; }
        }


        private void Initialize(bool isEnabled)
        {
            if (isEnabled)
            {
                this.CheckIncompatibility();

                Mod.Settings = Config.LoadConfig<Configuration>(Mod.SettingsFilename);
                Mod.Log.EnableDebugLogging = Mod.Settings.ExtraDebugLogging;

                if (Mod.Settings.ExtraDebugLogging)
                {
                    Mod.Log.Warning("Extra debug logging is enabled, please use this only to get more information while hunting for bugs; don't use this when playing normally!");
                }

                AdvancedOptions.CreateAdvancedOptions();
            }
            else
            {
                AdvancedOptions.DestroyAdvancedOptions();
            }
        }

        private void CheckIncompatibility()
        {
            var list = PluginUtils.GetPluginInfosOf(IncompatibleMods);
            if (list.Count > 0)
            {
                string text = string.Join(", ",
                    list.Where(kvp => kvp.Value.isEnabled)
                        .Select(kvp => string.Format("{0} ({1})", kvp.Value.GetInstances<IUserMod>()[0].Name, kvp.Value.publishedFileID.AsUInt64.ToString()))
                        .OrderBy(s => s)
                        .ToArray());
                Mod.Log.Warning("You've got some known incompatible mods enabled! It's possible that this mod doesn't work as expected.\nThe following incompatible mods are enabled: {0}.", text);
            }
        }

        /// <summary>
        /// Our entry point. Here we load the mod.
        /// </summary>
        /// <param name="mode">The game mode.</param>
        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);
            this.Initialize(true);
            PatchAmbientSounds();
            PatchEffectSounds();
        }

        /// <summary>
        /// Our exit point. Here we unload everything we have loaded.
        /// </summary>
        public override void OnLevelUnloading()
        {
            base.OnLevelUnloading();
            Settings.SaveConfig(Mod.SettingsFilename);

            // Set isLoaded to false again so the mod will load again at the main menu
            this.isLoaded = false;
        }

        internal static void PatchAmbientSounds()
        {
            int ambientSoundsCount = AmbientsPatcher.OriginalVolumes.Count;
            int backedUpAmbientSoundsCount = AmbientsPatcher.BackupAmbientVolumes();
            if (backedUpAmbientSoundsCount < ambientSoundsCount)
            {
                Mod.Log.Warning("{0}/{1} ambient sound volumes have been backed up", backedUpAmbientSoundsCount, ambientSoundsCount);
            }
            int patchedAmbientSoundsCount = AmbientsPatcher.PatchAmbientVolumes();
            if (patchedAmbientSoundsCount < ambientSoundsCount)
            {
                Mod.Log.Warning("{0}/{1} ambient sound volumes have been patched", patchedAmbientSoundsCount, ambientSoundsCount);
            }

            Mod.Log.Info("Ambient sound volumes have been patched");
        }

        internal static void PatchEffectSounds()
        {
            // Patch the sirens for compatibility first!
            var patchResult = SirensPatcher.PatchPoliceSiren();
            switch (patchResult)
            {
                case SirensPatcher.PatchResult.Success:
                    Mod.Log.Info("Police sirens have been patched");
                    break;
                case SirensPatcher.PatchResult.AlreadyPatched:
                    Mod.Log.Info("Police sirens have been patched already");
                    break;
                case SirensPatcher.PatchResult.NotFound:
                    Mod.Log.Warning("Could not patch the police sirens to be different from the ambulance sirens");
                    break;
            }

            int effectSoundsCount = EffectsPatcher.OriginalVolumes.Count;
            int backedUpEffectSoundsCount = EffectsPatcher.BackupEffectVolumes();
            if (backedUpEffectSoundsCount < effectSoundsCount)
            {
                Mod.Log.Warning("{0}/{1} effect sound volumes have been backed up", backedUpEffectSoundsCount, effectSoundsCount);
            }
            int patchedEffectSoundsCount = EffectsPatcher.PatchEffectVolumes();
            if (patchedEffectSoundsCount < effectSoundsCount)
            {
                Mod.Log.Warning("{0}/{1} effect sound volumes have been patched", patchedEffectSoundsCount, effectSoundsCount);
            }

            Mod.Log.Info("Effect sound volumes have been patched");
        }

    }
}
