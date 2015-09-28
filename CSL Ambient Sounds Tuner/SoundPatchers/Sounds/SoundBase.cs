using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.SoundPack.Migration;

namespace AmbientSoundsTuner.SoundPatchers.Sounds
{
    /// <summary>
    /// Provides an abstract base class that implements <see cref="ISound`1"/>.
    /// </summary>
    [SoundVolume(1, 1)]
    public abstract class SoundBase : ISound
    {
        public virtual string Id { get { return this.GetAttribute<SoundAttribute>().Id; } }

        public virtual string Name { get { return this.GetAttribute<SoundAttribute>().Name; } }

        public virtual string CategoryId { get { return this.GetAttribute<SoundCategoryAttribute>().CategoryId; } }

        public virtual string CategoryName { get { return this.GetAttribute<SoundCategoryAttribute>().CategoryName; } }

        public virtual string SubCategoryName { get { return this.GetAttribute<SoundCategoryAttribute>().SubCategoryName; } }

        public virtual float DefaultVolume { get { return this.GetAttribute<SoundVolumeAttribute>().DefaultVolume; } }

        public virtual float MaxVolume { get { return this.GetAttribute<SoundVolumeAttribute>().MaxVolume; } }

        public virtual bool IngameOnly { get { return this.GetAttribute<SoundAttribute>().IngameOnly; } }

        private T GetAttribute<T>() where T : Attribute
        {
            return Attribute.GetCustomAttribute(this.GetType(), typeof(T), true) as T;
        }


        public SoundPacksFileV1.Audio OldSound { get; protected set; }

        public float? OldVolume { get; protected set; }


        public abstract SoundContainer GetSoundInstance();


        public virtual void BackUpSound()
        {
            try
            {
                var sound = this.GetSoundInstance();
                this.OldSound = SoundsPatcher.GetAudioInfo(sound);
                if (this.OldSound == null)
                    throw new SoundBackupException(string.Format("{0}.{1}", this.CategoryId, this.Id), "AudioInfo is null");
            }
            catch (Exception ex)
            {
                if (ex is SoundBackupException) throw ex;
                else throw new SoundBackupException(string.Format("{0}.{1}", this.CategoryId, this.Id), ex);
            }
        }

        public virtual void PatchSound(SoundPack.Migration.SoundPacksFileV1.Audio newSound)
        {
            try
            {
                var sound = this.GetSoundInstance();

                if (!SoundsPatcher.SetAudioInfo(sound, newSound))
                    throw new SoundPatchException(string.Format("{0}.{1}"), "Failed to set AudioInfo");
            }
            catch (Exception ex)
            {
                if (ex is SoundPatchException) throw ex;
                else throw new SoundPatchException(string.Format("{0}.{1}"), ex);
            }
        }

        public virtual void RevertSound()
        {
            if (this.OldSound != null)
                this.PatchSound(this.OldSound);
        }

        public virtual void BackUpVolume()
        {
            try
            {
                var sound = this.GetSoundInstance();
                float? volume = SoundsPatcher.GetVolume(sound);
                if (!volume.HasValue)
                    throw new SoundBackupException(string.Format("{0}.{1}", this.CategoryId, this.Id), "Sound has no volume set");
                this.OldVolume = volume.Value;
            }
            catch (Exception ex)
            {
                if (ex is SoundBackupException) throw ex;
                else throw new SoundBackupException(string.Format("{0}.{1}", this.CategoryId, this.Id), ex);
            }
        }

        public virtual void PatchVolume(float volume)
        {
            try
            {
                var sound = this.GetSoundInstance();

                if (!SoundsPatcher.SetVolume(sound, volume))
                    throw new SoundPatchException(string.Format("{0}.{1}"), "Failed to set volume");
            }
            catch (Exception ex)
            {
                if (ex is SoundPatchException) throw ex;
                else throw new SoundPatchException(string.Format("{0}.{1}"), ex);
            }
        }

        public virtual void RevertVolume()
        {
            if (this.OldVolume.HasValue)
                this.PatchVolume(this.OldVolume.Value);
        }
    }
}
