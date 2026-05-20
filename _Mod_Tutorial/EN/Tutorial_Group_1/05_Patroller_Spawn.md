Patroller Spawn
================================
This tutorial changes or adds a patroller that spawns at stage start.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_{YourName}\Patrollers\Mod Initial Spawns\Initial Spawns\Stage 1

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Initial Spawns", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "patrollerSpawns",
  "entry": "Initial Spawns",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "patrollerSpawns".
2. "entry" must match the folder name next to mod.json. In this case, it is "Initial Spawns".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "patroller_spawn.orc.stage_01.json" from:
StreamingAssets\Patrollers\Initial Spawns\Stage 1
Paste it into:
Mods\Mod_Chikuatta\Patrollers\Mod Initial Spawns\Initial Spawns\Stage 1

Step 5
--------
Edit "patroller_spawn.orc.stage_01.json".
1. "id": keep it when overriding; use a unique new ID when adding a new spawn.
2. "enabled": if false, this spawn is disabled.
3. "patrollerId": patroller data ID to spawn.
4. "spawnRegionId": region UUID. See Reference_Data.
5. "xOffsetFromRegionCenter": horizontal offset from region center.
6. "useSpawnRegionCheckpointPosition": if true, use the region checkpoint position.
7. "patrolRegionIds": optional region UUIDs assigned to this patroller.
8. "patrolRegionAssignmentMode": how the patrol regions are assigned.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. Patroller IDs are in Reference_Data.
2. Region UUIDs are in Reference_Data.
3. The spawn must also be included in the patroller spawn loadout.
