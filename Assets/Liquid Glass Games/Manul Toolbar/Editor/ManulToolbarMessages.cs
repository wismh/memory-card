// Copyright (c) 2024 Liquid Glass Studios. All rights reserved.

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Manul.Toolbar
{
	public enum Message
	{
		None,
		NoSettingsAsset,
		NoButtonListAsset,
		NoButtonListSetAsset,
		NoIndexInButtonListSetAsset,
		NoButtonListAssetInButtonListSetAsset,
		EmptyPrefFieldForSet,
		TwoOrMoreSetingsFiles,
		CantFindGameObject,
		CantFindFolder,
		CantFindMethod,
		CantFindClass,
		NoObjectForButton,
		NoTypeOnScene,
		MoreThanOneTypeOnScene,
		NoComponent,
		ZeroEvents,
		NoSceneObject,
		InvalidSceneObject,
		PlayerPrefsCleared,
		NoAssetsSelected
	}

	public enum Modal
	{
		None,
		UppgrdeTitle,
		UppgrdeInfo,
		ReimportTitle,
		ReimportInfo,
		WelcomeTitle,
		ImportantTitle,
		PreviousInfo,
		ImportantInfo,
		ImportantInfo2,
		Warning,
		ClearPrefsInfo,
		ReserializeInfo,
		ReserializeAllInfo
	}

	public enum Tooltip
	{
		None,

		Settings,
		LeftSide,
		RightSide,

		Offsets,
		LeftBegin,
		RightBegin,
		Between,
		DefaultStyles,
		StyleButton,
		StyleToggle,
		StyleLabel,
		StylePopup,
		StyleNumber,
		StyleText,
		Other,
		ConsoleMessages,
		DisableToolbar,
		DisabledColor,
		Override,
		OverrideLeftSide,
		SOForLeftSide,
		SOForLeftSideButton,
		SOSetForLeftSide,
		SOSetForLeftSideButton,
		OverrideRightSide,
		SOForRightSide,
		SOForRightSideButton,
		SOSetForRightSide,
		SOSetForRightSideButton,

		EntryFoldout,
		EntryType,
		EntryIsActive,
		LabelType,
		LabelText,
		LabelIcon,
		LabelIconPath,
		UseLabelIconPath,
		RowStyle,
		RowWidth,
		RowColors,
		RowTooltip,
		StyleType,
		StyleName,
		StyleSkin,
		FixedWidth,
		ColorsHeader,
		ColorsGlobal,
		ColorsContent,
		ColorsBackground,
		TooltipHeader,
		TooltipText,
		TooltipLinesCount,
		PrefToggle,
		PrefPopup,
		PopupFillButton,
		PopupNamesList,
		PrefList,
		ListFillButton,
		PrefText,
		NumberType,
		PrefNumberFloat,
		PrefNumberInt,
		SliderType,
		PrefSliderFloat,
		SliderFloatMin,
		SliderFloatMax,
		PrefSliderInt,
		SliderIntMin,
		SliderIntMax,
		OtherType,
		TimeUseLabel,
		TimeUseResetButton,
		TimeDefault,
		TimeMin,
		TimeMax,
		FPSUseLabel,
		FPSUseResetButton,
		FPSDefault,
		FPSMin,
		FPSMax,
		ButtonActions,
		UseOnChangeActions,
		OnChangeActions,
		ActionsList,

		ButtonType,
		MouseButton,
		KeyboardButton,
		ListActionName,
		ButtonObject,
		ButtonEvent,
		StaticClass,
		StaticMethod,
		TypeClass,
		TypeMethod,
		GOName,
		GOComponent,
		GOMethod,
		OpenFolder,
		FindGO,
		MenuItem,
		MenuItemButton,
		LoadAdditive,

		PopupListItemName,
		PopupListItemIndex,
		SetName,
		SetSO,
		SetSOButton,
		ListButtons,
		SetPrefInt,
		SetWidth,
		SetListEntries,
		SetUseActionsToggle,
		SetUseActions
	}

	public static class ManulToolbarMessages
	{
		static string messagePrefix = "[MANUL Inspector message] ";

		public static void ShowMessage(Message type, MessageType messageType, string[] infos = null)
		{
			string messageText = messagePrefix;

			switch (type)
			{
				case Message.NoSettingsAsset:
					messageText += "Manul Toolbar Settings asset cannot be found in the Resources folder.";
					Debug.LogWarning(messageText);
					return;
			}

			if (!ManulToolbar.settings.settings.showConsoleMessages) return;

			switch (type)
			{
				case Message.None:
					messageText += "None";
					break;

				case Message.NoButtonListAsset:
					messageText += "Manul Toolbar Button List asset for " + infos[0] + " toolbar side is missing! Please add this asset to the 'Override " + infos[1] + " Side' field in the Manul Toolbar Settings.";
					break;

				case Message.NoButtonListSetAsset:
					messageText += "Manul Toolbar Button List Set asset for " + infos[0] + " toolbar side is missing! Please add this asset to the 'Override " + infos[1] + " Side' field in the Manul Toolbar Settings.";
					break;

				case Message.NoIndexInButtonListSetAsset:
					messageText += "Manul Toolbar Button List Set asset in the 'Override " + infos[0] + " Side' field does not have an entry with index: " + infos[1];
					break;

				case Message.NoButtonListAssetInButtonListSetAsset:
					messageText += "Manul Toolbar Button List Set asset in the 'Override " + infos[0] + " Side' field has an entry with index " + infos[1] + ", but this entry does not have the Manul Toolbar Button List asset.";
					break;

				case Message.EmptyPrefFieldForSet:
					messageText += "Manul Toolbar Button List Set asset in the 'Override " + infos[0] + " Side' field has an empty 'Int Pref Name' field.";
					break;

				case Message.CantFindFolder:
					messageText += "Cannot find a folder with the path: '" + infos[0] + "' for the button named '" + infos[1] + "'.";
					break;

				case Message.CantFindGameObject:
					messageText += "Cannot find GameObject with name '" + infos[0] + "' in the current opened scene in the button named '" + infos[1] + "'.";
					break;

				case Message.NoObjectForButton:
					messageText += "Object field in the '" + infos[0] + "' action in the button named '" + infos[1] + "' is empty.";
					break;

				case Message.NoTypeOnScene:
					messageText += "No object of type '" + infos[0] + "' was found in the current scene so the method '" + infos[3] + "' (from action '" + infos[2] + "' in the button named '" + infos[1] + "') cannot be invoked.";
					break;

				case Message.MoreThanOneTypeOnScene:
					messageText += "Two or more objects of the type '" + infos[0] + "' were found in the current scene so the method '" + infos[3] + "' (from action '" + infos[2] + "' in the button named '" + infos[1] + "') cannot be invoked. " +
						"Make sure that there is exactly one object of the given type.";
					break;

				case Message.CantFindClass:
					messageText += "Cannot find a class named '" + infos[0] + "' for the action '" + infos[2] + "' in the button named '" + infos[1] + "'. Make sure you typed the class name with the namespace (for example: 'Manul.Toolbar.Examples.ExampleComponent').";
					break;

				case Message.CantFindMethod:
					messageText += "Cannot find a method '" + infos[3] + "' in the class named '" + infos[0] + "' for the action '" + infos[2] + "' in the button named '" + infos[1] + "'.";
					break;

				case Message.TwoOrMoreSetingsFiles:
					messageText +=
						"You have two or more Manul Toolbar Settings assets named 'Manul Toolbar', but they are located in different 'Resources' folders. " +
						"There must be exactly one Manul Toolbar Settings asset named 'Manul Toolbar' in exactly one 'Resource' folder, " +
						"otherwise you will encounter problems while using the Manul Toolbar tool.";
					break;

				case Message.ZeroEvents:
					messageText += "Action '" + infos[0] + "' in the button named '" + infos[1] + "' has no events.";
					break;

				case Message.NoSceneObject:
					messageText += "Action '" + infos[0] + "' in the button named '" + infos[1] + "' has no scene asset.";
					break;

				case Message.InvalidSceneObject:
					messageText += "Action '" + infos[0] + "' in the button named '" + infos[1] + "' has invalid scene asset.";
					break;

				case Message.PlayerPrefsCleared:
					messageText += "All Player Prefs were deleted.";
					break;

				case Message.NoAssetsSelected:
					messageText += "No assets were selected to force reserialize.";
					break;
			}

			switch (messageType)
			{
				case MessageType.Info: Debug.Log(messageText); break;
				case MessageType.Warning: Debug.LogWarning(messageText); break;
				case MessageType.Error: Debug.LogError(messageText); break;
			}
		}

		public static string GetModal(Modal type, string[] infos = null)
		{
			string modalText = "";

			switch (type)
			{
				case Modal.UppgrdeTitle:
					modalText = "Welcome to the MANUL Toolbar!";
					break;

				case Modal.UppgrdeInfo:
					modalText = "The MANUL Toolbar was successfully upgraded to version: " + infos[0] + "\n\nImportant information: you can move the Manul Toolbar settings asset into any of the 'Resources' folder but remember not to change the name of this asset ('Manul Toolbar').";
					break;

				case Modal.ReimportTitle:
					modalText = "Please re-import MANUL Toolbar";
					break;

				case Modal.ReimportInfo:
					modalText = "Manul Toolbar Settings (Default) file was not found. Please re-import the package.";
					break;

				case Modal.WelcomeTitle:
					modalText = "Welcome to the MANUL Toolbar!";
					break;

				case Modal.ImportantTitle:
					modalText = "Important information";
					break;

				case Modal.PreviousInfo:
					modalText = "Your settings from the previous version were copied to the new 'Manul Toolbar' settings asset located in the Assets/Resources folder. \n\n Would you like to delete the old settings asset (recommended)?";
					break;

				case Modal.ImportantInfo:
					modalText = "You can move the new Manul Toolbar settings asset into any of the 'Resources' folder but remember not to change the name of this asset ('Manul Toolbar').";
					break;

				case Modal.ImportantInfo2:
					modalText = "Important information: you can move the Manul Toolbar settings asset into any of the 'Resources' folder but remember not to change its name ('Manul Toolbar').";
					break;

				case Modal.Warning:
					modalText = "Warning";
					break;

				case Modal.ClearPrefsInfo:
					modalText = "Are you sure you want to delete all Player Prefs? This operation cannot be undone.";
					break;

				case Modal.ReserializeInfo:
					modalText = "Are you sure you want to force reserialize selected assets?";
					break;

				case Modal.ReserializeAllInfo:
					modalText = "Are you sure you want to force reserialize all assets in your project? This operation can take a lot of time.";
					break;
			}

			return modalText;
		}

		public static string GetTooltip(Tooltip type)
		{
			string tooltipText = "";

			switch (type)
			{
				case Tooltip.None:
					tooltipText = "";
					break;

				#region Manul Toolbar Settings

				case Tooltip.LeftSide:
					tooltipText = "The list of items’ properties for the left side of the upper toolbar.";
					break;

				case Tooltip.RightSide:
					tooltipText = "The list of items’ properties for the right side of the upper toolbar.";
					break;

				#endregion

				#region Manul Toolbar Preferences

				case Tooltip.Offsets:
					tooltipText = "This section contains different distances between the items on the toolbar.";
					break;

				case Tooltip.LeftBegin:
					tooltipText = "The horizontal distance between the last Unity button on the left (Version Control) and the beginning of the left row with items";
					break;

				case Tooltip.RightBegin:
					tooltipText = "The horizontal distance between the Unity Step button and the beginning of the right row with items.";
					break;

				case Tooltip.Between:
					tooltipText = "The horizontal distance between the items on the toolbar. The value applies to the items on both the left and right rows.";
					break;

				case Tooltip.DefaultStyles:
					tooltipText = "This section contains the default styles for most of item types.";
					break;

				case Tooltip.StyleButton:
					tooltipText = "Default style for the Button item type.";
					break;

				case Tooltip.StyleToggle:
					tooltipText = "Default style for the Toggle item type.";
					break;

				case Tooltip.StyleLabel:
					tooltipText = "Default style for the Label item type.";
					break;

				case Tooltip.StylePopup:
					tooltipText = "Default style for the Popup item type.";
					break;

				case Tooltip.StyleNumber:
					tooltipText = "Default style for the Number item type.";
					break;

				case Tooltip.StyleText:
					tooltipText = "Default style for the Text item type.";
					break;

				case Tooltip.Other:
					tooltipText = "This section contains three various and unrelated options.";
					break;

				case Tooltip.ConsoleMessages:
					tooltipText = "Enable this option to enable log, warning, and error messages in the Unity Console window. All messages from Manul Toolbar have the [MANUL Toolbar message] prefix.";
					break;

				case Tooltip.DisableToolbar:
					tooltipText = "Enable this option to disable the display of items on the toolbar";
					break;

				case Tooltip.DisabledColor:
					tooltipText = "The tint that will be applied to the options of an element if it is disabled on the items list.";
					break;

				case Tooltip.Override:
					tooltipText = "This section lets you override the left or right side of the toolbar with assets containing a list of items. ";
					break;

				case Tooltip.OverrideLeftSide:
					tooltipText = "Override the left side of the toolbar by setting the dropdown to one of the following options:" + "\n" +
					" None - no overriding" + "\n" +
					" List - override with the Manul Toolbar Button List asset" + "\n" +
					" Set  - override with the Manul Toolbar Button List Set asset";
					break;

				case Tooltip.SOForLeftSide:
					tooltipText = "Drag and drop the Manul Toolbar Button List asset here.";
					break;

				case Tooltip.SOForLeftSideButton:
					tooltipText = "Opens a property window with the dragged and dropped Manul Toolbar Button List asset’s settings.";
					break;

				case Tooltip.SOSetForLeftSide:
					tooltipText = "Drag and drop the Manul Toolbar Button List Set asset here.";
					break;

				case Tooltip.SOSetForLeftSideButton:
					tooltipText = "Opens a property window with the dragged and dropped Manul Toolbar Button List Set asset’s settings.";
					break;

				case Tooltip.OverrideRightSide:
					tooltipText = "Override the right side of the toolbar by setting the dropdown to one of the following options:" + "\n" +
					" None - no overriding" + "\n" +
					" List - override with the Manul Toolbar Button List asset" + "\n" +
					" Set  - override with the Manul Toolbar Button List Set asset";
					break;

				case Tooltip.SOForRightSide:
					tooltipText = "Drag and drop the Manul Toolbar Button List asset here.";
					break;

				case Tooltip.SOForRightSideButton:
					tooltipText = "Opens a property window with the dragged and dropped Manul Toolbar Button List asset’s settings.";
					break;

				case Tooltip.SOSetForRightSide:
					tooltipText = "Drag and drop the Manul Toolbar Button List Set asset here.";
					break;

				case Tooltip.SOSetForRightSideButton:
					tooltipText = "Opens a property window with the dragged and dropped Manul Toolbar Button List Set asset’s settings.";
					break;

				#endregion

				#region Manul Toolbar Entry

				case Tooltip.EntryFoldout:
					tooltipText = "Left-click on this foldout to expand the element options.";
					break;

				case Tooltip.EntryType:
					tooltipText = "Select the item type. See Section 6. Item Types for more information about item types.";
					break;

				case Tooltip.EntryIsActive:
					tooltipText = "Check this toggle to enable this item. Uncheck it to disable it. Disabled items will not be drawn on the toolbar.";
					break;

				case Tooltip.LabelType:
					tooltipText = "This dropdown allows you to specify what the item's label will consist of: Text, Icon, Both, or None.";
					break;

				case Tooltip.LabelText:
					tooltipText = "Enter the text that this item will display on the toolbar.";
					break;

				case Tooltip.LabelIcon:
					tooltipText = "Drag and drop a Texture2D image here. This image will be displayed by the item on the toolbar as icon.";
					break;

				case Tooltip.LabelIconPath:
					tooltipText = "Enter the path for a built-in editor icon. It will be displayed by the item on the toolbar. See Section 5.1.2 Icon for a list of all built-in editor icon paths.";
					break;

				case Tooltip.UseLabelIconPath:
					tooltipText = "Enable this toggle to use built-in editor icons instead of dragging and dropping Texture2D images.";
					break;

				case Tooltip.RowStyle:
					tooltipText = "If this toggle is disabled, the item will use one of the default styles (see Default Styles group in the Settings). Enable this toggle to use a custom style.";
					break;

				case Tooltip.RowWidth:
					tooltipText = "If this toggle is disabled, the width of the item will be determined automatically, based on the length of the text the item displays. Enable this toggle to specify a fixed width.";
					break;

				case Tooltip.RowColors:
					tooltipText = "If this toggle is disabled, the item colors will have no tint. Enable this toggle to modify the tint.";
					break;

				case Tooltip.RowTooltip:
					tooltipText = "If this toggle is disabled, the item will have no tooltip. Enable this toggle to add a tooltip for this item. ";
					break;

				case Tooltip.StyleType:
					tooltipText = "Select the style type:" + "\n" +
					" Default - the item will use the default style(see Default Styles group in the Settings)" + "\n" +
					" Find By Name - lets you enter the style’s name and(optionally) drag and drop a GUI Skin asset" + "\n" +
					" (Other) - select a Unity style with this name";
					break;

				case Tooltip.StyleName:
					tooltipText = "Enter the style’s name here. The item will be displayed using this style. The style will be searched for in the GUI Skin asset you dragged and dropped in the field next to the style dropdown." + "\n\n" +
					"If the field for the GUI Skin asset is empty, the style will be searched for in a standard skin - DarkSkin or LightSkin - depending on which Editor Theme you have selected in Unity editor.";
					break;

				case Tooltip.StyleSkin:
					tooltipText = "Drag and drop GUI Skin asset in this field.";
					break;

				case Tooltip.FixedWidth:
					tooltipText = "Enter the value of the fixed width for this item.";
					break;

				case Tooltip.ColorsHeader:
					tooltipText = "Modify the colors in the following row (global, content, background) to change the tint of this item.";
					break;

				case Tooltip.ColorsGlobal:
					tooltipText = "Modifies the tint of the background and content (text) colors (GUI.color).";
					break;

				case Tooltip.ColorsContent:
					tooltipText = "Modifies the tint of the content (text) color (GUI.contentColor).";
					break;

				case Tooltip.ColorsBackground:
					tooltipText = "Modifies the tint of the background color (GUI.backgroundColor).";
					break;

				case Tooltip.TooltipHeader:
					tooltipText = "Create a tooltip for this item.";
					break;

				case Tooltip.TooltipText:
					tooltipText = "Enter the text of the tooltip.";
					break;

				case Tooltip.TooltipLinesCount:
					tooltipText = "Specify how many lines the field for the tooltip text will have.";
					break;

				case Tooltip.PrefToggle:
					tooltipText = "Enter the name of the bool editor pref that this Toggle will use to store its value.";
					break;

				case Tooltip.PrefPopup:
					tooltipText = "Enter the name of the int editor pref that this Popup will use to store its value.";
					break;

				case Tooltip.PopupFillButton:
					tooltipText = "This button will automatically put a number in each element of the Actions List (starting with index 0).";
					break;

				case Tooltip.PopupNamesList:
					tooltipText = "A list of items which will be shown after left-clicking on the popup in the toolbar. Each element consists of a name (left text field) and an index associated with this name (right integer field).";
					break;

				case Tooltip.PrefList:
					tooltipText = "Enter the name of the int editor pref that this List will use to store its value.";
					break;

				case Tooltip.ListFillButton:
					tooltipText = "This button will automatically create names for action elements. The name that is created depends on the type of the action (see Section 6.3.4 Fill Button). The name will be created only if the field for the name in the action element is empty.";
					break;

				case Tooltip.PrefText:
					tooltipText = "Enter the name of the string editor pref that this Text will use to store its value.";
					break;

				case Tooltip.NumberType:
					tooltipText = "Select the data type for this Number item: Float or Int.";
					break;

				case Tooltip.PrefNumberFloat:
					tooltipText = "Enter the name of the float editor pref that this Number will use to store its value.";
					break;

				case Tooltip.PrefNumberInt:
					tooltipText = "Enter the name of the int editor pref that this Number will use to store its value.";
					break;

				case Tooltip.SliderType:
					tooltipText = "Select the data type for this Slider item: Float or Int.";
					break;

				case Tooltip.PrefSliderFloat:
					tooltipText = "Enter the name of the float editor pref that this Slider will use to store its value.";
					break;

				case Tooltip.SliderFloatMin:
					tooltipText = "The minimum value of the Slider.";
					break;

				case Tooltip.SliderFloatMax:
					tooltipText = "The maximum value of the Slider.";
					break;

				case Tooltip.PrefSliderInt:
					tooltipText = "Enter the name of the int editor pref that this Slider will use to store its value.";
					break;

				case Tooltip.SliderIntMin:
					tooltipText = "The minimum value of the Slider.";
					break;

				case Tooltip.SliderIntMax:
					tooltipText = "The maximum value of the Slider.";
					break;

				case Tooltip.OtherType:
					tooltipText = "Select the subtype of the Other item type.";
					break;

				case Tooltip.TimeUseLabel:
					tooltipText = "Enable this toggle to display the label on the toolbar for this Time Scale slider.";
					break;

				case Tooltip.TimeUseResetButton:
					tooltipText = "Enable this toggle to show the Reset button on the toolbar for this Time Scale slider. When the user clicks this button, it will set the slider to a value that is typed in the Default field.";
					break;

				case Tooltip.TimeDefault:
					tooltipText = "The default value of the Time Scale Slider. This value will be set when the user clicks the Reset Button.";
					break;

				case Tooltip.TimeMin:
					tooltipText = "The minimum value of the Time Scale slider.";
					break;

				case Tooltip.TimeMax:
					tooltipText = "The maximum value of the Time Scale slider.";
					break;

				case Tooltip.FPSUseLabel:
					tooltipText = "Enable this toggle to display the label on the toolbar for this Frame Rate slider.";
					break;

				case Tooltip.FPSUseResetButton:
					tooltipText = "Enable this toggle to show the Reset button on the toolbar for this Frame Rate slider. When the user clicks this button, it will set the slider to a value that is typed in the Default field.";
					break;

				case Tooltip.FPSDefault:
					tooltipText = "The default value of the Frame Rate Slider. This value will be set when the user clicks the Reset Button.";
					break;

				case Tooltip.FPSMin:
					tooltipText = "The minimum value of the Frame Rate slider.";
					break;

				case Tooltip.FPSMax:
					tooltipText = "The maximum value of the Frame Rate slider.";
					break;

				case Tooltip.ButtonActions:
					tooltipText = "The list of actions that will be performed when the user clicks this Button item.";
					break;

				case Tooltip.UseOnChangeActions:
					tooltipText = "Enable this toggle to be able to create a list of actions that will be performed when the user changes the value of this item.";
					break;

				case Tooltip.OnChangeActions:
					tooltipText = "The list of actions that will be performed when the user changes the value of this item.";
					break;

				case Tooltip.ActionsList:
					tooltipText = "The list of options (actions) that will be shown in the popup List created by this item. ";
					break;

				#endregion

				#region Manul Toolbar Action

				case Tooltip.ButtonType:
					tooltipText = "Select the action type. See Section 7. Action Types for more information about action types.";
					break;

				case Tooltip.MouseButton:
					tooltipText = "Select the mouse button (LMB - Left Mouse Button, RMB - Right Mouse Button, MMB - Middle Mouse Button) that must be pressed after hovering the cursor over the button for the action to be performed.";
					break;

				case Tooltip.KeyboardButton:
					tooltipText = "Select the optional keyboard button that must be held down during the click for the action to take place. By default, it is None. You can change it to Ctrl, Shift, or Alt.";
					break;

				case Tooltip.ListActionName:
					tooltipText = "Type an option name that will be associated with this action. This name, together with other options’ names, will be displayed as a List popup in the toolbar. ";
					break;

				case Tooltip.ButtonObject:
					tooltipText = "Drag and drop an asset (from the Project window) which this action will use.";
					break;

				case Tooltip.ButtonEvent:
					tooltipText = "Standard Unity event. Drag a scriptable object or a prefab with components. Make sure that these objects have methods that you can invoke." + "\n\n" +
					"Important: By default the dropdown above the object field is set to ‘Runtime Only’. Change it to ‘Editor And Runtime’, otherwise this event will not be invoked in the Edit Mode.";
					break;

				case Tooltip.StaticClass:
					tooltipText = "Type a full name of the class (including namespace) that contains a method which this action will invoke.";
					break;

				case Tooltip.StaticMethod:
					tooltipText = "Type the name of the method that will be invoked by this action. A method must be in the class whose name you entered in the field above.";
					break;

				case Tooltip.TypeClass:
					tooltipText = "Type a full name of the class (including namespace) that contains a method which this action will invoke. This class will be searched for in the current scene by using the Object.FindObjectOfType<T> method. Make sure that there is exactly one object of this type in the current scene.";
					break;

				case Tooltip.TypeMethod:
					tooltipText = "Type the name of the method that will be invoked by this action. A method must be in the class whose name you entered in the field above.";
					break;

				case Tooltip.GOName:
					tooltipText = "Type the name of the gameObject that this action will look for in the current scene. The search is performed by using the GameObject.Find method. Make sure that there is exactly one gameObject with this name in the current scene.";
					break;

				case Tooltip.GOComponent:
					tooltipText = "Type a full name of the component class (including namespace) that contains a method which this action will invoke. At least one component of this type must be added to the gameObject whose name you entered in the field above.";
					break;

				case Tooltip.GOMethod:
					tooltipText = "Type the name of the method that will be invoked by this action. The method must be in the component class whose name you entered in the field above.";
					break;

				case Tooltip.OpenFolder:
					tooltipText = "Asset path for a folder whose contents will be shown by this action." + "\n\n" +
					"Tip: It is easier to copy and paste the path of the folder rather than type it manually(right-click on it in the Project window, select Copy Path, and paste the copied path in the text field).";
					break;

				case Tooltip.FindGO:
					tooltipText = "Type the name of the gameObject that this action will look for in the current scene. The search is performed by using the GameObject.Find method. Make sure that there is exactly one gameObject with this name in the current scene.";
					break;

				case Tooltip.MenuItem:
					tooltipText = "Type the full path of the menu item (it’s better to use the button next to the field to get the exact menu item path).";
					break;

				case Tooltip.MenuItemButton:
					tooltipText = "Opens a window with a list of all menu items. You can type something in the upper text field to narrow your search and then double-click on an item that you want to choose.";
					break;

				case Tooltip.LoadAdditive:
					tooltipText = "Drag and drop a scene asset (from the Project window) which this action will use.";
					break;

				#endregion

				#region Manul Toolbar Other

				case Tooltip.PopupListItemName:
					tooltipText = "Type the name that will be show as an option in the Popup.";
					break;

				case Tooltip.PopupListItemIndex:
					tooltipText = "Enter the index which will represent this option in the Popup.";
					break;

				case Tooltip.SetName:
					tooltipText = "Type the name that will be shown as an option in the List Set popup.";
					break;

				case Tooltip.SetSO:
					tooltipText = "Drag and drop the Manul Toolbar Button List asset here.";
					break;

				case Tooltip.SetSOButton: 
					tooltipText = "Opens a property window with the dragged and dropped Manul Toolbar Button List asset’s settings.";
					break;

				case Tooltip.ListButtons:
					tooltipText = "The list of items’ properties. You can override one of the toolbar sides with this asset.";
					break;

				case Tooltip.SetPrefInt:
					tooltipText = "Enter the name of the int editor pref that this List Set will use to save the currently selected items list.";
					break;

				case Tooltip.SetWidth:
					tooltipText = "Enter the value of the fixed width for this List Set popup.";
					break;

				case Tooltip.SetListEntries:
					tooltipText = "List of entries from which the List Set popup will be created. Each entry represents an option in this popup.";
					break;

				case Tooltip.SetUseActionsToggle:
					tooltipText = " --- TO DO --- ";
					break;

				case Tooltip.SetUseActions:
					tooltipText = " --- TO DO --- ";
					break;

					#endregion
			}

			return tooltipText;
		}
	}
}

#endif

