# memory-card

<p align="left">
  <img src="https://img.shields.io/badge/Unity-6000.3.10f1-black.svg?style=flat-square&logo=unity&logoColor=white" alt="Unity Version" />
  <img src="https://img.shields.io/badge/C%23-12-blue.svg?style=flat-square&logo=csharp&logoColor=white" alt="C#" />
  <img src="https://img.shields.io/badge/URP-6000.3.10-grey.svg?style=flat-square&logo=unity&logoColor=white" alt="URP" />
  <img src="https://img.shields.io/badge/DI-Zenject-0b5394.svg?style=flat-square" alt="Zenject" />
  <img src="https://img.shields.io/badge/Async-UniTask-674ea7.svg?style=flat-square" alt="UniTask" />
  <img src="https://img.shields.io/badge/Animation-DOTween-c0392b.svg?style=flat-square" alt="DOTween" />
  <img src="https://img.shields.io/badge/Content-Addressables-16a085.svg?style=flat-square" alt="Addressables" />
  <img src="https://img.shields.io/badge/Platform-Android%20%7C%20iOS-3DDC84.svg?style=flat-square&logo=android&logoColor=white" alt="Platform" />
</p>

Card-matching (memory/pairs) mobile game built on the [lumenwake/template](https://github.com/wismh/untiy-template-project) foundation with a clean, decoupled MVP architecture.

Open this folder in Unity Hub (Editor `6000.3.10f1`). First import will restore UPM packages (Zenject, UniTask, Odin Inspector, Addressables, HierarchyDecorator).

## Tech Stack & Architecture

| Area | Technology / Pattern | Role in Project |
|------|----------------------|-----------------|
| **Engine & Pipeline** | Unity `6000.3.10f1` (URP) | High-performance 2D mobile rendering with Universal Render Pipeline. |
| **Dependency Injection** | [Zenject / Extenject](https://github.com/modesttree/Zenject) | Scoped DI (`ProjectContext` $\to$ Scene contexts) for loose coupling and testability. |
| **Asynchronous Logic** | [UniTask](https://github.com/Cysharp/UniTask) | Allocation-free async/await lifecycle, scene loading, and screen transitions (no Coroutines). |
| **Animation System** | [DOTween (Demigiant)](http://dotween.demigiant.com/) | Smooth 3D card flips, popup bounce transitions, and map animations. |
| **Asset Management** | Unity Addressables | Decoupled on-demand loading of level content, card types, and board assets. |
| **Input System** | New Input System | Touch-optimized, event-driven player input for mobile devices. |
| **Game Flow** | State Machine (`StateMachineModule`) | High-level lifecycle management (Global $\leftrightarrow$ Menu $\leftrightarrow$ Round $\leftrightarrow$ Reload). |
| **Presentation** | MVP (Model-View-Presenter) | Strict separation of domain logic (`CardModel`, `LevelContext`), UI views, and presenters. |
| **Save System** | `SaveServiceModule` + Newtonsoft.Json | Strong-typed persistence with automated versioning and migrations pipeline. |
| **Inspector Tooling** | Odin Inspector & Serializer | Clean editor workflows, custom inspectors, and asset validation. |

## Game Design Overview (Short GDD)

**Genre:** Casual / Puzzle / Memory & Pairs matching mobile game.  
**Target Platform:** Mobile (Android & iOS).

### Core Gameplay Loop
1. **Level Selection:** Choose an available level on the winding progression map.
2. **Card Matching:** The board deals face-down cards in pairs. Tap two cards to flip them:
   - **Match:** Cards remain revealed.
   - **Mismatch:** Cards flip back face-down after a brief reveal.
3. **Round Victory:** Match all pairs to clear the board.
4. **Progression & Rewards:** The win popup presents the round completion time. Progress is saved, the next level unlocks, and the player avatar advances along the animated map path.

### Key Mechanics
- **Dynamic Board Composition:** Configurable board size and card count per level via `LevelConfig` / `LevelsDb`.
- **Card Flip & Feedback:** DOTween-driven flip animations, responsive touch feedback, and sound effects for flips, matches, and victories.
- **Round Timer:** Tracks elapsed time from start to completion.
- **Save & Progression:** Persistent progress tracking (`SaveServiceModule`) storing completed levels and unlocking sequential stages.
- **Animated Map Path:** Bezier-curve level map navigation where the avatar travels between level nodes upon stage completion.

## Layout

| Path | What lives here |
|------|-----------------|
| `Assets/_Lumenwake/` | Shared tools. Do not put game-specific code here. |
| `Assets/_Project/` | This game: scripts, scenes, art, audio, generated constants. |
| `Assets/Plugins/` | Zenject, UniTask, Odin Inspector, DOTween (Demigiant) |
| `Assets/BetterFolders/`, `Assets/HierarchyDecorator/` | Project-window folder colors/decoration |
| `Assets/AddressableAssetsData/` | Addressables groups (board/level content is loaded via Addressables) |

## `_Lumenwake` modules

Runtime (`Project.Core.*` / `Lumenwake.*`):

- **AssetLoaderModule** — Addressables + Resources facade
- **SceneLoaderServiceModule** — built-in + Addressables scenes
- **StateMachineModule** — lightweight state machine
- **SaveServiceModule** — generic `SaveService<T>`, JSON (Newtonsoft), migrations
- **UIModule** — `BaseScreen`/`BaseScreenManager` screen stack
- **ComponentRegistry**, **Global** (`LoggingSystem`, `Result`), **_Global** (`AsyncGetter`)

Editor: source generators (scenes in build, addressable constants, prefab names), `ToolbarShortcuts` (vendored, same as template — bootstrap play/pause/scene-dropdown toolbar element).

## Main Game Systems

`Assets/_Project/Scripts/Runtime/` folders and namespaces follow the `Project.<Domain>` convention (no `_` prefix, no `Features` segment, no `Module` suffix), matching template/sorcery-strife/tactics-cards conventions.

| System | Path | Description & Key Types |
|--------|------|-------------------------|
| **Board** | `Board/` | `BoardComposer` and `BoardComposerConfig` generate and position cards from a level configuration; `BoardPresenter` and `BoardView` (MVP) handle board interaction and completion events. |
| **Cards** | `Card/` | `CardModel`, `CardType`, `CardFactory`, `CardView`, and `CardFlipAnimator` (smooth DOTween-driven flip animations). |
| **Game Flow** | `GameFlow/` | High-level state machine driving transitions between menu, round start, round reload, and global flow (`GameFlowStateMachine`, `GoMenuFlowState`, `StartRoundFlowState`, `ReloadRoundFlowState`). |
| **Gameplay** | `GamePlay/` | `GamePlayPresenter` orchestrates the gameplay round, connecting board completion, round timer, progress recording, and victory popup triggers. |
| **Bootstrap** | `GameBootstrap/` | Scene and project initialization via Zenject: `ProjectContextInstaller`, `GlobalSceneBootstrap`, `RoundSceneBootstrap`. |
| **HUD** | `Hud/` | In-game UI controls (`Root/` with timer and return button) and the win popup (`WonPopup/WonPopupPresenter` deriving from `BaseScreen`). |
| **Levels** | `Levels/` | Level definitions and runtime context: `LevelConfig` (ScriptableObject per stage), `LevelsDb`, `LevelContext`, `LevelResult`. |
| **Main Menu** | `MainMenu/` | Level selection screen (`LevelSelectPresenter`, `LevelSelectView`) and map progress presenter (`LevelMapProgressPresenter`). |
| **Progress & Saves** | `Progress/` | Save data models (`GameSaveData`, `GameSaveVersion`), save migration pipeline (`Migrations/SaveMigration_0_To_1`), and `LevelProgressService` built on `_Lumenwake`'s `SaveServiceModule` (`ISaveService<GameSaveData>`). |
| **Sound** | `Sound/` | Audio playback systems: `AudioPlayer`, `SoundAsset`, and `PlaySoundOnButtonClick`. |
| **Level Map Path** | `UIMapPath/` | Bezier-curve map animations for player progression (`BezierMath`, `LevelMapPath`, `LevelMapPlayer`). |

## Boot Chain & Architecture

- **Zenject ProjectContext:** Configured via `ProjectContextInstaller` (`GameBootstrap/ProjectContextInstaller.cs`), binding project-wide singletons: `ProgressInstaller`, `SceneLoaderServiceModuleInstaller`, `GameFlowStateMachineInstaller`, `AssetLoaderInstaller`.
- **Scene Entry Points:** 
  - `GlobalSceneBootstrap` resolves `GameFlowStateMachine` and transitions to `GoMenuFlowState`.
  - `RoundSceneBootstrap` bootstraps the gameplay round scene dependencies and triggers round flow.
- **UI Architecture:** Screens such as `WonPopupPresenter` (Overlay layer) and `LevelSelectPresenter` (Default layer) inherit from `BaseScreen` (`Lumenwake.UIModule`) and are opened via `IScreenManager.OpenScreen<T>()`. `HudInstaller` and `MenuInstaller` bind `BaseScreenManager` and `IScreenManager` directly to manage the screen stack.

## Starting point

This project was created from the Lumenwake template. For a fresh game built the same way:

1. Set **Company / Product / Bundle ID** in Player Settings (`com.lumenwake.<game>`).
2. Wire game installers next to `ProjectContextInstaller` (`Assets/_Project/Scripts/Runtime/GameBootstrap/`).
3. Regenerate constants via the source-generator menu after adding scenes or addressables.
