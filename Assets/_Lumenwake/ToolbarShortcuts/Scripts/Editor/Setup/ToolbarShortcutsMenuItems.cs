using ToolbarShortcuts.Editor;
using UnityEditor;

namespace ToolbarShortcuts.Editor.Setup
{
    public static class ToolbarShortcutsMenuItems
    {
        [MenuItem(ToolbarShortcutsMenuPaths.SetupToolbar, priority = 1)]
        public static void SetupToolbar()
        {
            ToolbarShortcutsSetup.Run();
            EditorUtility.DisplayDialog(
                "Toolbar Shortcuts",
                "Toolbar configured: default Play hidden, Toolbar Shortcuts items enabled, Shortcuts Window opened.",
                "OK");
        }

        [MenuItem(ToolbarShortcutsMenuPaths.ResetSetupState, priority = 100)]
        public static void ResetSetupState()
        {
            if (!EditorUtility.DisplayDialog(
                    "Toolbar Shortcuts",
                    "Reset setup state? The welcome window will appear again after the next package import.",
                    "Reset",
                    "Cancel"))
                return;

            ToolbarShortcutsSettings.ResetSetup();
        }
    }
}
