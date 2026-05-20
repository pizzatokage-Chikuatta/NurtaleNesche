Patroller Director Stage Loadout
================================
This tutorial changes which patroller director loadout is used by a stage.

This is a caution target because wrong IDs can disable director spawning.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_Chikuatta\Patrollers\Mod Patroller Director Stage Loadouts\Patroller Director Stage Loadouts

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Patroller Director Stage Loadouts", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "patrollerDirectorStageLoadouts",
  "entry": "Patroller Director Stage Loadouts",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "patrollerDirectorStageLoadouts".
2. "entry" must match the folder name next to mod.json. In this case, it is "Patroller Director Stage Loadouts".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "patroller_director_loadout.stage_03.json" from:
StreamingAssets\Patrollers\Spawn Directors\Stage Loadouts
Paste it into:
Mods\Mod_Chikuatta\Patrollers\Mod Patroller Director Stage Loadouts\Patroller Director Stage Loadouts

Step 5
--------
Edit "patroller_director_loadout.stage_03.json".
1. "id": Do not change it when overriding an existing stage loadout.
2. director IDs: must point to existing patroller director data.
3. stage/spawn point mappings: decide where directors are used.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. This does not define the director itself. It only chooses which director data is used.
2. If nothing changes in-game, check whether the stage actually uses that loadout.

Replacement List Warning
--------

This file is a replacement list, not a small additive patch. If your mod overrides the same "id", the final list is the list written in your mod file. Keep built-in entries you still want to use.

