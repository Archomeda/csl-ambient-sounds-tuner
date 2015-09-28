using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmbientSoundsTuner.SoundPatchers.Sounds
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SoundAttribute : Attribute
    {
        public SoundAttribute()
        {
            this.IngameOnly = true;
        }

        public SoundAttribute(string id, string name)
            : this()
        {
            this.Id = id;
            this.Name = name;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public bool IngameOnly { get; set; }
    }
}
