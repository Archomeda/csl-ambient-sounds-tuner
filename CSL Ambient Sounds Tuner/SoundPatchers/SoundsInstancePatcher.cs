using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.SoundPack;
using AmbientSoundsTuner.SoundPack.Migration;

namespace AmbientSoundsTuner.SoundPatchers
{
    /// <summary>
    /// An abstract class that holds a generic implementation to patch sound instances.
    /// </summary>
    /// <typeparam name="T">The type of the sound ids.</typeparam>
    public abstract class SoundsInstancePatcher<T>
    {
        /// <summary>
        /// Gets the ids used by this patcher.
        /// </summary>
        public abstract T[] Ids { get; }

        private Dictionary<T, float> defaultVolumes = new Dictionary<T, float>();

        /// <summary>
        /// Gets the default volumes.
        /// </summary>
        public Dictionary<T, float> DefaultVolumes
        {
            get { return this.defaultVolumes; }
        }

        private Dictionary<T, float> defaultMaxVolumes = new Dictionary<T, float>();

        /// <summary>
        /// Gets the default max volumes.
        /// </summary>
        public Dictionary<T, float> DefaultMaxVolumes
        {
            get { return this.defaultMaxVolumes; }
        }

        private Dictionary<T, float> oldVolumes = new Dictionary<T, float>();

        /// <summary>
        /// Gets the backed up volumes.
        /// </summary>
        public Dictionary<T, float> OldVolumes
        {
            get { return this.oldVolumes; }
        }

        private Dictionary<T, SoundPacksFileV1.Audio> oldSounds = new Dictionary<T, SoundPacksFileV1.Audio>();

        /// <summary>
        /// Gets the backed up sounds.
        /// </summary>
        public Dictionary<T, SoundPacksFileV1.Audio> OldSounds
        {
            get { return this.oldSounds; }
        }

        /// <summary>
        /// Gets the unique prefix id for audios for this patcher.
        /// </summary>
        public abstract string AudioPrefixId { get; }

