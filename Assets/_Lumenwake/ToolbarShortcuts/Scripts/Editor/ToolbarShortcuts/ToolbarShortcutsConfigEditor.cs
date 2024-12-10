using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ToolbarShortcuts.Editor
{
    [CustomEditor(typeof(ToolbarShortcutsConfig))]
    public class ToolbarShortcutsConfigEditor : UnityEditor.Editor
    {
        ReorderableList _list;

        void OnEnable()
        {
            ToolbarShortcutsRegistry.Config = (ToolbarShortcutsConfig)target;

            SerializedProperty shortcuts = serializedObject.FindProperty(nameof(ToolbarShortcutsConfig.shortcuts));
            _list = new ReorderableList(serializedObject, shortcuts, true, true, true, true)
            {
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Toolbar Shortcuts"),
                drawElementCallback = DrawElement,
                elementHeightCallback = GetElementHeight,
                onReorderCallback = _ => ApplyChanges(),
                onAddCallback = list =>
                {
                    int index = list.serializedProperty.arraySize;
                    list.serializedProperty.arraySize++;
                    SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
                    element.FindPropertyRelative(nameof(ToolbarShortcutEntry.label)).stringValue = "Shortcut";
                    ApplyChanges();
                },
                onRemoveCallback = list =>
                {
                    ReorderableList.defaultBehaviours.DoRemoveButton(list);
                    ApplyChanges();
                }
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.HelpBox(
                "Placement: Toolbar, Shortcuts Window (Tools → Toolbar Shortcuts menu), or Both. " +
                "Left/Right Margin applies to toolbar shortcuts only (spacer slot count). " +
                "Window Columns controls the Shortcuts Window grid (1–3 per row). " +
                "Select Asset focuses Project and highlights the assigned asset. " +
                "Opening scenes always shows Unity's save dialog when there are unsaved changes. " +
                "If Icon is empty, the asset/scene thumbnail is used automatically. " +
                "Open Window: Unity presets or any menu path (e.g. Window/General/Scene). " +
                "Static methods must be parameterless.",
                MessageType.Info);

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty(nameof(ToolbarShortcutsConfig.leftMargin)),
                new GUIContent("Left Margin (slots)"));
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty(nameof(ToolbarShortcutsConfig.rightMargin)),
                new GUIContent("Right Margin (slots)"));
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty(nameof(ToolbarShortcutsConfig.windowColumns)),
                new GUIContent("Window Columns"));
            if (EditorGUI.EndChangeCheck())
                ApplyChanges();

            _list.DoLayoutList();

            if (serializedObject.ApplyModifiedProperties())
                ApplyChanges();
        }

        void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = _list.serializedProperty.GetArrayElementAtIndex(index);
            float line = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;
            float y = rect.y + 2f;

            DrawFoldoutHeader(ref y, rect, element, index);

            if (!element.FindPropertyRelative("_foldout").boolValue)
                return;

            EditorGUI.indentLevel++;

            y = DrawProperty(ref y, rect, element, nameof(ToolbarShortcutEntry.enabled), "Enabled");
            y = DrawProperty(ref y, rect, element, nameof(ToolbarShortcutEntry.label), "Label");
            y = DrawProperty(ref y, rect, element, nameof(ToolbarShortcutEntry.tooltip), "Tooltip");
            y = DrawProperty(ref y, rect, element, nameof(ToolbarShortcutEntry.placement), "Placement");

            var placement = (ToolbarShortcutPlacement)element
                .FindPropertyRelative(nameof(ToolbarShortcutEntry.placement)).enumValueIndex;

            if (placement == ToolbarShortcutPlacement.Toolbar || placement == ToolbarShortcutPlacement.Both)
                y = DrawProperty(ref y, rect, element, nameof(ToolbarShortcutEntry.dock), "Dock");

            y = DrawProperty(ref y, rect, element, nameof(ToolbarShortcutEntry.order), "Order");

            y = DrawProperty(
                ref y,
                rect,
                element,
                nameof(ToolbarShortcutEntry.icon),
                "Icon (optional, else asset thumbnail)");

            y = DrawProperty(ref y, rect, element, nameof(ToolbarShortcutEntry.actionType), "Action");

            var action = (ToolbarShortcutActionType)element
                .FindPropertyRelative(nameof(ToolbarShortcutEntry.actionType)).enumValueIndex;

            switch (action)
            {
                case ToolbarShortcutActionType.OpenAsset:
                case ToolbarShortcutActionType.SelectAsset:
                    y = DrawProperty(ref y, rect, element, nameof(ToolbarShortcutEntry.asset), "Asset");
                    break;
                case ToolbarShortcutActionType.OpenScene:
                    y = DrawProperty(ref y, rect, element, nameof(ToolbarShortcutEntry.primaryScene), "Primary Scene");
                    y = DrawProperty(ref y, rect, element, nameof(ToolbarShortcutEntry.additiveScenes), "Additive Scenes");
                    break;
                case ToolbarShortcutActionType.OpenWindow:
                    y = DrawProperty(ref y, rect, element, nameof(ToolbarShortcutEntry.windowOpenKind), "Open Via");
                    DrawOpenWindowFields(ref y, rect, element);
                    break;
                case ToolbarShortcutActionType.InvokeStaticMethod:
                    y = DrawProperty(ref y, rect, element, nameof(ToolbarShortcutEntry.staticTypeName), "Type Name");
                    y = DrawProperty(ref y, rect, element, nameof(ToolbarShortcutEntry.staticMethodName), "Method Name");
                    break;
            }

            EditorGUI.indentLevel--;
        }

        static float DrawFoldoutHeader(ref float y, Rect rect, SerializedProperty element, int index)
        {
            string label = element.FindPropertyRelative(nameof(ToolbarShortcutEntry.label)).stringValue;
            if (string.IsNullOrEmpty(label))
                label = $"Shortcut {index}";

            SerializedProperty foldout = element.FindPropertyRelative("_foldout");

            float line = EditorGUIUtility.singleLineHeight;
            var headerRect = new Rect(rect.x, y, rect.width, line);
            foldout.boolValue = EditorGUI.Foldout(headerRect, foldout.boolValue, label, true);
            y += line + EditorGUIUtility.standardVerticalSpacing;
            return y;
        }

        static float DrawProperty(ref float y, Rect rect, SerializedProperty element, string propertyName, string label)
        {
            SerializedProperty property = element.FindPropertyRelative(propertyName);
            float height = EditorGUI.GetPropertyHeight(property, true);
            EditorGUI.PropertyField(new Rect(rect.x, y, rect.width, height), property, new GUIContent(label), true);
            y += height + EditorGUIUtility.standardVerticalSpacing;
            return y;
        }

        float GetElementHeight(int index)
        {
            SerializedProperty element = _list.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty foldout = element.FindPropertyRelative("_foldout");
            float line = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 4f;

            if (foldout == null || !foldout.boolValue)
                return line;

            float height = line;
            height += GetPropertyHeight(element, nameof(ToolbarShortcutEntry.enabled));
            height += GetPropertyHeight(element, nameof(ToolbarShortcutEntry.label));
            height += GetPropertyHeight(element, nameof(ToolbarShortcutEntry.tooltip));
            height += GetPropertyHeight(element, nameof(ToolbarShortcutEntry.placement));

            var placement = (ToolbarShortcutPlacement)element
                .FindPropertyRelative(nameof(ToolbarShortcutEntry.placement)).enumValueIndex;

            if (placement == ToolbarShortcutPlacement.Toolbar || placement == ToolbarShortcutPlacement.Both)
                height += GetPropertyHeight(element, nameof(ToolbarShortcutEntry.dock));

            height += GetPropertyHeight(element, nameof(ToolbarShortcutEntry.order));
            height += GetPropertyHeight(element, nameof(ToolbarShortcutEntry.icon));
            height += GetPropertyHeight(element, nameof(ToolbarShortcutEntry.actionType));

            var action = (ToolbarShortcutActionType)element
                .FindPropertyRelative(nameof(ToolbarShortcutEntry.actionType)).enumValueIndex;

            switch (action)
            {
                case ToolbarShortcutActionType.OpenAsset:
                case ToolbarShortcutActionType.SelectAsset:
                    height += GetPropertyHeight(element, nameof(ToolbarShortcutEntry.asset));
                    break;
                case ToolbarShortcutActionType.OpenScene:
                    height += GetPropertyHeight(element, nameof(ToolbarShortcutEntry.primaryScene));
                    height += GetPropertyHeight(element, nameof(ToolbarShortcutEntry.additiveScenes));
                    break;
                case ToolbarShortcutActionType.OpenWindow:
                    height += GetOpenWindowFieldsHeight(element);
                    break;
                case ToolbarShortcutActionType.InvokeStaticMethod:
                    height += GetPropertyHeight(element, nameof(ToolbarShortcutEntry.staticTypeName));
                    height += GetPropertyHeight(element, nameof(ToolbarShortcutEntry.staticMethodName));
                    break;
            }

            return height + 6f;
        }

        static float GetPropertyHeight(SerializedProperty element, string propertyName)
        {
            SerializedProperty property = element.FindPropertyRelative(propertyName);
            return EditorGUI.GetPropertyHeight(property, true) + EditorGUIUtility.standardVerticalSpacing;
        }

        static void DrawOpenWindowFields(ref float y, Rect rect, SerializedProperty element)
        {
            var kind = (ToolbarShortcutWindowOpenKind)element
                .FindPropertyRelative(nameof(ToolbarShortcutEntry.windowOpenKind)).enumValueIndex;

            switch (kind)
            {
                case ToolbarShortcutWindowOpenKind.UnityWindow:
                    y = DrawProperty(ref y, rect, element, nameof(ToolbarShortcutEntry.windowTarget), "Window");
                    break;
                case ToolbarShortcutWindowOpenKind.MenuPath:
                    y = DrawProperty(ref y, rect, element, nameof(ToolbarShortcutEntry.menuPath), "Menu Path");
                    break;
            }
        }

        static float GetOpenWindowFieldsHeight(SerializedProperty element)
        {
            float height = GetPropertyHeight(element, nameof(ToolbarShortcutEntry.windowOpenKind));

            var kind = (ToolbarShortcutWindowOpenKind)element
                .FindPropertyRelative(nameof(ToolbarShortcutEntry.windowOpenKind)).enumValueIndex;

            switch (kind)
            {
                case ToolbarShortcutWindowOpenKind.UnityWindow:
                    height += GetPropertyHeight(element, nameof(ToolbarShortcutEntry.windowTarget));
                    break;
                case ToolbarShortcutWindowOpenKind.MenuPath:
                    height += GetPropertyHeight(element, nameof(ToolbarShortcutEntry.menuPath));
                    break;
            }

            return height;
        }

        void ApplyChanges()
        {
            serializedObject.ApplyModifiedProperties();
            ToolbarShortcutsRegistry.Config = (ToolbarShortcutsConfig)target;
            ToolbarShortcutsToolbar.RefreshAll();
            ToolbarShortcutsWindow.RepaintAll();
        }
    }
}
