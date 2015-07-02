using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalFramework.UI;
using CommonShared.Defs;
using UnityEngine;

namespace CommonShared.UI.Extensions
{
    /// <summary>
    /// A class that contains extensions for <see cref="UIHelperBase"/> objects.
    /// </summary>
    public static class UIHelperBaseExtensions
    {
        /// <summary>
        /// Creates a tab strip.
        /// </summary>
        /// <param name="helper">The <see cref="UIHelper"/>.</param>
        /// <returns>The newly created tab strip.</returns>
        public static UITabstrip AddTabstrip(this UIHelper helper)
        {
            UITabstrip tabstrip = ((UIComponent)helper.self).AddUIComponent<UITabstrip>();
            tabstrip.tabPages = ((UIComponent)helper.self).AddUIComponent<UITabContainer>();
            return tabstrip;
        }

        /// <summary>
        /// Creates a tab.
        /// </summary>
        /// <param name="helper">The <see cref="UIHelper"/>.</param>
        /// <param name="tabstrip">The <see cref="UITabstrip"/> this tab should belong to.</param>
        /// <param name="buttonWidth">The width of the tab button.</param>
        /// <param name="title">The title of the tab.</param>
        /// <returns>A <see cref="UIHelper"/> object of the newly created tab.</returns>
        public static UIHelper AddTab(this UIHelper helper, UITabstrip tabstrip, float buttonWidth, string title)
        {
            UITabstrip keyMappingTabstrip = GameObject.Find(GameObjectDefs.ID_KEYMAPPING_TABSTRIP).GetComponent<UITabstrip>();
            UIButton buttonTemplate = null;
            if (keyMappingTabstrip == null)
            {
                buttonTemplate = keyMappingTabstrip.GetComponentInChildren<UIButton>();
            }

            UIButton tabButton = tabstrip.AddTab(title, buttonTemplate, true);
            tabButton.playAudioEvents = buttonTemplate.playAudioEvents;
            tabButton.pressedTextColor = buttonTemplate.pressedTextColor;
            tabButton.focusedTextColor = buttonTemplate.focusedTextColor;
            tabButton.disabledTextColor = buttonTemplate.disabledTextColor;
            tabButton.width = buttonWidth;

            UIPanel tab = tabstrip.tabPages.components.Last() as UIPanel;
            tab.autoLayout = true;
            tab.autoLayoutDirection = LayoutDirection.Vertical;
            tab.autoLayoutPadding = new RectOffset(0, 0, 2, 0);
            return new UIHelper(tab);
        }

        /// <summary>
        /// Creates a scrolling tab.
        /// </summary>
        /// <param name="helper">The <see cref="UIHelper"/>.</param>
        /// <param name="tabstrip">The <see cref="UITabstrip"/> this tab should belong to.</param>
        /// <param name="buttonWidth">The width of the tab button.</param>
        /// <param name="title">The title of the tab.</param>
        /// <returns>A <see cref="UIHelper"/> object of the newly created tab.</returns>
        public static UIHelper AddScrollingTab(this UIHelper helper, UITabstrip tabstrip, float buttonWidth, string title)
        {
            UIPanel panelTemplate = (UIPanel)UITemplateManager.Peek(UITemplateDefs.ID_OPTIONS_SCROLL_PANEL_TEMPLATE);
            return helper.AddCustomTab(tabstrip, panelTemplate, buttonWidth, title);
        }

        /// <summary>
        /// Creates a custom tab.
        /// </summary>
        /// <param name="helper">The <see cref="UIHelper"/>.</param>
        /// <param name="tabstrip">The <see cref="UITabstrip"/> this tab should belong to.</param>
        /// <param name="panelTemplate">The panel template.</param>
        /// <param name="buttonWidth">The width of the tab button.</param>
        /// <param name="title">The title of the tab.</param>
        /// <returns>A <see cref="UIHelper"/> object of the newly created tab.</returns>
        public static UIHelper AddCustomTab(this UIHelper helper, UITabstrip tabstrip, UIPanel panelTemplate, float buttonWidth, string title)
        {
            UITabstrip keyMappingTabstrip = GameObject.Find(GameObjectDefs.ID_KEYMAPPING_TABSTRIP).GetComponent<UITabstrip>();
            UIButton buttonTemplate = null;
            if (keyMappingTabstrip != null)
            {
                buttonTemplate = keyMappingTabstrip.GetComponentInChildren<UIButton>();
            }

            UIButton tabButton = tabstrip.AddCustomTab(title, buttonTemplate, panelTemplate, true);
            tabButton.playAudioEvents = buttonTemplate.playAudioEvents;
            tabButton.pressedTextColor = buttonTemplate.pressedTextColor;
            tabButton.focusedTextColor = buttonTemplate.focusedTextColor;
            tabButton.disabledTextColor = buttonTemplate.disabledTextColor;
            tabButton.width = buttonWidth;

            UIPanel tab = tabstrip.tabPages.components.Last() as UIPanel;
            UIScrollablePanel scrollableTab = tab.GetComponentInChildren<UIScrollablePanel>();
            if (scrollableTab != null)
            {
                scrollableTab.autoLayout = true;
                scrollableTab.autoLayoutDirection = LayoutDirection.Vertical;
                scrollableTab.autoLayoutPadding = new RectOffset(0, 0, 2, 0);
                return new UIHelper(scrollableTab);
            }
            else
            {
                tab.autoLayout = true;
                tab.autoLayoutDirection = LayoutDirection.Vertical;
                tab.autoLayoutPadding = new RectOffset(0, 0, 2, 0);
                return new UIHelper(tab);
            }
        }

        /// <summary>
        /// Creates a group.
        /// </summary>
        /// <param name="parentHelper">The parent <see cref="UIHelper"/>.</param>
        /// <param name="title">The title of the group.</param>
        /// <returns>A <see cref="UIHelper"/> object of the newly created group.</returns>
        public static UIHelper AddGroup2(this UIHelper parentHelper, string title)
        {
            UIHelper groupHelper = (UIHelper)parentHelper.AddGroup(title);
            ((UIComponent)groupHelper.self).parent.width = ((UIComponent)parentHelper.self).width - 20;
            ((UIComponent)groupHelper.self).width = ((UIComponent)groupHelper.self).parent.width;
            return groupHelper;
        }
    }
}
