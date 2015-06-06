using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbientSoundsTuner.Utils;
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

            this.volumeWorld = Configuration.Instance.State.AmbientWorld;
            this.volumeForest = Configuration.Instance.State.AmbientForest;
            this.volumeSea = Configuration.Instance.State.AmbientSea;
            this.volumeStream = Configuration.Instance.State.AmbientStream;
            this.volumeIndustrial = Configuration.Instance.State.AmbientIndustrial;
            this.volumePlaza = Configuration.Instance.State.AmbientPlaza;
            this.volumeSuburban = Configuration.Instance.State.AmbientSuburban;
            this.volumeCity = Configuration.Instance.State.AmbientCity;
            this.volumeAgricultural = Configuration.Instance.State.AmbientAgricultural;

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
            Configuration.Instance.State.AmbientWorld = volumeWorld;
            Configuration.Instance.State.AmbientForest = volumeForest;
            Configuration.Instance.State.AmbientSea = volumeSea;
            Configuration.Instance.State.AmbientStream = volumeStream;
            Configuration.Instance.State.AmbientIndustrial = volumeIndustrial;
            Configuration.Instance.State.AmbientPlaza = volumePlaza;
            Configuration.Instance.State.AmbientSuburban = volumeSuburban;
            Configuration.Instance.State.AmbientCity = volumeCity;
            Configuration.Instance.State.AmbientAgricultural = volumeAgricultural;
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
