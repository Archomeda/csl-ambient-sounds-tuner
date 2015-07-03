using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalFramework.UI;
using CommonShared.UI.Extensions;
using UnityEngine;

namespace CommonShared.UI
{
    /// <summary>
    /// An abstract class that provides basic logic for implementing a configuration panel for a mod.
    /// </summary>
    public abstract class ConfigPanelBase
    {
        /// <summary>
        /// Gets or sets the <see cref="UIHelper"/> of the root panel.
        /// </summary>
        protected UIHelper RootHelper { get; set; }

        /// <summary>
        /// Gets the root panel of this options panel.
        /// </summary>
        public UIScrollablePanel RootPanel
        {
            get
            {
                return this.RootHelper.self as UIScrollablePanel;
            }
        }

        /// <summary>
        /// Gets the size of the inner area of the root panel.
        /// </summary>
        public Vector2 RootPanelInnerArea
        {
            get
            {
                return new Vector2(this.RootPanel.width - ((UIPanel)this.RootPanel.parent).padding.horizontal - 20, this.RootPanel.height - ((UIPanel)this.RootPanel.parent).padding.vertical - 20);
            }
        }

        private bool prevVisible = false;

        /// <summary>
        /// Creates a new panel for the configuration of a mod.
        /// </summary>
        /// <param name="helper">The <see cref="UIHelper"/> of the root panel.</param>
        public ConfigPanelBase(UIHelper helper)
        {
            this.RootHelper = helper;
        }

        /// <summary>
        /// Performs the layout.
        /// </summary>
        public virtual void PerformLayout()
        {
            if (this.RootPanel == null)
            {
                return;
            }

            this.PopulateUI();

            this.RootPanel.eventVisibilityChanged += RootPanel_eventVisibilityChanged;
        }

        /// <summary>
        /// Populates the UI panel.
        /// </summary>
        protected abstract void PopulateUI();

        /// <summary>
        /// This method gets called when the visibility of the panel changes to false.
        /// This can be overridden to save settings or do other tasks upon exiting the config panel for example.
        /// </summary>
        protected virtual void OnClose() { }

        private void RootPanel_eventVisibilityChanged(UIComponent component, bool value)
        {
            if (this.prevVisible && !value)
            {
                this.OnClose();
            }
            this.prevVisible = value;
        }
    }
}
