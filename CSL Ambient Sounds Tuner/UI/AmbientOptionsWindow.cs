using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.Utils;
using AmbientSoundsTuner.Extensions;
using ColossalFramework.DataBinding;
using ColossalFramework.UI;
using UnityEngine;

namespace AmbientSoundsTuner.UI
{
    public class AmbientOptionsWindow : Window
    {
        protected GameObject[] VolumeSettingObjects = new GameObject[9];

        public float volumeWorld = 1;
        public float volumeForest = 1;
        public float volumeSea = 1;
        public float volumeStream = 1;
        public float volumeIndustrial = 1;
        public float volumePlaza = 1;
        public float volumeSuburban = 1;
        public float volumeCity = 1;
        public float volumeAgricultural = 1;

        public override void Start()
        {
            this.width = 321;
            this.height = 361;
            this.Title = "AMBIENT SOUNDS TUNER";
            base.Start();

            Configuration.Instance.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.World, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.World], out this.volumeWorld);
            Configuration.Instance.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Forest, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.Forest], out this.volumeForest);
            Configuration.Instance.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Sea, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.Sea], out this.volumeSea);
            Configuration.Instance.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Stream, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.Stream], out this.volumeStream);
            Configuration.Instance.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Industrial, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.Industrial], out this.volumeIndustrial);
            Configuration.Instance.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Plaza, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.Plaza], out this.volumePlaza);
            Configuration.Instance.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Suburban, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.Suburban], out this.volumeSuburban);
            Configuration.Instance.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.City, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.City], out this.volumeCity);
            Configuration.Instance.State.AmbientVolumes.TryGetValueOrDefault(AudioManager.AmbientType.Agricultural, AmbientsPatcher.OriginalVolumes[AudioManager.AmbientType.Agricultural], out this.volumeAgricultural);

            this.ContentPanel.autoLayout = true;
            this.ContentPanel.autoLayoutDirection = LayoutDirection.Vertical;
            this.ContentPanel.autoLayoutPadding = new UnityEngine.RectOffset(10, 10, 5, 5);

            this.VolumeSettingObjects[0] = this.CreateVolumeSetting("World");
            this.VolumeSettingObjects[1] = this.CreateVolumeSetting("Forest");
            this.VolumeSettingObjects[2] = this.CreateVolumeSetting("Sea");
            this.VolumeSettingObjects[3] = this.CreateVolumeSetting("Stream");
            this.VolumeSettingObjects[4] = this.CreateVolumeSetting("Industrial");
            this.VolumeSettingObjects[5] = this.CreateVolumeSetting("Plaza");
            this.VolumeSettingObjects[6] = this.CreateVolumeSetting("Suburban");
            this.VolumeSettingObjects[7] = this.CreateVolumeSetting("City");
            this.VolumeSettingObjects[8] = this.CreateVolumeSetting("Agricultural");
        }

        public override void Close()
        {
            base.Close();
            Configuration.Instance.State.AmbientVolumes[AudioManager.AmbientType.World] = volumeWorld;
            Configuration.Instance.State.AmbientVolumes[AudioManager.AmbientType.Forest] = volumeForest;
            Configuration.Instance.State.AmbientVolumes[AudioManager.AmbientType.Sea] = volumeSea;
            Configuration.Instance.State.AmbientVolumes[AudioManager.AmbientType.Stream] = volumeStream;
            Configuration.Instance.State.AmbientVolumes[AudioManager.AmbientType.Industrial] = volumeIndustrial;
            Configuration.Instance.State.AmbientVolumes[AudioManager.AmbientType.Plaza] = volumePlaza;
            Configuration.Instance.State.AmbientVolumes[AudioManager.AmbientType.Suburban] = volumeSuburban;
            Configuration.Instance.State.AmbientVolumes[AudioManager.AmbientType.City] = volumeCity;
            Configuration.Instance.State.AmbientVolumes[AudioManager.AmbientType.Agricultural] = volumeAgricultural;
            Configuration.Save();
            Mod.PatchAmbientSounds();
        }

        protected GameObject CreateVolumeSetting(string name)
        {
            GameObject setting = new GameObject("AmbientVolumeSetting");
            UIPanel panel = setting.AddComponent<UIPanel>();
            panel.transform.SetParent(this.ContentPanel.transform);
            panel.width = 300;
            panel.height = 25;

            UILabel label = panel.AddUIComponent<UILabel>();
            label.width = 100;
            label.text = name;
            label.position = new Vector3(0, 0);

            GameObject sliderObject = UnityEngine.Object.Instantiate(GameObject.Find("SliderAmbientVolume"));
            panel.AttachUIComponent(sliderObject);

            UISlider slider = sliderObject.GetComponent<UISlider>();
            slider.width = 200;
            slider.position = new Vector3(100, 0);

            BindProperty binding = sliderObject.GetComponent<BindProperty>();
            binding.dataSource.component = this;
            binding.dataSource.memberName = "volume" + name;
            binding.dataTarget.component = slider;
            binding.dataTarget.memberName = "value";

            return setting;
        }
    }
}
