using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalFramework.UI;
using UnityEngine;

namespace CommonShared.Events
{
    /// <summary>
    /// Contains various events related to the toolbar.
    /// </summary>
    public static class ToolbarEvents
    {
        private static bool isToolbarOpen = false;

        public delegate void ToolbarOpenedEventHandler();

        private static event ToolbarOpenedEventHandler toolbarOpened;
        /// <summary>
        /// Gets fired when the toolbar has been opened.
        /// </summary>

        public static event ToolbarOpenedEventHandler ToolbarOpened
        {
            add
            {
                StartToolbarEvents();
                toolbarOpened += value;
            }
            remove
            {
                toolbarOpened -= value;
            }
        }

        private static void OnToolbarOpened()
        {
            var handler = toolbarOpened;
            if (handler != null)
                handler();
        }


        public delegate void ToolbarClosedEventHandler();

        private static event ToolbarClosedEventHandler toolbarClosed;
        /// <summary>
        /// Gets fired when the toolbar has been closed.
        /// </summary>

        public static event ToolbarClosedEventHandler ToolbarClosed
        {
            add
            {
                StartToolbarEvents();
                toolbarClosed += value;
            }
            remove
            {
                toolbarClosed -= value;
            }
        }

        private static void OnToolbarClosed()
        {
            var handler = toolbarClosed;
            if (handler != null)
                handler();
        }


        private static bool toolbarEventsStarted;

        private static void StartToolbarEvents()
        {
            if (!toolbarEventsStarted)
            {
                switch (SimulationManager.instance.m_metaData.m_updateMode)
                {
                    case SimulationManager.UpdateMode.NewGame:
                    case SimulationManager.UpdateMode.LoadGame:
                    case SimulationManager.UpdateMode.NewMap:
                    case SimulationManager.UpdateMode.LoadMap:
                        HookToolbar();
                        break;
                    case SimulationManager.UpdateMode.NewAsset:
                    case SimulationManager.UpdateMode.LoadAsset:
                        AssetEditorEvents.AssetEditorModeChanged += AssetEditorEvents_AssetEditorModeChanged;
                        break;
                }
                toolbarEventsStarted = true;
            }
        }

        private static void AssetEditorEvents_AssetEditorModeChanged(PrefabInfo info)
        {
            OnToolbarClosed();
            HookToolbar();
        }

        private static void HookToolbar()
        {
            UITabContainer tsContainer = GameObject.Find("TSContainer").GetComponent<UITabContainer>();
            if (tsContainer != null)
            {
                foreach (UIScrollablePanel panel in tsContainer.GetComponentsInChildren<UIScrollablePanel>())
                {
                    panel.eventVisibilityChanged += ToolbarPanel_VisibilityChanged;
                }
            }
        }

        private static void ToolbarPanel_VisibilityChanged(UIComponent component, bool value)
        {
            // We have to check the visibility of the parent of the parent of the UIScrollablePanel,
            // since the UIScrollablePanel is bound to a specific tab and we catch this event from every UIScrollablePanel.
            // This might cause a race condition for invisible tabs that send the event later than visible tabs.
            if (component.parent.parent.isVisible)
            {
                // We have to double check if the panel that has been opened, is actually a toolbar panel.
                // We can do that by checking how many child components the visible UIScrollablePanel has,
                // if it has one or more child components, we continue. Otherwise, don't do anything.
                if (component.childCount > 0)
                {
                    if (!isToolbarOpen)
                    {
                        OnToolbarOpened();
                        isToolbarOpen = true;
                    }
                }
            }
            else if (isToolbarOpen)
            {
                OnToolbarClosed();
                isToolbarOpen = false;
            }
        }
    }
}
