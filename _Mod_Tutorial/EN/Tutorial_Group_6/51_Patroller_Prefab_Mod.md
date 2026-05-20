Patroller Prefab Mod
================================

This tutorial explains how to connect an AssetBundle prefab to a new Patroller Data entry.

This is not a full tutorial for designing a completely new enemy AI from zero. The safest first test is to modify a known working patroller prefab shape and a known working Patroller Data JSON.

Before starting, import the sample prefab package from:

~~~text
Mods\Developer Reference Packs\assetbundle_prefab_reference_pack
~~~

Open the sample patroller prefab and study its hierarchy/components. Duplicate that sample and edit the duplicate. Do not start from a blank prefab unless you already understand the runtime component requirements.

Goal
--------

We will create a new patroller id that loads its prefab from an AssetBundle.

The connection is:

~~~text
Patroller Data id
  -> assetBundleFileName
  -> prefabAssetPathInsideBundle
  -> Patroller Spawn JSON
  -> Patroller Spawn Loadout JSON
~~~

Required Prefab Checklist
--------

The spawned prefab must contain the components expected by the game.

At minimum, the runtime spawn pipeline expects:

1. Commons.
2. DungeonDwellerCommons.
3. A component that implements IPatrollerControl.

For most real enemies, the prefab also needs the correct animation components, hit manager, colliders, rigidbody, renderers, sense-related objects, audio/event components, and any controller-specific child objects.

If you do not know these components yet, start by modifying a known working patroller prefab instead of building a completely original prefab.

Step 1
--------

Build an AssetBundle from your patroller prefab in Unity Editor.

Example output:

~~~text
my_shadow_bundle
my_shadow_bundle.manifest
~~~

Open "my_shadow_bundle.manifest" and find the "Assets:" section.

Example:

~~~text
Assets:
- Assets/MyMod/Patrollers/MyShadow.prefab
~~~

This means:

1. "assetBundleFileName" is "my_shadow_bundle".
2. "prefabAssetPathInsideBundle" is "Assets/MyMod/Patrollers/MyShadow.prefab".

Step 2
--------

Create this folder:

~~~text
Mods\Mod_YourName\Patrollers\My AssetBundle Patroller\Patroller Data\Stage 6
~~~

Put the actual AssetBundle file next to the patroller JSON:

~~~text
Mods\Mod_YourName\Patrollers\My AssetBundle Patroller\Patroller Data\Stage 6\my_shadow_bundle
~~~

Do not put only the .manifest file there. The actual bundle file is required.

Step 3
--------

Copy "mod.template_folder_backed.json" from "Mods\mod.json templates".

Paste it into:

~~~text
Mods\Mod_YourName\Patrollers\My AssetBundle Patroller
~~~

Rename it to "mod.json".

Edit "mod.json" like this:

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "patrollerData",
  "entry": "Patroller Data",
  "overwriteExisting": true
}
~~~

Step 4
--------

Create a Patroller Data JSON:

~~~text
Mods\Mod_YourName\Patrollers\My AssetBundle Patroller\Patroller Data\Stage 6\patroller.my_shadow.stage_06.json
~~~

For the first test, copy a known working Patroller Data JSON and edit it. Do not write a tiny file from zero unless you know which defaults are safe.

The important AssetBundle fields are:

~~~json
{
  "id": "patroller.my_shadow.stage_06",
  "NameInAlphabet": "My Shadow",
  "assetBundleFileName": "my_shadow_bundle",
  "prefabAssetPathInsideBundle": "Assets/MyMod/Patrollers/MyShadow.prefab",
  "statusProfileId": "profile_status_effect.patroller.enemy_default",
  "actionRuleProfileId": "profile_action_rule.patroller.enemy_default",
  "dropTableProfileId": "drop_table.shadow_creature.stage_06",
  "shadowProfileId": "shadow_profile.shadow_creature",
  "animationSetId": "Shadow Creature_AC",
  "showUpStage": 6,
  "tasks": {
    "providers": [
      {
        "id": "task.warp_idle.shadow_creature",
        "enabled": true,
        "config": {
          "priority": "Idle",
          "providerCooldownMedian": 3
        }
      }
    ]
  }
}
~~~

