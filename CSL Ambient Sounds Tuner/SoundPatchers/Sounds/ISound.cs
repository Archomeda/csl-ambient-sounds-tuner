using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.SoundPack.Migration;

namespace AmbientSoundsTuner.SoundPatchers.Sounds
{
    /// <summary>
    /// An interface for a sound definition.
    /// Check <see cref="SoundBase"/> for an easy implementation.
    /// </summary>
    public interface ISound
    {
        /// <summary>
        /// Gets the sound id.
        /// </summary>
        /// <value>
        /// The sound id.
        /// </value>
        string Id { get; }

        /// <summary>
        /// Gets the sound name.
        /// </summary>
        /// <value>
        /// The sound name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the sound category id.
        /// </summary>
        /// <value>
        /// The sound category id.
        /// </value>
        string CategoryId { get; }

        /// <summary>
        /// Gets the sound category name.
        /// </summary>
        /// <value>
        /// The sound category name.
        /// </value>
        string CategoryName { get; }

        /// <summary>
        /// Gets the sound sub category name.
        /// </summary>
        /// <value>
        /// The sound sub category name.
        /// </value>
        string SubCategoryName { get; }

        /// <summary>
        /// Gets the default minimum sound volume.
        /// </summary>
        /// <value>
        /// The default minimum sound volume.
        /// </value>
        float DefaultVolume { get; }

        /// <summary>
        /// Gets the default maximum sound volume.
        /// </summary>
        /// <value>
        /// The default maximum sound volume.
        /// </value>
        float MaxVolume { get; }

        /// <summary>
        /// Gets a value indicating whether this sound is active while in-game only.
        /// </summary>
        /// <value>
        /// <c>true</c> if this sound is active while in-game only; otherwise, <c>false</c>.
        /// </value>
        bool IngameOnly { get; }


        /// <summary>
        /// Gets the sound instance.
        /// </summary>
        /// <returns>The sound instance.</returns>
        SoundContainer GetSoundInstance();

        /// <summary>
        /// Backs up the sound.
        /// </summary>
        void BackUpSound();

        /// <summary>
        /// Patches the sound.
        /// </summary>
        /// <param name="newSound">The new sound.</param>
        void PatchSound(SoundPacksFileV1.Audio newSound);

        /// <summary>
        /// Reverts the sound.
        /// </summary>
        void RevertSound();

        /// <summary>
        /// Backs up the volume.
        /// </summary>
        void BackUpVolume();

        /// <summary>
        /// Patches the volume.
        /// </summary>
        /// <param name="volume">The volume.</param>
        void PatchVolume(float volume);

        /// <summary>
        /// Reverts the volume.
        /// </summary>
        void RevertVolume();
    }
}
