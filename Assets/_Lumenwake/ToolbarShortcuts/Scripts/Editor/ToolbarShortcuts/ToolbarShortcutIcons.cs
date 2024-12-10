using UnityEditor;
using UnityEngine;

namespace ToolbarShortcuts.Editor
{
    public static class ToolbarShortcutIcons
    {
        public static Texture2D Resolve(ToolbarShortcutEntry entry)
        {
            if (entry.icon != null)
                return entry.icon;

            Object iconSource = entry.actionType switch
            {
                ToolbarShortcutActionType.OpenScene => GetSceneIconSource(entry),
                ToolbarShortcutActionType.OpenAsset => entry.asset,
                ToolbarShortcutActionType.SelectAsset => entry.asset,
                ToolbarShortcutActionType.OpenWindow => GetWindowIcon(entry),
                _ => null
            };

            if (iconSource is Texture2D texture)
                return texture;

            if (iconSource == null)
                return null;

            return AssetPreview.GetMiniThumbnail(iconSource);
        }

        static Texture2D GetWindowIcon(ToolbarShortcutEntry entry)
        {
            if (entry.windowOpenKind == ToolbarShortcutWindowOpenKind.UnityWindow)
            {
                string iconName = entry.windowTarget switch
                {
                    ToolbarShortcutWindowTarget.SceneView => "SceneView Icon",
                    ToolbarShortcutWindowTarget.GameView => "GameView Icon",
                    ToolbarShortcutWindowTarget.Hierarchy => "Hierarchy Icon",
                    ToolbarShortcutWindowTarget.Project => "Project Icon",
                    ToolbarShortcutWindowTarget.Inspector => "Inspector Icon",
                    ToolbarShortcutWindowTarget.Console => "Console Icon",
                    ToolbarShortcutWindowTarget.ProjectSettings => "SettingsIcon",
                    ToolbarShortcutWindowTarget.PackageManager => "Package Manager",
                    ToolbarShortcutWindowTarget.BuildSettings => "BuildSettings.Editor.Small",
                    ToolbarShortcutWindowTarget.Preferences => "SettingsIcon",
                    _ => "d_UnityEditor.GameView"
                };

                Texture2D icon = EditorGUIUtility.IconContent(iconName).image as Texture2D;
                if (icon != null)
                    return icon;
            }

            return EditorGUIUtility.IconContent("d_UnityEditor.GameView").image as Texture2D;
        }

        static Object GetSceneIconSource(ToolbarShortcutEntry entry)
        {
            if (entry.primaryScene != null)
                return entry.primaryScene;

            if (entry.additiveScenes == null)
                return null;

            foreach (SceneAsset scene in entry.additiveScenes)
            {
                if (scene != null)
                    return scene;
            }

            return null;
        }
    }
}
