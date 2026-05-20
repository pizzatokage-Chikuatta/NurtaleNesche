Item Definition
================================
This tutorial changes the core definition of an item.

This is a caution target because item behavior can depend on code and other registries.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_Chikuatta\Lootings\Mod Item Definitions\Item Definitions\MISC

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Item Definitions", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "itemDefinitions",
  "entry": "Item Definitions",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "itemDefinitions".
2. "entry" must match the folder name next to mod.json. In this case, it is "Item Definitions".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "item.biscuit.json" from:
StreamingAssets\Lootings\Items And Equipments\Item Definitions\MISC
Paste it into:
Mods\Mod_Chikuatta\Lootings\Mod Item Definitions\Item Definitions\MISC

Step 5
--------
Edit "item.biscuit.json".
1. "id": Do not change it when overriding an existing item.
2. display/name fields: often linked to item name data.
3. sprite/effect fields: must point to valid IDs.
4. unknown behavior fields: leave unchanged unless you know what they do.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. Item IDs are in Reference_Data.
2. Changing an item definition does not automatically change drop tables.

Visibility Dependency
--------

Importing a new item/equipment definition only registers the data. To make it appear in-game, it must also be referenced by drop tables, item boxes, shops/loadouts, or other gameplay data. Name, sprite, and UI data may also need separate matching entries.

