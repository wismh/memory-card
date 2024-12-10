using System;
using System.IO;
using UnityEditor;

namespace ToolbarShortcuts.Editor
{
    internal static class ToolbarShortcutsPackagePaths
    {
        public const string PackageFolderName = "Lumenwake";
        public const string DefaultConfigFileName = "ToolbarShortcutsConfig.asset";
        public const string DefaultBootstrapConfigFileName = "BootstrapConfig.asset";

        static string _packageRoot;

        public static string PackageRoot
        {
            get
            {
                if (!string.IsNullOrEmpty(_packageRoot))
                    return _packageRoot;

                _packageRoot = ResolvePackageRoot();
                return _packageRoot;
            }
        }

        public static string ResourcesFolder => $"{PackageRoot}/Resources";

        public static string SettingsFolder => $"{PackageRoot}/Settings";

        public static string DefaultConfigAssetPath =>
            $"{SettingsFolder}/{DefaultConfigFileName}";

        public static string DefaultBootstrapConfigAssetPath =>
            $"{ResourcesFolder}/{DefaultBootstrapConfigFileName}";

        public static bool IsPackageAssetPath(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
                return false;

            string path = assetPath.Replace('\\', '/');
            if (path.EndsWith(".meta", StringComparison.OrdinalIgnoreCase))
                return false;

            if (path.Contains($"/{PackageFolderName}/", StringComparison.Ordinal))
                return true;

            return path.StartsWith("Packages/", StringComparison.Ordinal) &&
                   path.Contains("com.lumenwake.toolbar-shortcuts", StringComparison.OrdinalIgnoreCase);
        }

        static string ResolvePackageRoot()
        {
            string[] guids = AssetDatabase.FindAssets($"{nameof(ToolbarShortcutsPackagePaths)} t:Script");
            foreach (string guid in guids)
            {
                string scriptPath = AssetDatabase.GUIDToAssetPath(guid);
                string root = ExtractPackageRoot(scriptPath);
                if (!string.IsNullOrEmpty(root))
                    return root;
            }

            string[] configGuids = AssetDatabase.FindAssets($"t:{nameof(ToolbarShortcutsConfig)}");
            if (configGuids.Length > 0)
            {
                string configPath = AssetDatabase.GUIDToAssetPath(configGuids[0]);
                string root = ExtractPackageRoot(configPath);
                if (!string.IsNullOrEmpty(root))
                    return root;
            }

            return $"Assets/{PackageFolderName}";
        }

        static string ExtractPackageRoot(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
                return null;

            string normalized = assetPath.Replace('\\', '/');
            string marker = $"/{PackageFolderName}/";
            int index = normalized.IndexOf(marker, StringComparison.Ordinal);
            if (index < 0)
                return null;

            return normalized.Substring(0, index + marker.Length - 1);
        }

        public static void EnsureFolder(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath))
                return;

            string normalized = folderPath.Replace('\\', '/');
            if (AssetDatabase.IsValidFolder(normalized))
                return;

            string parent = Path.GetDirectoryName(normalized)?.Replace('\\', '/');
            string folderName = Path.GetFileName(normalized);
            if (string.IsNullOrEmpty(parent) || string.IsNullOrEmpty(folderName))
                return;

            if (!AssetDatabase.IsValidFolder(parent))
                EnsureFolder(parent);

            AssetDatabase.CreateFolder(parent, folderName);
        }
    }
}
