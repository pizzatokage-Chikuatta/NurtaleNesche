BGM Stage Loadout
================================
This tutorial changes which BGM plays in a stage.

This is a caution target because wrong track IDs can make music silent.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_Chikuatta\Audio\Mod BGM Stage Loadouts\BGM Stage Loadouts

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod BGM Stage Loadouts", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "bgmStageLoadouts",
  "entry": "BGM Stage Loadouts",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "bgmStageLoadouts".
2. "entry" must match the folder name next to mod.json. In this case, it is "BGM Stage Loadouts".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "bgm_stage_loadouts.json" from:
StreamingAssets\Audio\BGM\Stage Loadouts
Paste it into:
Mods\Mod_Chikuatta\Audio\Mod BGM Stage Loadouts\BGM Stage Loadouts

Step 5
--------
Edit "bgm_stage_loadouts.json".
1. "id": Do not change it when overriding a stage loadout.
2. track IDs: BGM track IDs used by the stage.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. The BGM track ID must exist.
2. This assigns BGM; it does not define the audio file itself.

Replacement List Warning
--------

This file is a replacement list, not a small additive patch. If your mod overrides the same "id", the final list is the list written in your mod file. Keep built-in entries you still want to use.

File Dependency Chain
--------

Registering a file and making gameplay use it are separate steps. For example, audio usually flows like this: audio file -> clip ID -> classification/track/loadout -> gameplay reference.

