Equipment Definition
================================
This tutorial changes equipment definitions such as dagger, crossbow, hammer, and similar equipment.

This is a caution target because equipment can affect input, UI, combat, and actions.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_Chikuatta\Lootings\Mod Equipment Definitions\Equipment Definitions

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Equipment Definitions", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "equipmentDefinitions",
  "entry": "Equipment Definitions",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "equipmentDefinitions".
2. "entry" must match the folder name next to mod.json. In this case, it is "Equipment Definitions".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "equipment.crossbow.json" from:
StreamingAssets\Lootings\Items And Equipments\Equipment Definitions
Paste it into:
Mods\Mod_Chikuatta\Lootings\Mod Equipment Definitions\Equipment Definitions

Step 5
--------
Edit "equipment.crossbow.json".
1. "id": Do not change it when overriding an existing equipment.
2. stat/effect fields: edit carefully.
3. sprite/UI fields: must point to valid data.
4. unknown behavior fields: leave unchanged.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. Equipment IDs are in Reference_Data.
2. Equipment UI data may need a separate mod if display behavior is changed.

Visibility Dependency
--------

Importing a new item/equipment definition only registers the data. To make it appear in-game, it must also be referenced by drop tables, item boxes, shops/loadouts, or other gameplay data. Name, sprite, and UI data may also need separate matching entries.

