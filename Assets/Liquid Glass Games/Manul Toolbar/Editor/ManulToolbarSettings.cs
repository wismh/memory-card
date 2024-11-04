// Copyright (c) 2024 Liquid Glass Studios. All rights reserved.

#if UNITY_EDITOR

using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using U_ToolbarExtender;
using UnityEngine.Events;
using UnityEditor.SceneManagement;

namespace Manul.Toolbar
{
	#region ============== Main Scriptable Object & Classes ==============

	#region Classes

	[CreateAssetMenu(fileName = "Manul Toolbar Settings", menuName = "Manul Tools/Manul Toolbar Settings")]
	public class ManulToolbarSettings : ScriptableObject
	{
		public bool settingsExpanded;
		public string currentVersion;
		public ManulToolbarPreferences settings;
		public List<ManulToolbarEntry> leftSide;
		public List<ManulToolbarEntry> rightSide;
	}

	[System.Serializable]
	public class ManulToolbarPreferences
	{
		public float leftBeginOffset;
		public float rightBeginOffset;
		public float betweenOffset;

		public EditorStylesEnum defaultButtonStyle;
		public EditorStylesEnum defaultToggleStyle;
		public EditorStylesEnum defaultLabelStyle;
		public EditorStylesEnum defaultPopupStyle;
		public EditorStylesEnum defaultNumberStyle;
		public EditorStylesEnum defaultTextStyle;

		public bool showConsoleMessages;
		public bool disableToolbar;
		public Color mutedColor;

		public OverrideSideType overrideLeftType;
		public ManulToolbarButtonList SOForLeftSide;
		public ManulToolbarButtonListSet SOSetForLeftSide;

		public OverrideSideType overrideRightType;
		public ManulToolbarButtonList SOForRightSide;
		public ManulToolbarButtonListSet SOSetForRightSide;
	}

	[System.Serializable]
	public class ManulToolbarEntry
	{
		public bool isActive;
		public bool isExpanded;
		public ToolbarEntryType type;

		public ToolbarEntryLabelType labelType;
		public string labelText;
		public Texture2D labelIcon;
		public bool labelIconUsePath;
		public string labelIconPath;

		public bool useStyle;
		public EditorStylesEnum editorStyle;
		public string styleName;
		public GUISkin skin;

		public bool useWidth;
		public float width;

		public bool useColors;
		public Color globalColor = Color.white;
		public Color contentColor = Color.white;
		public Color backgroundColor = Color.white;

		public bool useTooltip;
		public int linesCount;
		public string labelTooltip;

		public string togglePrefName;
		public List<ManulToolbarAction> actions;

		public bool showNameField;

		public ManulToolbarPopupEntry[] intNamesList;

		public bool useOnChangeActions;

		public NumberType numberType;

		public int sliderIntMin;
		public int sliderIntMax;
		public float sliderFloatMin;
		public float sliderFloatMax;

		public OtherType otherType;
		public float defaultSliderValue;
		public int defaultSliderValueInt;
		public bool useOtherLabel;
		public bool useResetButton;
		public int currentFrameRate;

		public ManulToolbarEntry() { }

		public ManulToolbarEntry(string newName, ToolbarButtonType newType, UnityEngine.Object buttonObject, string path, bool useCombo)
		{
			isActive = true;
			isExpanded = true;
			type = ToolbarEntryType.Button;
			labelText = newName;

			actions = new List<ManulToolbarAction>();
			actions.Add(new ManulToolbarAction(ToolbarMouseButton.LMB, newType, buttonObject, path));

			if (useCombo)
			{
				actions.Add(new ManulToolbarAction(ToolbarMouseButton.RMB, ToolbarButtonType.SelectAsset, buttonObject, path));
			}
		}

		public ManulToolbarEntry(string listName)
		{
			isActive = true;
			isExpanded = true;
			type = ToolbarEntryType.List;
			labelText = listName;
			actions = new List<ManulToolbarAction>();
		}
	}

	[System.Serializable]
	public class ManulToolbarButtonListSetEntry
	{
		public string setName;
		public ManulToolbarButtonList setSO;
	}

	[System.Serializable]
	public class ManulToolbarPopupEntry
	{
		public string itemName;
		public int itemIndex;
	}

	[System.Serializable]
	public class ManulToolbarAction
	{
		public ToolbarMouseButton mouseButton;
		public ToolbarKeyboardButton keyboardButton;
		public ToolbarButtonType buttonType;
		public UnityEngine.Object buttonObject;
		public UnityEvent buttonEvent = new UnityEvent();

		public string objectName;
		public string className;
		public string methodName;
		public string listActionName;

		public int isPartOfList;

		public ManulToolbarAction() { }

		public ManulToolbarAction(ToolbarMouseButton mouseType, ToolbarButtonType newButtonType, UnityEngine.Object newButtonObject, string newPath)
		{
			mouseButton = mouseType;
			keyboardButton = ToolbarKeyboardButton.None;
			buttonType = newButtonType;
			buttonObject = newButtonObject;
			className = newPath;
		}

		public ManulToolbarAction(ToolbarButtonType type, UnityEngine.Object newButtonObject, string newPath)
		{
			listActionName = newButtonObject.name;
			buttonType = type;
			buttonObject = newButtonObject;
			className = newPath;
		}
	}

	#endregion

	#region Enums

	public enum ToolbarMouseButton
	{
		LMB,
		RMB,
		MMB
	}

	public enum ToolbarKeyboardButton
	{
		None,
		Ctrl,
		Shift,
		Alt
	}

	public enum ToolbarButtonType
	{
		None,
		OpenAsset,
		SelectAsset,
		ShowAssetInExplorer,
		PropertiesWindow,
		StaticMethod,
		ObjectOfTypeMethod,
		ComponentMethod,
		OpenFolder,
		FindGameobject,
		ExecuteMenuItem,
		InvokeEvent,
		LoadSceneAdditive
	}

	public enum ToolbarEntryType
	{
		None,
		Button,
		Toggle,
		Label,
		Popup,
		Slider,
		Number,
		Text,
		List,
		Other
	}

	public enum ToolbarEntryLabelType
	{
		Text,
		Icon,
		Both,
		None
	}

	public enum ToolbarEntrySideType
	{
		None,
		Left,
		Right
	}

	public enum OverrideSideType
	{
		None,
		List,
		Set
	}

	public enum NumberType
	{
		Float,
		Int
	}

	public enum OtherType
	{
		None,
		TimeScale,
		FrameRate
	}


	#endregion

	#endregion

	#region ================= Manul Toolbar Main Class ===================

	[InitializeOnLoad]
	static class ManulToolbar
	{
		const string currentVersion = "1.4.0";

		#region --------------- Getters ---------------

		static Color tempGlobalColor;
		static Color tempContentColor;
		static Color tempBackgroundColor;

		static ManulToolbarSettings _settings;
		public static ManulToolbarSettings settings
		{
			get
			{
				if (_settings == null)
				{
					VersionCheck();
				}

				if (_settings == null)
				{
					ManulToolbarMessages.ShowMessage(Message.NoSettingsAsset, MessageType.Warning, null);
					return null;
				}

				return _settings;
			}
		}

		static Texture2D _openIconWhite;
		public static Texture2D openIconWhite
		{
			get
			{
				if (_openIconWhite == null)
				{
					_openIconWhite = Resources.Load("open_icon_white") as Texture2D;
				}

				return _openIconWhite;
			}
		}

		static Texture2D _openIconBlack;
		public static Texture2D openIconBlack
		{
			get
			{
				if (_openIconBlack == null)
				{
					_openIconBlack = Resources.Load("open_icon_black") as Texture2D;
				}

				return _openIconBlack;
			}
		}

		static Texture2D _searchIconWhite;
		public static Texture2D searchIconWhite
		{
			get
			{
				if (_searchIconWhite == null)
				{
					_searchIconWhite = Resources.Load("search_icon_white") as Texture2D;
				}

				return _searchIconWhite;
			}
		}

