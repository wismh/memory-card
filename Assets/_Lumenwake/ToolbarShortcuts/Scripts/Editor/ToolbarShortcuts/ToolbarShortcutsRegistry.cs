using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ToolbarShortcuts.Editor
{
    public static class ToolbarShortcutsRegistry
    {
        const string k_configGuidKey = "com.lumenwake.toolbar-shortcuts.Config.Guid";

        static ToolbarShortcutsConfig _config;

        public static ToolbarShortcutsConfig Config
        {
            get
            {
                if (_config != null)
                    return _config;

                string guid = EditorPrefs.GetString(k_configGuidKey, string.Empty);
                if (!string.IsNullOrEmpty(guid))
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    if (!string.IsNullOrEmpty(path))
                        _config = AssetDatabase.LoadAssetAtPath<ToolbarShortcutsConfig>(path);
                }

                if (_config == null)
                {
                    string[] guids = AssetDatabase.FindAssets($"t:{nameof(ToolbarShortcutsConfig)}");
                    if (guids.Length > 0)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                        _config = AssetDatabase.LoadAssetAtPath<ToolbarShortcutsConfig>(path);
                        if (_config != null)
                            EditorPrefs.SetString(k_configGuidKey, guids[0]);
                    }
                }

                return _config;
            }
            set
            {
                _config = value;
                if (value == null)
                {
                    EditorPrefs.DeleteKey(k_configGuidKey);
                    return;
                }

                string path = AssetDatabase.GetAssetPath(value);
                EditorPrefs.SetString(k_configGuidKey, AssetDatabase.AssetPathToGUID(path));
            }
        }

        public static IReadOnlyList<ToolbarShortcutEntry> GetToolbarShortcuts(ToolbarShortcutDock dock) =>
            QueryShortcuts(ToolbarShortcutPlacement.Toolbar, ToolbarShortcutPlacement.Both)
                .Where(pair => pair.shortcut.dock == dock)
                .Select(pair => pair.shortcut)
                .ToList();

        public static IReadOnlyList<ToolbarShortcutEntry> GetWindowShortcuts() =>
            QueryShortcuts(ToolbarShortcutPlacement.Window, ToolbarShortcutPlacement.Both)
                .Select(pair => pair.shortcut)
                .ToList();

        static IEnumerable<(ToolbarShortcutEntry shortcut, int index)> QueryShortcuts(
            ToolbarShortcutPlacement includeA,
            ToolbarShortcutPlacement includeB)
        {
            if (Config == null || Config.shortcuts == null)
                yield break;

            var ordered = Config.shortcuts
                .Select((shortcut, index) => (shortcut, index))
                .Where(pair => pair.shortcut != null && pair.shortcut.enabled)
                .Where(pair =>
                {
                    ToolbarShortcutPlacement placement = pair.shortcut.placement;
                    return placement == includeA || placement == includeB;
                })
                .OrderBy(pair => pair.shortcut.order)
                .ThenBy(pair => pair.index);

            foreach ((ToolbarShortcutEntry shortcut, int index) pair in ordered)
                yield return pair;
        }

        public static void Reload() => _config = null;

        public static int AddSelectAssetShortcuts(
            IEnumerable<Object> assets,
            ToolbarShortcutPlacement placement = ToolbarShortcutPlacement.Window)
        {
            ToolbarShortcutsConfig config = Config;
            if (config == null || assets == null)
                return 0;

            if (config.shortcuts == null)
                config.shortcuts = new List<ToolbarShortcutEntry>();

            int nextOrder = config.shortcuts.Count > 0
                ? config.shortcuts.Max(shortcut => shortcut.order) + 1
                : 0;
            int added = 0;

            foreach (Object asset in assets)
            {
                if (asset == null || string.IsNullOrEmpty(AssetDatabase.GetAssetPath(asset)))
                    continue;

                if (added == 0)
                    Undo.RecordObject(config, "Add Shortcut");

                config.shortcuts.Add(new ToolbarShortcutEntry
                {
                    enabled = true,
                    label = asset.name,
                    placement = placement,
                    order = nextOrder++,
                    actionType = ToolbarShortcutActionType.SelectAsset,
                    asset = asset
                });
                added++;
            }

            if (added == 0)
                return 0;

            EditorUtility.SetDirty(config);
            ToolbarShortcutsToolbar.RefreshAll();
            ToolbarShortcutsWindow.RepaintAll();
            return added;
        }

        public static bool RemoveShortcut(ToolbarShortcutEntry entry)
        {
            ToolbarShortcutsConfig config = Config;
            if (config?.shortcuts == null || entry == null)
                return false;

            int index = config.shortcuts.IndexOf(entry);
            if (index < 0)
                return false;

            Undo.RecordObject(config, "Remove Shortcut");
            config.shortcuts.RemoveAt(index);
            EditorUtility.SetDirty(config);
            ToolbarShortcutsToolbar.RefreshAll();
            ToolbarShortcutsWindow.RepaintAll();
            return true;
        }
    }
}
