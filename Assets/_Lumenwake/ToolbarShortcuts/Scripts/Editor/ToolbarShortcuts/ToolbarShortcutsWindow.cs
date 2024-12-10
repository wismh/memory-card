using System.Collections.Generic;
using System.Linq;
using ToolbarShortcuts.Editor.Setup;
using UnityEditor;
using UnityEngine;

namespace ToolbarShortcuts.Editor
{
    public class ToolbarShortcutsWindow : EditorWindow
    {
        const float ButtonHeight = 28f;
        const float RowSpacing = 4f;
        const float ColumnSpacing = 4f;
        const float EdgePadding = 2f;

        static GUIStyle s_buttonStyle;

        Vector2 _scroll;

        [MenuItem(ToolbarShortcutsMenuPaths.ShortcutsWindow)]
        public static void Open() => OpenDockedAboveInspector();

        public static void OpenDockedAboveInspector()
        {
            ToolbarShortcutsWindow window = AcquireSingleWindow();
            ToolbarShortcutsWindowDocking.EnsureDockedAboveInspector(window);
        }

        static ToolbarShortcutsWindow AcquireSingleWindow()
        {
            List<ToolbarShortcutsWindow> windows = Resources.FindObjectsOfTypeAll<ToolbarShortcutsWindow>()
                .Where(w => w != null)
                .ToList();

            ToolbarShortcutsWindow primary = windows.FirstOrDefault(ToolbarShortcutsWindowDocking.HasDockParent)
                ?? windows.FirstOrDefault();

            foreach (ToolbarShortcutsWindow duplicate in windows)
            {
                if (duplicate != primary)
                    duplicate.Close();
            }

            if (primary != null)
                return primary;

            var created = CreateInstance<ToolbarShortcutsWindow>();
            created.titleContent = new GUIContent("Shortcuts");
            created.minSize = new Vector2(120, 120);
            return created;
        }

        public static void RepaintAll()
        {
            foreach (ToolbarShortcutsWindow window in Resources.FindObjectsOfTypeAll<ToolbarShortcutsWindow>())
            {
                if (window != null)
                    window.Repaint();
            }
        }

        void OnGUI()
        {
            HandleDragAndDrop();

            ToolbarShortcutsConfig config = ToolbarShortcutsRegistry.Config;
            if (config == null)
            {
                EditorGUILayout.HelpBox(
                    "Create a Toolbar Shortcuts Config via Assets → Create → ToolbarShortcuts → Toolbar Shortcuts Config.",
                    MessageType.Info);
                return;
            }

            IReadOnlyList<ToolbarShortcutEntry> shortcuts = ToolbarShortcutsRegistry.GetWindowShortcuts();
            if (shortcuts.Count == 0)
            {
                EditorGUILayout.HelpBox(
                    "No shortcuts with Placement set to Window or Both. Drag assets from the Project window here to create Select Asset shortcuts.",
                    MessageType.Info);
                return;
            }

            int columns = Mathf.Clamp((int)config.windowColumns, 1, 3);
            DrawShortcutScrollView(shortcuts, columns);
        }

        static void HandleDragAndDrop()
        {
            Event evt = Event.current;
            if (evt.type != EventType.DragUpdated && evt.type != EventType.DragPerform)
                return;

            if (!ContainsProjectAssets(DragAndDrop.objectReferences))
                return;

            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (evt.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                ToolbarShortcutsRegistry.AddSelectAssetShortcuts(DragAndDrop.objectReferences);
            }

            evt.Use();
        }

        static bool ContainsProjectAssets(Object[] objects)
        {
            if (objects == null || objects.Length == 0)
                return false;

            foreach (Object obj in objects)
            {
                if (obj != null && !string.IsNullOrEmpty(AssetDatabase.GetAssetPath(obj)))
                    return true;
            }

            return false;
        }

