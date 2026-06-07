Chainpoint Prefab Mod
================================

This tutorial explains how to connect an AssetBundle prefab to a runtime-spawned chainpoint.

Chainpoint prefab mods are advanced. Many chainpoints are not just visual objects; they contain gameplay scripts, status effects, checkpoint identity, interaction data, animation state bindings, and save/restore identity.

Before starting, import the sample prefab package from:

~~~text
Mods\Developer Reference Packs\assetbundle_prefab_reference_pack
~~~

Open the sample chainpoint prefab and study its hierarchy/components. Duplicate that sample and edit the duplicate. Do not start from a blank prefab unless you already understand Chainpoint, Checkpoint, state animator bindings, and status/interaction wiring.

Goal
--------

We will create:

1. A chainpoint definition.
2. A chainpoint spawn entry.
3. A chainpoint spawn loadout entry.

The connection is:

~~~text
chainPointDefinitions id
  -> assetBundleFileName
  -> prefabAssetPathInsideBundle
  -> chainPointSpawns chainPointId
  -> chainPointSpawnLoadouts chainPointSpawnIds
~~~

Required Prefab Checklist
--------

The spawned prefab must contain:

1. A Chainpoint-derived component somewhere in the prefab hierarchy.
2. A Checkpoint component that Chainpoint.EnsureSetup can resolve.
3. Components required by the specific chainpoint type.
4. Any IChainpointSpawnDataReceiver components that need spawn data.
5. Any IInitializable components must be safe to initialize from the chainpoint spawn pipeline.

If this is your first chainpoint prefab mod, start from a known working chainpoint prefab shape.

Step 1
--------

Build an AssetBundle from your chainpoint prefab in Unity Editor.

Example output:

~~~text
my_chainpoint_bundle
my_chainpoint_bundle.manifest
~~~

Open "my_chainpoint_bundle.manifest" and find the "Assets:" section.

Example:

~~~text
Assets:
- Assets/MyMod/Chainpoints/MyMachine.prefab
~~~

This means:

1. "assetBundleFileName" is "my_chainpoint_bundle".
2. "prefabAssetPathInsideBundle" is "Assets/MyMod/Chainpoints/MyMachine.prefab".

Step 2
--------

Create a chainpoint definition mod folder:

~~~text
Mods\Mod_YourName\Chainpoints\My AssetBundle Chainpoint\Definitions
~~~

Put the actual AssetBundle file next to the definition JSON:

~~~text
Mods\Mod_YourName\Chainpoints\My AssetBundle Chainpoint\Definitions\my_chainpoint_bundle
~~~

Create "mod.json" next to "Definitions":

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "chainPointDefinitions",
  "entry": "Definitions",
  "overwriteExisting": true
}
~~~

Step 3
--------

Create the chainpoint definition JSON:

~~~text
Mods\Mod_YourName\Chainpoints\My AssetBundle Chainpoint\Definitions\chainpoint.my_machine.json
~~~

Example:

~~~json
{
  "id": "chainpoint.my_machine",
  "statusId": "status_effect.breast_milk_potion_machine",
  "typeEnum": "Able_To_Play_With",
  "accessScope": "Public",
  "isNeedLockPickToEscape": false,
  "runtimeSpawnable": true,
  "interactionLoadout": "profile.milk_machine",
  "assetBundleFileName": "my_chainpoint_bundle",
  "prefabAssetPathInsideBundle": "Assets/MyMod/Chainpoints/MyMachine.prefab",
  "stateAnimatorBindings": [
    {
      "chainpointStateId": "empty",
      "mainAnimatorId": "body",
      "entryMode": "play",
      "entryStateName": "New State",
      "monitorStateNames": [
        "New State"
      ]
    },
    {
      "chainpointStateId": "captured_idle",
      "mainAnimatorId": "body",
      "entryMode": "play",
      "entryStateName": "Idle",
      "monitorStateNames": [
        "Idle"
      ]
    }
  ]
}
~~~

Important:

