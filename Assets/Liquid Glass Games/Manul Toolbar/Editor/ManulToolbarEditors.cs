// Copyright (c) 2024 Liquid Glass Studios. All rights reserved.

#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Manul.Toolbar
{
	/// Manul Toolbar Settings

	[CustomEditor(typeof(ManulToolbarSettings))]
	public class ManulToolbarSettings_Editor : Editor
	{
		ManulToolbarSettings obj;
		SerializedProperty settings;
		SerializedProperty settingsExpanded;
		SerializedProperty leftSide;
		SerializedProperty rightSide;

		protected void OnEnable()
		{
			obj = (ManulToolbarSettings)target;
			settings = serializedObject.FindProperty("settings");
			leftSide = serializedObject.FindProperty("leftSide");
			rightSide = serializedObject.FindProperty("rightSide");
			settingsExpanded = serializedObject.FindProperty("settingsExpanded");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.BeginHorizontal();
			GUI.Box(new Rect(0, 0, Screen.width, 38), "Manul Toolbar", ManulToolbarStyles.HeaderStyle);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("");
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("");
			EditorGUILayout.EndHorizontal();

			EditorGUI.BeginChangeCheck();

			EditorGUILayout.BeginHorizontal();
			settingsExpanded.boolValue = EditorGUILayout.Foldout(settingsExpanded.boolValue, new GUIContent("Settings"), true);
			EditorGUILayout.EndHorizontal();

			if (settingsExpanded.boolValue)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(settings);
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(leftSide, new GUIContent("Left Side", ManulToolbarMessages.GetTooltip(Tooltip.LeftSide)));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(rightSide, new GUIContent("Right Side", ManulToolbarMessages.GetTooltip(Tooltip.RightSide)));
			EditorGUILayout.EndHorizontal();

			if (EditorGUI.EndChangeCheck())
			{
				ManulToolbar.RefreshToolbar();
				EditorUtility.SetDirty(obj);
			}

			serializedObject.ApplyModifiedProperties();
		}
	}

	/// Manul Toolbar Settings: Preferences

	[CustomPropertyDrawer(typeof(ManulToolbarPreferences))]
	public class ManulToolbarPreferences_Drawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			float x = position.x;
			float y = position.y;
			float w = position.width;
			float h = EditorGUIUtility.singleLineHeight;

			/// Offsets

			EditorGUI.LabelField(new Rect(x, y, w, h), new GUIContent(" Offsets ", ManulToolbarMessages.GetTooltip(Tooltip.Offsets)), EditorStyles.centeredGreyMiniLabel);

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			float currentLabelWidth = EditorGUIUtility.labelWidth; 

			EditorGUIUtility.labelWidth = 75;

			property.FindPropertyRelative("leftBeginOffset").floatValue = EditorGUI.FloatField(new Rect(x, y, w, h), new GUIContent("Left Begin"), property.FindPropertyRelative("leftBeginOffset").floatValue);
			EditorGUI.LabelField(new Rect(x, y, w, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.LeftBegin)));

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			property.FindPropertyRelative("rightBeginOffset").floatValue = EditorGUI.FloatField(new Rect(x, y, w, h), new GUIContent("Right Begin"), property.FindPropertyRelative("rightBeginOffset").floatValue);
			EditorGUI.LabelField(new Rect(x, y, w, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.RightBegin)));

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			property.FindPropertyRelative("betweenOffset").floatValue = EditorGUI.FloatField(new Rect(x, y, w, h), new GUIContent("Between"), property.FindPropertyRelative("betweenOffset").floatValue);
			EditorGUI.LabelField(new Rect(x, y, w, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.Between)));

			/// Default Styles

			y += 10;

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.LabelField(new Rect(x, y, w, h), new GUIContent(" Default Styles ", ManulToolbarMessages.GetTooltip(Tooltip.DefaultStyles)), EditorStyles.centeredGreyMiniLabel);

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUIUtility.labelWidth = 55;

			EditorGUI.PropertyField(new Rect(x, y, w, h), property.FindPropertyRelative("defaultButtonStyle"), new GUIContent("Button"));
			EditorGUI.LabelField(new Rect(x, y, w, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.StyleButton)));

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.PropertyField(new Rect(x, y, w, h), property.FindPropertyRelative("defaultToggleStyle"), new GUIContent("Toggle"));
			EditorGUI.LabelField(new Rect(x, y, w, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.StyleToggle)));

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.PropertyField(new Rect(x, y, w, h), property.FindPropertyRelative("defaultLabelStyle"), new GUIContent("Label"));
			EditorGUI.LabelField(new Rect(x, y, w, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.StyleLabel)));

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.PropertyField(new Rect(x, y, w, h), property.FindPropertyRelative("defaultPopupStyle"), new GUIContent("Popup"));
			EditorGUI.LabelField(new Rect(x, y, w, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.StylePopup)));

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.PropertyField(new Rect(x, y, w, h), property.FindPropertyRelative("defaultNumberStyle"), new GUIContent("Number"));
			EditorGUI.LabelField(new Rect(x, y, w, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.StyleNumber)));

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.PropertyField(new Rect(x, y, w, h), property.FindPropertyRelative("defaultTextStyle"), new GUIContent("Text"));
			EditorGUI.LabelField(new Rect(x, y, w, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.StyleText)));

			/// Other

			y += 10;

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.LabelField(new Rect(x, y, w, h), new GUIContent(" Other ", ManulToolbarMessages.GetTooltip(Tooltip.Other)), EditorStyles.centeredGreyMiniLabel);

			EditorGUIUtility.labelWidth = 120;

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.PropertyField(new Rect(x, y, w, h), property.FindPropertyRelative("showConsoleMessages"), new GUIContent("Console Messages"));
			EditorGUI.LabelField(new Rect(x, y, w, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.ConsoleMessages)));

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.PropertyField(new Rect(x, y, w, h), property.FindPropertyRelative("disableToolbar"), new GUIContent("Disable Toolbar"));
			EditorGUI.LabelField(new Rect(x, y, w, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.DisableToolbar)));

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.PropertyField(new Rect(x, y, w, h), property.FindPropertyRelative("mutedColor"), new GUIContent("Disabled Color"));
			EditorGUI.LabelField(new Rect(x, y, w, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.DisabledColor)));

			/// Override

			y += 10;

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.LabelField(new Rect(x, y, w, h), new GUIContent(" Override ", ManulToolbarMessages.GetTooltip(Tooltip.Override)), EditorStyles.centeredGreyMiniLabel);

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.LabelField(new Rect(x, y, w, h), "Use external assets to override button lists:", EditorStyles.helpBox);

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUIUtility.labelWidth = 115;

			float fieldWidth = 190;
			float buttonWidth = 25;

			EditorGUI.PropertyField(new Rect(x, y, fieldWidth - 10, h), property.FindPropertyRelative("overrideLeftType"), new GUIContent("Override Left Side"));
			EditorGUI.LabelField(new Rect(x, y, fieldWidth - 10, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.OverrideLeftSide)));


			switch (property.FindPropertyRelative("overrideLeftType").intValue)
			{
				case 0:
					break;

				case 1:
					EditorGUI.PropertyField(new Rect(x + fieldWidth, y, w - fieldWidth - buttonWidth, h), property.FindPropertyRelative("SOForLeftSide"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x + fieldWidth, y, w - fieldWidth - buttonWidth, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.SOForLeftSide)));

					if (GUI.Button(new Rect(x + fieldWidth + (w - fieldWidth - buttonWidth) + 7, y + 1, buttonWidth - 7, h), 
						new GUIContent(EditorGUIUtility.isProSkin ? ManulToolbar.openIconWhite : ManulToolbar.openIconBlack, ManulToolbarMessages.GetTooltip(Tooltip.SOForLeftSideButton)),
						EditorStyles.iconButton))
					{
						if (property.FindPropertyRelative("SOForLeftSide").objectReferenceValue != null)
						{
#if UNITY_2021_1_OR_NEWER
							EditorUtility.OpenPropertyEditor(property.FindPropertyRelative("SOForLeftSide").objectReferenceValue);
#else
							EditorGUIUtility.PingObject(property.FindPropertyRelative("SOForLeftSide").objectReferenceValue);
#endif
						}
					}

					break;

				case 2:
					EditorGUI.PropertyField(new Rect(x + fieldWidth, y, w - fieldWidth - buttonWidth, h), property.FindPropertyRelative("SOSetForLeftSide"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x + fieldWidth, y, w - fieldWidth - buttonWidth, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.SOSetForLeftSide)));

					if (GUI.Button(new Rect(x + fieldWidth + (w - fieldWidth - buttonWidth) + 7, y + 1, buttonWidth - 7, h),
						new GUIContent(EditorGUIUtility.isProSkin ? ManulToolbar.openIconWhite : ManulToolbar.openIconBlack, ManulToolbarMessages.GetTooltip(Tooltip.SOSetForLeftSideButton)),
						EditorStyles.iconButton))
					{
						if (property.FindPropertyRelative("SOSetForLeftSide").objectReferenceValue != null)
						{
#if UNITY_2021_1_OR_NEWER
							EditorUtility.OpenPropertyEditor(property.FindPropertyRelative("SOSetForLeftSide").objectReferenceValue);
#else
							EditorGUIUtility.PingObject(property.FindPropertyRelative("SOSetForLeftSide").objectReferenceValue);
#endif
						}
					}

					break;
			}

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.PropertyField(new Rect(x, y, 180, h), property.FindPropertyRelative("overrideRightType"), new GUIContent("Override Right Side"));
			EditorGUI.LabelField(new Rect(x, y, 180, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.OverrideRightSide)));

			switch (property.FindPropertyRelative("overrideRightType").intValue)
			{
				case 0:
					break;

				case 1:
					EditorGUI.PropertyField(new Rect(x + fieldWidth, y, w - fieldWidth - buttonWidth, h), property.FindPropertyRelative("SOForRightSide"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x + fieldWidth, y, w - fieldWidth - buttonWidth, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.SOForRightSide)));

					if (GUI.Button(new Rect(x + fieldWidth + (w - fieldWidth - buttonWidth) + 7, y + 1, buttonWidth - 7, h),
						new GUIContent(EditorGUIUtility.isProSkin ? ManulToolbar.openIconWhite : ManulToolbar.openIconBlack, ManulToolbarMessages.GetTooltip(Tooltip.SOForRightSideButton)),
						EditorStyles.iconButton))
					{
						if (property.FindPropertyRelative("SOForRightSide").objectReferenceValue != null)
						{
#if UNITY_2021_1_OR_NEWER
							EditorUtility.OpenPropertyEditor(property.FindPropertyRelative("SOForRightSide").objectReferenceValue);
#else
							EditorGUIUtility.PingObject(property.FindPropertyRelative("SOForRightSide").objectReferenceValue);
#endif
						}
					}

					break;

				case 2:
					EditorGUI.PropertyField(new Rect(x + fieldWidth, y, w - fieldWidth - buttonWidth, h), property.FindPropertyRelative("SOSetForRightSide"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x + fieldWidth, y, w - fieldWidth - buttonWidth, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.SOSetForRightSide)));

					if (GUI.Button(new Rect(x + fieldWidth + (w - fieldWidth - buttonWidth) + 7, y + 1, buttonWidth - 7, h),
						new GUIContent(EditorGUIUtility.isProSkin ? ManulToolbar.openIconWhite : ManulToolbar.openIconBlack, ManulToolbarMessages.GetTooltip(Tooltip.SOSetForRightSideButton)),
						EditorStyles.iconButton))
					{
						if (property.FindPropertyRelative("SOSetForRightSide").objectReferenceValue != null)
						{
#if UNITY_2021_1_OR_NEWER
							EditorUtility.OpenPropertyEditor(property.FindPropertyRelative("SOSetForRightSide").objectReferenceValue);
#else
							EditorGUIUtility.PingObject(property.FindPropertyRelative("SOSetForRightSide").objectReferenceValue);
#endif
						}
					}

					break;
			}

			EditorGUIUtility.labelWidth = currentLabelWidth;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 19 + 40;
		}
	}

	/// Manul Toolbar Entry

	[CustomPropertyDrawer(typeof(ManulToolbarEntry))]
	public class ManulToolbarEntry_Drawer : PropertyDrawer
	{
		Color tempMutedColor;  

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			float x = position.x;
			float y = position.y;
			float w = position.width;
			float h = EditorGUIUtility.singleLineHeight;    

			EditorGUI.BeginProperty(position, label, property);

			if (!property.FindPropertyRelative("isActive").boolValue)
			{
				tempMutedColor = GUI.color;
				GUI.color = ManulToolbar.settings.settings.mutedColor;
			}

			#region Row 1 - Foldout & Is Active

			property.FindPropertyRelative("isExpanded").boolValue = EditorGUI.Foldout(new Rect(x, y, w - 70 - 15 - 10, h), 
				property.FindPropertyRelative("isExpanded").boolValue, 
				new GUIContent(property.FindPropertyRelative("labelText").stringValue, ManulToolbarMessages.GetTooltip(Tooltip.EntryFoldout)), true);

			EditorGUI.PropertyField(new Rect(x += (w - 70 - 15 - 5), y, 70, h), property.FindPropertyRelative("type"), GUIContent.none);
			EditorGUI.LabelField(new Rect(x, y, 70, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.EntryType)));

			GUI.enabled = true;

			if (property.FindPropertyRelative("type").intValue == 0)
			{
				property.FindPropertyRelative("isActive").boolValue = true;
				GUI.enabled = false;
			}

			property.FindPropertyRelative("isActive").boolValue = EditorGUI.Toggle(new Rect(x += 75, y, 15, h), property.FindPropertyRelative("isActive").boolValue);
			EditorGUI.LabelField(new Rect(x, y, 15, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.EntryIsActive)));

			GUI.enabled = true;

			if (!property.FindPropertyRelative("isExpanded").boolValue)
			{
				if (!property.FindPropertyRelative("isActive").boolValue)
				{
					GUI.color = tempMutedColor;
				}

				EditorGUI.EndProperty();
				return;
			}

			#endregion

			#region Row 3 / 4 - Label Type / Label Text / Label Icon

			x = position.x;
			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

			switch (property.FindPropertyRelative("type").intValue)
			{					 
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:

					property.FindPropertyRelative("labelIconUsePath").boolValue = false;
					property.FindPropertyRelative("labelType").intValue = 0;

					EditorGUI.PropertyField(new Rect(x, y, w, h), property.FindPropertyRelative("labelText"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x, y, w, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.LabelText)));

					break; 

				default:

					if (property.FindPropertyRelative("type").intValue == 0)
					{
						property.FindPropertyRelative("labelIconUsePath").boolValue = false;
						property.FindPropertyRelative("labelType").intValue = 0;
						GUI.enabled = false;
					}

					EditorGUI.PropertyField(new Rect(x, y, 55, h), property.FindPropertyRelative("labelType"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x, y, 55, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.LabelType)));

					GUI.enabled = true;

					EditorGUI.PropertyField(new Rect(x + 60, y, w - 60, h), property.FindPropertyRelative("labelText"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x += 60, y, w - 60, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.LabelText)));

					if (property.FindPropertyRelative("labelType").intValue == 1 || property.FindPropertyRelative("labelType").intValue == 2)
					{
						x = position.x;
						y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

						if (property.FindPropertyRelative("labelIconUsePath").boolValue)
						{
							EditorGUI.PropertyField(new Rect(x, y, w - 55, h), property.FindPropertyRelative("labelIconPath"), GUIContent.none);
							EditorGUI.LabelField(new Rect(x, y, w - 55, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.LabelIconPath)));
						}
						else
						{
							EditorGUI.PropertyField(new Rect(x, y, w - 55, h), property.FindPropertyRelative("labelIcon"), GUIContent.none);
							EditorGUI.LabelField(new Rect(x, y, w - 55, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.LabelIcon)));
						}

						EditorGUI.LabelField(new Rect(x += w - 50, y, 35, h), new GUIContent(" Path ", ManulToolbarMessages.GetTooltip(Tooltip.UseLabelIconPath)));

						property.FindPropertyRelative("labelIconUsePath").boolValue = EditorGUI.Toggle(new Rect(x + 35, y, 15, h), property.FindPropertyRelative("labelIconUsePath").boolValue);
						EditorGUI.LabelField(new Rect(x + 35, y, 15, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.UseLabelIconPath)));
					}

					break;
			}

			#endregion

			#region Row 5 - Toggle Buttons

			if (property.FindPropertyRelative("type").intValue != 0)
			{

				x = position.x + 2;
				y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
				w = (position.width - 210 - 2) / 4;

				/// Style 

				switch (property.FindPropertyRelative("type").intValue)
				{
					case 5:
					case 6:
					case 7:
					case 9:
						property.FindPropertyRelative("useStyle").boolValue = false;
						GUI.enabled = false;
						property.FindPropertyRelative("useStyle").boolValue = GUI.Toggle(new Rect(x, y, 45 + w, h), 
							property.FindPropertyRelative("useStyle").boolValue, new GUIContent("Style", ManulToolbarMessages.GetTooltip(Tooltip.RowStyle)), EditorStyles.miniButtonMid);
						GUI.enabled = true;
						break;

					default:
						property.FindPropertyRelative("useStyle").boolValue = GUI.Toggle(new Rect(x, y, 45 + w, h), 
							property.FindPropertyRelative("useStyle").boolValue, new GUIContent("Style", ManulToolbarMessages.GetTooltip(Tooltip.RowStyle)), EditorStyles.miniButtonMid);
						break;
				}

				/// Width

				property.FindPropertyRelative("useWidth").boolValue = GUI.Toggle(new Rect(x += 50 + w, y, 45 + w, h), 
					property.FindPropertyRelative("useWidth").boolValue, new GUIContent("Width", ManulToolbarMessages.GetTooltip(Tooltip.RowWidth)), EditorStyles.miniButtonMid);

				/// Colors

				EditorGUI.BeginChangeCheck();

				property.FindPropertyRelative("useColors").boolValue = GUI.Toggle(new Rect(x += 50 + w, y, 50 + w, h), 
					property.FindPropertyRelative("useColors").boolValue, new GUIContent("Colors", ManulToolbarMessages.GetTooltip(Tooltip.RowColors)), EditorStyles.miniButtonMid);

				if (EditorGUI.EndChangeCheck())
				{
					if (property.FindPropertyRelative("useColors").boolValue)
					{
						if (property.FindPropertyRelative("globalColor").colorValue == Color.clear &&
							property.FindPropertyRelative("contentColor").colorValue == Color.clear &&
							property.FindPropertyRelative("backgroundColor").colorValue == Color.clear)
						{
							property.FindPropertyRelative("globalColor").colorValue = Color.white;
							property.FindPropertyRelative("contentColor").colorValue = Color.white;
							property.FindPropertyRelative("backgroundColor").colorValue = Color.white;

							property.serializedObject.ApplyModifiedProperties();
						}
					}
				}

				/// Tooltip

				switch (property.FindPropertyRelative("type").intValue)
				{
					case 1:
					case 2:
					case 3:
						property.FindPropertyRelative("useTooltip").boolValue = GUI.Toggle(new Rect(x += 55 + w, y, 55 + w, h), 
							property.FindPropertyRelative("useTooltip").boolValue, new GUIContent("Tooltip", ManulToolbarMessages.GetTooltip(Tooltip.RowTooltip)), EditorStyles.miniButtonMid);
						break;

					default:

						property.FindPropertyRelative("useTooltip").boolValue = false;
						GUI.enabled = false;
						property.FindPropertyRelative("useTooltip").boolValue = GUI.Toggle(new Rect(x += 55 + w, y, 55 + w, h), 
							property.FindPropertyRelative("useTooltip").boolValue, new GUIContent("Tooltip", ManulToolbarMessages.GetTooltip(Tooltip.RowTooltip)), EditorStyles.miniButtonMid);
						GUI.enabled = true;

						break;
				}
			}
			else
			{
				property.FindPropertyRelative("useStyle").boolValue = false;
				property.FindPropertyRelative("useColors").boolValue = false;
				property.FindPropertyRelative("useWidth").boolValue = false;
				property.FindPropertyRelative("useTooltip").boolValue = false;
			}


			#endregion

			#region Row 6, 7 - Style Rows

			w = position.width;

			if (property.FindPropertyRelative("useStyle").boolValue)
			{
				x = position.x;
				y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

				EditorGUI.LabelField(new Rect(x, y, 38, h), new GUIContent(" Style:", ManulToolbarMessages.GetTooltip(Tooltip.StyleType)));

				switch (property.FindPropertyRelative("editorStyle").intValue)
				{
					case 1:

						EditorGUI.PropertyField(new Rect(x += 55, y, 100, h), property.FindPropertyRelative("editorStyle"), GUIContent.none);
						EditorGUI.LabelField(new Rect(x, y, 100, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.StyleType)));

						EditorGUI.PropertyField(new Rect(x += 105, y, w - 55 - 105, h), property.FindPropertyRelative("skin"), GUIContent.none);
						EditorGUI.LabelField(new Rect(x, y, w - 55 - 105, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.StyleSkin)));

						x = position.x;
						y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
						w = position.width;
	
						EditorGUI.PropertyField(new Rect(x += 55, y, w - 55, h), property.FindPropertyRelative("styleName"), GUIContent.none);
						EditorGUI.LabelField(new Rect(x, y, w - 55, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.StyleName)));

						break;

					default:

						EditorGUI.PropertyField(new Rect(x += 55, y, w - 55, h), property.FindPropertyRelative("editorStyle"), GUIContent.none);
						EditorGUI.LabelField(new Rect(x, y, w - 55, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.StyleType)));
						break;
				}
			}

			#endregion

			#region Row 8 - Width Row

			if (property.FindPropertyRelative("useWidth").boolValue)
			{
				x = position.x;
				y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

				float currentLabelWidth = EditorGUIUtility.labelWidth;
				EditorGUIUtility.labelWidth = 54;
				EditorGUI.PropertyField(new Rect(x, y, w, h), property.FindPropertyRelative("width"), new GUIContent(" Width:"));
				EditorGUIUtility.labelWidth = currentLabelWidth;

				x = position.x;

				EditorGUI.LabelField(new Rect(x, y, w, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.FixedWidth)));
			}

			#endregion

			#region Row 9 - Colors Row

			if (property.FindPropertyRelative("useColors").boolValue)
			{
				x = position.x;
				y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
				w = (position.width - 65) / 3;

				EditorGUI.LabelField(new Rect(x, y, 55, h), new GUIContent(" Colors:", ManulToolbarMessages.GetTooltip(Tooltip.ColorsHeader)));

				EditorGUI.PropertyField(new Rect(x += 55, y, w, h), property.FindPropertyRelative("globalColor"), GUIContent.none);
				EditorGUI.PropertyField(new Rect(x += w + 5, y, w, h), property.FindPropertyRelative("contentColor"), GUIContent.none);
				EditorGUI.PropertyField(new Rect(x += w + 5, y, w, h), property.FindPropertyRelative("backgroundColor"), GUIContent.none);

				x = position.x;

				EditorGUI.LabelField(new Rect(x += 55, y, w, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.ColorsGlobal)));
				EditorGUI.LabelField(new Rect(x += w + 5, y, w, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.ColorsContent)));
				EditorGUI.LabelField(new Rect(x += w + 5, y, w, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.ColorsBackground)));
			}

			#endregion

			#region Row 10 - Tooltip Row

			if (property.FindPropertyRelative("useTooltip").boolValue)
			{
				x = position.x;
				y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
				w = position.width;

				EditorGUI.LabelField(new Rect(x, y, 50, h), new GUIContent(" Tooltip:", ManulToolbarMessages.GetTooltip(Tooltip.TooltipHeader)));	 

				float tooltipHeight;

				if (property.FindPropertyRelative("linesCount").intValue > 1)
				{
					tooltipHeight = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * property.FindPropertyRelative("linesCount").intValue +
						(EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
				}
				else
				{
					tooltipHeight = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2;
				}

				property.FindPropertyRelative("labelTooltip").stringValue = EditorGUI.TextArea(new Rect(x += 55, y, w - 55, tooltipHeight), property.FindPropertyRelative("labelTooltip").stringValue, EditorStyles.textArea);
				EditorGUI.LabelField(new Rect(x, y, w - 55, tooltipHeight), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.TooltipText)));

				x = position.x;
				y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
				h = EditorGUIUtility.singleLineHeight;

				property.FindPropertyRelative("linesCount").intValue = EditorGUI.IntField(new Rect(x += 5, y, 40, h), property.FindPropertyRelative("linesCount").intValue);
				EditorGUI.LabelField(new Rect(x, y, 40, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.TooltipLinesCount)));

				y += tooltipHeight - 2 * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
			}

			#endregion

			#region Row 2 - Bool Editor Pref / List

			switch (property.FindPropertyRelative("type").intValue)
			{
				#region Toggle

				case 2:

					x = position.x;
					y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

					EditorGUI.LabelField(new Rect(x, y, 135, h), new GUIContent(" Editor Pref Bool Name", ManulToolbarMessages.GetTooltip(Tooltip.PrefToggle)));  
					EditorGUI.PropertyField(new Rect(x += 135, y, w - 135, h), property.FindPropertyRelative("togglePrefName"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x, y, w, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.PrefToggle)));

					break;

				#endregion

				#region Popup

				case 4:			 

					x = position.x;
					y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

					EditorGUI.LabelField(new Rect(x, y, 125, h), new GUIContent(" Editor Pref Int Name", ManulToolbarMessages.GetTooltip(Tooltip.PrefPopup)));
					EditorGUI.LabelField(new Rect(x, y, w - 50, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.PrefPopup)));
					EditorGUI.PropertyField(new Rect(x += 125, y, w - 125 - 50, h), property.FindPropertyRelative("togglePrefName"), GUIContent.none);

					if (GUI.Button(new Rect(x += (w - 125 - 45), y, 45, h), new GUIContent("Fill", ManulToolbarMessages.GetTooltip(Tooltip.PopupFillButton))))
					{
						for (int i = 0; i < property.FindPropertyRelative("intNamesList").arraySize; i++)
						{
							property.FindPropertyRelative("intNamesList").GetArrayElementAtIndex(i).FindPropertyRelative("itemIndex").intValue = i;
						}

						EditorUtility.SetDirty(property.FindPropertyRelative("intNamesList").serializedObject.targetObject);
					}

					x = position.x;
					y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

					EditorGUI.PropertyField(new Rect(x + 15, y, w - 15, h), property.FindPropertyRelative("intNamesList"), new GUIContent("Names List: ", ManulToolbarMessages.GetTooltip(Tooltip.PopupNamesList)));

					if (property.FindPropertyRelative("intNamesList").isExpanded)
					{
						y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
						y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
						y += 5;

						if (property.FindPropertyRelative("intNamesList").arraySize > 1)
						{
							for (int i = 1; i < property.FindPropertyRelative("intNamesList").arraySize; i++)
							{
								y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
							}
						}
					}

					break;

				#endregion

				#region List

				case 8:

					x = position.x;
					y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

					EditorGUI.LabelField(new Rect(x, y, 125, h), new GUIContent(" Editor Pref Int Name", ManulToolbarMessages.GetTooltip(Tooltip.PrefList)));
					EditorGUI.LabelField(new Rect(x, y, w - 50, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.PrefList)));
					EditorGUI.PropertyField(new Rect(x += 125, y, w - 125 - 50, h), property.FindPropertyRelative("togglePrefName"), new GUIContent(""));

					if (GUI.Button(new Rect(x += (w - 125 - 45), y, 45, h), new GUIContent("Fill", ManulToolbarMessages.GetTooltip(Tooltip.ListFillButton))))
					{
						for (int i = 0; i < property.FindPropertyRelative("actions").arraySize; i++)
						{
							SerializedProperty elementName = property.FindPropertyRelative("actions").GetArrayElementAtIndex(i).FindPropertyRelative("listActionName");

							if (!string.IsNullOrWhiteSpace(elementName.stringValue)) continue;

							switch (property.FindPropertyRelative("actions").GetArrayElementAtIndex(i).FindPropertyRelative("buttonType").enumValueIndex)
							{
								case 0: /// None

									break;

								case 1:     /// OpenAsset
								case 2:     /// SelectAsset
								case 3:     /// ShowAssetInExplorer
								case 4:     /// PropertiesWindow 
								case 12:    /// LoadSceneAdditive 

									UnityEngine.Object buttonObject = property.FindPropertyRelative("actions").GetArrayElementAtIndex(i).FindPropertyRelative("buttonObject").objectReferenceValue;

									if (buttonObject != null)
									{
										string[] pathSplit = AssetDatabase.GetAssetPath(buttonObject).Split("/");
										string[] dotSplit = pathSplit[pathSplit.Length - 1].Split(".");
										string finalName = "";

										for (int j = 0; j < dotSplit.Length - 1; j++) finalName += dotSplit[j];

										elementName.stringValue = finalName;
									}

									break;

								case 5: /// StaticMethod
								case 6: /// ObjectOfTypeMethod						 
								case 7: /// ComponentMethod

									elementName.stringValue = property.FindPropertyRelative("actions").GetArrayElementAtIndex(i).FindPropertyRelative("methodName").stringValue;

									break;

								case 8: /// OpenFolder

									string[] folderSplit = property.FindPropertyRelative("actions").GetArrayElementAtIndex(i).FindPropertyRelative("className").stringValue.Split("/");
									elementName.stringValue = folderSplit[folderSplit.Length - 1];

									break;

								case 9: /// FindGameobject

									elementName.stringValue = property.FindPropertyRelative("actions").GetArrayElementAtIndex(i).FindPropertyRelative("className").stringValue;

									break;

								case 10: /// ExecuteMenuItem

									string[] menuItemSplit = property.FindPropertyRelative("actions").GetArrayElementAtIndex(i).FindPropertyRelative("className").stringValue.Split("/");
									elementName.stringValue = menuItemSplit[menuItemSplit.Length - 1];

									break;

								case 11: /// InvokeEvent   

									elementName.stringValue = "Event";

									break;
							}
						}

						EditorUtility.SetDirty(property.FindPropertyRelative("actions").serializedObject.targetObject);
					}

					break;

				#endregion

				#region Text

				case 7:

					x = position.x;
					y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

					EditorGUI.LabelField(new Rect(x, y, 140, h), new GUIContent(" Editor Pref String Name", ManulToolbarMessages.GetTooltip(Tooltip.PrefText)));
					EditorGUI.PropertyField(new Rect(x += 140, y, w - 140, h), property.FindPropertyRelative("togglePrefName"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x, y, w, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.PrefText)));
					break;

				#endregion

				#region Number

				case 6:

					x = position.x;
					y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

					EditorGUI.PropertyField(new Rect(x, y, 55, h), property.FindPropertyRelative("numberType"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x, y, 55, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.NumberType)));

					switch (property.FindPropertyRelative("numberType").intValue)
					{
						case 0: /// Float
							EditorGUI.LabelField(new Rect(x += 60, y, 140, h), new GUIContent(" Editor Pref Float Name", ManulToolbarMessages.GetTooltip(Tooltip.PrefNumberFloat)));
							EditorGUI.LabelField(new Rect(x, y, w - 60, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.PrefNumberFloat)));
							EditorGUI.PropertyField(new Rect(x += 140, y, w - 140 - 60, h), property.FindPropertyRelative("togglePrefName"), GUIContent.none);
							break;

						case 1: /// Int
							EditorGUI.LabelField(new Rect(x += 60, y, 125, h), new GUIContent(" Editor Pref Int Name", ManulToolbarMessages.GetTooltip(Tooltip.PrefNumberInt)));
							EditorGUI.LabelField(new Rect(x, y, w - 60, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.PrefNumberInt)));
							EditorGUI.PropertyField(new Rect(x += 125, y, w - 125 - 60, h), property.FindPropertyRelative("togglePrefName"), GUIContent.none);
							break;
					}

					break;

				#endregion

				#region Slider

				case 5:

					x = position.x;
					y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

					EditorGUI.PropertyField(new Rect(x, y, 55, h), property.FindPropertyRelative("numberType"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x, y, 55, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.SliderType)));

					float space = (w - 72) / 2;

					switch (property.FindPropertyRelative("numberType").intValue)
					{
						case 0: /// Float

							EditorGUI.LabelField(new Rect(x += 60, y, 140, h), new GUIContent(" Editor Pref Float Name", ManulToolbarMessages.GetTooltip(Tooltip.PrefSliderFloat)));
							EditorGUI.LabelField(new Rect(x, y, w - 60, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.PrefSliderFloat)));
							EditorGUI.PropertyField(new Rect(x += 140, y, w - 140 - 60, h), property.FindPropertyRelative("togglePrefName"), GUIContent.none);

							x = position.x;
							y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

							EditorGUI.LabelField(new Rect(x, y, 32, h), new GUIContent(" Min", ManulToolbarMessages.GetTooltip(Tooltip.SliderFloatMin)));
							EditorGUI.PropertyField(new Rect(x += 32, y, space, h), property.FindPropertyRelative("sliderFloatMin"), GUIContent.none);
							EditorGUI.LabelField(new Rect(x, y, space, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.SliderFloatMin)));

							EditorGUI.LabelField(new Rect(x += space + 5, y, 35, h), new GUIContent(" Max", ManulToolbarMessages.GetTooltip(Tooltip.SliderFloatMax)));
							EditorGUI.PropertyField(new Rect(x += 35, y, space, h), property.FindPropertyRelative("sliderFloatMax"), GUIContent.none);
							EditorGUI.LabelField(new Rect(x, y, space, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.SliderFloatMax)));

							break;

						case 1: /// Int

							EditorGUI.LabelField(new Rect(x += 60, y, 125, h), new GUIContent(" Editor Pref Int Name", ManulToolbarMessages.GetTooltip(Tooltip.PrefSliderInt)));
							EditorGUI.LabelField(new Rect(x, y, w - 60, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.PrefSliderInt)));
							EditorGUI.PropertyField(new Rect(x += 125, y, w - 125 - 60, h), property.FindPropertyRelative("togglePrefName"), new GUIContent(""));

							x = position.x;
							y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

							EditorGUI.LabelField(new Rect(x, y, 32, h), new GUIContent(" Min", ManulToolbarMessages.GetTooltip(Tooltip.SliderIntMin)));
							EditorGUI.PropertyField(new Rect(x += 32, y, space, h), property.FindPropertyRelative("sliderIntMin"), GUIContent.none);
							EditorGUI.LabelField(new Rect(x, y, space, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.SliderIntMin)));

							EditorGUI.LabelField(new Rect(x += space + 5, y, 35, h), new GUIContent(" Max", ManulToolbarMessages.GetTooltip(Tooltip.SliderIntMax)));
							EditorGUI.PropertyField(new Rect(x += 35, y, space, h), property.FindPropertyRelative("sliderIntMax"), GUIContent.none);
							EditorGUI.LabelField(new Rect(x, y, space, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.SliderIntMax)));

							break;
					}

					break;

				#endregion

				#region Other

				case 9:

					x = position.x;
					y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

					float space2 = w - 80 - 122;
					EditorGUI.PropertyField(new Rect(x, y, space2 - 10, h), property.FindPropertyRelative("otherType"), new GUIContent(""));
					EditorGUI.LabelField(new Rect(x, y, space2 - 10, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.OtherType)));

					switch (property.FindPropertyRelative("otherType").intValue)
					{
						case 1:

							property.FindPropertyRelative("useOtherLabel").boolValue = EditorGUI.ToggleLeft(new Rect(x += space2, y, 80, h), "Use Label", property.FindPropertyRelative("useOtherLabel").boolValue);
							EditorGUI.LabelField(new Rect(x, y, 80, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.TimeUseLabel)));

							property.FindPropertyRelative("useResetButton").boolValue = EditorGUI.ToggleLeft(new Rect(x += 85, y, 120, h), "Use Reset Button", property.FindPropertyRelative("useResetButton").boolValue);
							EditorGUI.LabelField(new Rect(x, y, 120, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.TimeUseResetButton)));

							space2 = (w - 52 - 32 - 33 - 12) / 3;

							x = position.x;
							y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

							EditorGUI.LabelField(new Rect(x, y, 52, h), new GUIContent(" Default", ManulToolbarMessages.GetTooltip(Tooltip.TimeDefault)));
							EditorGUI.PropertyField(new Rect(x += 52, y, space2, h), property.FindPropertyRelative("defaultSliderValue"), GUIContent.none);
							EditorGUI.LabelField(new Rect(x, y, space2, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.TimeDefault)));

							EditorGUI.LabelField(new Rect(x += space2 + 5, y, 32, h), new GUIContent(" Min", ManulToolbarMessages.GetTooltip(Tooltip.TimeMin)));
							EditorGUI.PropertyField(new Rect(x += 32, y, space2, h), property.FindPropertyRelative("sliderFloatMin"), GUIContent.none);
							EditorGUI.LabelField(new Rect(x, y, space2, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.TimeMin)));

							EditorGUI.LabelField(new Rect(x += space2 + 5, y, 35, h), new GUIContent(" Max", ManulToolbarMessages.GetTooltip(Tooltip.TimeMax)));
							EditorGUI.PropertyField(new Rect(x += 35, y, space2, h), property.FindPropertyRelative("sliderFloatMax"), GUIContent.none);
							EditorGUI.LabelField(new Rect(x, y, space2, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.TimeMax)));

							break;

						case 2:

							property.FindPropertyRelative("useOtherLabel").boolValue = EditorGUI.ToggleLeft(new Rect(x += space2, y, 80, h), "Use Label", property.FindPropertyRelative("useOtherLabel").boolValue);
							EditorGUI.LabelField(new Rect(x, y, 80, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.FPSUseLabel)));

							property.FindPropertyRelative("useResetButton").boolValue = EditorGUI.ToggleLeft(new Rect(x += 85, y, 120, h), "Use Reset Button", property.FindPropertyRelative("useResetButton").boolValue);
							EditorGUI.LabelField(new Rect(x, y, 120, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.FPSUseResetButton)));

							space2 = (w - 52 - 32 - 33 - 12) / 3;

							x = position.x;
							y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

							EditorGUI.LabelField(new Rect(x, y, 52, h), new GUIContent(" Default", ManulToolbarMessages.GetTooltip(Tooltip.FPSDefault)));
							EditorGUI.PropertyField(new Rect(x += 52, y, space2, h), property.FindPropertyRelative("defaultSliderValueInt"), GUIContent.none);
							EditorGUI.LabelField(new Rect(x, y, space2, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.FPSDefault)));

							EditorGUI.LabelField(new Rect(x += space2 + 5, y, 32, h), new GUIContent(" Min", ManulToolbarMessages.GetTooltip(Tooltip.FPSMin)));
							EditorGUI.PropertyField(new Rect(x += 32, y, space2, h), property.FindPropertyRelative("sliderIntMin"), GUIContent.none);
							EditorGUI.LabelField(new Rect(x, y, space2, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.FPSMin)));

							EditorGUI.LabelField(new Rect(x += space2 + 5, y, 35, h), new GUIContent(" Max", ManulToolbarMessages.GetTooltip(Tooltip.FPSMax)));
							EditorGUI.PropertyField(new Rect(x += 35, y, space2, h), property.FindPropertyRelative("sliderIntMax"), GUIContent.none);
							EditorGUI.LabelField(new Rect(x, y, space2, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.FPSMax)));

							break;
					}

					break;

				#endregion
			}

			#endregion

			#region Row 11 - Button Actions / Action List / On Change Actions

			switch (property.FindPropertyRelative("type").intValue)
			{
				case 1:

					x = position.x;
					y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					w = position.width;

					for (int i = 0; i < property.FindPropertyRelative("actions").arraySize; i++)
					{
						property.FindPropertyRelative("actions").GetArrayElementAtIndex(i).FindPropertyRelative("isPartOfList").intValue = 0;
					}

					EditorGUI.PropertyField(new Rect(x += 15, y, w - 15, h), property.FindPropertyRelative("actions"), new GUIContent("Button Actions", ManulToolbarMessages.GetTooltip(Tooltip.ButtonActions)), true);

					break;

				case 2:
				case 4:
				case 5:
				case 6:
				case 7:
				case 9:

					x = position.x;
					y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					w = position.width;	 

					property.FindPropertyRelative("useOnChangeActions").boolValue = EditorGUI.ToggleLeft(new Rect(x, y, w, h), "Use On Change Value Actions", property.FindPropertyRelative("useOnChangeActions").boolValue);
					EditorGUI.LabelField(new Rect(x, y, w, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.UseOnChangeActions)));

					if (property.FindPropertyRelative("useOnChangeActions").boolValue)
					{	
						x = position.x;
						y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
						w = position.width;

						for (int i = 0; i < property.FindPropertyRelative("actions").arraySize; i++)
						{
							property.FindPropertyRelative("actions").GetArrayElementAtIndex(i).FindPropertyRelative("isPartOfList").intValue = 2;
						}

						EditorGUI.PropertyField(new Rect(x += 15, y, w - 15, h), property.FindPropertyRelative("actions"), new GUIContent("On Change Value Actions", ManulToolbarMessages.GetTooltip(Tooltip.OnChangeActions)), true);
					}

					break;

				case 8:

					x = position.x;
					y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					w = position.width;

					for (int i = 0; i < property.FindPropertyRelative("actions").arraySize; i++)
					{
						property.FindPropertyRelative("actions").GetArrayElementAtIndex(i).FindPropertyRelative("isPartOfList").intValue = 1;
					}

					EditorGUI.PropertyField(new Rect(x += 15, y, w - 15, h), property.FindPropertyRelative("actions"), new GUIContent("Actions List", ManulToolbarMessages.GetTooltip(Tooltip.ActionsList)), true);

					break;
			}

			#endregion

			if (!property.FindPropertyRelative("isActive").boolValue)
			{
				GUI.color = tempMutedColor;
			}

			EditorGUI.EndProperty();
		} 

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{ 
			/// Row 1 - Folout & Is Active

			float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 5;

			if (!property.FindPropertyRelative("isExpanded").boolValue) return height;

			/// Row 3 - Label Type & Label Text

			height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

			/// Row 4 - Label Icon

			if (property.FindPropertyRelative("labelType").intValue == 1 || property.FindPropertyRelative("labelType").intValue == 2)
			{
				height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
			}

			/// Row 5 - Toggle Buttons

			if (property.FindPropertyRelative("type").intValue != 0)
			{
				height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
			}			 

			/// Row 6, 7 - Style Rows

			if (property.FindPropertyRelative("useStyle").boolValue)
			{
				height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

				if (property.FindPropertyRelative("editorStyle").intValue == 1)
				{
					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
				}
			}

			/// Row 8 - Width Row

			if (property.FindPropertyRelative("useWidth").boolValue)
			{
				height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
			}

			/// Row 9 - Colors Row

			if (property.FindPropertyRelative("useColors").boolValue)
			{
				height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
			}

			/// Row 10 - Tooltip Row

			if (property.FindPropertyRelative("useTooltip").boolValue)
			{
				if (property.FindPropertyRelative("linesCount").intValue > 1)
				{
					height += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * property.FindPropertyRelative("linesCount").intValue +
						(EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
				}
				else
				{
					height += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2;
				}
			}

			/// Row 2 - Editor Pref

			switch (property.FindPropertyRelative("type").intValue)
			{
				case 2:
				case 6:
				case 7:
				case 8:
					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					break;

				case 5:
					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					break;

				case 4:

					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2; 

					if (property.FindPropertyRelative("intNamesList").isExpanded)
					{
						height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
						height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
						height += 5;

						if (property.FindPropertyRelative("intNamesList").arraySize > 1)
						{
							for (int i = 1; i < property.FindPropertyRelative("intNamesList").arraySize; i++)
							{
								height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
							}
						}
					}

					break;
			} 

			/// Row 11a - Use On Change Value Actions

			switch (property.FindPropertyRelative("type").intValue)
			{ 
				case 2:
				case 4:
				case 5:
				case 6:
				case 7:

					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					break;

				case 9:

					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

					switch (property.FindPropertyRelative("otherType").intValue)
					{
						case 1:
						case 2:
							height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
							break;
					}
					break;
			} 

			/// Row 11b - Button Actions / Action List / On Change Actions

			switch (property.FindPropertyRelative("type").intValue)
			{ 
				case 1:
				case 8:

					if (property.FindPropertyRelative("actions").isExpanded)
					{
						if (property.FindPropertyRelative("actions").arraySize < 1)
						{
							height += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 3 + 10;
						}
						else
						{
							height += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2 + 10;

							for (int i = 0; i < property.FindPropertyRelative("actions").arraySize; i++)
							{
								height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("actions").GetArrayElementAtIndex(i)) + EditorGUIUtility.standardVerticalSpacing;
							}
						}
					}
					else
					{
						height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					}

					break;

				case 2:
				case 4:
				case 5:
				case 6:
				case 7:
				case 9:

					if (property.FindPropertyRelative("useOnChangeActions").boolValue)
					{
						if (property.FindPropertyRelative("actions").isExpanded)
						{
							if (property.FindPropertyRelative("actions").arraySize < 1)
							{
								height += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 3 + 10;
							}
							else
							{
								height += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2 + 10;

								for (int i = 0; i < property.FindPropertyRelative("actions").arraySize; i++)
								{
									height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("actions").GetArrayElementAtIndex(i)) + EditorGUIUtility.standardVerticalSpacing;
								}
							}
						}
						else
						{
							height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
						}
					} 

					break; 
			} 

			return height;
		}
	}

	/// Manul Toolbar Action

	[CustomPropertyDrawer(typeof(ManulToolbarAction))]
	public class ManulToolbarAction_Drawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			float x = position.x;
			float y = position.y;
			float w = position.width;
			float h = EditorGUIUtility.singleLineHeight;

			switch (property.FindPropertyRelative("isPartOfList").intValue)
			{
				case 0:

					EditorGUI.PropertyField(new Rect(x, y, w - 120, h), property.FindPropertyRelative("buttonType"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x, y, w - 120, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.ButtonType)));

					EditorGUI.PropertyField(new Rect(x += (w - 120) + 5, y, 50, h), property.FindPropertyRelative("mouseButton"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x, y, 50, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.MouseButton)));

					EditorGUI.PropertyField(new Rect(x += 50 + 5, y, 60, h), property.FindPropertyRelative("keyboardButton"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x, y, w, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.KeyboardButton)));

					break;

				case 1:

					EditorGUI.PropertyField(new Rect(x, y, 155, h), property.FindPropertyRelative("buttonType"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x, y, 155, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.ButtonType)));

					EditorGUI.PropertyField(new Rect(x + 160, y, w - 160, h), property.FindPropertyRelative("listActionName"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x + 160, y, w - 160, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.ListActionName)));

					break;

				case 2:

					EditorGUI.PropertyField(new Rect(x, y, w, h), property.FindPropertyRelative("buttonType"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x, y, w, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.ButtonType)));

					break;	 
			} 

			int buttonOption = property.FindPropertyRelative("buttonType").intValue;

			x = position.x;
			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

			switch (buttonOption)
			{
				case 1:
				case 2:
				case 3:
				case 4:

					EditorGUI.PropertyField(new Rect(x + 1, y, w - 1, h), property.FindPropertyRelative("buttonObject"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x + 1, y, w - 1, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.ButtonObject)));
					
					break; 

				case 5:

					EditorGUI.LabelField(new Rect(x, y, 43, h), new GUIContent(" Class", ManulToolbarMessages.GetTooltip(Tooltip.StaticClass)));
					EditorGUI.PropertyField(new Rect(x += 43, y, w - 43, h), property.FindPropertyRelative("className"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x, y, w - 43, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.StaticClass)));

					x = position.x;
					y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

					EditorGUI.LabelField(new Rect(x, y, 55, h), new GUIContent(" Method", ManulToolbarMessages.GetTooltip(Tooltip.StaticMethod)));
					EditorGUI.PropertyField(new Rect(x += 55, y, w - 55, h), property.FindPropertyRelative("methodName"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x, y, w - 55, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.StaticMethod)));

					break;

				case 6:

					EditorGUI.LabelField(new Rect(x, y, 40, h), new GUIContent(" Type", ManulToolbarMessages.GetTooltip(Tooltip.TypeClass)));
					EditorGUI.PropertyField(new Rect(x += 40, y, w - 40, h), property.FindPropertyRelative("className"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x, y, w - 40, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.TypeClass)));

					x = position.x;
					y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

					EditorGUI.LabelField(new Rect(x, y, 55, h), new GUIContent(" Method", ManulToolbarMessages.GetTooltip(Tooltip.TypeMethod)));
					EditorGUI.PropertyField(new Rect(x += 55, y, w - 55, h), property.FindPropertyRelative("methodName"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x, y, w - 55, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.TypeMethod)));

					break;

				case 7:

					EditorGUI.LabelField(new Rect(x, y, 82, h), new GUIContent(" GameObject", ManulToolbarMessages.GetTooltip(Tooltip.GOName)));
					EditorGUI.PropertyField(new Rect(x += 82, y, w - 82, h), property.FindPropertyRelative("objectName"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x, y, w - 82, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.GOName)));

					x = position.x;
					y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

					EditorGUI.LabelField(new Rect(x, y, 77, h), new GUIContent(" Component", ManulToolbarMessages.GetTooltip(Tooltip.GOComponent)));
					EditorGUI.PropertyField(new Rect(x += 77, y, w - 77, h), property.FindPropertyRelative("className"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x, y, w - 77, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.GOComponent)));

					x = position.x;
					y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

					EditorGUI.LabelField(new Rect(x, y, 55, h), new GUIContent(" Method", ManulToolbarMessages.GetTooltip(Tooltip.GOMethod)));
					EditorGUI.PropertyField(new Rect(x += 55, y, w - 55, h), property.FindPropertyRelative("methodName"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x, y, w - 55, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.GOMethod)));

					break;

				case 8:

					EditorGUI.PropertyField(new Rect(x + 1, y, w - 1, h), property.FindPropertyRelative("className"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x + 1, y, w - 1, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.OpenFolder)));

					break;

				case 9:

					EditorGUI.PropertyField(new Rect(x + 1, y, w - 1, h), property.FindPropertyRelative("className"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x + 1, y, w - 1, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.FindGO)));

					break;

				case 10:

					EditorGUI.PropertyField(new Rect(x + 1, y, w - 1 - 28, h), property.FindPropertyRelative("className"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x + 1, y, w - 1 - 28, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.MenuItem)));

					if (GUI.Button(new Rect(x + 1 + (w - 1 - 23), y, 23, h), new GUIContent(EditorGUIUtility.isProSkin ? ManulToolbar.searchIconWhite : ManulToolbar.searchIconBlack, ManulToolbarMessages.GetTooltip(Tooltip.MenuItemButton))))
					{
						ManulToolbarBrowser.OpenWindow(position, property);
					}

					break;

				case 11:

					EditorGUI.PropertyField(new Rect(x + 1, y, w - 1, h), property.FindPropertyRelative("buttonEvent"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x + 1, y, w - 1, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.ButtonEvent)));
					break;

				case 12:

					EditorGUI.PropertyField(new Rect(x + 1, y, w - 1, h), property.FindPropertyRelative("buttonObject"), GUIContent.none);
					EditorGUI.LabelField(new Rect(x + 1, y, w - 1, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.LoadAdditive)));
					break;
			}

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 5;

			int buttonOption = property.FindPropertyRelative("buttonType").intValue;

			switch (buttonOption)
			{
				case 1:
				case 2:
				case 3:
				case 4:
				case 8:
				case 9:
				case 10:
				case 12:
					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					break;

				case 11:
					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					height += 74;

					int eventCallsSize = property.FindPropertyRelative("buttonEvent").FindPropertyRelative("m_PersistentCalls").FindPropertyRelative("m_Calls").arraySize;

					if (eventCallsSize > 1)
					{
						height += 49 * (eventCallsSize - 1);
					}

					break;

				case 5:
				case 6:
					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					break;

				case 7:
					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					break;
			}

			return height;
		}
	}

	/// Manul Toolbar Popup Entry

	[CustomPropertyDrawer(typeof(ManulToolbarPopupEntry))]
	public class ManulToolbarPopupEntry_Drawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			float x = position.x;
			float y = position.y;
			float w = position.width;
			float h = EditorGUIUtility.singleLineHeight;

			EditorGUI.PropertyField(new Rect(x, y, w - 50, h), property.FindPropertyRelative("itemName"), GUIContent.none);
			EditorGUI.LabelField(new Rect(x, y, w - 50, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.PopupListItemName)));

			EditorGUI.PropertyField(new Rect(x + (w - 45), y, 45, h), property.FindPropertyRelative("itemIndex"), GUIContent.none);
			EditorGUI.LabelField(new Rect(x + (w - 45), y, 45, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.PopupListItemIndex)));
		}
	}

	/// Manul Toolbar Button List Set Entry

	[CustomPropertyDrawer(typeof(ManulToolbarButtonListSetEntry))]
	public class ManulToolbarButtonListSetEntry_Drawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			float x = position.x;
			float y = position.y;
			float w = position.width;
			float h = EditorGUIUtility.singleLineHeight;
			float buttonWidth = 25;
			float width = (w / 2) - buttonWidth / 2;

			EditorGUI.PropertyField(new Rect(x, y, width, h), property.FindPropertyRelative("setName"), GUIContent.none);
			EditorGUI.LabelField(new Rect(x, y, width, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.SetName)));

			EditorGUI.PropertyField(new Rect(x + width + 5f, y, width, h), property.FindPropertyRelative("setSO"), GUIContent.none);
			EditorGUI.LabelField(new Rect(x + width + 5f, y, width, h), new GUIContent(" ", ManulToolbarMessages.GetTooltip(Tooltip.SetSO)));

			if (GUI.Button(new Rect(x + width + 5f + width + 5f, y + 1, buttonWidth - 7, h), 
				new GUIContent(EditorGUIUtility.isProSkin ? ManulToolbar.openIconWhite : ManulToolbar.openIconBlack, ManulToolbarMessages.GetTooltip(Tooltip.SetSOButton)), EditorStyles.iconButton))
			{
				if (property.FindPropertyRelative("setSO").objectReferenceValue != null)
				{
#if UNITY_2021_1_OR_NEWER
					EditorUtility.OpenPropertyEditor(property.FindPropertyRelative("setSO").objectReferenceValue);
#else
					EditorGUIUtility.PingObject(property.FindPropertyRelative("setSO").objectReferenceValue);
#endif
				}
			}

		}
	}

	/// Manul Toolbar Button List Set

	[CustomEditor(typeof(ManulToolbarButtonListSet))]
	public class ManulToolbarButtonListSet_Editor : Editor
	{
		ManulToolbarButtonListSet obj;

		protected void OnEnable()
		{
			obj = (ManulToolbarButtonListSet)target;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			float currentLabelWidth = EditorGUIUtility.labelWidth;

			EditorGUIUtility.labelWidth = 85;

			EditorGUI.BeginChangeCheck();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("intPrefName"), new GUIContent("Int Pref Name", ManulToolbarMessages.GetTooltip(Tooltip.SetPrefInt)));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("popupWidth"), new GUIContent("Popup Width", ManulToolbarMessages.GetTooltip(Tooltip.SetWidth)));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("listEntries"), new GUIContent("Items List", ManulToolbarMessages.GetTooltip(Tooltip.SetListEntries)));
			EditorGUILayout.EndHorizontal();

			EditorGUIUtility.labelWidth = 140;

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("useOnChangeValueActions"), new GUIContent("Use On Change Actions", ManulToolbarMessages.GetTooltip(Tooltip.SetUseActionsToggle)));
			EditorGUILayout.EndHorizontal();

			EditorGUIUtility.labelWidth = currentLabelWidth;

			if (serializedObject.FindProperty("useOnChangeValueActions").boolValue)
			{
				for (int i = 0; i < serializedObject.FindProperty("onChangeValueActions").arraySize; i++)
				{
					serializedObject.FindProperty("onChangeValueActions").GetArrayElementAtIndex(i).FindPropertyRelative("isPartOfList").intValue = 2;
				}

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(serializedObject.FindProperty("onChangeValueActions"), new GUIContent("On Change Actions", ManulToolbarMessages.GetTooltip(Tooltip.SetUseActions)));
				EditorGUILayout.EndHorizontal();
			} 

			if (EditorGUI.EndChangeCheck())
			{
				ManulToolbar.RefreshToolbar();
				EditorUtility.SetDirty(obj);
			}

			serializedObject.ApplyModifiedProperties();
		}
	}

	/// Manul Toolbar Button List Set

	[CustomEditor(typeof(ManulToolbarButtonList))]
	public class ManulToolbarButtonList_Editor : Editor
	{
		ManulToolbarButtonList obj;

		protected void OnEnable()
		{
			obj = (ManulToolbarButtonList)target;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
 
			EditorGUI.BeginChangeCheck();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("buttons"), new GUIContent("Items List", ManulToolbarMessages.GetTooltip(Tooltip.ListButtons)));
			EditorGUILayout.EndHorizontal();

			if (EditorGUI.EndChangeCheck())
			{
				ManulToolbar.RefreshToolbar();
				EditorUtility.SetDirty(obj);
			}

			serializedObject.ApplyModifiedProperties();
		}
	} 

	/// Manul Toolbar Styles

	public static class ManulToolbarStyles
	{
		public static GUIStyle HeaderStyle { get; private set; }

		static ManulToolbarStyles()
		{
			HeaderStyle = new GUIStyle(GUI.skin.box);
			HeaderStyle.fontSize = 13;
			HeaderStyle.alignment = TextAnchor.MiddleCenter;
			HeaderStyle.fontStyle = FontStyle.Bold;
			HeaderStyle.normal.textColor = EditorGUIUtility.isProSkin ? new Color(1, 1, 1, 0.75f) : new Color(0.15f, 0.15f, 0.15f, 0.75f);
		}
	} 
}

#endif