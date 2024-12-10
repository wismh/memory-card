using System.Linq;
using UnityEditor;

namespace ToolbarShortcuts.Editor.Setup
{
    [InitializeOnLoad]
    static class ToolbarShortcutsEditorSession
    {
        static ToolbarShortcutsEditorSession() =>
            EditorApplication.delayCall += ToolbarShortcutsWelcomeWindow.TryShowOnImport;
    }

    public class ToolbarShortcutsImportProcessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            if (!HasPackageImport(importedAssets) &&
                !HasPackageImport(movedAssets) &&
                !HasPackageImport(movedFromAssetPaths))
                return;

            ToolbarShortcutsWelcomeWindow.TryShowOnImport();
        }

        static bool HasPackageImport(string[] paths)
        {
            if (paths == null || paths.Length == 0)
                return false;

            return paths.Any(ToolbarShortcutsPackagePaths.IsPackageAssetPath);
        }
    }
}
