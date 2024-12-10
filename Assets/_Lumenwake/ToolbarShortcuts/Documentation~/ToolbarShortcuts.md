# Toolbar Shortcuts — User Guide

## Overview

Toolbar Shortcuts is an editor-only package for Unity 6. It replaces the default Play controls with a bootstrap-aware workflow and adds configurable shortcut buttons to the main toolbar.

## First-time setup

1. Import the package.
2. When the **Toolbar Shortcuts** welcome window appears, read the overview.
3. Open **File → Build Settings** and place your bootstrap (startup) scene at index **0**.
4. Click **Setup**.

Setup will:

- Hide **Play Mode Controls** on the main toolbar
- Show **ToolbarShortcuts/** toolbar items
- Enable all shortcuts in the bundled config
- Open the **Shortcuts Window** docked above the Inspector when supported

## Bootstrap Play

Toolbar item: `ToolbarShortcuts/Play`

| Control | Description |
|---------|-------------|
| Play / Stop | Starts Play Mode or stops it |
| Pause | Pauses Play Mode |
| Scene dropdown | Bootstrap (Build Settings index 0) or Current scene |

When using **Bootstrap**, the editor opens the bootstrap scene before Play Mode if needed, and returns to your previous scene after exiting Play Mode.

Optional: create **Bootstrap Config** via **Assets → Create → ToolbarShortcuts → Bootstrap Config** and place it in a `Resources` folder if you prefer assigning the bootstrap scene on the asset instead of relying only on Build Settings.

## Toolbar shortcuts

Configure `ToolbarShortcutsConfig`:

| Field | Description |
|-------|-------------|
| Placement | Toolbar, Shortcuts Window, or Both |
| Dock | Left or Right (toolbar only) |
| Order | Sort order among shortcuts |
| Action | Open scene, asset, window, static method, etc. |

Bundled config path: `Assets/Lumenwake/Resources/ToolbarShortcutsConfig.asset`

## Shortcuts Window

Open via **ToolbarShortcuts → Shortcuts Window**.

Shows buttons for entries with **Placement** set to **Window** or **Both**. Column count is configured on `ToolbarShortcutsConfig` (**Window Columns**).

## Troubleshooting

| Issue | Suggestion |
|-------|------------|
| Setup did not change the toolbar | Run **ToolbarShortcuts → Setup Toolbar** after the editor finishes compiling |
| Bootstrap Play does nothing | Ensure Build Settings has a scene at index 0 |
| Shortcuts missing on toolbar | Check **Enabled** and **Placement** on each entry; run **Refresh Toolbar** |
| Welcome shows again | **ToolbarShortcuts → Reset Setup State**, then reimport or run Setup |

## File layout

```
Assets/Lumenwake/
  package.json
  README.md
  CHANGELOG.md
  LICENSE.md
  Documentation~/
  Resources/
    ToolbarShortcutsConfig.asset
  Scripts/Editor/
    (editor scripts)
  Settings/          (optional, created by user)
```
