using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.Compatibility;
using AmbientSoundsTuner.Detour;
using AmbientSoundsTuner.Migration;
using AmbientSoundsTuner.SoundPack;
using AmbientSoundsTuner.SoundPatchers;
using AmbientSoundsTuner.UI;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using CommonShared;
using CommonShared.Configuration;
using CommonShared.Utils;
using ICities;
using UnityEngine;

namespace AmbientSoundsTuner
{
    public class Mod : LoadingExtensionBase, IUserMod
    {
        internal static Configuration Settings { get; private set; }
        internal static string SettingsFilename { get; private set; }
        internal static Logger Log { get; private set; }
        internal static Mod Instance { get; private set; }

        internal ModOptionsPanel OptionsPanel { get; private set; }

        internal static HashSet<ulong> IncompatibleMods = new HashSet<ulong>()
        {
            421050717, // [ARIS] Remove Cows
            421052798, // [ARIS] Remove Pigs
            421041154, // [ARIS] Remove Seagulls
            421527612, // SilenceObnoxiousSirens
        };

        private bool isLoadedInMainMenu = false;


        #region IUserMod members

        public string Name
        {
            get { return "Ambient Sounds Tuner"; }
        }

        public string Description
        {
            get { return "Tune your ambient sounds volumes individually"; }
        }

        public void OnSettingsUI(UIHelperBase helper)
        {
            // Since this method gets called on the main menu when this mod is enabled, we will also hook some of our loading here
            if (SimulationManager.instance.m_metaData == null || SimulationManager.instance.m_metaData.m_updateMode == SimulationManager.UpdateMode.Undefined)
            {
                // Here we ensure this gets only loaded on main menu, and not in-game
                this.Init();
                this.Load(GameState.MainMenu);
            }

            // Do regular settings UI stuff
            UIHelper uiHelper = helper as UIHelper;
            if (uiHelper != null)
            {
                this.OptionsPanel = new ModOptionsPanel(uiHelper);
                this.OptionsPanel.PerformLayout();
                Mod.Log.Debug("Options panel created");
            }
            else
            {
                Mod.Log.Warning("Could not populate the settings panel, helper is null or not a UIHelper");
            }
        }

        #endregion

        public string BuildVersion
        {
            get { return "dev version"; }
        }

        #region Loading / Unloading

        private void Init()
        {
            SettingsFilename = Path.Combine(FileUtils.GetStorageFolder(this), "AmbientSoundsTuner.xml");
            Log = new Logger(this);
            Instance = this;

            Mod.Log.Debug("Mod initialized");
        }

        private void Load(GameState state)
        {
            // Pre-load in main menu
            if (state == GameState.MainMenu)
            {
                if (this.isLoadedInMainMenu) { return; }

                Action<bool> pluginStateChangeCallback = null;
                pluginStateChangeCallback = new Action<bool>(isEnabled =>
                {
                    if (!isEnabled)
                    {
                        this.Unload();
                        PluginUtils.UnsubscribePluginStateChange(this, pluginStateChangeCallback);
                    }
                });
                PluginUtils.SubscribePluginStateChange(this, pluginStateChangeCallback);
                this.isLoadedInMainMenu = true;
            }

            // Load regular
            this.CheckIncompatibility();

            Mod.Settings = VersionedConfig.LoadConfig<Configuration>(Mod.SettingsFilename, new ConfigurationMigrator());
            Mod.Log.EnableDebugLogging = Mod.Settings.ExtraDebugLogging;

            if (Mod.Settings.ExtraDebugLogging)
            {
                Mod.Log.Warning("Extra debug logging is enabled, please use this only to get more information while hunting for bugs; don't use this when playing normally!");
            }

            // Load sound packs
            SoundPacksManager.instance.InitSoundPacks();

            // Patch sounds based on game state
            CustomPlayClickSound.Detour();

            if (state == GameState.InGame)
            {
                this.PatchSounds();
            }
            else if (state == GameState.MainMenu)
            {
                this.PatchUISounds();
                this.isLoadedInMainMenu = true;
            }

            Mod.Log.Debug("Mod loaded");
        }