		static Texture2D _searchIconBlack;
		public static Texture2D searchIconBlack
		{
			get
			{
				if (_searchIconBlack == null)
				{
					_searchIconBlack = Resources.Load("search_icon_black") as Texture2D;
				}

				return _searchIconBlack;
			}
		}

		static Texture2D _browseBorder;
		public static Texture2D browseBorder
		{
			get
			{
				if (_browseBorder == null)
				{
					_browseBorder = Resources.Load("browse_border") as Texture2D;
				}

				return _browseBorder;
			}
		}

		#endregion

		#region -------- Main Handling Methods --------

		public static void RefreshToolbar()
		{
			if (settings == null) return;

			ToolbarCallback.RefreshContainers();
		}

		static ManulToolbar()
		{
			EditorApplication.delayCall -= InitializeToolbar;
			EditorApplication.delayCall += InitializeToolbar;
		}

		static void InitializeToolbar()
		{
			ToolbarExtender.LeftToolbarGUI.Add(HandleToolbarLeftSide);
			ToolbarExtender.RightToolbarGUI.Add(HandleToolbarRightSide);
		}

		static void HandleToolbarLeftSide()
		{
			if (settings == null) return;

			HandleToolbarSide(settings.settings.overrideLeftType, settings.settings.leftBeginOffset, settings.settings.SOSetForLeftSide,
					GetEntries(settings.settings.overrideLeftType, settings.leftSide, settings.settings.SOForLeftSide, settings.settings.SOSetForLeftSide, "left", "Left"));
		}

		static void HandleToolbarRightSide()
		{
			if (settings == null) return;

			HandleToolbarSide(settings.settings.overrideRightType, settings.settings.rightBeginOffset, settings.settings.SOSetForRightSide,
				GetEntries(settings.settings.overrideRightType, settings.rightSide, settings.settings.SOForRightSide, settings.settings.SOSetForRightSide, "right", "Right"));
		}

		static List<ManulToolbarEntry> GetEntries(OverrideSideType overrideType, List<ManulToolbarEntry> defaultSide, ManulToolbarButtonList list, ManulToolbarButtonListSet set, string sideName, string sideNameUpper)
		{
			if (settings == null) return null;

			switch (overrideType)
			{
				case OverrideSideType.None:
					return defaultSide;

				case OverrideSideType.List:

					if (list == null)
					{
						ManulToolbarMessages.ShowMessage(Message.NoButtonListAsset, MessageType.Warning, new string[2] { sideName, sideNameUpper });
						return null;
					}

					if (list.buttons == null)
					{
						list.buttons = new List<ManulToolbarEntry>();
					}

					return list.buttons;


				case OverrideSideType.Set:

					if (set == null)
					{
						ManulToolbarMessages.ShowMessage(Message.NoButtonListSetAsset, MessageType.Warning, new string[2] { sideName, sideNameUpper });
						return null;
					}

					if (string.IsNullOrEmpty(set.intPrefName) || set.intPrefName == "")
					{
						ManulToolbarMessages.ShowMessage(Message.EmptyPrefFieldForSet, MessageType.Warning, new string[1] { sideNameUpper });
						return null;
					}

					int currentIndex = EditorPrefs.GetInt(set.intPrefName, 0);

					if (set.listEntries == null)
					{
						set.listEntries = new List<ManulToolbarButtonListSetEntry>();
					}

					if (currentIndex >= set.listEntries.Count)
					{
						ManulToolbarMessages.ShowMessage(Message.NoIndexInButtonListSetAsset, MessageType.Warning, new string[2] { sideNameUpper, currentIndex.ToString() });
						return null;
					}

					if (set.listEntries[currentIndex].setSO == null)
					{
						ManulToolbarMessages.ShowMessage(Message.NoButtonListAssetInButtonListSetAsset, MessageType.Warning, new string[2] { sideNameUpper, currentIndex.ToString() });
						return null;
					}

					if (set.listEntries[currentIndex].setSO.buttons == null)
					{
						set.listEntries[currentIndex].setSO.buttons = new List<ManulToolbarEntry>();
					}

					return set.listEntries[currentIndex].setSO.buttons;
			}

			return null;
		}

		static void HandleToolbarSide(OverrideSideType overrideType, float beginOffset, ManulToolbarButtonListSet set, List<ManulToolbarEntry> entries)
		{
			if (settings == null) return;

			if (settings.settings.disableToolbar) return;

			if (entries == null) return;

			GUILayout.BeginHorizontal();

			GUILayout.Label(new GUIContent(""), EditorStyles.inspectorDefaultMargins, GUILayout.Width(0));

			GUILayout.Space(beginOffset);

			switch (overrideType)
			{
				case OverrideSideType.None:
				case OverrideSideType.List:
					break;

				case OverrideSideType.Set:

					int popup = EditorPrefs.GetInt(set.intPrefName);

					GUI.changed = false;

					GUIContent[] contentNamesList = new GUIContent[set.listEntries.Count];

					for (int i = 0; i < set.listEntries.Count; i++)
					{
						contentNamesList[i] = new GUIContent(set.listEntries[i].setName);
					}

					popup = EditorGUILayout.Popup(GUIContent.none, popup, contentNamesList, EditorStyles.popup, GetGUILayoutOptions(true, set.popupWidth));

					if (GUI.changed)
					{
						EditorPrefs.SetInt(set.intPrefName, popup);

						if (set.useOnChangeValueActions)
						{
							for (int i = 0; i < set.onChangeValueActions.Count; i++)
							{
								System.Action action = CreateAction(set.onChangeValueActions[i], "Button Set (" + set.intPrefName + " int pref)");

								action?.Invoke();
							}				 					
						}
					} 

					break;
			}

			CreateToolbarSide(entries);

			GUILayout.EndHorizontal();
		}

		#endregion

		#region --------- Create Toolbar Side ---------
		static GUIContent GetGUIContent(ManulToolbarEntry entry)
		{
			switch (entry.labelType)
			{
				case ToolbarEntryLabelType.Text:

					return entry.useTooltip ? new GUIContent(entry.labelText, entry.labelTooltip) : new GUIContent(entry.labelText);

				case ToolbarEntryLabelType.Icon:

					if (entry.labelIconUsePath && EditorGUIUtility.IconContent(entry.labelIconPath) != null)
					{
						return entry.useTooltip ? new GUIContent(EditorGUIUtility.IconContent(entry.labelIconPath).image, entry.labelTooltip) : new GUIContent(EditorGUIUtility.IconContent(entry.labelIconPath).image);
					}
					else
					{
						return entry.useTooltip ? new GUIContent(entry.labelIcon, entry.labelTooltip) : new GUIContent(entry.labelIcon);
					}

				case ToolbarEntryLabelType.Both:

					if (entry.labelIconUsePath)
					{
						return entry.useTooltip ? new GUIContent(entry.labelText, EditorGUIUtility.IconContent(entry.labelIconPath).image, entry.labelTooltip) : new GUIContent(entry.labelText, EditorGUIUtility.IconContent(entry.labelIconPath).image);
					}
					else
					{
						return entry.useTooltip ? new GUIContent(entry.labelText, entry.labelIcon, entry.labelTooltip) : new GUIContent(entry.labelText, entry.labelIcon);
					}
			}

			return GUIContent.none;
		}

		static GUILayoutOption[] GetGUILayoutOptions(bool useWidth, float fixedWidth)
		{
			List<GUILayoutOption> options = new List<GUILayoutOption>();

			options.Add(GUILayout.ExpandWidth(false));

			if (useWidth) options.Add(GUILayout.Width(fixedWidth));

			return options.ToArray();
		}

		static void UseColorStart(ManulToolbarEntry entry)
		{
			if (entry.useColors)
			{
				tempGlobalColor = GUI.color;
				tempContentColor = GUI.contentColor;
				tempBackgroundColor = GUI.backgroundColor;

				GUI.color = entry.globalColor;
				GUI.contentColor = entry.contentColor;
				GUI.backgroundColor = entry.backgroundColor;
			}
		}

