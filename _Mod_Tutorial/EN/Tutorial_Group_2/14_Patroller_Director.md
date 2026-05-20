Patroller Director
================================
This tutorial changes extra patroller spawning controlled during a stage.

This is a caution target because it can change stage pacing and spawn pressure.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_Chikuatta\Patrollers\Mod Patroller Directors\Patroller Directors\Stage 3

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Patroller Directors", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "patrollerDirectors",
  "entry": "Patroller Directors",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "patrollerDirectors".
2. "entry" must match the folder name next to mod.json. In this case, it is "Patroller Directors".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "patroller_director.01.stage_03.json" from:
StreamingAssets\Patrollers\Spawn Directors\Stage 3
Paste it into:
Mods\Mod_Chikuatta\Patrollers\Mod Patroller Directors\Patroller Directors\Stage 3

Step 5
--------
Edit "patroller_director.01.stage_03.json".
1. "id": Do not change it when overriding an existing director.
2. spawn timing fields: control when extra patrollers can appear.
3. spawn IDs/loadout IDs: must point to existing data.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. If referenced spawn data is missing, the director cannot spawn that patroller.
2. Test in the target stage after changing this.
