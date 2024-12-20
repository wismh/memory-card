# memory-card

Card-matching (memory/pairs) mobile game. Unity **6000.3.10f1**, URP, New Input System, built on the [lumenwake/template](https://github.com/) foundation.

Open this folder in Unity Hub (Editor `6000.3.10f1`). First import will restore UPM packages (Zenject, UniTask, Odin Inspector, Addressables, HierarchyDecorator).

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

## Main game systems

See [`AGENTS.md`](AGENTS.md) for the full systems table and code-style rules. `Assets/_Project/Scripts/Runtime/` folders and namespaces follow `Project.<Domain>` (no `_` prefix, no `Features` segment, no `Module` suffix), matching template/sorcery-strife/tactics-cards.

## Starting point

This project was created from the Lumenwake template. For a fresh game built the same way:

1. Set **Company / Product / Bundle ID** in Player Settings (`com.lumenwake.<game>`).
2. Wire game installers next to `ProjectContextInstaller` (`Assets/_Project/Scripts/Runtime/GameBootstrap/`).
3. Regenerate constants via the source-generator menu after adding scenes or addressables.