		static void UseColorEnd(ManulToolbarEntry entry)
		{
			if (entry.useColors)
			{
				GUI.color = tempGlobalColor;
				GUI.contentColor = tempContentColor;
				GUI.backgroundColor = tempBackgroundColor;
			}
		}

		static void CreateButton(ManulToolbarEntry entry)
		{
			GUIContent guiContent = GetGUIContent(entry);

			GUILayoutOption[] layoutOptions = GetGUILayoutOptions(entry.useWidth, entry.width);

			switch (entry.type)
			{
				#region Button

				case ToolbarEntryType.Button:

					if (entry.useStyle)
					{
						switch (entry.editorStyle)
						{
							case EditorStylesEnum.Default:

								if (GUILayout.Button(guiContent, GetEditorStyle.GetStyle(ManulToolbar.settings.settings.defaultButtonStyle), layoutOptions)) PerformButtonAction(entry);

								break;

							case EditorStylesEnum.FindByName:

								if (entry.useStyle && entry.skin != null)
								{
									GUISkin currentSkin = GUI.skin;
									GUI.skin = entry.skin;

									if (GUILayout.Button(guiContent, entry.styleName, layoutOptions)) PerformButtonAction(entry);

									GUI.skin = currentSkin;
								}
								else
								{
									if (GUILayout.Button(guiContent, entry.styleName, layoutOptions)) PerformButtonAction(entry);
								}

								break;

							default:

								if (GUILayout.Button(guiContent, GetEditorStyle.GetStyle(entry.editorStyle), layoutOptions)) PerformButtonAction(entry);

								break;
						}
					}
					else
					{
						if (GUILayout.Button(guiContent, GetEditorStyle.GetStyle(ManulToolbar.settings.settings.defaultButtonStyle), layoutOptions)) PerformButtonAction(entry);
					}

					break;

				#endregion

				#region Toggle

				case ToolbarEntryType.Toggle:

					bool toggle = EditorPrefs.GetBool(entry.togglePrefName);

					GUI.changed = false;

					if (entry.useStyle)
					{
						switch (entry.editorStyle)
						{
							case EditorStylesEnum.Default:

								GUILayout.Toggle(toggle, guiContent, GetEditorStyle.GetStyle(ManulToolbar.settings.settings.defaultToggleStyle), layoutOptions);

								break;

							case EditorStylesEnum.FindByName:

								if (entry.useStyle && entry.skin != null)
								{
									GUISkin currentSkin = GUI.skin;
									GUI.skin = entry.skin;

									GUILayout.Toggle(toggle, guiContent, entry.styleName, layoutOptions);

									GUI.skin = currentSkin;
								}
								else
								{
									GUILayout.Toggle(toggle, guiContent, entry.styleName, layoutOptions);
								}

								break;

							default:

								GUILayout.Toggle(toggle, guiContent, GetEditorStyle.GetStyle(entry.editorStyle), layoutOptions);

								break;
						}
					}
					else
					{
						GUILayout.Toggle(toggle, guiContent, GetEditorStyle.GetStyle(ManulToolbar.settings.settings.defaultToggleStyle), layoutOptions);
					}

					if (GUI.changed)
					{
						EditorPrefs.SetBool(entry.togglePrefName, !toggle);

						PerformOnChangeValueActions(entry);
					}

					break;

				#endregion

				#region Label

				case ToolbarEntryType.Label:

					if (entry.useStyle)
					{
						switch (entry.editorStyle)
						{
							case EditorStylesEnum.Default:

								GUILayout.Label(guiContent, GetEditorStyle.GetStyle(ManulToolbar.settings.settings.defaultLabelStyle), layoutOptions);

								break;

							case EditorStylesEnum.FindByName:

								if (entry.useStyle && entry.skin != null)
								{
									GUISkin currentSkin = GUI.skin;
									GUI.skin = entry.skin;

									GUILayout.Label(guiContent, entry.styleName, layoutOptions);

									GUI.skin = currentSkin;
								}
								else
								{
									GUILayout.Label(guiContent, entry.styleName, layoutOptions);
								}

								break;

							default:

								GUILayout.Label(guiContent, GetEditorStyle.GetStyle(entry.editorStyle), layoutOptions);

								break;
						}
					}
					else
					{
						GUILayout.Label(guiContent, GetEditorStyle.GetStyle(ManulToolbar.settings.settings.defaultLabelStyle), layoutOptions);
					}

					break;

				#endregion

				#region Popup

				case ToolbarEntryType.Popup:

					int popup = EditorPrefs.GetInt(entry.togglePrefName);

					GUI.changed = false;

					GUIContent[] contentNamesList = new GUIContent[entry.intNamesList.Length];
					int[] intList = new int[entry.intNamesList.Length];

					for (int i = 0; i < entry.intNamesList.Length; i++)
					{
						contentNamesList[i] = new GUIContent(entry.intNamesList[i].itemName);
						intList[i] = entry.intNamesList[i].itemIndex;
					}

					if (entry.useStyle)
					{
						switch (entry.editorStyle)
						{
							case EditorStylesEnum.Default:

								popup = EditorGUILayout.IntPopup(GUIContent.none, popup, contentNamesList, intList, GetEditorStyle.GetStyle(ManulToolbar.settings.settings.defaultPopupStyle), layoutOptions);

								break;

							case EditorStylesEnum.FindByName:

								if (entry.useStyle && entry.skin != null)
								{
									GUISkin currentSkin = GUI.skin;
									GUI.skin = entry.skin;

									popup = EditorGUILayout.IntPopup(GUIContent.none, popup, contentNamesList, intList, entry.styleName, layoutOptions);

									GUI.skin = currentSkin;
								}
								else
								{
									popup = EditorGUILayout.IntPopup(GUIContent.none, popup, contentNamesList, intList, entry.styleName, layoutOptions);
								}

								break;

							default:

								popup = EditorGUILayout.IntPopup(GUIContent.none, popup, contentNamesList, intList, GetEditorStyle.GetStyle(entry.editorStyle), layoutOptions);

								break;
						}
					}
					else
					{
						popup = EditorGUILayout.IntPopup(GUIContent.none, popup, contentNamesList, intList, GetEditorStyle.GetStyle(ManulToolbar.settings.settings.defaultPopupStyle), layoutOptions);
					}

					if (GUI.changed)
					{
						EditorPrefs.SetInt(entry.togglePrefName, popup);

						PerformOnChangeValueActions(entry);
					}

					break;

				#endregion

				#region Slider

				case ToolbarEntryType.Slider:

					switch (entry.numberType)
					{
						case NumberType.Float:

							float numberFloat = EditorPrefs.GetFloat(entry.togglePrefName);

							GUI.changed = false;

							numberFloat = EditorGUILayout.Slider(numberFloat, entry.sliderFloatMin, entry.sliderFloatMax, layoutOptions);

							if (GUI.changed)
							{
								EditorPrefs.SetFloat(entry.togglePrefName, numberFloat);

								PerformOnChangeValueActions(entry);
							}

							break;

						case NumberType.Int:

							int numberInt = EditorPrefs.GetInt(entry.togglePrefName);

							GUI.changed = false;

							numberInt = EditorGUILayout.IntSlider(numberInt, entry.sliderIntMin, entry.sliderIntMax, layoutOptions);

							if (GUI.changed)
							{
								EditorPrefs.SetInt(entry.togglePrefName, numberInt);

								PerformOnChangeValueActions(entry);
							}

							break;
					}

					break;

				#endregion

				#region Number

				case ToolbarEntryType.Number:

					switch (entry.numberType)
					{
						case NumberType.Float:

							float numberFloat = EditorPrefs.GetFloat(entry.togglePrefName);

							GUI.changed = false;

							if (entry.useStyle)
							{
								switch (entry.editorStyle)
								{
									case EditorStylesEnum.Default:
										numberFloat = EditorGUILayout.DelayedFloatField(numberFloat, GetEditorStyle.GetStyle(settings.settings.defaultNumberStyle), layoutOptions);
										break;

									case EditorStylesEnum.FindByName:

										if (entry.useStyle && entry.skin != null)
										{
											GUISkin currentSkin = GUI.skin;
											GUI.skin = entry.skin;

											numberFloat = EditorGUILayout.DelayedFloatField(numberFloat, entry.styleName, layoutOptions);

											GUI.skin = currentSkin;
										}
										else
										{
											numberFloat = EditorGUILayout.DelayedFloatField(numberFloat, entry.styleName, layoutOptions);
										}

										break;

									default:

										numberFloat = EditorGUILayout.DelayedFloatField(numberFloat, GetEditorStyle.GetStyle(ManulToolbar.settings.settings.defaultNumberStyle), layoutOptions);
										break;
								}
							}
							else
							{
								numberFloat = EditorGUILayout.DelayedFloatField(numberFloat, GetEditorStyle.GetStyle(settings.settings.defaultNumberStyle), layoutOptions);
							}

							if (GUI.changed)
							{
								EditorPrefs.SetFloat(entry.togglePrefName, numberFloat);

								PerformOnChangeValueActions(entry);
							}

							break;

						case NumberType.Int:

							int numberInt = EditorPrefs.GetInt(entry.togglePrefName);

							GUI.changed = false;

							if (entry.useStyle)
							{
								switch (entry.editorStyle)
								{
									case EditorStylesEnum.Default:
										numberInt = EditorGUILayout.DelayedIntField(numberInt, GetEditorStyle.GetStyle(settings.settings.defaultNumberStyle), layoutOptions);

										break;

									case EditorStylesEnum.FindByName:

										if (entry.useStyle && entry.skin != null)
										{
											GUISkin currentSkin = GUI.skin;
											GUI.skin = entry.skin;

											numberInt = EditorGUILayout.DelayedIntField(numberInt, entry.styleName, layoutOptions);

											GUI.skin = currentSkin;
										}
										else
										{
											numberInt = EditorGUILayout.DelayedIntField(numberInt, entry.styleName, layoutOptions);
										}

										break;

									default:

										numberInt = EditorGUILayout.DelayedIntField(numberInt, GetEditorStyle.GetStyle(ManulToolbar.settings.settings.defaultNumberStyle), layoutOptions);
										break;
								}
							}
							else
							{
								numberInt = EditorGUILayout.DelayedIntField(numberInt, GetEditorStyle.GetStyle(settings.settings.defaultNumberStyle), layoutOptions);
							}

							if (GUI.changed)
							{
								EditorPrefs.SetInt(entry.togglePrefName, numberInt);

								PerformOnChangeValueActions(entry);
							}

							break;
					}

					break;

				#endregion

				#region Text

				case ToolbarEntryType.Text:

					string textValue = EditorPrefs.GetString(entry.togglePrefName);

					GUI.changed = false;

					if (entry.useStyle)
					{
						switch (entry.editorStyle)
						{
							case EditorStylesEnum.Default:
								textValue = EditorGUILayout.DelayedTextField(textValue, GetEditorStyle.GetStyle(settings.settings.defaultTextStyle), layoutOptions);

								break;

							case EditorStylesEnum.FindByName:

								if (entry.useStyle && entry.skin != null)
								{
									GUISkin currentSkin = GUI.skin;
									GUI.skin = entry.skin;

									textValue = EditorGUILayout.DelayedTextField(textValue, entry.styleName, layoutOptions);

									GUI.skin = currentSkin;
								}
								else
								{
									textValue = EditorGUILayout.DelayedTextField(textValue, entry.styleName, layoutOptions);
								}

								break;

							default:

								textValue = EditorGUILayout.DelayedTextField(textValue, GetEditorStyle.GetStyle(ManulToolbar.settings.settings.defaultTextStyle), layoutOptions);
								break;
						}
					}
					else
					{
						textValue = EditorGUILayout.DelayedTextField(textValue, GetEditorStyle.GetStyle(settings.settings.defaultTextStyle), layoutOptions);
					}

					if (GUI.changed)
					{
						EditorPrefs.SetString(entry.togglePrefName, textValue);

						PerformOnChangeValueActions(entry);
					}

					break;

				#endregion

				#region List

				case ToolbarEntryType.List:

					int listIndex = EditorPrefs.GetInt(entry.togglePrefName);

					GUI.changed = false;

					List<GUIContent> actionsNamesList = new List<GUIContent>();
					List<int> actionsIntList = new List<int>();

					if (entry.actions != null)
					{
						if (entry.actions.Count > 0)
						{
							actionsNamesList.Add(new GUIContent(entry.actions[0].listActionName));
							actionsIntList.Add(0);

							for (int i = 1; i < entry.actions.Count; i++)
							{
								bool shouldAdd = true;

								for (int j = 0; j < actionsNamesList.Count; j++)
								{
									if (actionsNamesList[j].text == entry.actions[i].listActionName)
									{
										shouldAdd = false;
										break;
									}
								}

								if (!shouldAdd) continue;

								actionsNamesList.Add(new GUIContent(entry.actions[i].listActionName));
								actionsIntList.Add(i);
							}
						}
					}

					if (entry.useStyle)
					{
						switch (entry.editorStyle)
						{
							case EditorStylesEnum.Default:

								listIndex = EditorGUILayout.IntPopup(GUIContent.none, listIndex, actionsNamesList.ToArray(), actionsIntList.ToArray(), GetEditorStyle.GetStyle(ManulToolbar.settings.settings.defaultPopupStyle), layoutOptions);

								break;

							case EditorStylesEnum.FindByName:

								if (entry.useStyle && entry.skin != null)
								{
									GUISkin currentSkin = GUI.skin;
									GUI.skin = entry.skin;

									listIndex = EditorGUILayout.IntPopup(GUIContent.none, listIndex, actionsNamesList.ToArray(), actionsIntList.ToArray(), entry.styleName, layoutOptions);

									GUI.skin = currentSkin;
								}
								else
								{
									listIndex = EditorGUILayout.IntPopup(GUIContent.none, listIndex, actionsNamesList.ToArray(), actionsIntList.ToArray(), entry.styleName, layoutOptions);
								}

								break;

							default:

								listIndex = EditorGUILayout.IntPopup(GUIContent.none, listIndex, actionsNamesList.ToArray(), actionsIntList.ToArray(), GetEditorStyle.GetStyle(entry.editorStyle), layoutOptions);

								break;
						}
					}
					else
					{
						listIndex = EditorGUILayout.IntPopup(GUIContent.none, listIndex, actionsNamesList.ToArray(), actionsIntList.ToArray(), GetEditorStyle.GetStyle(ManulToolbar.settings.settings.defaultPopupStyle), layoutOptions);
					}

					if (GUI.changed)
					{
						EditorPrefs.SetInt(entry.togglePrefName, listIndex);

						for (int i = 0; i < entry.actions.Count; i++)
						{
							if (entry.actions[listIndex].listActionName == entry.actions[i].listActionName)
							{
								System.Action action = CreateAction(entry.actions[i], entry.labelText);

								action?.Invoke();
							}

						}
					}

					break;

				#endregion

				#region Other

				case ToolbarEntryType.Other:

					switch (entry.otherType)
					{
						case OtherType.TimeScale:

							if (entry.sliderFloatMin < 0) entry.sliderFloatMin = 0f;
							if (entry.sliderFloatMax > 100f) entry.sliderFloatMax = 100f;

							entry.defaultSliderValue = Mathf.Clamp(entry.defaultSliderValue, 0f, 100f);

							if (entry.useOtherLabel)
							{
								GUILayout.Label(guiContent, GUILayout.ExpandWidth(false));
							}

							GUI.changed = false;

							Time.timeScale = EditorGUILayout.Slider(Time.timeScale, entry.sliderFloatMin, entry.sliderFloatMax, layoutOptions);

							if (entry.useResetButton)
							{
								if (GUILayout.Button("Reset", GUILayout.ExpandWidth(false)))
								{
									Time.timeScale = entry.defaultSliderValue;
								}
							}

							if (GUI.changed)
							{
								PerformOnChangeValueActions(entry);
							}

							break;

						case OtherType.FrameRate:

							if (EditorApplication.isPlaying)
							{
								if (Application.targetFrameRate != entry.currentFrameRate)
								{
									entry.currentFrameRate = Application.targetFrameRate;
								}
							}

							if (entry.sliderIntMin < 0) entry.sliderIntMin = 0;
							if (entry.defaultSliderValueInt < 0) entry.defaultSliderValueInt = 0;

							if (entry.useOtherLabel)
							{
								GUILayout.Label(guiContent, GUILayout.ExpandWidth(false));
							}

							GUI.changed = false;

							entry.currentFrameRate = EditorGUILayout.IntSlider(entry.currentFrameRate, entry.sliderIntMin, entry.sliderIntMax, layoutOptions);

							if (entry.useResetButton)
							{
								if (GUILayout.Button("Reset", GUILayout.ExpandWidth(false)))
								{
									entry.currentFrameRate = entry.defaultSliderValueInt;

									if (EditorApplication.isPlaying)
									{
										if (Application.targetFrameRate != entry.currentFrameRate)
										{
											Application.targetFrameRate = entry.currentFrameRate;
										}
									}
								}
							}

							if (GUI.changed)
							{
								if (EditorApplication.isPlaying)
								{
									if (Application.targetFrameRate != entry.currentFrameRate)
									{
										Application.targetFrameRate = entry.currentFrameRate;
									}
								}

								PerformOnChangeValueActions(entry);
							}
							break;
					}

					break;

					#endregion

			}
		}

