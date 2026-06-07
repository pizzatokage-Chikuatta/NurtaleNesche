Item Name
================================
This tutorial changes displayed item names.

This is a caution target because IDs must match item definitions.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_Chikuatta\Lootings\Mod Item Names\Item Names

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Item Names", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "itemNames",
  "entry": "Item Names",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "itemNames".
2. "entry" must match the folder name next to mod.json. In this case, it is "Item Names".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "item.biscuit.json" from:
StreamingAssets\Lootings\Items And Equipments\Item Names
Paste it into:
Mods\Mod_Chikuatta\Lootings\Mod Item Names\Item Names

Step 5
--------
Edit "item.biscuit.json".
1. "id" or item ID keys: must match the item you want to rename.
2. language text fields: displayed item name per language.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. This changes display text only.
2. It does not change item behavior.
