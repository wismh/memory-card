using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace ToolbarShortcuts.Editor
{
    [InitializeOnLoad]
    public static class ToolbarShortcutsToolbar
    {
        public const string LeftPath = "ToolbarShortcuts/Shortcuts Left";
        public const string RightPath = "ToolbarShortcuts/Shortcuts Right";

        static ToolbarShortcutsToolbar()
        {
            EditorApplication.projectChanged += RefreshAll;
            EditorApplication.playModeStateChanged += _ => RefreshAll();
        }

        [MainToolbarElement(LeftPath, defaultDockPosition = MainToolbarDockPosition.Left)]
        public static IEnumerable<MainToolbarElement> CreateLeftShortcuts() =>
            CreateShortcuts(ToolbarShortcutDock.Left);

        [MainToolbarElement(RightPath, defaultDockPosition = MainToolbarDockPosition.Right)]
        public static IEnumerable<MainToolbarElement> CreateRightShortcuts() =>
            CreateShortcuts(ToolbarShortcutDock.Right);

        public static void RefreshAll()
        {
            MainToolbar.Refresh(LeftPath);
            MainToolbar.Refresh(RightPath);
        }

        static IEnumerable<MainToolbarElement> CreateShortcuts(ToolbarShortcutDock dock)
        {
            ToolbarShortcutsConfig config = ToolbarShortcutsRegistry.Config;
            bool isLeft = dock == ToolbarShortcutDock.Left;
            int marginSlots = isLeft ? config?.leftMargin ?? 0 : config?.rightMargin ?? 0;

            if (isLeft)
            {
                foreach (MainToolbarElement slot in ToolbarShortcutMargin.CreateSlots(marginSlots))
                    yield return slot;
            }

            IReadOnlyList<ToolbarShortcutEntry> entries = ToolbarShortcutsRegistry.GetToolbarShortcuts(dock);
            foreach (ToolbarShortcutEntry entry in entries)
                yield return CreateShortcutButton(entry);

            if (!isLeft)
            {
                foreach (MainToolbarElement slot in ToolbarShortcutMargin.CreateSlots(marginSlots))
                    yield return slot;
            }
        }

        static MainToolbarButton CreateShortcutButton(ToolbarShortcutEntry entry)
        {
            string tooltip = string.IsNullOrEmpty(entry.tooltip) ? entry.label : entry.tooltip;
            var content = new MainToolbarContent(entry.label, ToolbarShortcutIcons.Resolve(entry), tooltip);

            return new MainToolbarButton(content, () => ToolbarShortcutExecutor.Execute(entry))
            {
                enabled = !EditorApplication.isPlaying ||
                          entry.actionType == ToolbarShortcutActionType.SelectAsset ||
                          entry.actionType == ToolbarShortcutActionType.InvokeStaticMethod ||
                          entry.actionType == ToolbarShortcutActionType.OpenWindow
            };
        }

    }
}