		static void CreateToolbarSide(List<ManulToolbarEntry> entries)
		{
			foreach (var entry in entries)
			{
				if (!entry.isActive) continue;

				if (entry.type == ToolbarEntryType.None) continue;

				UseColorStart(entry);

				if (entry.showNameField)
				{
					entry.labelText = GUILayout.TextField(entry.labelText, GUILayout.MaxWidth(150));

					if (GUILayout.Button("OK", GUILayout.Width(27), GUILayout.MinWidth(27), GUILayout.MaxWidth(27)))
					{
						entry.showNameField = false;

						EditorUtility.SetDirty(settings);

						switch (settings.settings.overrideLeftType)
						{
							case OverrideSideType.List:
								if (settings.settings.SOForLeftSide != null) EditorUtility.SetDirty(settings.settings.SOForLeftSide);
								break;

							case OverrideSideType.Set:
								if (settings.settings.SOSetForLeftSide != null) EditorUtility.SetDirty(settings.settings.SOSetForLeftSide);
								break;
						}

						switch (settings.settings.overrideRightType)
						{
							case OverrideSideType.List:
								if (settings.settings.SOForRightSide != null) EditorUtility.SetDirty(settings.settings.SOForRightSide);
								break;

							case OverrideSideType.Set:
								if (settings.settings.SOSetForRightSide != null) EditorUtility.SetDirty(settings.settings.SOSetForRightSide);
								break;
						}
					}
				}
				else
				{
					CreateButton(entry);
				}

				GUILayout.Space(settings.settings.betweenOffset);

				UseColorEnd(entry);
			}
		}

