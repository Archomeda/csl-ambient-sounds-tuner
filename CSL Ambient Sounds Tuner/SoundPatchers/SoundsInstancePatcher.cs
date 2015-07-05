using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.SoundPack;

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

        private Dictionary<T, SoundPackFile.Audio> oldSounds = new Dictionary<T, SoundPackFile.Audio>();

        /// <summary>
        /// Gets the backed up sounds.
        /// </summary>
        public Dictionary<T, SoundPackFile.Audio> OldSounds
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
        public virtual IDictionary<string, SoundPackFile.Audio> GetAvailableAudiosForType(string audioType)
        {
            return SoundPacksManager.instance.AudioFiles.Where(kvp => kvp.Key.StartsWith(this.AudioPrefixId + "." + audioType)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        /// <summary>
        /// Gets an audio by its name.
        /// </summary>
        /// <param name="audioType">The audio type.</param>
        /// <param name="audioName">The name of the audio.</param>
        /// <returns>The audio if it exists; null otherwise.</returns>
        public virtual SoundPackFile.Audio GetAudioByName(string audioType, string audioName)
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


        private int BackupAll(Func<T, bool> backupFunc)
        {
            int counter = 0;
            foreach (T key in this.Ids)
            {
                if (backupFunc(key))
                {
                    Mod.Log.Debug("Sound instance of '{0}' has been backed up", key);
                    counter++;
                }
                else
                {
                    Mod.Log.Debug("Sound instance of '{0}' has not been backed up", key);
                }
            }
            return counter;
        }

        private int PatchAll<V>(Func<T, V, bool> patchFunc, IDictionary<T, V> newItems)
        {
            int counter = 0;
            foreach (var newItem in newItems)
            {
                if (patchFunc(newItem.Key, newItem.Value))
                {
                    Mod.Log.Debug("Sound instance of '{0}' has been patched", newItem.Key);
                    counter++;
                }
                else
                {
                    Mod.Log.Debug("Sound instance of '{0}' has not been patched", newItem.Key);
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
            return this.BackupAll(this.BackupVolume);
        }

        /// <summary>
        /// Backs up a specific volume.
        /// </summary>
        /// <param name="id">The id of the volume to back up.</param>
        /// <returns>True if successful; false otherwise.</returns>
        public virtual bool BackupVolume(T id)
        {
            if (SimulationManager.instance.m_metaData != null && SimulationManager.instance.m_metaData.m_updateMode != SimulationManager.UpdateMode.Undefined)
            {
                SoundContainer sound = this.GetSoundInstance(id);
                float? volume = SoundsPatcher.GetVolume(sound);
                if (volume.HasValue)
                {
                    this.OldVolumes[id] = volume.Value;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Patches the volumes.
        /// </summary>
        /// <param name="newVolumes">The new volumes.</param>
        /// <returns>The number of succeeded operations.</returns>
        public virtual int PatchAllVolumes(IDictionary<T, float> newVolumes)
        {
            return this.PatchAll(this.PatchVolume, newVolumes);
        }

        /// <summary>
        /// Patches a specific volume.
        /// </summary>
        /// <param name="id">The id of the volume to patch.</param>
        /// <param name="newVolume">The new volume.</param>
        /// <returns>True if successful; false otherwise.</returns>
        public virtual bool PatchVolume(T id, float newVolume)
        {
            if (SimulationManager.instance.m_metaData != null && SimulationManager.instance.m_metaData.m_updateMode != SimulationManager.UpdateMode.Undefined)
            {
                SoundContainer sound = this.GetSoundInstance(id);
                return SoundsPatcher.SetVolume(sound, newVolume);
            }
            return false;
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
        public virtual bool RevertVolume(T id)
        {
            if (this.OldVolumes.ContainsKey(id))
                return this.PatchVolume(id, this.OldVolumes[id]);
            return false;
        }


        /// <summary>
        /// Backs up the sounds.
        /// </summary>
        /// <returns>The number of succeeded operations.</returns>
        public virtual int BackupAllSounds()
        {
            return this.BackupAll(this.BackupSound);
        }

        /// <summary>
        /// Backs up a specific sound.
        /// </summary>
        /// <param name="id">The id of the sound to back up.</param>
        /// <returns>True if successful; false otherwise.</returns>
        public virtual bool BackupSound(T id)
        {
            if (SimulationManager.instance.m_metaData != null && SimulationManager.instance.m_metaData.m_updateMode != SimulationManager.UpdateMode.Undefined)
            {
                SoundContainer sound = this.GetSoundInstance(id);
                this.OldSounds[id] = SoundsPatcher.GetAudioInfo(sound);
                return this.OldSounds[id] != null;
            }
            return false;
        }

        /// <summary>
        /// Patches the sounds.
        /// </summary>
        /// <param name="newSounds">The new sounds.</param>
        /// <returns>The number of succeeded operations.</returns>
        public virtual int PatchAllSounds(IDictionary<T, SoundPackFile.Audio> newSounds)
        {
            return this.PatchAll(this.PatchSound, newSounds);
        }

        /// <summary>
        /// Patches a specific sound.
        /// </summary>
        /// <param name="id">The id of the sound to patch.</param>
        /// <param name="newSound">The new sound.</param>
        /// <returns>True if successful; false otherwise.</returns>
        public virtual bool PatchSound(T id, SoundPackFile.Audio newSound)
        {
            if (SimulationManager.instance.m_metaData != null && SimulationManager.instance.m_metaData.m_updateMode != SimulationManager.UpdateMode.Undefined)
            {
                SoundContainer sound = this.GetSoundInstance(id);
                return SoundsPatcher.SetAudioInfo(sound, newSound);
            }
            return false;
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
        public virtual bool RevertSound(T id)
        {
            if (this.OldSounds.ContainsKey(id))
                return this.PatchSound(id, this.OldSounds[id]);
            return false;
        }
    }
}
