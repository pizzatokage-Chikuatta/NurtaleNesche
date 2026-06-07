Patroller Data
================================
This tutorial changes patroller HP, movement speed, senses, tasks, and other patroller data.

This is a caution target. Start by overriding an existing built-in file and change only one small part at a time.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_Chikuatta\Patrollers\Mod Patroller Data\Patroller Data\Stage 1

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Patroller Data", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "patrollerData",
  "entry": "Patroller Data",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "patrollerData".
2. "entry" must match the folder name next to mod.json. In this case, it is "Patroller Data".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "patroller.orc.stage_01.json" from:
StreamingAssets\Patrollers\Data\Stage 1
Paste it into:
Mods\Mod_Chikuatta\Patrollers\Mod Patroller Data\Patroller Data\Stage 1

Step 5
--------
Edit "patroller.orc.stage_01.json".
1. "id": Do not change it when overriding an existing patroller.
2. "builtInPrefabAddress": prefab address. Usually do not change it.
3. "statusProfileId": status loadout used by this patroller.
4. "actionRuleProfileId": action rule profile. Advanced; usually do not change it.
5. "senses": detection settings.
6. "taskProviders" or task-related fields: behavior settings. Easy to break.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. Patroller IDs are in Reference_Data.
2. Wrong tasks/senses can make a patroller idle forever or behave strangely.