		#endregion

		#region ------- Create & Perform Action -------

		static void PerformOnChangeValueActions(ManulToolbarEntry entry)
		{
			if (!entry.useOnChangeActions) return;

			for (int i = 0; i < entry.actions.Count; i++)
			{
				System.Action action = CreateAction(entry.actions[i], entry.labelText);

				action?.Invoke();
			}
		}

		static void PerformButtonAction(ManulToolbarEntry entry)
		{
			Event E = Event.current;

			if (E.button == 0 && E.control && E.shift && !E.alt)
			{
				entry.showNameField = true;
			}
			else if (E.button == 0 && E.control && E.shift && E.alt)
			{
				entry.isActive = false;
			}
			else
			{
				foreach (var a in entry.actions)
				{
					System.Action action = CreateAction(a, entry.labelText);

					if ((E.button == 0 && a.mouseButton == ToolbarMouseButton.LMB) ||
						(E.button == 1 && a.mouseButton == ToolbarMouseButton.RMB) ||
						(E.button == 2 && a.mouseButton == ToolbarMouseButton.MMB))
					{
						if (a.keyboardButton == ToolbarKeyboardButton.None)
						{
							if (!E.control && !E.shift && !E.alt)
							{
								action?.Invoke();
							}
						}
						else
						{
							if ((E.control && a.keyboardButton == ToolbarKeyboardButton.Ctrl) ||
								(E.shift && a.keyboardButton == ToolbarKeyboardButton.Shift) ||
								(E.alt && a.keyboardButton == ToolbarKeyboardButton.Alt))
							{
								action?.Invoke();
							}
						}
					}
				}
			}

			E.Use();
		}

