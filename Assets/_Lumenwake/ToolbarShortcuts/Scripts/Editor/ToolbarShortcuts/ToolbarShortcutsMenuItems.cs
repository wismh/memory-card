using ToolbarShortcuts.Editor.Bootstrap;
using UnityEditor;
using UnityEngine;

namespace ToolbarShortcuts.Editor
{
    public static class ToolbarShortcutsMenuItems
    {
        [MenuItem("Assets/Create/ToolbarShortcuts/Toolbar Shortcuts Config", priority = 201)]
        public static void CreateConfigAsset()
        {
            string assetPath = ToolbarShortcutsPackagePaths.DefaultConfigAssetPath;
            ToolbarShortcutsPackagePaths.EnsureFolder(ToolbarShortcutsPackagePaths.SettingsFolder);

            var existing = AssetDatabase.LoadAssetAtPath<ToolbarShortcutsConfig>(assetPath);
            if (existing != null)
            {
                Selection.activeObject = existing;
                EditorGUIUtility.PingObject(existing);
                return;
            }

            var config = ScriptableObject.CreateInstance<ToolbarShortcutsConfig>();
            AssetDatabase.CreateAsset(config, assetPath);
            AssetDatabase.SaveAssets();

            ToolbarShortcutsRegistry.Config = config;
            Selection.activeObject = config;
            EditorGUIUtility.PingObject(config);
        }

        [MenuItem("Assets/Create/ToolbarShortcuts/Bootstrap Config", priority = 202)]
        public static void CreateBootstrapConfigAsset()
        {
            string assetPath = ToolbarShortcutsPackagePaths.DefaultBootstrapConfigAssetPath;
            ToolbarShortcutsPackagePaths.EnsureFolder(ToolbarShortcutsPackagePaths.ResourcesFolder);

            var existing = AssetDatabase.LoadAssetAtPath<BootstrapConfig>(assetPath);
            if (existing != null)
            {
                Selection.activeObject = existing;
                EditorGUIUtility.PingObject(existing);
                return;
            }

            var config = ScriptableObject.CreateInstance<BootstrapConfig>();
            AssetDatabase.CreateAsset(config, assetPath);
            AssetDatabase.SaveAssets();

            Selection.activeObject = config;
            EditorGUIUtility.PingObject(config);
        }

        [MenuItem(ToolbarShortcutsMenuPaths.RefreshToolbar)]
        public static void RefreshToolbar() => ToolbarShortcutsToolbar.RefreshAll();
    }
}
