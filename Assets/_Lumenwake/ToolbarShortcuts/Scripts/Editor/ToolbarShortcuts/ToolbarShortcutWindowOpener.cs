using System;
using UnityEditor;
using UnityEngine;

namespace ToolbarShortcuts.Editor
{
    public static class ToolbarShortcutWindowOpener
    {
        public static void Open(ToolbarShortcutEntry entry)
        {
            switch (entry.windowOpenKind)
            {
                case ToolbarShortcutWindowOpenKind.MenuPath:
                    OpenViaMenuPath(entry.menuPath);
                    return;
                default:
                    if (TryOpenPreset(entry.windowTarget))
                        return;

                    Debug.LogWarning($"Toolbar shortcut: unknown window target '{entry.windowTarget}'.");
                    break;
            }
        }

        static bool TryOpenPreset(ToolbarShortcutWindowTarget target)
        {
            if (target == ToolbarShortcutWindowTarget.ProjectSettings)
            {
                SettingsService.OpenProjectSettings();
                return true;
            }

            if (target == ToolbarShortcutWindowTarget.Preferences)
            {
                SettingsService.OpenUserPreferences();
                return true;
            }

            string menuPath = GetPresetMenuPath(target);
            if (!string.IsNullOrEmpty(menuPath) && EditorApplication.ExecuteMenuItem(menuPath))
                return true;

            Type windowType = GetPresetWindowType(target);
            if (windowType != null)
            {
                EditorWindow.GetWindow(windowType);
                return true;
            }

            return false;
        }

        static void OpenViaMenuPath(string menuPath)
        {
            if (string.IsNullOrWhiteSpace(menuPath))
            {
                Debug.LogWarning("Toolbar shortcut: menu path is empty.");
                return;
            }

            if (!EditorApplication.ExecuteMenuItem(menuPath.Trim()))
                Debug.LogError($"Toolbar shortcut: menu item not found '{menuPath}'.");
        }

        static string GetPresetMenuPath(ToolbarShortcutWindowTarget target) => target switch
        {
            ToolbarShortcutWindowTarget.SceneView => "Window/General/Scene",
            ToolbarShortcutWindowTarget.GameView => "Window/General/Game",
            ToolbarShortcutWindowTarget.Hierarchy => "Window/General/Hierarchy",
            ToolbarShortcutWindowTarget.Project => "Window/General/Project",
            ToolbarShortcutWindowTarget.Inspector => "Window/General/Inspector",
            ToolbarShortcutWindowTarget.Console => "Window/General/Console",
            ToolbarShortcutWindowTarget.PackageManager => "Window/Package Management/Package Manager",
            ToolbarShortcutWindowTarget.BuildSettings => "File/Build Settings...",
            _ => null
        };

        static Type GetPresetWindowType(ToolbarShortcutWindowTarget target) => target switch
        {
            ToolbarShortcutWindowTarget.SceneView => typeof(SceneView),
            ToolbarShortcutWindowTarget.GameView => Type.GetType("UnityEditor.GameView, UnityEditor"),
            _ => null
        };
    }
}
