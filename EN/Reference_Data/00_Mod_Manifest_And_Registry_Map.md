mod.json Manifest And Registry Map
================================

This file is the quick overview for choosing the "type" field in mod.json.

Use the step-by-step tutorials when you are making an actual mod. Use this file when you need to see the whole map at once.

Experimental v1 Rule
--------

Always write this in mod.json:

~~~json
{
  "experimentalApiVersion": "v1"
}
~~~

v1 is experimental. JSON, PNG, audio, animation, AssetBundle, and code/DLL mods can still change after game updates.

The Three mod.json Shapes
--------

Most non-DLL mods use one of these three shapes.

Shape 1: folder-backed registry mod
--------

Use this when one folder contains one or more JSON files.

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "dropTables",
  "entry": "Drop Tables",
  "overwriteExisting": true
}
~~~

Fields:

1. "type" chooses which game registry receives the JSON files.
2. "entry" is the folder name next to mod.json.
3. "overwriteExisting": true means this mod can replace built-in records with the same id.

Most JSON mods use this shape.

Shape 2: actor-targeted animation/controller mod
--------

Use this when the mod targets one animation set, such as an Orc controller or Nesche animation set.

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "animatorController",
  "target": "Orc_AC Edit",
  "variant": "",
  "entry": "AnimationController"
}
~~~

Fields:

1. "type" is one of "animation", "animationClipOverride", or "animatorController".
2. "target" is the animation target id, not a patroller data id. Examples: "Orc_AC Edit" or "Nesche".
3. "variant" is usually "" for base. Player/Nesche restraint variants use exact values such as "armbinder", "ccc", or "yoke_bondage".
4. "entry" is the folder name next to mod.json.

Shape 3: file-backed mod
--------

Use this when the mod points to one standalone file.

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "language",
  "file": "language.json"
}
~~~

Currently this is mainly used by "language".

GameServices Registry Map
--------

This map answers: "Which mod.json type feeds which game registry?"

This is an authoring map. It is not a promise that code mods should directly use every registry.

| Game registry / surface | mod.json "type" |
| --- | --- |
| ActionRule | actionRules |
| AnimationMods | animation |
| AnimationClipOverrideMods | animationClipOverride |
| AnimationControllerMods | animatorController |
| AnimationSfx | animationSFX |
| AnimationTrackLayouts | animationTrackLayouts |
| BgmStageLoadouts | bgmStageLoadouts |
| BgmTracks | bgmTracks |
| ChainPoints | chainPointDefinitions |
| ChainPointSpawnLoadouts | chainPointSpawnLoadouts |
| ChainPointSpawns | chainPointSpawns |
| CombinationLoadouts | itemCombinationLoadouts |
| DropTableLoadouts | dropTableLoadouts |
| DropTables | dropTables |
| EquipmentUi | equipmentUi |
| Equipments | equipmentDefinitions |
| InteractionLoadouts | interactionLoadouts |
| InteractionOptionMeta | interactionOptionMeta |
| ItemBoxPrefabs | itemBoxPrefabs |
| ItemBoxSpawnLoadouts | itemBoxSpawnLoadouts |
| ItemBoxSpawns | itemBoxSpawns |
| ItemCombinations | itemCombinations |
| ItemNames | itemNames |
| ItemSelectionRequestDefinitions | itemSelectionRequestDefinitions |
| Items | itemDefinitions |
| Language | language |
| PatrollerDirectorStageLoadouts | patrollerDirectorStageLoadouts |
| PatrollerDirectors | patrollerDirectors |
| PatrollerSpawnLoadouts | patrollerSpawnLoadouts |
| PatrollerSpawns | patrollerSpawns |
| Patrollers | patrollerData |
| PlayerStats | playerStats |
| PotionEffects | potionEffects |
| SfxClassifications | classificationID |
| SfxClips | audioFiles |
| ShadowProfiles | shadowProfiles |
| Sprites | sprites |
| StatusEffectMetaRegistry | statusMetadata |
| StatusLoadouts | statusLoadouts |

Folder-backed Types
--------

These usually use the folder-backed shape with "entry":

~~~text
actionRules
animationSFX
animationTrackLayouts
audioFiles
bgmStageLoadouts
bgmTracks
chainPointDefinitions
chainPointSpawnLoadouts
chainPointSpawns
classificationID
dropTableLoadouts
dropTables
equipmentDefinitions
equipmentUi
interactionLoadouts
interactionOptionMeta
itemBoxPrefabs
itemBoxSpawnLoadouts
itemBoxSpawns
itemCombinationLoadouts
itemCombinations
itemDefinitions
itemNames
itemSelectionRequestDefinitions
patrollerData
patrollerDirectorStageLoadouts
patrollerDirectors
patrollerSpawnLoadouts
patrollerSpawns
playerStats
potionEffects
shadowProfiles
sprites
statusLoadouts
statusMetadata
~~~

Actor-targeted Types
--------

These use "target", "variant", and "entry":

~~~text
animation
animationClipOverride
animatorController
~~~

File-backed Types
--------

These use "file":

~~~text
language
~~~

Important Notes
--------

1. The "type" string must match exactly. Case matters.
2. "entry" is relative to the folder containing mod.json.
3. "file" is relative to the folder containing mod.json.
4. If you override built-in data, keep the built-in "id" unchanged inside the JSON file.
5. If you create new data, use your own unique id.
6. After editing a mod, open the game main menu -> Mods -> Mod Report and check the import result.
7. For animation mods, do not use ids like "patroller.goblin.stage_06" as target. Patroller ids are gameplay data ids, not animation targets.