1. "id" is the chainpoint definition id.
2. "runtimeSpawnable" must be true if you want the stage-initial spawner to spawn it.
3. "statusId" must be a real status effect id.
4. "interactionLoadout" must be a real interaction loadout id.
5. "stateAnimatorBindings" must match animator ids and state names in the prefab.
6. "assetBundleFileName" must be the actual bundle file.
7. "prefabAssetPathInsideBundle" must be copied from the .manifest "Assets:" section.

Step 4
--------

Create a chainpoint spawn mod folder:

~~~text
Mods\Mod_YourName\Chainpoints\My AssetBundle Chainpoint Spawns\Initial Spawns
~~~

Create "mod.json" next to "Initial Spawns":

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "chainPointSpawns",
  "entry": "Initial Spawns",
  "overwriteExisting": true
}
~~~

Create a spawn JSON:

~~~text
Mods\Mod_YourName\Chainpoints\My AssetBundle Chainpoint Spawns\Initial Spawns\chainpoint_spawn.my_machine.stage_06.001.json
~~~

Example:

~~~json
{
  "id": "chainpoint_spawn.my_machine.stage_06.001",
  "checkpointUuid": "b0cfc6bb-46c5-4998-8181-80c34f36aeca",
  "enabled": true,
  "regionId": "4e270969-c8a4-4ade-8d50-83c82409e62d",
  "offsetX": 0,
  "heightFromGround": 2,
  "rotationY": 0,
  "chainPointId": "chainpoint.my_machine"
}
~~~

Important:

1. "checkpointUuid" must be a newly generated GUID. Do not copy another chainpoint's UUID.
2. "regionId" must be a real region UUID from Reference_Data.
3. "chainPointId" must match the definition id from Step 3.

Step 5
--------

Create a chainpoint spawn loadout mod folder:

~~~text
Mods\Mod_YourName\Chainpoints\My AssetBundle Chainpoint Loadout\Initial Spawn Loadouts
~~~

Create "mod.json" next to "Initial Spawn Loadouts":

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "chainPointSpawnLoadouts",
  "entry": "Initial Spawn Loadouts",
  "overwriteExisting": true
}
~~~

Create or copy the stage loadout JSON:

~~~text
Mods\Mod_YourName\Chainpoints\My AssetBundle Chainpoint Loadout\Initial Spawn Loadouts\chainpoint_spawn_loadout.stage_06.json
~~~

Example:

~~~json
{
  "chainPointSpawnLoadoutId": "chainpoint_spawn_loadout.stage_06",
  "chainPointSpawnIds": [
    "chainpoint_spawn.my_machine.stage_06.001"
  ]
}
~~~

If the stage already has built-in chainpoint spawns, copy the original loadout from StreamingAssets and add your id instead of replacing the list blindly.

Step 6
--------

Start the game and check:

1. Main Menu -> Mods -> Mod Report.
2. Enter the target stage.
3. Confirm the chainpoint spawned.
4. Confirm interaction works.
5. Save/load if the chainpoint is meant to persist.

Troubleshooting
--------

1. "Unknown chainpoint spawn id": the loadout references a spawn id that does not exist.
2. "Unknown chainpoint id": the spawn JSON points to a definition id that does not exist.
3. "AssetBundle not found": the bundle file is not next to the definition JSON, or "assetBundleFileName" is wrong.
4. "Prefab not found in bundle": "prefabAssetPathInsideBundle" does not match the .manifest path.
5. Chainpoint does not spawn: "runtimeSpawnable" may be false, regionId may be wrong, or the loadout may not include the spawn id.
6. Chainpoint spawns but interaction fails: interactionLoadout, statusId, Chainpoint-derived script, or Checkpoint setup is wrong.
7. Save/restore acts strange: checkpointUuid is duplicated or was changed after a save file already used it.

GUID Rule
--------

Every runtime-spawned chainpoint needs a stable unique "checkpointUuid".

Use a GUID generator, for example:

~~~text
b0cfc6bb-46c5-4998-8181-80c34f36aeca
~~~

Do not use the same GUID for two chainpoints. Do not change the GUID after releasing a mod unless you intentionally want old saves to stop matching that chainpoint.
