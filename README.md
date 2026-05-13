# Nurtale Nesche Modding API

## Status

This modding API is currently v1 experimental.

Current documented game version: `1.0.5.0b`.

`b` means beta. JSON/PNG mods, animation mods, and code/DLL mods may work, but no v1 surface has a compatibility guarantee yet. Later versions may promote specific surfaces to stable after they are versioned, validated, documented, and exposed through `NurtaleNesche.Modding.*`.

Use this README as the public entry point. Internal game scripts are not automatically public API.

For code/DLL mods, start from documented `NurtaleNesche.Modding.*` types. Some v1 types in that namespace are experimental aliases over current runtime types; that makes them usable now, but it does not make the underlying runtime namespaces or concrete gameplay classes stable.

## Start Here

- For a short setup flow, read `docs/MODDING_QUICK_START.md`.
- For JSON/PNG examples, start from `samples/json`.
- For animation examples, start from `samples/animation`.
- For experimental code/DLL examples, start from `samples/code`.

## Supported Mod Folder

Place mods in the project/game root `Mods` folder:

```text
Nurtale Nesche 2022 LTS/
  Mods/
    YourModName/
      ...
```

Do not place normal player mods under `Assets/Mods` for release-style testing. `Assets/Mods` is a Unity project asset path and can behave differently in the editor.

## Required Manifest Field

Every `mod.json` or `mod.template.json` should include:

```json
{
  "experimentalApiVersion": "v1"
}
```

In v1 this is warning-only. Missing or non-`v1` values may still load, but they are reported by validation tools and should be fixed.

## Common `mod.json` Shapes

### Folder-backed JSON registry mod

Use this for registries loaded from a folder of JSON files.

```json
{
  "experimentalApiVersion": "v1",
  "type": "patrollerData",
  "entry": "Patroller Data",
  "overwriteExisting": true
}
```

`entry` is the folder path relative to the `mod.json` file.

### Actor-targeted animation/controller mod

Use this for `animation`, `animationClipOverride`, and `animatorController`.

```json
{
  "experimentalApiVersion": "v1",
  "type": "animatorController",
  "target": "patroller.goblin",
  "variant": "",
  "entry": "AnimationController"
}
```

### Standalone file-backed mod

Use this for standalone file surfaces such as `language`.

```json
{
  "experimentalApiVersion": "v1",
  "type": "language",
  "file": "language.json"
}
```

## V1 Experimental Data Mod Types

These `type` values are available in v1, but are still experimental:

- `actionRules`
- `animation`
- `animationClipOverride`
- `animationSFX`
- `animationTrackLayouts`
- `animatorController`
- `audioFiles`
- `bgmStageLoadouts`
- `bgmTracks`
- `chainPointDefinitions`
- `chainPointSpawnLoadouts`
- `chainPointSpawns`
- `classificationID`
- `dropTableLoadouts`
- `dropTables`
- `equipmentDefinitions`
- `equipmentUi`
- `interactionLoadouts`
- `interactionOptionMeta`
- `itemBoxPrefabs`
- `itemBoxSpawnLoadouts`
- `itemBoxSpawns`
- `itemCombinationLoadouts`
- `itemCombinations`
- `itemDefinitions`
- `itemNames`
- `itemSelectionRequestDefinitions`
- `language`
- `patrollerData`
- `patrollerDirectorStageLoadouts`
- `patrollerDirectors`
- `patrollerSpawnLoadouts`
- `patrollerSpawns`
- `playerStats`
- `potionEffects`
- `shadowProfiles`
- `sprites`
- `statusLoadouts`
- `statusMetadata`

Prefer copying from the example mod packs instead of writing new manifests from memory.

### Chainpoint Spawn GUIDs

For `chainPointSpawns`, each spawned chainpoint JSON needs its own stable `checkpointUuid`.

- Generate a fresh GUID once for each spawn entry.
- Keep that GUID unchanged after publishing the mod.
- Do not reuse scene object UUIDs or copy the same UUID across multiple spawn entries.

Useful generation commands:

```powershell
[guid]::NewGuid().ToString()
```

```csharp
System.Guid.NewGuid().ToString()
```

## Built-in Reference Data

Built-in JSON/PNG reference data lives under:

```text
Assets/StreamingAssets/
```

For animation sprites, use `Assets/StreamingAssets/AnimationClips/*` as the mod authoring reference.

Do not use this internal acceleration mirror as a public authoring path:

```text
Assets/Built_Ins/AnimSprites/*
```

That folder is for built-in Addressables-backed runtime loading.

## Validation

In the Unity editor, run:

```text
Tools > Mod Data > Validate V1 Experimental Mods
```

The validator writes:

```text
Assets/Scripts/MODDING_V1_VALIDATION_REPORT.md
```

