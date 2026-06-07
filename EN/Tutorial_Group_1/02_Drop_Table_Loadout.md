Drop Table Loadout
================================
This tutorial makes a mod that changes a drop table loadout. A loadout is a list of drop table IDs used together.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_{YourName}\Lootings\Mod Drop Table Loadouts\Drop Table Loadouts\Stage Common

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Drop Table Loadouts", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "dropTableLoadouts",
  "entry": "Drop Table Loadouts",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "dropTableLoadouts".
2. "entry" must match the folder name next to mod.json. In this case, it is "Drop Table Loadouts".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "drop_table_loadout.stage_common.stage_01.json" from:
StreamingAssets\Lootings\Drop Table Loadouts\Stage Common
Paste it into:
Mods\Mod_Chikuatta\Lootings\Mod Drop Table Loadouts\Drop Table Loadouts\Stage Common

Step 5
--------
Edit "drop_table_loadout.stage_common.stage_01.json".
1. "id": Do not change it when overriding a built-in loadout.
2. "dropTableIds": list of drop table IDs to use.
3. You can include item box unique drop tables too, such as `drop_table.item_box.stage_01.0`.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. Drop table IDs must exist.
2. If a loadout points to a missing drop table, that table cannot be used.

Replacement List Warning
--------

This file is a replacement list, not a small additive patch. If your mod overrides the same "id", the final list is the list written in your mod file. Keep built-in entries you still want to use.

