using UnityEditor;

namespace ToolbarShortcuts.Editor.Setup
{
    public static class ToolbarShortcutsSettings
    {
        const string k_setupCompletedKey = "com.lumenwake.toolbar-shortcuts.SetupCompleted";
        const string k_welcomeVersionKey = "com.lumenwake.toolbar-shortcuts.WelcomeVersion";
        public const int WelcomeVersion = 1;

        public const string DefaultPlayOverlayId = "Play Mode Controls";
        public const string PackageOverlayPrefix = "ToolbarShortcuts/";

        public static bool IsSetupCompleted =>
            EditorPrefs.GetBool(k_setupCompletedKey, false);

        public static void MarkSetupCompleted()
        {
            EditorPrefs.SetBool(k_setupCompletedKey, true);
            EditorPrefs.SetInt(k_welcomeVersionKey, WelcomeVersion);
        }

        public static void ResetSetup()
        {
            EditorPrefs.DeleteKey(k_setupCompletedKey);
            EditorPrefs.DeleteKey(k_welcomeVersionKey);
        }
    }
}