		static System.Action CreateAction(ManulToolbarAction toolbarAction, string buttonName)
		{
			BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

			switch (toolbarAction.buttonType)
			{
				#region Open Asset

				case ToolbarButtonType.OpenAsset:

					return () =>
					{
						if (toolbarAction.buttonObject == null)
						{
							ManulToolbarMessages.ShowMessage(Message.NoObjectForButton, MessageType.Error, new string[] { "Open Asset", buttonName });
							return;
						}

						AssetDatabase.OpenAsset(toolbarAction.buttonObject);
						SceneView.FrameLastActiveSceneView();
					};

				#endregion

				#region Select Asset

				case ToolbarButtonType.SelectAsset:

					return () =>
					{
						if (toolbarAction.buttonObject == null)
						{
							ManulToolbarMessages.ShowMessage(Message.NoObjectForButton, MessageType.Error, new string[] { "Select Asset", buttonName });
							return;
						}

						EditorGUIUtility.PingObject(toolbarAction.buttonObject);
						Selection.activeObject = toolbarAction.buttonObject;
					};

				#endregion

				#region Show Asset In Explorer

				case ToolbarButtonType.ShowAssetInExplorer:

					return () =>
					{
						if (toolbarAction.buttonObject == null)
						{
							ManulToolbarMessages.ShowMessage(Message.NoObjectForButton, MessageType.Error, new string[] { "Show Asset In Explorer", buttonName });
							return;
						}

						EditorUtility.RevealInFinder(AssetDatabase.GetAssetPath(toolbarAction.buttonObject));
					};

				#endregion

				#region Open Properties Window

				case ToolbarButtonType.PropertiesWindow:

					return () =>
					{
						if (toolbarAction.buttonObject == null)
						{
							ManulToolbarMessages.ShowMessage(Message.NoObjectForButton, MessageType.Error, new string[] { "Properties Window", buttonName });
							return;
						}

#if UNITY_2021_1_OR_NEWER
						EditorUtility.OpenPropertyEditor(toolbarAction.buttonObject);
#else
						EditorGUIUtility.PingObject(toolbarAction.buttonObject);
#endif
					};

				#endregion

				#region Open Folder

				case ToolbarButtonType.OpenFolder:

					return () =>
					{
						UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(toolbarAction.className);

						if (obj == null)
						{
							ManulToolbarMessages.ShowMessage(Message.CantFindFolder, MessageType.Error, new string[] { toolbarAction.className, buttonName });
							return;
						}

						ShowFolder(obj.GetInstanceID());
					};

				#endregion

				#region Find and Select GameObject In Scene

				case ToolbarButtonType.FindGameobject:

					return () =>
					{
						GameObject GO = GameObject.Find(toolbarAction.className);

						if (GO == null)
						{
							ManulToolbarMessages.ShowMessage(Message.CantFindGameObject, MessageType.Error, new string[] { toolbarAction.className, buttonName });
							return;
						}

						Selection.activeObject = GO;
					};

				#endregion

				#region Invoke a Static Method

				case ToolbarButtonType.StaticMethod:

					return () =>
					{
						var actionClass = System.AppDomain.CurrentDomain.GetClass(toolbarAction.className);

						if (actionClass == null)
						{
							ManulToolbarMessages.ShowMessage(Message.CantFindClass, MessageType.Error, new string[] { toolbarAction.className, buttonName, "Static Method" });
							return;
						}

						var method = actionClass.GetMethod(toolbarAction.methodName, bindingFlags);

						if (method == null)
						{
							ManulToolbarMessages.ShowMessage(Message.CantFindMethod, MessageType.Error, new string[] { toolbarAction.className, buttonName, "Static Method", toolbarAction.methodName });
							return;
						}

						method.Invoke(null, null);
					};

				#endregion

				#region Invoke a Method from Component attached to a GameObject

				case ToolbarButtonType.ComponentMethod:

					return () =>
					{
						GameObject GO = GameObject.Find(toolbarAction.objectName);

						if (GO == null)
						{
							ManulToolbarMessages.ShowMessage(Message.CantFindGameObject, MessageType.Error, new string[] { toolbarAction.objectName, buttonName, "Component Method" });
							return;
						}

						var actionClass = System.AppDomain.CurrentDomain.GetClass(toolbarAction.className);

						if (actionClass == null)
						{
							ManulToolbarMessages.ShowMessage(Message.CantFindClass, MessageType.Error, new string[] { toolbarAction.className, buttonName, "Component Method" });
							return;
						}

						UnityEngine.Object obj = GO.GetComponent(actionClass);

						if (obj == null)
						{
							ManulToolbarMessages.ShowMessage(Message.NoComponent, MessageType.Error, new string[] { toolbarAction.className, buttonName, "Component Method" });
							return;
						}

						var method = actionClass.GetMethod(toolbarAction.methodName, bindingFlags);

						if (method == null)
						{
							ManulToolbarMessages.ShowMessage(Message.CantFindMethod, MessageType.Error, new string[] { toolbarAction.className, buttonName, "Component Method", toolbarAction.methodName });
							return;
						}

						obj.GetType().GetMethod(toolbarAction.methodName, bindingFlags).Invoke(obj, null);
					};

				#endregion

				#region Invoke a Method from Object found by Type

				case ToolbarButtonType.ObjectOfTypeMethod:

					return () =>
					{
						var actionClass = System.AppDomain.CurrentDomain.GetClass(toolbarAction.className);

						if (actionClass == null)
						{
							ManulToolbarMessages.ShowMessage(Message.CantFindClass, MessageType.Error, new string[] { toolbarAction.className, buttonName, "Object Of Type Method" });
							return;
						}

						UnityEngine.Object[] objects = UnityEngine.Object.FindObjectsOfType(actionClass, true);

						if (objects.Length < 1)
						{
							ManulToolbarMessages.ShowMessage(Message.NoTypeOnScene, MessageType.Error, new string[] { toolbarAction.className, buttonName, "Object Of Type Method", toolbarAction.methodName });
							return;
						}

						if (objects.Length > 1)
						{
							ManulToolbarMessages.ShowMessage(Message.MoreThanOneTypeOnScene, MessageType.Error, new string[] { toolbarAction.className, buttonName, "Object Of Type Method", toolbarAction.methodName });
							return;
						}

						var method = objects[0].GetType().GetMethod(toolbarAction.methodName, bindingFlags);

						if (method == null)
						{
							ManulToolbarMessages.ShowMessage(Message.CantFindMethod, MessageType.Error, new string[] { toolbarAction.className, buttonName, "Object Of Type Method", toolbarAction.methodName });
							return;
						}

						method.Invoke(objects[0], null);
					};

				#endregion

				#region Execute Menu Item

				case ToolbarButtonType.ExecuteMenuItem:

					return () =>
					{
						EditorApplication.ExecuteMenuItem(toolbarAction.className); 
					};

				#endregion

				#region Invoke Event

				case ToolbarButtonType.InvokeEvent:

					return () =>
					{
						if (toolbarAction.buttonEvent.GetPersistentEventCount() < 1)
						{
							ManulToolbarMessages.ShowMessage(Message.ZeroEvents, MessageType.Error, new string[] { "Invoke Event", buttonName });
							return;
						}

						toolbarAction.buttonEvent?.Invoke();
					};

				#endregion

				#region Load Scene Additive

				case ToolbarButtonType.LoadSceneAdditive:

					return () =>
					{
						if (toolbarAction.buttonObject == null)
						{
							ManulToolbarMessages.ShowMessage(Message.NoSceneObject, MessageType.Error, new string[] { "Open Scene Additive", buttonName });
							return;
						}

						SceneAsset scene = (SceneAsset)toolbarAction.buttonObject;

						if (scene == null)
						{
							ManulToolbarMessages.ShowMessage(Message.InvalidSceneObject, MessageType.Error, new string[] { "Open Scene Additive", buttonName });
							return;
						}

						EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(toolbarAction.buttonObject), OpenSceneMode.Additive);
					};

				#endregion

				default:

					return null;
			}
		}

		#endregion

		#region ----------- Helper Methods ------------

		static System.Type GetClass(this System.AppDomain aAppDomain, string name)
		{
			Assembly[] assemblies = aAppDomain.GetAssemblies();
			foreach (var A in assemblies)
			{
				Type[] types = A.GetTypes();
				foreach (var T in types)
				{
					if (T.FullName == name) return T;
				}
			}
			return null;
		}

		static void ShowFolder(int folderID)
		{
			System.Type projectWindowType = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");

			MethodInfo showFolderContents = projectWindowType.GetMethod("ShowFolderContents", BindingFlags.Instance | BindingFlags.NonPublic);

			UnityEngine.Object[] projectWindows = UnityEngine.Resources.FindObjectsOfTypeAll(projectWindowType);

			if (projectWindows.Length > 0)
			{
				for (int i = 0; i < projectWindows.Length; i++)
				{
					ShowFolderInternal(projectWindows[i], showFolderContents, folderID);
				}
			}
			else
			{
				EditorWindow projectWindow = EditorWindow.GetWindow(projectWindowType);

				projectWindow.Show();

				MethodInfo initMethod = projectWindowType.GetMethod("Init", BindingFlags.Instance | BindingFlags.Public);

				initMethod.Invoke(projectWindow, null);

				ShowFolderInternal(projectWindow, showFolderContents, folderID);
			}
		}

		static void ShowFolderInternal(UnityEngine.Object projectWindow, MethodInfo showContents, int folderID)
		{
			bool isTwoColumns = new SerializedObject(projectWindow).FindProperty("m_ViewMode").enumValueIndex == 1;

			if (!isTwoColumns)
			{
				MethodInfo setTwoColumns = projectWindow.GetType().GetMethod("SetTwoColumns", BindingFlags.Instance | BindingFlags.NonPublic);

				setTwoColumns.Invoke(projectWindow, null);
			}

			showContents.Invoke(projectWindow, new object[] { folderID, true });
		}

		#endregion

		#region -------- Create Button / List ---------

		public static void CreateList(ToolbarEntrySideType side, ToolbarButtonType type)
		{
			if (Selection.objects == null) return;

			if (Selection.objects.Length < 1) return;

			ManulToolbarEntry entry = new ManulToolbarEntry("List");

			for (int i = 0; i < Selection.objects.Length; i++)
			{
				switch (type)
				{
					case ToolbarButtonType.OpenAsset:
					case ToolbarButtonType.SelectAsset:
					case ToolbarButtonType.ShowAssetInExplorer:
					case ToolbarButtonType.PropertiesWindow:
					case ToolbarButtonType.OpenFolder:
					case ToolbarButtonType.LoadSceneAdditive:

						string assetPath = AssetDatabase.GetAssetPath(Selection.objects[i]);

						string[] splitPath = assetPath.Split("/");

						string folderPath = "";

						for (int j = 0; j < splitPath.Length - 1; j++)
						{
							folderPath += splitPath[j] + "/";
						}

						folderPath = folderPath.Substring(0, folderPath.Length - 1);

						string[] fullAssetName = splitPath[splitPath.Length - 1].Split(".");

						string assetName = fullAssetName[0];

						if (type == ToolbarButtonType.OpenFolder)
						{
							assetName = splitPath[splitPath.Length - 2];
						}

						entry.actions.Add(new ManulToolbarAction(type, Selection.objects[i], folderPath));

						break;

					case ToolbarButtonType.FindGameobject:

						if (Selection.gameObjects[i] != null)
						{
							string gameObjectName = Selection.gameObjects[0].name;

							entry.actions.Add(new ManulToolbarAction(type, Selection.objects[i], gameObjectName)); 
						}

						break;
				} 

			}

			if (entry == null) return;

			AddEntry(entry, side);
		}


