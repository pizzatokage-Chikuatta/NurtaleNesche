Equipment UI
================================
This tutorial changes how equipment is shown in the UI, such as key slot or action text.

This is a caution target because UI IDs must match equipment/action data.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_Chikuatta\Lootings\Mod Equipment UI\Equipment UI

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Equipment UI", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "equipmentUi",
  "entry": "Equipment UI",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "equipmentUi".
2. "entry" must match the folder name next to mod.json. In this case, it is "Equipment UI".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "dagger.json" from:
StreamingAssets\Lootings\Items And Equipments\Equipment UI
Paste it into:
Mods\Mod_Chikuatta\Lootings\Mod Equipment UI\Equipment UI

Step 5
--------
Edit "dagger.json".
1. "id": Do not change it when overriding an existing UI definition.
2. equipment/action IDs: must point to valid equipment/action data.
3. display fields: text/icon/key slot settings.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. This changes UI data, not the equipment behavior itself.
2. If the equipment works but the UI is wrong, check this data.