The v1 validator is a first-pass tool. It checks manifests, common metadata, basic `patrollerData` / `playerStats` / `patrollerSpawns` / `patrollerSpawnLoadouts` / `patrollerDirectors` / `patrollerDirectorStageLoadouts` / `statusMetadata` / `statusLoadouts` / `actionRules` / `animationTrackLayouts` / `chainPointDefinitions` / `chainPointSpawns` / `chainPointSpawnLoadouts` / `sprites` / `audioFiles` / `classificationID` / `bgmTracks` / `bgmStageLoadouts` / `shadowProfiles` / `itemDefinitions` / `equipmentDefinitions` / `itemNames` / `potionEffects` / `dropTables` / `dropTableLoadouts` / `itemCombinations` / `itemCombinationLoadouts` / `itemBoxPrefabs` / `itemBoxSpawns` / `itemBoxSpawnLoadouts` shape, and code-manifest type names, but it is not a complete schema validator yet.

The validation rules live in the UnityEditor-free `NurtaleNesche.Modding.Validation.ModdingV1ValidationCore`, so a smaller standalone validator wrapper can be shipped later without exposing the developer-only export hub.

In the Unity project, those validation rules are isolated in `NurtaleNesche.Modding.Validation.dll`.

The Unity Editor validator wrapper can enable code-manifest type-resolution diagnostics. That check may load `.dll` files under `Mods` for validation-only reporting and does not change runtime loading behavior.

## Code/DLL Mods

Code/DLL mods are experimental in v1.

They are trusted local code. They are not sandboxed. They are desktop/Mono-first and should not be treated as cross-platform unless a target build is explicitly verified.

Code mods that use public modding constants or id attributes should reference:

```text
NurtaleNesche.Modding.Abstractions.dll
```

Task-provider code mods still also need the current runtime assembly in v1:

```text
NurtaleNesche.Runtime.dll
```

Depending on the compiler/toolchain, runtime-coupled code mods may also need the Unity package or third-party DLLs referenced by `NurtaleNesche.Runtime.dll`.

For task-provider code mods, see:

```text
MODDING_CODE_MOD_TASK_PROVIDER_GUIDE.md
```

For typed registrar helper examples, see:

```text
MODDING_CODE_MOD_REGISTRY_HELPERS.md
```

For code/DLL manifest file shapes, see:

```text
MODDING_CODE_MOD_MANIFESTS.md
```

The SDK namespace root is:

```text
NurtaleNesche
```

The v1 experimental mod-facing namespace is:

```text
NurtaleNesche.Modding.*
```

Current v1 exposes experimental constants and manifest contracts in that namespace:

- `NurtaleNesche.Modding.ExperimentalModApi`
- `NurtaleNesche.Modding.ModManifestFieldNames`
- `NurtaleNesche.Modding.ModManifestTypeIds`

These constants and public id attributes live in `NurtaleNesche.Modding.Abstractions.dll`.

It also exposes experimental manifest aliases under `NurtaleNesche.Modding.Json`:

- `ModDataBase`
- `FileModDataBase`
- `ActorFileModDataBase`
- `ActorFolderModDataBase`
- `FolderModDataBase`
- `FolderRegistryModData`

Patroller task authoring is available through experimental public aliases. Modders may use it, but it is not stable yet:

- `NurtaleNesche.Modding.Patrollers.Tasks.TaskProviderCore`
- `NurtaleNesche.Modding.Patrollers.Tasks.TaskSetCore`
- `NurtaleNesche.Modding.Patrollers.Tasks.TaskOption`
- `NurtaleNesche.Modding.Patrollers.Tasks.TaskPriorities`
- `NurtaleNesche.Modding.Patrollers.Tasks.TaskInfo`
- `NurtaleNesche.Modding.Patrollers.Tasks.TaskSequenceBuilder`
- task-option interfaces under `NurtaleNesche.Modding.Patrollers.Tasks`
- id attributes under `NurtaleNesche.Modding.Patrollers.Tasks`
- registrar aliases under `NurtaleNesche.Modding.Registries`
- typed registrar helpers under `NurtaleNesche.Modding.Registries.ModRegistryExtensions`, including task-provider, task-option, sense, status-effect, and item-selection factory helpers

These code-mod surfaces are also available as experimental public aliases:

- custom status effects under `NurtaleNesche.Modding.StatusEffects`
- custom patroller senses under `NurtaleNesche.Modding.Patrollers.Senses`
- item-selection request factories under `NurtaleNesche.Modding.Items`
- sense/status/item-selection registrars under `NurtaleNesche.Modding.Registries`

Until a stable SDK is published, non-documented runtime types remain internal implementation details even if they are technically visible from a code mod. This includes `ActionScheduler2`, concrete built-in states, concrete built-in task providers/options, managers, player controllers, patroller controllers, and scene event scripts.

## Compatibility Rules For V1

- Use canonical ids exactly.
- Keep `experimentalApiVersion` set to `v1`.
- Prefer JSON/PNG mods before code mods.
- Prefer example templates over hand-written manifests.
- Do not depend on `Assets/Built_Ins/AnimSprites`.
- Do not treat internal runtime scripts as stable API.
- Re-run the validator after every manifest or data-shape change.
