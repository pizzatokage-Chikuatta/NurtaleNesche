Item Box Unique Drop Table
================================
This tutorial makes a mod that changes an item box unique drop table.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_{YourName}\Lootings\Mod Drop Tables\Drop Tables\Stage 1\Item Box

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Drop Tables", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "dropTables",
  "entry": "Drop Tables",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "dropTables".
2. "entry" must match the folder name next to mod.json. In this case, it is "Drop Tables".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "drop_table.item_box.stage_01.0.json" from:
StreamingAssets\Lootings\Drop Tables\Stage 1\Item Box
Paste it into:
Mods\Mod_Chikuatta\Lootings\Mod Drop Tables\Drop Tables\Stage 1\Item Box

Step 5
--------
Edit "drop_table.item_box.stage_01.0.json".
1. "id": Do not change it when overriding a built-in drop table.
2. "guaranteedItem": item IDs that always drop.
3. "guaranteedEquipment": equipment IDs that always drop.
4. "groups": random drop groups.
5. "rolls": how many times this group rolls.
6. "itemId": item ID. `null` means nothing drops for that roll.
7. "weight": higher weight means this entry is more likely to be selected.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. Item/equipment IDs are listed in Reference_Data.
2. Item boxes can use both stage-common drop tables and unique drop tables.
3. If you empty only the unique drop table, the box may still drop stage-common items.