		public static void CreateButton(ToolbarEntrySideType side, ToolbarButtonType type, bool useCombo = false)
		{
			if (Selection.objects == null) return;

			if (Selection.objects.Length < 1) return;

			for (int i = 0; i < Selection.objects.Length; i++)
			{
				CreateButtonInternal(Selection.objects[i], side, type, useCombo);
			}
		}

		public static void CreateButtonInternal(UnityEngine.Object selectedObject, ToolbarEntrySideType side, ToolbarButtonType type, bool useCombo)
		{
			ManulToolbarEntry entry = null;

			switch (type)
			{
				case ToolbarButtonType.OpenAsset:
				case ToolbarButtonType.SelectAsset:
				case ToolbarButtonType.ShowAssetInExplorer:
				case ToolbarButtonType.PropertiesWindow:
				case ToolbarButtonType.OpenFolder:
				case ToolbarButtonType.LoadSceneAdditive:

					string assetPath = AssetDatabase.GetAssetPath(selectedObject);

					string[] splitPath = assetPath.Split("/");

					string folderPath = "";

					for (int i = 0; i < splitPath.Length - 1; i++)
					{
						folderPath += splitPath[i] + "/";
					}

					folderPath = folderPath.Substring(0, folderPath.Length - 1);

					string[] fullAssetName = splitPath[splitPath.Length - 1].Split(".");

					string assetName = fullAssetName[0];

					if (type == ToolbarButtonType.OpenFolder)
					{
						assetName = splitPath[splitPath.Length - 2];
					}

					entry = new ManulToolbarEntry(assetName, type, selectedObject, folderPath, useCombo);

					break;

				case ToolbarButtonType.FindGameobject:

					if (Selection.activeGameObject != null)
					{
						string gameObjectName = Selection.activeGameObject.name;

						entry = new ManulToolbarEntry(gameObjectName, type, selectedObject, gameObjectName, false);
					}

					break;
			}

			if (entry == null) return;

			AddEntry(entry, side);
		}

		static void AddEntry(ManulToolbarEntry entry, ToolbarEntrySideType side)
		{
			List<ManulToolbarEntry> entryList = null;

			switch (side)
			{
				case ToolbarEntrySideType.Left:
					entryList = GetEntries(settings.settings.overrideLeftType, settings.leftSide, settings.settings.SOForLeftSide, settings.settings.SOSetForLeftSide, "left", "Left");
					break;

				case ToolbarEntrySideType.Right:
					entryList = GetEntries(settings.settings.overrideRightType, settings.rightSide, settings.settings.SOForRightSide, settings.settings.SOSetForRightSide, "right", "Right");
					break;
			}

			if (entryList != null)
			{
				entryList.Add(entry);

				switch (side)
				{
					case ToolbarEntrySideType.Left:

						switch (settings.settings.overrideLeftType)
						{
							case OverrideSideType.None:
								EditorUtility.SetDirty(settings);
								break;

							case OverrideSideType.List:
								EditorUtility.SetDirty(settings.settings.SOForLeftSide);
								break;

							case OverrideSideType.Set:
								int currentIndex = EditorPrefs.GetInt(settings.settings.SOSetForLeftSide.intPrefName, 0);
								EditorUtility.SetDirty(settings.settings.SOSetForLeftSide.listEntries[currentIndex].setSO);
								break;
						}

						break;

					case ToolbarEntrySideType.Right:

						switch (settings.settings.overrideLeftType)
						{
							case OverrideSideType.None:
								EditorUtility.SetDirty(settings);
								break;

							case OverrideSideType.List:
								EditorUtility.SetDirty(settings.settings.SOForRightSide);
								break;

							case OverrideSideType.Set:
								int currentIndex = EditorPrefs.GetInt(settings.settings.SOSetForRightSide.intPrefName, 0);
								EditorUtility.SetDirty(settings.settings.SOSetForRightSide.listEntries[currentIndex].setSO);
								break;
						}

						break;
				}
			}
		}





		#endregion

		#region ------------ Version Check ------------

		static void VersionCheck()
		{
			/// Check if new version exists

			_settings = Resources.Load("Manul Toolbar") as ManulToolbarSettings;

			if (_settings != null)
			{
				if (string.IsNullOrEmpty(_settings.currentVersion) || _settings.currentVersion != currentVersion)
				{
					if (EditorUtility.DisplayDialog(ManulToolbarMessages.GetModal(Modal.UppgrdeTitle), ManulToolbarMessages.GetModal(Modal.UppgrdeInfo, new string[] { currentVersion }), "OK")) { }

					_settings.currentVersion = currentVersion;
					EditorUtility.SetDirty(settings);
				}
			}

			/// There is no 'new' Manul Toolbar Settings

			else
			{
				ManulToolbarSettings newSettings = ScriptableObject.CreateInstance<ManulToolbarSettings>();

				newSettings.currentVersion = currentVersion;

				ManulToolbarSettings settingsOldVersion = Resources.Load("ManulToolbar") as ManulToolbarSettings;

				/// Installed version 1.3.1 or above

				if (settingsOldVersion != null)
				{
					EditorUtility.CopySerialized(settingsOldVersion, newSettings);
				}

				/// Installed first time

				else
				{
					ManulToolbarSettings settingsDefault = Resources.Load("Manul Toolbar Settings (Default)") as ManulToolbarSettings;

					if (settingsDefault == null)
					{
						if (!EditorPrefs.GetBool("PleaseReimportMessage"))
						{
							EditorPrefs.SetBool("PleaseReimportMessage", true);

							if (EditorUtility.DisplayDialog(ManulToolbarMessages.GetModal(Modal.ReimportTitle), ManulToolbarMessages.GetModal(Modal.ReimportInfo), "OK")) { }
						}

						return;
					}

					EditorUtility.CopySerialized(settingsDefault, newSettings);
				}

				if (!AssetDatabase.IsValidFolder("Assets/Resources"))
				{
					AssetDatabase.CreateFolder("Assets", "Resources");
				}

				AssetDatabase.CreateAsset(newSettings, "Assets/Resources/Manul Toolbar.asset");
				AssetDatabase.SaveAssetIfDirty(newSettings);

				if (settingsOldVersion != null)
				{
					if (EditorUtility.DisplayDialog(ManulToolbarMessages.GetModal(Modal.WelcomeTitle), ManulToolbarMessages.GetModal(Modal.PreviousInfo), "Yes", "No"))
					{
						string oldSettingsPath = AssetDatabase.GetAssetPath(settingsOldVersion);
						AssetDatabase.DeleteAsset(oldSettingsPath);
					}

					if (EditorUtility.DisplayDialog(ManulToolbarMessages.GetModal(Modal.ImportantTitle), ManulToolbarMessages.GetModal(Modal.ImportantInfo), "OK")) { }
				}
				else
				{
					if (EditorUtility.DisplayDialog(ManulToolbarMessages.GetModal(Modal.WelcomeTitle), ManulToolbarMessages.GetModal(Modal.ImportantInfo2), "OK")) { }
				}

				_settings = Resources.Load("Manul Toolbar") as ManulToolbarSettings;
			}
		}

		public static void FindManulToolbar()
		{
			if (settings == null) return;

			GetManulToolbar().actions[1].buttonObject = settings;
		}