        void DrawShortcutScrollView(IReadOnlyList<ToolbarShortcutEntry> shortcuts, int columns)
        {
            float viewWidth = position.width;
            float viewHeight = position.height;
            float rowStride = ButtonHeight + RowSpacing;
            int rowCount = columns == 1
                ? shortcuts.Count
                : (shortcuts.Count + columns - 1) / columns;
            float contentHeight = rowCount * rowStride - RowSpacing;
            float contentWidth = GetContentWidth(viewHeight, contentHeight);

            var viewRect = new Rect(0f, 0f, viewWidth, viewHeight);
            var contentRect = new Rect(0f, 0f, contentWidth, contentHeight);

            _scroll = GUI.BeginScrollView(viewRect, _scroll, contentRect);

            float y = 0f;
            if (columns == 1)
            {
                foreach (ToolbarShortcutEntry entry in shortcuts)
                {
                    var buttonRect = new Rect(0f, y, contentWidth, ButtonHeight);
                    DrawShortcutButton(entry, buttonRect);
                    y += rowStride;
                }
            }
            else
            {
                for (int row = 0; row < rowCount; row++)
                {
                    int buttonsInRow = Mathf.Min(columns, shortcuts.Count - row * columns);
                    float cellWidth = (contentWidth - ColumnSpacing * (buttonsInRow - 1)) / buttonsInRow;
                    float x = 0f;

                    for (int col = 0; col < buttonsInRow; col++)
                    {
                        int index = row * columns + col;
                        var buttonRect = new Rect(x, y, cellWidth, ButtonHeight);
                        DrawShortcutButton(shortcuts[index], buttonRect);
                        x += cellWidth + ColumnSpacing;
                    }

                    y += rowStride;
                }
            }

            GUI.EndScrollView();
        }

        float GetContentWidth(float viewHeight, float contentHeight)
        {
            float width = position.width - EdgePadding * 2f;
            if (contentHeight > viewHeight)
                width -= GUI.skin.verticalScrollbar.fixedWidth;

            return Mathf.Max(48f, width);
        }

        static void DrawShortcutButton(ToolbarShortcutEntry entry, Rect rect)
        {
            if (TryShowShortcutContextMenu(entry, rect))
                GUI.Label(rect, BuildButtonContent(entry), ButtonStyle);
            else if (GUI.Button(rect, BuildButtonContent(entry), ButtonStyle))
                ToolbarShortcutExecutor.Execute(entry);
        }

        static bool TryShowShortcutContextMenu(ToolbarShortcutEntry entry, Rect rect)
        {
            Event evt = Event.current;
            if (!rect.Contains(evt.mousePosition))
                return false;

            switch (evt.type)
            {
                case EventType.MouseDown when evt.button == 1:
                    ShowShortcutContextMenu(entry);
                    evt.Use();
                    return true;

                case EventType.ContextClick:
                    ShowShortcutContextMenu(entry);
                    evt.Use();
                    return true;

                case EventType.MouseUp when evt.button == 1:
                    evt.Use();
                    return true;
            }

            return false;
        }

        static void ShowShortcutContextMenu(ToolbarShortcutEntry entry)
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Delete"), false, () => ToolbarShortcutsRegistry.RemoveShortcut(entry));
            menu.ShowAsContext();
        }

        static GUIContent BuildButtonContent(ToolbarShortcutEntry entry)
        {
            string tooltip = string.IsNullOrEmpty(entry.tooltip) ? entry.label : entry.tooltip;
            Texture2D icon = ToolbarShortcutIcons.Resolve(entry);
            return new GUIContent(entry.label, icon, tooltip);
        }

        static GUIStyle ButtonStyle
        {
            get
            {
                if (s_buttonStyle != null)
                    return s_buttonStyle;

                s_buttonStyle = new GUIStyle(GUI.skin.button)
                {
                    clipping = TextClipping.Clip,
                    wordWrap = false
                };
                return s_buttonStyle;
            }
        }
    }
}
