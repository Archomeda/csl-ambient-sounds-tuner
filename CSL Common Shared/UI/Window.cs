using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalFramework.UI;
using UnityEngine;

namespace CommonShared.UI
{
    /// <summary>
    /// A default options window.
    /// </summary>
    public class Window : UIPanel
    {
        public const float TitleBarHeight = 40;

        public override void Start()
        {
            base.Start();
            this.Hide();
            this.backgroundSprite = "UnlockingPanel2";
            UITextureAtlas atlas = Resources.FindObjectsOfTypeAll<UITextureAtlas>().FirstOrDefault(a => a.name == "Ingame");
            if (atlas != null)
            {
                this.atlas = atlas;
            }
            this.color = new Color32(58, 88, 104, 255);
            this.canFocus = true;
            this.isInteractive = true;
            this.autoSize = true;

            this.CreateTitle();
            this.CreateDragHandle();
            this.CreateCloseButton();
            this.CreateContentPanel();
        }

        public virtual void Close()
        {
            this.Hide();
            if (this.parent != null)
            {
                this.parent.Focus();
            }
        }

        private string title = "";
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
                if (this.TitleObject != null)
                {
                    this.TitleObject.GetComponent<UILabel>().text = value;
                }
            }
        }

        public UIPanel ContentPanel
        {
            get
            {
                return this.ContentPanelObject.GetComponent<UIPanel>();
            }
        }

        protected GameObject TitleObject { get; private set; }
        protected GameObject DragHandleObject { get; private set; }
        protected GameObject CloseButtonObject { get; private set; }
        protected GameObject ContentPanelObject { get; private set; }

        protected virtual void CreateTitle()
        {
            this.TitleObject = new GameObject("Title");
            this.TitleObject.transform.parent = this.transform;
            this.TitleObject.transform.localPosition = Vector3.zero;

            UILabel title = this.TitleObject.AddComponent<UILabel>();
            title.text = this.title;
            title.textAlignment = UIHorizontalAlignment.Center;
            title.position = new Vector3(this.width / 2 - title.width / 2, -TitleBarHeight / 2 + title.height / 2).RoundToInt();
            title.anchor = UIAnchorStyle.Top | UIAnchorStyle.Left | UIAnchorStyle.Right;
            title.atlas = this.atlas;
        }

        protected virtual void CreateDragHandle()
        {
            this.DragHandleObject = new GameObject("DragHandler");
            this.DragHandleObject.transform.parent = this.transform;
            this.DragHandleObject.transform.localPosition = Vector3.zero;

            UIDragHandle dragHandle = this.DragHandleObject.AddComponent<UIDragHandle>();
            dragHandle.anchor = UIAnchorStyle.Top | UIAnchorStyle.Left | UIAnchorStyle.Right;
            dragHandle.size = new Vector2(this.width, TitleBarHeight);
        }

        protected virtual void CreateCloseButton()
        {
            this.CloseButtonObject = new GameObject("CloseButton");
            this.CloseButtonObject.transform.parent = this.transform;
            this.CloseButtonObject.transform.localPosition = Vector3.zero;

            UIButton closeButton = this.CloseButtonObject.AddComponent<UIButton>();
            closeButton.anchor = UIAnchorStyle.Top | UIAnchorStyle.Right;
            closeButton.size = new Vector2(32, 32);
            closeButton.normalBgSprite = "buttonclose";
            closeButton.hoveredBgSprite = "buttonclosehover";
            closeButton.pressedBgSprite = "buttonclosepressed";
            closeButton.relativePosition = new Vector3(this.width - closeButton.width - 4, 4);
            closeButton.playAudioEvents = true;
            closeButton.eventClick += (component, eventParam) => this.Close();
            closeButton.atlas = this.atlas;
        }

        protected virtual void CreateContentPanel()
        {
            this.ContentPanelObject = new GameObject("ContentPanel");
            this.ContentPanelObject.transform.parent = this.transform;
            this.ContentPanelObject.transform.localPosition = Vector3.zero;

            UIPanel contentPanel = this.ContentPanelObject.AddComponent<UIPanel>();
            contentPanel.anchor = UIAnchorStyle.Bottom | UIAnchorStyle.Left | UIAnchorStyle.Right;
            contentPanel.position = new Vector3(0, -TitleBarHeight);
            contentPanel.size = new Vector2(this.width, Mathf.Max(0, this.height - TitleBarHeight));
            contentPanel.atlas = this.atlas;
        }

        protected override void OnKeyDown(UIKeyEventParameter p)
        {
            if (!p.used && p.keycode == KeyCode.Escape)
            {
                this.Close();
                p.Use();
            }
            base.OnKeyDown(p);
        }

        public static void ShowWindow(Window window)
        {
            window.CenterToParent();
            window.Show(true);
            window.Focus();
        }
    }
}
