Item Combination
================================
This tutorial changes recipes that combine items into another item.

This is a caution target because all input/output item IDs must exist.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_Chikuatta\Lootings\Mod Item Combinations\Item Combinations

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Item Combinations", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "itemCombinations",
  "entry": "Item Combinations",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "itemCombinations".
2. "entry" must match the folder name next to mod.json. In this case, it is "Item Combinations".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "item_combination.stage_01_rope_and_hook.json" from:
StreamingAssets\Lootings\Combinations\Item Combinations
Paste it into:
Mods\Mod_Chikuatta\Lootings\Mod Item Combinations\Item Combinations

Step 5
--------
Edit "item_combination.stage_01_rope_and_hook.json".
1. "id": Do not change it when overriding an existing recipe.
2. input item IDs: items required for the recipe.
3. output item/equipment ID: result of the recipe.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. Item IDs are in Reference_Data.
2. The recipe may also need to be included in an item combination loadout.