Important:

1. "id" is the new patroller id.
2. "assetBundleFileName" must match the actual bundle file name.
3. "prefabAssetPathInsideBundle" must match the path from the bundle .manifest file.
4. "animationSetId" or "animationActorId" must match an animation set the game can resolve.
5. "tasks.providers" must contain valid task provider IDs. If tasks are wrong, the patroller can fall back to idle.
6. "senses.providers" controls detection. If senses are wrong, the patroller may not detect anything.

Step 5
--------

Create a Patroller Spawn mod if you want the new patroller to appear at stage start.

Create this folder:

~~~text
Mods\Mod_YourName\Patrollers\My AssetBundle Patroller Spawn\Initial Spawns\Stage 6
~~~

Create "mod.json" next to "Initial Spawns":

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "patrollerSpawns",
  "entry": "Initial Spawns",
  "overwriteExisting": true
}
~~~

Create this spawn JSON:

~~~text
Mods\Mod_YourName\Patrollers\My AssetBundle Patroller Spawn\Initial Spawns\Stage 6\patroller_spawn.my_shadow.stage_06.001.json
~~~

Example:

~~~json
{
  "id": "patroller_spawn.my_shadow.stage_06.001",
  "patrollerId": "patroller.my_shadow.stage_06",
  "enabled": true,
  "spawnRegionId": "0294cdd3-1dba-4ef8-b24a-fc7a5add47d0",
  "patrolRegionIds": [
    "4e270969-c8a4-4ade-8d50-83c82409e62d"
  ],
  "useSpawnRegionCheckpointPosition": false,
  "xOffsetFromRegionCenter": 0,
  "yOffsetFromGround": 0
}
~~~

Use "Reference_Data" to choose real region UUIDs for the target stage.

Step 6
--------

Create a Patroller Spawn Loadout mod so the stage uses your spawn id.

Create this folder:

~~~text
Mods\Mod_YourName\Patrollers\My AssetBundle Patroller Spawn Loadout\Initial Spawn Loadouts
~~~

Create "mod.json" next to "Initial Spawn Loadouts":

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "patrollerSpawnLoadouts",
  "entry": "Initial Spawn Loadouts",
  "overwriteExisting": true
}
~~~

Create or copy the stage loadout JSON:

~~~text
Mods\Mod_YourName\Patrollers\My AssetBundle Patroller Spawn Loadout\Initial Spawn Loadouts\patroller_spawn_loadout.stage_06.json
~~~

Example:

~~~json
{
  "patrollerSpawnLoadoutId": "patroller_spawn_loadout.stage_06",
  "patrollerSpawnIds": [
    "patroller_spawn.my_shadow.stage_06.001"
  ],
  "commonPatrolRegionIds": [
    "4e270969-c8a4-4ade-8d50-83c82409e62d"
  ],
  "commonPatrolRegionExpansions": []
}
~~~

If you want to keep existing built-in spawns, copy the original loadout from StreamingAssets and add your spawn id instead of replacing the list blindly.

Step 7
--------

Start the game and check:

1. Main Menu -> Mods -> Mod Report.
2. Enter the target stage.
3. Confirm the patroller spawns.
4. Confirm it can animate, move, detect, be hit, and run at least one task.

Troubleshooting
--------

1. "AssetBundle not found": the bundle file is not next to the patroller data JSON, or "assetBundleFileName" is wrong.
2. "Prefab not found in bundle": "prefabAssetPathInsideBundle" does not match the .manifest "Assets:" path.
3. Patroller spawns but does nothing: task provider IDs or senses are wrong.
4. Patroller is invisible: animation set, renderers, or JSON animation bindings are wrong.
5. Patroller does not get hit: hit manager/collider setup is missing.
6. Spawn id warning: the spawn loadout references an id that does not exist in patrollerSpawns.
7. The mod worked once but not after changing the bundle: restart the game before testing the rebuilt bundle.
