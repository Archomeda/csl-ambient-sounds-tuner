using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonShared.Events
{
    /// <summary>
    /// Contains various events related to the asset editor.
    /// </summary>
    public static class AssetEditorEvents
    {
        /// <summary>
        /// Gets fired when the mode in the asset editor changes.
        /// This is basically a forward to <see cref="ToolController.eventEditPrefabChanged"/>, but provided here for easy access.
        /// </summary>
        public static event ToolController.EditPrefabChanged AssetEditorModeChanged
        {
            add { ToolsModifierControl.toolController.eventEditPrefabChanged += value; }
            remove { ToolsModifierControl.toolController.eventEditPrefabChanged -= value; }
        }
    }
}
