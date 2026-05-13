# Modding Quick Start

## V1 Experimental Status

v1 modding is experimental.

JSON/PNG mods, animation mods, and code/DLL mods may work in v1, but none of them have compatibility guarantees yet. Later versions can promote specific surfaces to stable after they are versioned, validated, documented, and exposed through `NurtaleNesche.Modding.*`.

Current game version for these notes: `1.0.5.0b`. The `b` suffix means beta.

Code/DLL mods are trusted local code. They are desktop/Mono-first and should not be treated as sandboxed or cross-platform unless a target build is explicitly verified.

For code/DLL mods, start from documented `NurtaleNesche.Modding.*` namespaces. Current v1 public types may still bridge into runtime classes, so do not treat undocumented game namespaces as stable API.

`experimentalApiVersion` is warning-only in v1. Missing, empty, or non-`v1` values warn in `ModLoader`, but the mod still loads because v1 is experimental.

This project now has a mostly JSON-driven runtime pipeline. The safest rule is:

- use canonical ids exactly
- export into staging first when possible
- replace `.json` files, not `.meta`

## mod.json Shapes

Most mod content enters through one of these three shapes.

Validation tool:

- `Tools > Mod Data > Validate V1 Experimental Mods`
- Writes `Assets/Scripts/MODDING_V1_VALIDATION_REPORT.md`
- Checks `mod.json` type, `experimentalApiVersion`, entry folder existence, basic `patrollerData`, `playerStats`, `patrollerSpawns`, `patrollerSpawnLoadouts`, `patrollerDirectors`, `patrollerDirectorStageLoadouts`, `statusMetadata`, `statusLoadouts`, `actionRules`, `animationTrackLayouts`, `chainPointDefinitions`, `chainPointSpawns`, `chainPointSpawnLoadouts`, `sprites`, `audioFiles`, `classificationID`, `bgmTracks`, `bgmStageLoadouts`, `shadowProfiles`, `itemDefinitions`, `equipmentDefinitions`, `itemNames`, `potionEffects`, `dropTables`, `dropTableLoadouts`, `itemCombinations`, `itemCombinationLoadouts`, `itemBoxPrefabs`, `itemBoxSpawns`, and `itemBoxSpawnLoadouts` file shape, and code-mod manifest version, known fields, `id`, and `type`.
- In the Unity Editor, code-manifest type-resolution diagnostics may load `.dll` files under `Mods` for validation-only checks. Runtime loading behavior is unchanged.

Code home for the shared `mod.json` contracts:

- `Assets/Scripts/Core/Mods/ModJsonContracts.cs`
- folder-backed registry mods: `Mods.Types.FolderRegistryModData`
- standalone file mods: `Mods.Types.FileModDataBase`
- standalone actor-targeted mods: `Mods.Types.ActorFileModDataBase`

Concrete standalone mod classes still live with their feature areas, for example:

- `AnimationClipOverrideModData`
- `AnimationModData`
- `AnimationControllerModData`
- `LanguageModData`

Folder-backed registry mod:

```json
{
  "experimentalApiVersion": "v1",
  "type": "itemBoxPrefabs",
  "entry": "Item Boxes",
  "overwriteExisting": true
}
```

Use this for registries loaded from a folder of JSON files.

Actor-targeted animation/controller mod:

```json
{
  "experimentalApiVersion": "v1",
  "type": "animatorController",
  "target": "patroller.goblin",
  "variant": "",
  "entry": "AnimationController"
}
```

Use this for:

- `animation`
- `animationClipOverride`
- `animatorController`

Standalone file-backed mod:

```json
{
  "experimentalApiVersion": "v1",
  "type": "language",
  "file": "language.json"
}
```

Currently this is mainly for:

- `language`

## GameServices Registry Map

Use this as the short "which mod.json type feeds which registry?" map. This is an authoring map, not a stable code API for directly using `IGameServices` or concrete registries.

- `Items`: `itemDefinitions`
- `ItemNames`: `itemNames`
- `PotionEffects`: `potionEffects`
- `Equipments`: `equipmentDefinitions`
- `EquipmentUi`: `equipmentUi`
- `Sprites`: `sprites`
- `DropTables`: `dropTables`
- `DropTableLoadouts`: `dropTableLoadouts`
- `ItemCombinations`: `itemCombinations`
- `CombinationLoadouts`: `itemCombinationLoadouts`
- `Patrollers`: `patrollerData`
- `PlayerStats`: `playerStats`
- `ItemBoxPrefabs`: `itemBoxPrefabs`
- `ItemBoxSpawns`: `itemBoxSpawns`
- `ItemBoxSpawnLoadouts`: `itemBoxSpawnLoadouts`
- `PatrollerSpawns`: `patrollerSpawns`
- `PatrollerSpawnLoadouts`: `patrollerSpawnLoadouts`
- `PatrollerDirectors`: `patrollerDirectors`
- `PatrollerDirectorStageLoadouts`: `patrollerDirectorStageLoadouts`
- `StatusLoadouts`: `statusLoadouts`
- `StatusEffectMetaRegistry`: `statusMetadata`
- `ActionRule`: `actionRules`
- `AnimationTrackLayouts`: `animationTrackLayouts`
- `SfxClips`: `audioFiles`
- `SfxClassifications`: `classificationID`
- `AnimationSfx`: `animationSFX`
- `ShadowProfiles`: `shadowProfiles`
- `InteractionOptionMeta`: `interactionOptionMeta`
- `InteractionLoadouts`: `interactionLoadouts`
- `ItemSelectionRequestDefinitions`: `itemSelectionRequestDefinitions`
- `ChainPoints`: `chainPointDefinitions`
- `ChainPointSpawns`: `chainPointSpawns`
- `ChainPointSpawnLoadouts`: `chainPointSpawnLoadouts`

