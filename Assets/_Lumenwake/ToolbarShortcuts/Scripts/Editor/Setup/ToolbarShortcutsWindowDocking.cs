using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ToolbarShortcuts.Editor.Setup
{
    static class ToolbarShortcutsWindowDocking
    {
        const BindingFlags k_flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        static readonly Type s_dockAreaType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.DockArea");
        static readonly Type s_splitViewType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.SplitView");
        static readonly Type s_viewType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.View");
        static readonly Type s_inspectorType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.InspectorWindow");

        public static bool HasDockParent(EditorWindow window) => GetEditorWindowParent(window) != null;

        public static void EnsureDockedAboveInspector(EditorWindow window)
        {
            if (window == null || s_dockAreaType == null || s_splitViewType == null || s_viewType == null)
                return;

            if (HasDockParent(window))
            {
                window.Show(true);
                window.Focus();
                return;
            }

            EditorWindow inspector = EditorWindow.GetWindow(s_inspectorType);
            if (inspector == null)
            {
                DockAsInspectorTab(window);
                return;
            }

            if (TryDockAbove(window, inspector))
            {
                window.Show(true);
                window.Focus();
                return;
            }

            DockAsInspectorTab(window);
        }

        static void DockAsInspectorTab(EditorWindow window)
        {
            if (window is not ToolbarShortcutsWindow shortcutsWindow)
            {
                window?.Show(true);
                return;
            }

            if (HasDockParent(shortcutsWindow))
            {
                shortcutsWindow.Show(true);
                shortcutsWindow.Focus();
                return;
            }

            string title = shortcutsWindow.titleContent?.text ?? "Shortcuts";
            var docked = EditorWindow.GetWindow<ToolbarShortcutsWindow>(title, false, s_inspectorType);

            if (!ReferenceEquals(docked, shortcutsWindow))
            {
                docked.minSize = shortcutsWindow.minSize;
                shortcutsWindow.Close();
            }

            docked.Show(true);
            docked.Focus();
        }

        static bool TryDockAbove(EditorWindow child, EditorWindow anchor)
        {
            if (HasDockParent(child))
                return true;

            try
            {
                object anchorDockArea = GetEditorWindowParent(anchor);
                if (anchorDockArea == null)
                    return false;

                object parentView = GetViewParent(anchorDockArea);
                object childDockArea = ScriptableObject.CreateInstance(s_dockAreaType);
                AddTab(childDockArea, child);

                if (parentView != null && parentView.GetType() == s_splitViewType && IsVerticalSplit(parentView))
                {
                    int index = IndexOfChild(parentView, anchorDockArea);
                    AddChild(parentView, childDockArea, index);
                    ReflowSplit(parentView);
                    MakeParentsSettingsMatchMe(child);
                    MakeParentsSettingsMatchMe(anchor);
                    return true;
                }

                if (parentView == null)
                    return false;

                int anchorIndex = IndexOfChild(parentView, anchorDockArea);
                RemoveChild(parentView, anchorDockArea);

                object split = ScriptableObject.CreateInstance(s_splitViewType);
                SetVertical(split, true);
                AddChild(split, childDockArea, 0);
                AddChild(split, anchorDockArea, 1);
                AddChild(parentView, split, anchorIndex);

                ReflowSplit(split);
                ReflowSplit(parentView);
                MakeParentsSettingsMatchMe(child);
                MakeParentsSettingsMatchMe(anchor);
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogWarning(
                    $"Toolbar Shortcuts: could not dock above Inspector ({exception.Message}). Using Inspector tab dock instead.");
                return false;
            }
        }

        static object GetEditorWindowParent(EditorWindow window)
        {
            FieldInfo field = typeof(EditorWindow).GetField("m_Parent", k_flags);
            return field?.GetValue(window);
        }

        static object GetViewParent(object view) =>
            s_viewType?.GetProperty("parent", k_flags)?.GetValue(view);

        static bool IsVerticalSplit(object splitView) =>
            (bool)s_splitViewType.GetField("vertical", k_flags).GetValue(splitView);

        static void SetVertical(object splitView, bool vertical) =>
            s_splitViewType.GetField("vertical", k_flags).SetValue(splitView, vertical);

        static int IndexOfChild(object parentView, object childView) =>
            (int)s_viewType.GetMethod("IndexOfChild", k_flags, null, new[] { s_viewType }, null)
                .Invoke(parentView, new[] { childView });

        static void AddChild(object parentView, object childView, int index) =>
            s_viewType.GetMethod("AddChild", k_flags, null, new[] { s_viewType, typeof(int) }, null)
                .Invoke(parentView, new object[] { childView, index });

        static void RemoveChild(object parentView, object childView) =>
            s_viewType.GetMethod("RemoveChild", k_flags, null, new[] { s_viewType }, null)
                .Invoke(parentView, new object[] { childView });

        static void AddTab(object dockArea, EditorWindow pane) =>
            s_dockAreaType.GetMethod("AddTab", k_flags, null, new[] { typeof(EditorWindow), typeof(bool) }, null)
                ?.Invoke(dockArea, new object[] { pane, true });

        static void ReflowSplit(object splitView) =>
            s_splitViewType.GetMethod("Reflow", k_flags)?.Invoke(splitView, null);

        static void MakeParentsSettingsMatchMe(EditorWindow window) =>
            typeof(EditorWindow).GetMethod("MakeParentsSettingsMatchMe", k_flags)?.Invoke(window, null);
    }
}
