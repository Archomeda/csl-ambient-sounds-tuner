using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmbientSoundsTuner.SoundPatchers.Sounds
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SoundCategoryAttribute : Attribute
    {
        public SoundCategoryAttribute(string categoryId, string categoryName, string subCategoryName)
        {
            this.CategoryId = categoryId;
            this.CategoryName = categoryName;
            this.SubCategoryName = subCategoryName;
        }

        public string CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string SubCategoryName { get; set; }
    }
}
