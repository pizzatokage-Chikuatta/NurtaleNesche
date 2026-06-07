Item Box Spawn Loadout
================================
This tutorial changes the list of item box spawn IDs used when a stage starts.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_{YourName}\Lootings\Item Boxes\Mod Stage Initial Spawn Loadouts\Stage Initial Spawn Loadouts

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Stage Initial Spawn Loadouts", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "itemBoxSpawnLoadouts",
  "entry": "Stage Initial Spawn Loadouts",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "itemBoxSpawnLoadouts".
2. "entry" must match the folder name next to mod.json. In this case, it is "Stage Initial Spawn Loadouts".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "item_box_spawn_loadout.stage_01.json" from:
StreamingAssets\Lootings\Item Boxes\Stage Initial Spawn Loadouts
Paste it into:
Mods\Mod_Chikuatta\Lootings\Item Boxes\Mod Stage Initial Spawn Loadouts\Stage Initial Spawn Loadouts

Step 5
--------
Edit "item_box_spawn_loadout.stage_01.json".
1. "id": do not change it when overriding the stage loadout.
2. "spawnIds": item box spawn IDs used by this stage.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. Adding a new item box spawn JSON is not enough. The loadout must include its ID.
2. If the spawn ID is missing or misspelled, the box will not spawn.

Replacement List Warning
--------

This file is a replacement list, not a small additive patch. If your mod overrides the same "id", the final list is the list written in your mod file. Keep built-in entries you still want to use.

