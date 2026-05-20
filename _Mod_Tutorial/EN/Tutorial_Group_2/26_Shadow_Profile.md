Shadow Profile
================================
This tutorial changes shadow appearance profiles.

This is a caution target because wrong values can make shadows look detached or invisible.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_Chikuatta\Patrollers\Mod Shadow Profiles\Shadow Profiles

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Shadow Profiles", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "shadowProfiles",
  "entry": "Shadow Profiles",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "shadowProfiles".
2. "entry" must match the folder name next to mod.json. In this case, it is "Shadow Profiles".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "Goblin_ShadowPropertySO.json" from:
StreamingAssets\Shadow\Shadow Property Json
Paste it into:
Mods\Mod_Chikuatta\Patrollers\Mod Shadow Profiles\Shadow Profiles

Step 5
--------
Edit "Goblin_ShadowPropertySO.json".
1. "id": Do not change it when overriding an existing shadow profile.
2. offset/scale/alpha fields: control shadow position, size, and visibility.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. PatrollerData may reference a shadow profile ID.
2. If no object uses the profile ID, changing it will not show in-game.
