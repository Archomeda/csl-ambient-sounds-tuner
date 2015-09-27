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
    public class Mod : UserModBase<Mod>, IUserModSettingsUI
    {
        protected override ulong WorkshopId { get { return 455958878; } }

        protected override IEnumerable<ulong> IncompatibleMods
        {
            get
            {
                return new HashSet<ulong>()
                {
                    //421050717, // [ARIS] Remove Cows
                    //421052798, // [ARIS] Remove Pigs
                    //421041154, // [ARIS] Remove Seagulls
                    421527612, // SilenceObnoxiousSirens
                };
            }
        }

        internal Configuration Settings { get; private set; }

        internal string SettingsFilename { get; private set; }

        internal ModOptionsPanel OptionsPanel { get; private set; }


        #region UserModBase members

        public override string ModName
        {
            get { return "Ambient Sounds Tuner"; }
        }

        public override string ModDescription
        {
            get { return "Tune your ambient sounds volumes individually"; }
        }

        public override void OnModInitializing(bool enabled)
        {
            this.SettingsFilename = Path.Combine(FileUtils.GetStorageFolder(this), "AmbientSoundsTuner.yml");
            this.Log.Debug("Mod initialized");
        }

        public override void OnMainMenuLoading()
        {
            this.Load();

            this.PatchUISounds();
            this.Log.Debug("Mod loaded in-menu");
        }

        public override void OnModDeactivated()
        {
            this.Unload();
        }

        public override void OnGameLoaded(LoadMode mode)
        {
            this.Load();

            // Before we patch, we export the current game sounds as an example file
            var exampleFile = SoundPatchersManager.instance.GetCurrentSoundSettingsAsSoundPack();
            exampleFile.SaveConfig(Path.Combine(FileUtils.GetStorageFolder(Mod.Instance), "Example." + SoundPacksManager.SOUNDPACKS_FILENAME_XML));
            exampleFile.SaveConfig(Path.Combine(FileUtils.GetStorageFolder(Mod.Instance), "Example." + SoundPacksManager.SOUNDPACKS_FILENAME_YAML));

            this.PatchSounds();
            this.Log.Debug("Mod loaded in-game");
        }

        public override void OnGameUnloading()
        {
            this.Unload();
        }

        #endregion


        #region IUserModSettingsUI

        public void OnSettingsUI(UIHelperBase helper)
        {
            // Do regular settings UI stuff
            UIHelper uiHelper = helper as UIHelper;
            if (uiHelper != null)
            {
                this.OptionsPanel = new ModOptionsPanel(uiHelper);
                this.OptionsPanel.PerformLayout();
                this.Log.Debug("Options panel created");
            }
            else
            {
                this.Log.Warning("Could not populate the settings panel, helper is null or not a UIHelper");
            }
        }

        #endregion


        public string BuildVersion
        {
            get { return "dev version"; }
        }


        #region Loading / Unloading

        private void Load()
        {
            // We have to properly migrate the outdated XML configuration file
            string oldXmlSettingsFilename = Path.Combine(Path.GetDirectoryName(this.SettingsFilename), Path.GetFileNameWithoutExtension(this.SettingsFilename)) + ".xml";
            if (File.Exists(oldXmlSettingsFilename) && !File.Exists(this.SettingsFilename))
            {
                this.Settings = Configuration.LoadConfig(oldXmlSettingsFilename, new ConfigurationMigrator());
                this.Settings.SaveConfig(this.SettingsFilename);
                File.Delete(oldXmlSettingsFilename);
            }
            else
            {
                this.Settings = Configuration.LoadConfig(this.SettingsFilename, new ConfigurationMigrator());
            }

            this.Log.EnableDebugLogging = this.Settings.ExtraDebugLogging;
            if (this.Settings.ExtraDebugLogging)
            {
                this.Log.Warning("Extra debug logging is enabled, please use this only to get more information while hunting for bugs; don't use this when playing normally!");
            }

            // Load sound packs
            SoundPacksManager.instance.InitSoundPacks();

            // Detour UI click sounds
            CustomPlayClickSound.Detour();
        }

        private void Unload()
        {
            this.Settings.SaveConfig(this.SettingsFilename);
            CustomPlayClickSound.UnDetour();

            // Actually, to be consistent and nice, we should also revert the other sound patching here.
            // But since that sounds are only patched in-game, and closing that game conveniently resets the other sounds, it's not really needed.
            // If it's needed at some point in the future, we can add that logic here.

            this.Log.Debug("Mod unloaded");
        }

        #endregion


        private void PatchSounds<T>(SoundsInstancePatcher<T> patcher, IDictionary<T, ConfigurationV4.Sound> newSounds)
        {
            int backedUpSounds = patcher.BackupAllSounds();
            this.Log.Debug("{0} sounds have been backed up through {1}", backedUpSounds, patcher.GetType().Name);

            int backedUpVolumes = patcher.BackupAllVolumes();
            this.Log.Debug("{0} volumes have been backed up through {1}", backedUpVolumes, patcher.GetType().Name);

            int patchedSounds = patcher.PatchAllSounds(
                newSounds
                    .Where(kvp => !string.IsNullOrEmpty(kvp.Value.SoundPack) && kvp.Value.SoundPack != "Default")
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => patcher.GetAudioByName(kvp.Key.ToString(), kvp.Value.SoundPack)
                    )
                );
            this.Log.Debug("{0} sounds have been patched through {1}", patchedSounds, patcher.GetType().Name);

            int patchedVolumes = patcher.PatchAllVolumes(newSounds.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Volume));
            this.Log.Debug("{0} volumes have been patched through {1}", patchedVolumes, patcher.GetType().Name);
        }

        internal void PatchSounds()
        {
            // Patch various sounds for compatibility first!
            switch (SoundDuplicator.PatchPoliceSiren())
            {
                case SoundDuplicator.PatchResult.Success:
                    this.Log.Debug("Police sirens have been patched for compatibility");
                    break;
                case SoundDuplicator.PatchResult.AlreadyPatched:
                    this.Log.Debug("Police sirens have been patched for compatibility already");
                    break;
                case SoundDuplicator.PatchResult.NotFound:
                    this.Log.Warning("Could not patch the police sirens for compatibility");
                    break;
            }
            switch (SoundDuplicator.PatchScooterSound())
            {
                case SoundDuplicator.PatchResult.Success:
                    this.Log.Debug("Scooter sounds have been patched for compatibility");
                    break;
                case SoundDuplicator.PatchResult.AlreadyPatched:
                    this.Log.Debug("Scooter sounds have been patched for compatibility already");
                    break;
                case SoundDuplicator.PatchResult.NotFound:
                    this.Log.Warning("Could not patch the scooter sounds for compatibility");
                    break;
            }
            switch (SoundDuplicator.PatchOilPowerPlant())
            {
                case SoundDuplicator.PatchResult.Success:
                    this.Log.Debug("Oil power plant sounds have been patched for compatibility");
                    break;
                case SoundDuplicator.PatchResult.AlreadyPatched:
                    this.Log.Debug("Oil power plant sounds have been patched for compatibility already");
                    break;
                case SoundDuplicator.PatchResult.NotFound:
                    this.Log.Warning("Could not patch the oil power plant sounds for compatibility");
                    break;
            }
            switch (SoundDuplicator.PatchWaterTreatmentPlant())
            {
                case SoundDuplicator.PatchResult.Success:
                    this.Log.Debug("Water treatment plant sounds have been patched for compatibility");
                    break;
                case SoundDuplicator.PatchResult.AlreadyPatched:
                    this.Log.Debug("Water treatment plant sounds have been patched for compatibility already");
                    break;
                case SoundDuplicator.PatchResult.NotFound:
                    this.Log.Warning("Could not patch the water treatment plant sounds for compatibility");
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
                this.Log.Warning("Could not patch sounds: {0}", ex);
            }
        }

        internal void PatchUISounds()
        {
            foreach (var id in new[] { MiscPatcher.ID_CLICK_SOUND, MiscPatcher.ID_DISABLED_CLICK_SOUND })
            {
                if (this.Settings.MiscSounds.ContainsKey(id))
                {
                    SoundPatchersManager.instance.MiscPatcher.PatchVolume(id, this.Settings.MiscSounds[id].Volume);
                }
            }
        }
    }
}