## Chainpoint Spawn GUIDs

Every `chainPointSpawns` entry must have a unique, stable `checkpointUuid`.

- Generate a new GUID for each new runtime-spawned chainpoint.
- Do not copy a UUID from an existing scene chainpoint or another spawn JSON.
- Do not change the UUID after publishing the mod, because save/restore and lookup systems use it as the spawned chainpoint's stable identity.

Recommended ways to generate one:

```powershell
[guid]::NewGuid().ToString()
```

```csharp
System.Guid.NewGuid().ToString()
```

These are code-registered, not folder-backed. They are experimental in v1:

- `SenseFactories`
- `TaskProviders`
- `CaptiveTreatingTaskOptions`
- `ItemSelectionRequestFactories`
- `StatusEffectFactoryRegistry`

These are actor-targeted animation mod types, separate from the generic folder-backed registry types:

- `AnimationMods`: `animation`
- `AnimationClipOverrideMods`: `animationClipOverride`
- `AnimationControllerMods`: `animatorController`

## Animation JSON

- Built-in live folder: `Assets/StreamingAssets/AnimationClips`
- Tool: `Tools > Mod Data > Export Hub > Animation Clips`
- Public slot identity is `tracks[].id`
- `bindingPath` is advanced reference data, not the main mod-facing slot

For player restraint overlays, use ids like:

- `body`
- `eyemask`
- `mouthpiece`
- `leg_shackles`
- `linked_nipple_piercing`
- `lactation`
- `rope_bondage`
- `cloth`

Use `One File Per Clip` when replacing the live clip JSON library.

## Controller JSON

- Built-in live folders:
  - `Assets/StreamingAssets/AnimationController`
  - `Assets/StreamingAssets/AnimationController_Detail`
- Tools:
  - `Controller JSON (Basic)` for runtime-friendly action staging
  - `Controller JSON (Detailed)` for graph / parameter / transition inspection

Controller schema note:

- top-level `id` = controller id
- nested `actions[].stateId` = controller-local action/state key
- treat the practical identity as `controllerId + stateId`

Stage loadout ids are authoritative again. Use ids like:

- `patroller_director_loadout.stage_03`
- `patroller_director_loadout.stage_05`

## Speech CSV

- Built-in speech folder: `Assets/StreamingAssets/CSV Files/Language_XX/Speech`
- CSV contract is:
  1. `speakerId`
  2. `speakerDisplayName`
  3. `action`
  4. `priority`
  5. `speech`

Use canonical speaker ids in column 1, for example:

- `patroller.goblin`
- `patroller.fat_goblin`
- `patroller.slave_merchant`

Use the UI-facing name in column 2, for example:

- `Goblin`
- `Fat Goblin`
- `Slave Merchant`

Task-provider speech is driven by the task tag string, so the CSV `action` column should match that emitted tag exactly.

## Status Effects

- Built-in metadata folder: `Assets/StreamingAssets/Status/Status Metadata`
- Built-in loadout folder: `Assets/StreamingAssets/Status/Status Loadouts`
- Runtime metadata registry: `StatusEffectMetaRegistry`
- Runtime loadout registry: `StatusLoadouts`

Built-in/simple case:

- fill `id`
- optionally fill `builtInPrefabAddress` only if this status really needs a prefab-backed view

Mod prefab case:

- fill `assetBundleFileName`
- fill `prefabAssetName`

For visual restraint markers on prefabs, use the exact status id in `RestraintMarker.statusEffectId`, for example:

- `status_effect.eyemask`
- `status_effect.mouthpiece`
- `status_effect.leg_shackles`

## Item Boxes

- Built-in live folder: `Assets/StreamingAssets/Lootings/Item Boxes`
- Tool: `Tools > Mod Data > Export Hub > Item Boxes`

The live spawn key is now `boxPrefabId`, not the old sample-box index.

Built-in prefab registry entries point to:

- `builtInPrefabAddress` for Addressables-backed built-in prefabs
- or bundle prefab fields for modded prefabs

Recommended mod flow:

1. base your prefab on a built-in item-box prefab
2. keep the shadow child objects inside the prefab
3. attach your custom MonoBehaviour from your mod assembly
4. use `ItemBox.OnItemBoxOpen` for open-time custom behavior

## Shadow Profiles

- Built-in live folder: `Assets/StreamingAssets/Shadow/Shadow Profiles`
- Tool: `Tools > Mod Data > Export Hub > Shadow Profiles`

Shadow JSON is exported from `ShadowAnimPropertySO` assets and is safe to regenerate when built-in shadow authoring changes.
