using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmbientSoundsTuner.SoundPatchers
{
    public class SoundPatchException : Exception
    {
        public SoundPatchException(string soundId, string message) :
            base(string.Format("Failed to patch sound {0}: {1}", soundId, message))
        {
            this.SoundId = soundId;
        }

        public SoundPatchException(string soundId, Exception innerException)
            : base(string.Format("Failed to patch sound {0}", soundId), innerException)
        {
            this.SoundId = soundId;
        }

        public string SoundId { get; set; }
    }
}
