using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalFramework.UI;
using CommonShared.Utils;
using UnityEngine;

namespace CommonShared.UI.Extensions
{
    /// <summary>
    /// A class that contains extensions for <see cref="UITabstrip"/> objects.
    /// </summary>
    public static class UITabstripExtensions
    {
        /// <summary>
        /// Custom extension method that will add a custom panel to a tab, instead of a default panel.
        /// </summary>
        /// <param name="tabstrip">The tab strip.</param>
        /// <param name="text">The tab name.</param>
        /// <param name="buttonTemplate">The button template.</param>
        /// <param name="panelTemplate">The panel template.</param>
        /// <param name="fillText">Should the button be updated with the tab name?</param>
        /// <returns>The added tab button.</returns>
        public static UIButton AddCustomTab(this UITabstrip tabstrip, string text, UIButton buttonTemplate, UIPanel panelTemplate, bool fillText)
        {
            UIButton button = tabstrip.AddUIComponent<UIButton>();
            button.name = text;
            button.atlas = tabstrip.atlas;
            if (fillText)
            {
                button.text = text;
            }
            button.group = tabstrip;
            if (buttonTemplate != null)
            {
                button.atlas = buttonTemplate.atlas;
                button.font = buttonTemplate.font;
                button.autoSize = buttonTemplate.autoSize;
                button.size = buttonTemplate.size;
                button.normalBgSprite = buttonTemplate.normalBgSprite;
                button.disabledBgSprite = buttonTemplate.disabledBgSprite;
                button.focusedBgSprite = buttonTemplate.focusedBgSprite;
                button.hoveredBgSprite = buttonTemplate.hoveredBgSprite;
                button.pressedBgSprite = buttonTemplate.pressedBgSprite;
                button.normalFgSprite = buttonTemplate.normalFgSprite;
                button.disabledFgSprite = buttonTemplate.disabledFgSprite;
                button.focusedFgSprite = buttonTemplate.focusedFgSprite;
                button.hoveredFgSprite = buttonTemplate.hoveredFgSprite;
                button.pressedFgSprite = buttonTemplate.pressedFgSprite;
                button.useDropShadow = buttonTemplate.useDropShadow;
                button.dropShadowColor = buttonTemplate.dropShadowColor;
                button.dropShadowOffset = buttonTemplate.dropShadowOffset;
                button.useOutline = buttonTemplate.useOutline;
                button.outlineColor = buttonTemplate.outlineColor;
                button.outlineSize = buttonTemplate.outlineSize;
                button.useGradient = buttonTemplate.useGradient;
                button.bottomColor = buttonTemplate.bottomColor;
                button.textColor = buttonTemplate.textColor;
                button.horizontalAlignment = buttonTemplate.horizontalAlignment;
                RectOffset textPadding = buttonTemplate.textPadding;
                button.textPadding = new RectOffset(textPadding.left, textPadding.right, textPadding.top, textPadding.bottom);
            }
            if (tabstrip.tabPages != null)
            {
                string name = "Tab " + (tabstrip.tabPages.childCount + 1);
                UIPanel panel = GameObject.Instantiate<UIPanel>(panelTemplate);
                tabstrip.tabPages.AttachUIComponent(panel.gameObject);
                panel.name = name;
                if (!string.IsNullOrEmpty(text))
                {
                    panel.name += " - " + text;
                }
                panel.atlas = tabstrip.tabPages.atlas;
                panel.anchor = UIAnchorStyle.All;
                panel.clipChildren = true;
                ReflectionUtils.InvokePrivateMethod(tabstrip.tabPages, "ArrangeTabs");
                tabstrip.tabPages.Invalidate();
            }
            ReflectionUtils.InvokePrivateMethod(tabstrip, "ArrangeTabs");
            tabstrip.Invalidate();
            return button;
        }
    }
}
