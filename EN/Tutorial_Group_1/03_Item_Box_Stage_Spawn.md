Item Box Stage Spawn
================================
This tutorial makes a mod that changes or adds an item box spawn in a stage.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_{YourName}\Lootings\Item Boxes\Mod Stage Initial Spawns\Stage Initial Spawns\Stage 01

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Stage Initial Spawns", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "itemBoxSpawns",
  "entry": "Stage Initial Spawns",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "itemBoxSpawns".
2. "entry" must match the folder name next to mod.json. In this case, it is "Stage Initial Spawns".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "item_box_spawn.stage_01.001.itembox.json" from:
StreamingAssets\Lootings\Item Boxes\Stage Initial Spawns\Stage 01
Paste it into:
Mods\Mod_Chikuatta\Lootings\Item Boxes\Mod Stage Initial Spawns\Stage Initial Spawns\Stage 01

Step 5
--------
Edit "item_box_spawn.stage_01.001.itembox.json".
1. "id": keep it when overriding an existing box; use a unique new ID when adding a new box.
2. "enabled": if false, this box will not spawn.
3. "boxPrefabId": item box prefab ID. Usually do not change it.
4. "spawnRegionId": region UUID where the box spawns. See Reference_Data.
5. "xOffsetFromRegionCenter": horizontal offset from the region center.
6. "useSpawnRegionCheckpointPosition": if true, spawn at the region checkpoint instead of center.
7. "yPosOffset": vertical offset.
8. "dropYVelocity": upward velocity for dropped contents.
9. "dontRespawn": if true, the box will not respawn.
10. "hasRandomSpawn": if true, random drop table logic is used.
11. "dontCurseItem": if true, items from this box will not be cursed.
12. "dontBrokeItem": if true, broken items will not appear.
13. "curseAll": if true, all items from this box become cursed.
14. "dontDropStageCommonDrops": if true, stage-common drops are not mixed in.
15. "openSFXId": SFX ID played when the box opens.
16. "uniqueDropTableIds": unique drop table IDs for this box.
17. "fixedRandomSpawnItemIds": direct item IDs to spawn when not using normal random drop behavior.
18. "fixedRandomSpawnQuantityList": quantities matching fixedRandomSpawnItemIds.
19. "spawnEquipmentIds": direct equipment IDs to spawn.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. Region UUIDs are in Reference_Data.
2. If `useSpawnRegionCheckpointPosition` is true, `xOffsetFromRegionCenter` usually does not matter.