        private void Unload()
        {
            Mod.Settings.SaveConfig(Mod.SettingsFilename);
            CustomPlayClickSound.UnDetour();

            // Actually, to be consistent and nice, we should also revert the other sound patching here.
            // But since that sounds are only patched in-game, and closing that game conveniently resets the other sounds, it's not really needed.
            // If it's needed at some point in the future, we can add that logic here.

            this.isLoadedInMainMenu = false;
            Mod.Log.Debug("Mod unloaded");
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

                if (text != "")
                {
                    Mod.Log.Warning("You've got some known incompatible mods enabled! It's possible that this mod doesn't work as expected.\nThe following incompatible mods are enabled: {0}.", text);
                }
            }

            Mod.Log.Debug("Incompatibility check completed");
        }

        #endregion


        #region LoadingExtensionBase members

        /// <summary>
        /// Our entry point. Here we load the mod.
        /// </summary>
        /// <param name="mode">The game mode.</param>
        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);
            this.Init();
            this.Load(GameState.InGame);
        }

        /// <summary>
        /// Our exit point. Here we unload everything we have loaded.
        /// </summary>
        public override void OnLevelUnloading()
        {
            base.OnLevelUnloading();
            this.Unload();
        }

        #endregion


        private void PatchSounds<T>(SoundsInstancePatcher<T> patcher, IDictionary<T, Configuration.Sound> newSounds)
        {
            int backedUpSounds = patcher.BackupAllSounds();
            Mod.Log.Debug("{0} sounds have been backed up through {1}", backedUpSounds, patcher.GetType().Name);

            int backedUpVolumes = patcher.BackupAllVolumes();
            Mod.Log.Debug("{0} volumes have been backed up through {1}", backedUpVolumes, patcher.GetType().Name);

            int patchedSounds = patcher.PatchAllSounds(newSounds.ToDictionary(kvp => kvp.Key, kvp =>
            {
                if (!string.IsNullOrEmpty(kvp.Value.Active))
                {
                    return patcher.GetAudioByName(kvp.Key.ToString(), kvp.Value.Active);
                }
                return null;
            }));
            Mod.Log.Debug("{0} sounds have been patched through {1}", patchedSounds, patcher.GetType().Name);

            int patchedVolumes = patcher.PatchAllVolumes(newSounds.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Volume));
            Mod.Log.Debug("{0} volumes have been patched through {1}", patchedVolumes, patcher.GetType().Name);
        }

        internal void PatchSounds()
        {
            // Patch the sirens for compatibility first!
            var patchResult = SirensPatcher.PatchPoliceSiren();
            switch (patchResult)
            {
                case SirensPatcher.PatchResult.Success:
                    Mod.Log.Debug("Police sirens have been patched");
                    break;
                case SirensPatcher.PatchResult.AlreadyPatched:
                    Mod.Log.Debug("Police sirens have been patched already");
                    break;
                case SirensPatcher.PatchResult.NotFound:
                    Mod.Log.Warning("Could not patch the police sirens to be different from the ambulance sirens");
                    break;
            }

            // Try patching the sounds
            try
            {
                this.PatchSounds(SoundPatchersManager.instance.AmbientsPatcher, Settings.AmbientSounds);
                this.PatchSounds(SoundPatchersManager.instance.AnimalsPatcher, Settings.AnimalSounds);
                this.PatchSounds(SoundPatchersManager.instance.BuildingsPatcher, Settings.BuildingSounds);
                this.PatchSounds(SoundPatchersManager.instance.VehiclesPatcher, Settings.VehicleSounds);
                this.PatchSounds(SoundPatchersManager.instance.MiscPatcher, Settings.MiscSounds);
            }
            catch (Exception ex)
            {
                Mod.Log.Warning("Could not patch sounds: {0}", ex);
            }
        }

        internal void PatchUISounds()
        {
            foreach (var id in new[] { MiscPatcher.ID_CLICK_SOUND, MiscPatcher.ID_DISABLED_CLICK_SOUND })
            {
                if (Mod.Settings.MiscSounds.ContainsKey(id))
                {
                    SoundPatchersManager.instance.MiscPatcher.PatchVolume(id, Mod.Settings.MiscSounds[id].Volume);
                }
            }
        }


        public enum GameState
        {
            MainMenu,
            InGame
        }
    }
}
