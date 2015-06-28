using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmbientSoundsTuner
{
    /// <summary>
    /// A class that holds a generic implementation to patch sound instances.
    /// </summary>
    /// <typeparam name="T">The type of the sound ids.</typeparam>
    public abstract class SoundsInstancePatcher<T>
    {
        private Dictionary<T, float> defaultVolumes = new Dictionary<T, float>();

        /// <summary>
        /// Gets the default volumes, either internally determined by the code, or set upon start of the game before any patch has occured by this mod.
        /// </summary>
        public Dictionary<T, float> DefaultVolumes
        {
            get { return this.defaultVolumes; }
        }

        /// <summary>
        /// Backs up the volumes.
        /// </summary>
        /// <returns>The number of succeeded operations.</returns>
        public int BackupVolumes()
        {
            List<T> volumeKeys = new List<T>(DefaultVolumes.Keys);
            int counter = 0;
            foreach (T key in volumeKeys)
            {
                if (this.BackupVolume(key))
                {
                    counter++;
                }
                else
                {
                    Mod.Log.Info("Sound instance of '{0}' has not been found.", key);
                }
            }
            return counter;
        }

        /// <summary>
        /// Backs up a specific volume.
        /// </summary>
        /// <param name="id">The id of the volume to back up.</param>
        /// <returns>True if successful; false otherwise.</returns>
        public abstract bool BackupVolume(T id);

        /// <summary>
        /// Patches the volumes.
        /// </summary>
        /// <param name="newVolumes">The new volumes.</param>
        /// <returns>The number of succeeded operations.</returns>
        public int PatchVolumes(IDictionary<T, float> newVolumes)
        {
            int counter = 0;
            foreach (var newVolume in newVolumes)
            {
                if (this.PatchVolume(newVolume.Key, newVolume.Value))
                {
                    counter++;
                }
                else
                {
                    Mod.Log.Info("Sound instance of '{0}' has not been found.", newVolume.Key);
                }
            }
            return counter;
        }

        /// <summary>
        /// Patches a specific volume.
        /// </summary>
        /// <param name="id">The id of the volume to patch.</param>
        /// <param name="newVolume">The new volume.</param>
        /// <returns>True if successful; false otherwise.</returns>
        public abstract bool PatchVolume(T id, float newVolume);

        /// <summary>
        /// Reverts the volumes to the default values.
        /// </summary>
        /// <returns>The number of succeeded operations.</returns>
        public int RevertVolumesToDefault()
        {
            List<T> volumeKeys = new List<T>(DefaultVolumes.Keys);
            int counter = 0;
            foreach (T key in volumeKeys)
            {
                float newVolume;
                if (this.DefaultVolumes.TryGetValue(key, out newVolume) && this.PatchVolume(key, newVolume))
                {
                    counter++;
                }
                else
                {
                    Mod.Log.Info("Sound instance of '{0}' has not been found.", key);
                }
            }
            return counter;
        }
    }
}