		public static ManulToolbarEntry GetManulToolbar()
		{
			if (settings == null) return null;

			int toolbarIndex = -1;

			ToolbarEntrySideType side = ToolbarEntrySideType.None;

			if (settings.rightSide != null)
			{
				for (int i = 0; i < settings.rightSide.Count; i++)
				{
					ManulToolbarEntry entry = settings.rightSide[i];

					if (entry.type == ToolbarEntryType.Button && entry.labelText == "Toolbar" && entry.actions != null)
					{
						if (entry.actions.Count == 2)
						{
							if (entry.actions[0].buttonType == ToolbarButtonType.StaticMethod &&
								(entry.actions[0].className == "ManulToolbar" || entry.actions[0].className == "Manul.Toolbar.ManulToolbar") &&
								entry.actions[0].methodName == "FindManulToolbar" &&
								entry.actions[1].buttonType == ToolbarButtonType.PropertiesWindow)
							{
								side = ToolbarEntrySideType.Right;
								toolbarIndex = i;
							}
						}
					}
				}
			}

			if (toolbarIndex < 0 && settings.leftSide != null)
			{
				for (int i = 0; i < settings.leftSide.Count; i++)
				{
					ManulToolbarEntry entry = settings.leftSide[i];

					if (entry.type == ToolbarEntryType.Button && entry.labelText == "Toolbar" && entry.actions != null)
					{
						if (entry.actions.Count == 2)
						{
							if (entry.actions[0].buttonType == ToolbarButtonType.StaticMethod &&
								(entry.actions[0].className == "ManulToolbar" || entry.actions[0].className == "Manul.Toolbar.ManulToolbar") &&
								entry.actions[0].methodName == "FindManulToolbar" &&
								entry.actions[1].buttonType == ToolbarButtonType.PropertiesWindow)
							{
								side = ToolbarEntrySideType.Left;
								toolbarIndex = i;
							}
						}
					}
				}
			}

			if (toolbarIndex > -1)
			{
				switch (side)
				{
					case ToolbarEntrySideType.Left:
						settings.leftSide[toolbarIndex].actions[0].className = "Manul.Toolbar.ManulToolbar";
						return settings.leftSide[toolbarIndex];

					case ToolbarEntrySideType.Right:
						settings.rightSide[toolbarIndex].actions[0].className = "Manul.Toolbar.ManulToolbar";
						return settings.rightSide[toolbarIndex];
				}
			}

			return null;
		}

		#endregion
	}

	#endregion

	#region ======================= Editor Styles ========================

	public enum EditorStylesEnum
	{
		Default,
		FindByName,
		boldLabel,
		centeredGreyMiniLabel,
		colorField,
		foldout,
		foldoutHeader,
		foldoutHeaderIcon,
		foldoutPreDrop,
		helpBox,
		iconButton,
		inspectorDefaultMargins,
		inspectorFullWidthMargins,
		label,
		largeLabel,
		layerMaskField,
		linkLabel,
		miniBoldLabel,
		miniButton,
		miniButtonLeft,
		miniButtonMid,
		miniButtonRight,
		miniLabel,
		miniPullDown,
		miniTextField,
		numberField,
		objectField,
		objectFieldMiniThumb,
		objectFieldThumb,
		popup,
		radioButton,
		selectionRect,
		textArea,
		textField,
		toggle,
		toggleGroup,
		toolbar,
		toolbarButton,
		toolbarDropDown,
		toolbarPopup,
		toolbarSearchField,
		toolbarTextField,
		whiteBoldLabel,
		whiteLabel,
		whiteLargeLabel,
		whiteMiniLabel,
		wordWrappedLabel,
		wordWrappedMiniLabel,
	}
	public static class GetEditorStyle
	{
		public static GUIStyle GetStyle(EditorStylesEnum style)
		{
			switch (style)
			{
				case EditorStylesEnum.boldLabel: return EditorStyles.boldLabel;
				case EditorStylesEnum.centeredGreyMiniLabel: return EditorStyles.centeredGreyMiniLabel;
				case EditorStylesEnum.colorField: return EditorStyles.colorField;
				case EditorStylesEnum.foldout: return EditorStyles.foldout;
				case EditorStylesEnum.foldoutHeader: return EditorStyles.foldoutHeader;
				case EditorStylesEnum.foldoutHeaderIcon: return EditorStyles.foldoutHeaderIcon;
				case EditorStylesEnum.foldoutPreDrop: return EditorStyles.foldoutPreDrop;
				case EditorStylesEnum.helpBox: return EditorStyles.helpBox;

#if UNITY_2021_1_OR_NEWER
				case EditorStylesEnum.iconButton: return EditorStyles.iconButton;
#endif

				case EditorStylesEnum.inspectorDefaultMargins: return EditorStyles.inspectorDefaultMargins;
				case EditorStylesEnum.inspectorFullWidthMargins: return EditorStyles.inspectorFullWidthMargins;
				case EditorStylesEnum.label: return EditorStyles.label;
				case EditorStylesEnum.largeLabel: return EditorStyles.largeLabel;
				case EditorStylesEnum.layerMaskField: return EditorStyles.layerMaskField;

#if UNITY_2020_1_OR_NEWER
				case EditorStylesEnum.linkLabel: return EditorStyles.linkLabel;
#endif
				case EditorStylesEnum.miniBoldLabel: return EditorStyles.miniBoldLabel;
				case EditorStylesEnum.miniButton: return EditorStyles.miniButton;
				case EditorStylesEnum.miniButtonLeft: return EditorStyles.miniButtonLeft;
				case EditorStylesEnum.miniButtonMid: return EditorStyles.miniButtonMid;
				case EditorStylesEnum.miniButtonRight: return EditorStyles.miniButtonRight;
				case EditorStylesEnum.miniLabel: return EditorStyles.miniLabel;
				case EditorStylesEnum.miniPullDown: return EditorStyles.miniPullDown;
				case EditorStylesEnum.miniTextField: return EditorStyles.miniTextField;
				case EditorStylesEnum.numberField: return EditorStyles.numberField;
				case EditorStylesEnum.objectField: return EditorStyles.objectField;
				case EditorStylesEnum.objectFieldMiniThumb: return EditorStyles.objectFieldMiniThumb;
				case EditorStylesEnum.objectFieldThumb: return EditorStyles.objectFieldThumb;
				case EditorStylesEnum.popup: return EditorStyles.popup;
				case EditorStylesEnum.radioButton: return EditorStyles.radioButton;

#if UNITY_2021_1_OR_NEWER
				case EditorStylesEnum.selectionRect: return EditorStyles.selectionRect;
#endif
				case EditorStylesEnum.textArea: return EditorStyles.textArea;
				case EditorStylesEnum.textField: return EditorStyles.textField;
				case EditorStylesEnum.toggle: return EditorStyles.toggle;
				case EditorStylesEnum.toggleGroup: return EditorStyles.toggleGroup;
				case EditorStylesEnum.toolbar: return EditorStyles.toolbar;
				case EditorStylesEnum.toolbarButton: return EditorStyles.toolbarButton;
				case EditorStylesEnum.toolbarDropDown: return EditorStyles.toolbarDropDown;
				case EditorStylesEnum.toolbarPopup: return EditorStyles.toolbarPopup;
				case EditorStylesEnum.toolbarSearchField: return EditorStyles.toolbarSearchField;
				case EditorStylesEnum.toolbarTextField: return EditorStyles.toolbarTextField;
				case EditorStylesEnum.whiteBoldLabel: return EditorStyles.whiteBoldLabel;
				case EditorStylesEnum.whiteLabel: return EditorStyles.whiteLabel;
				case EditorStylesEnum.whiteLargeLabel: return EditorStyles.whiteLargeLabel;
				case EditorStylesEnum.whiteMiniLabel: return EditorStyles.whiteMiniLabel;
				case EditorStylesEnum.wordWrappedLabel: return EditorStyles.wordWrappedLabel;
				case EditorStylesEnum.wordWrappedMiniLabel: return EditorStyles.wordWrappedMiniLabel;

				default: return EditorStyles.label;
			}
		}
	}

	#endregion
}

#endif