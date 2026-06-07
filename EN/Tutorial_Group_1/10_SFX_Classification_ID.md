SFX Classification ID
================================
This tutorial changes which SFX clip candidates are used for a sound classification.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_Chikuatta\Audio\Sound Effects\Mod Classification ID\Classification ID\Player

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Classification ID", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "classificationID",
  "entry": "Classification ID",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "classificationID".
2. "entry" must match the folder name next to mod.json. In this case, it is "Classification ID".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "sfx.nesche_run.json" from:
StreamingAssets\Audio\Sound Effects\Classification ID\Player
Paste it into:
Mods\Mod_Chikuatta\Audio\Sound Effects\Mod Classification ID\Classification ID\Player

Step 5
--------
Edit "sfx.nesche_run.json".
1. "id": Do not change it when overriding an existing classification ID.
2. "clips": SFX clip candidates used by this classification.
3. "clipId": sfx.clip.* ID registered by audioFiles.
4. "weight": higher weight means this clip is more likely to be selected.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. Register new sfx.clip.* data with audioFiles before referencing it here.
2. The safest workflow is copying an existing classification and changing only "clips".

File Dependency Chain
--------

Registering a file and making gameplay use it are separate steps. For example, audio usually flows like this: audio file -> clip ID -> classification/track/loadout -> gameplay reference.

