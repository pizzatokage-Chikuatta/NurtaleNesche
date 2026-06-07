Item Combination Loadout
================================
This tutorial changes which item combination recipes are available.

This is a caution target because missing recipe IDs make recipes unavailable.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_Chikuatta\Lootings\Mod Item Combination Loadouts\Item Combination Loadouts

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Item Combination Loadouts", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "itemCombinationLoadouts",
  "entry": "Item Combination Loadouts",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "itemCombinationLoadouts".
2. "entry" must match the folder name next to mod.json. In this case, it is "Item Combination Loadouts".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "item_combination_loadout.player.json" from:
StreamingAssets\Lootings\Combinations\Loadouts
Paste it into:
Mods\Mod_Chikuatta\Lootings\Mod Item Combination Loadouts\Item Combination Loadouts

Step 5
--------
Edit "item_combination_loadout.player.json".
1. "id": Do not change it when overriding an existing loadout.
2. combination IDs: recipes enabled by this loadout.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. Adding a recipe JSON is not enough if no loadout references it.
2. If a recipe does not appear, check this file first.

Replacement List Warning
--------

This file is a replacement list, not a small additive patch. If your mod overrides the same "id", the final list is the list written in your mod file. Keep built-in entries you still want to use.

