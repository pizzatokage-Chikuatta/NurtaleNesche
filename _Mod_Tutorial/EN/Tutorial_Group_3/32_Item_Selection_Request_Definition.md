Item Selection Request Definition
================================
This tutorial changes definitions that decide which items can be selected in an item selection UI.

This is an advanced JSON target. It is not recommended as your first mod.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_Chikuatta\Advanced\Mod Item Selection Requests\Item Selection Requests

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Item Selection Requests", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "itemSelectionRequestDefinitions",
  "entry": "Item Selection Requests",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "itemSelectionRequestDefinitions".
2. "entry" must match the folder name next to mod.json. In this case, it is "Item Selection Requests".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "item_selection_request_definitions.json" from:
StreamingAssets\Interaction\Item Selection Requests
Paste it into:
Mods\Mod_Chikuatta\Advanced\Mod Item Selection Requests\Item Selection Requests

Step 5
--------
Edit "item_selection_request_definitions.json".
1. "id": request definition ID.
2. allowed item/equipment IDs: selectable targets.
3. filter/category/status fields: selection restrictions.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. Item/equipment IDs are in Reference_Data.
2. If the allowed list is wrong or empty, the UI may open with nothing useful to select.

Copy-First Rule
--------

For this advanced target, copy a built-in file first and change one field at a time. Creating this JSON from empty is risky because hidden links to runtime code or other registries can be easy to miss.