        /// <summary>
        /// Gets all available audios for a specific audio type.
        /// </summary>
        /// <param name="audioType">The audio type.</param>
        /// <returns>The available audios.</returns>
        public virtual IDictionary<string, SoundPacksFileV1.Audio> GetAvailableAudiosForType(string audioType)
        {
            return SoundPacksManager.instance.AudioFiles.Where(kvp => kvp.Key.StartsWith(this.AudioPrefixId + "." + audioType)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        /// <summary>
        /// Gets an audio by its name.
        /// </summary>
        /// <param name="audioType">The audio type.</param>
        /// <param name="audioName">The name of the audio.</param>
        /// <returns>The audio if it exists; null otherwise.</returns>
        public virtual SoundPacksFileV1.Audio GetAudioByName(string audioType, string audioName)
        {
            string id = this.AudioPrefixId + "." + audioType + "." + audioName;
            if (SoundPacksManager.instance.AudioFiles.ContainsKey(id))
            {
                return SoundPacksManager.instance.AudioFiles[id];
            }
            return null;
        }


        /// <summary>
        /// Gets a sound instance.
        /// </summary>
        /// <param name="id">The sound id.</param>
        /// <returns>The sound if it exists; null otherwise.</returns>
        public abstract SoundContainer GetSoundInstance(T id);


        private int BackupAll(Action<T> backupFunc, Action<string, SoundBackupException> feedbackFunc)
        {
            int counter = 0;
            foreach (T key in this.Ids)
            {
                try
                {
                    backupFunc(key);
                    feedbackFunc(key.ToString(), null);
                    counter++;
                }
                catch (SoundBackupException ex)
                {
                    feedbackFunc(key.ToString(), ex);
                }
            }
            return counter;
        }

        private int PatchAll<V>(Action<T, V> patchFunc, IDictionary<T, V> newItems, Action<string, SoundPatchException> feedbackFunc)
        {
            int counter = 0;
            foreach (var newItem in newItems)
            {
                try
                {
                    patchFunc(newItem.Key, newItem.Value);
                    feedbackFunc(newItem.Key.ToString(), null);
                    counter++;
                }
                catch (SoundPatchException ex)
                {
                    feedbackFunc(newItem.Key.ToString(), ex);
                }
            }
            return counter;
        }


        /// <summary>
        /// Backs up the volumes.
        /// </summary>
        /// <returns>The number of succeeded operations.</returns>
        public virtual int BackupAllVolumes()
        {
            return this.BackupAll(this.BackupVolume, (id, ex) =>
            {
                if (ex == null)
                {
                    // Successfully backed up
                    Mod.Instance.Log.Debug("Sound volume of '{0}' has been successfully backed up", id);
                }
                else
                {
                    // Failed to back up
                    Mod.Instance.Log.Warning("Failed to back up the sound volume of '{0}'\r\n{1}", id, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
                }
            });
        }

        /// <summary>
        /// Backs up a specific volume.
        /// </summary>
        /// <param name="id">The id of the volume to back up.</param>
        /// <returns>True if successful; false otherwise.</returns>
        public virtual void BackupVolume(T id)
        {
            try
            {
                SoundContainer sound = this.GetSoundInstance(id);
                float? volume = SoundsPatcher.GetVolume(sound);

                if (!volume.HasValue)
                    throw new SoundBackupException(id.ToString(), "Sound has no volume set");

                this.OldVolumes[id] = volume.Value;
            }
            catch (Exception ex)
            {
                if (ex is SoundBackupException) throw ex;
                else throw new SoundBackupException(id.ToString(), ex);
            }
        }

        /// <summary>
        /// Patches the volumes.
        /// </summary>
        /// <param name="newVolumes">The new volumes.</param>
        /// <returns>The number of succeeded operations.</returns>
        public virtual int PatchAllVolumes(IDictionary<T, float> newVolumes)
        {
            return this.PatchAll(this.PatchVolume, newVolumes, (id, ex) =>
            {
                if (ex == null)
                {
                    // Successfully backed up
                    Mod.Instance.Log.Debug("Sound volume of '{0}' has been successfully patched", id);
                }
                else
                {
                    // Failed to back up
                    Mod.Instance.Log.Warning("Failed to patch the sound volume of '{0}'\r\n{1}", id, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
                }
            });
        }

        /// <summary>
        /// Patches a specific volume.
        /// </summary>
        /// <param name="id">The id of the volume to patch.</param>
        /// <param name="newVolume">The new volume.</param>
        /// <returns>True if successful; false otherwise.</returns>
        public virtual void PatchVolume(T id, float newVolume)
        {
            try
            {
                SoundContainer sound = this.GetSoundInstance(id);

                if (!SoundsPatcher.SetVolume(sound, newVolume))
                    throw new Exception("Failed to set volume");
            }
            catch (Exception ex)
            {
                if (ex is SoundPatchException) throw ex;
                else throw new SoundPatchException(id.ToString(), ex);
            }
        }

        /// <summary>
        /// Reverts the volumes to the default values.
        /// </summary>
        /// <returns>The number of succeeded operations.</returns>
        public virtual int RevertAllVolumes()
        {
            return this.PatchAllVolumes(this.OldVolumes);
        }

        /// <summary>
        /// Reverts a specific volume to its default value.
        /// </summary>
        /// <param name="id">The id of the volume to revert.</param>
        /// <returns>True if successful; false otherwise.</returns>
        public virtual void RevertVolume(T id)
        {
            if (this.OldVolumes.ContainsKey(id))
                this.PatchVolume(id, this.OldVolumes[id]);
        }


        /// <summary>
        /// Backs up the sounds.
        /// </summary>
        /// <returns>The number of succeeded operations.</returns>
        public virtual int BackupAllSounds()
        {
            return this.BackupAll(this.BackupSound, (id, ex) =>
            {
                if (ex == null)
                {
                    // Successfully backed up
                    Mod.Instance.Log.Debug("Sound instance of '{0}' has been successfully backed up", id);
                }
                else
                {
                    // Failed to back up
                    Mod.Instance.Log.Warning("Failed to back up the sound instance of '{0}'\r\n{1}", id, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
                }
            });
        }

        /// <summary>
        /// Backs up a specific sound.
        /// </summary>
        /// <param name="id">The id of the sound to back up.</param>
        /// <returns>True if successful; false otherwise.</returns>
        public virtual void BackupSound(T id)
        {
            try
            {
                SoundContainer sound = this.GetSoundInstance(id);
                this.OldSounds[id] = SoundsPatcher.GetAudioInfo(sound);

                if (this.OldSounds[id] == null)
                    throw new SoundBackupException(id.ToString(), "AudioInfo is null");
            }
            catch (Exception ex)
            {
                if (ex is SoundBackupException) throw ex;
                else throw new SoundBackupException(id.ToString(), ex);
            }
        }

        /// <summary>
        /// Patches the sounds.
        /// </summary>
        /// <param name="newSounds">The new sounds.</param>
        /// <returns>The number of succeeded operations.</returns>
        public virtual int PatchAllSounds(IDictionary<T, SoundPacksFileV1.Audio> newSounds)
        {
            return this.PatchAll(this.PatchSound, newSounds, (id, ex) =>
            {
                if (ex == null)
                {
                    // Successfully backed up
                    Mod.Instance.Log.Debug("Sound instance of '{0}' has been successfully backed up", id);
                }
                else
                {
                    // Failed to back up
                    Mod.Instance.Log.Warning("Failed to patch the sound instance of '{0}'\r\n{1}", id, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
                }
            });
        }

        /// <summary>
        /// Patches a specific sound.
        /// </summary>
        /// <param name="id">The id of the sound to patch.</param>
        /// <param name="newSound">The new sound.</param>
        /// <returns>True if successful; false otherwise.</returns>
        public virtual void PatchSound(T id, SoundPacksFileV1.Audio newSound)
        {
            try
            {
                SoundContainer sound = this.GetSoundInstance(id);

                if (!SoundsPatcher.SetAudioInfo(sound, newSound))
                    throw new SoundPatchException(id.ToString(), "Failed to set AudioInfo");
            }
            catch (Exception ex)
            {
                if (ex is SoundPatchException) throw ex;
                else throw new SoundPatchException(id.ToString(), ex);
            }
        }

        /// <summary>
        /// Reverts the sounds to the default values.
        /// </summary>
        /// <returns>The number of succeeded operations.</returns>
        public virtual int RevertAllSounds()
        {
            return this.PatchAllSounds(this.OldSounds);
        }

        /// <summary>
        /// Reverts a specific sound to its default value.
        /// </summary>
        /// <param name="id">The id of the sound to revert.</param>
        /// <returns>True if successful; false otherwise.</returns>
        public virtual void RevertSound(T id)
        {
            if (this.OldSounds.ContainsKey(id))
                this.PatchSound(id, this.OldSounds[id]);
        }
    }
}
