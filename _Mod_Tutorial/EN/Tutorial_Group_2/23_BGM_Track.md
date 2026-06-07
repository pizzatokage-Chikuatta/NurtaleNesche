BGM Track
================================
This tutorial changes or adds a BGM track ID and audio reference.

This is a caution target because audio file paths and track IDs must match.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_Chikuatta\Audio\Mod BGM Tracks\BGM Tracks

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod BGM Tracks", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "bgmTracks",
  "entry": "BGM Tracks",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "bgmTracks".
2. "entry" must match the folder name next to mod.json. In this case, it is "BGM Tracks".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "bgm_tracks.json" from:
StreamingAssets\Audio\BGM\Tracks
Paste it into:
Mods\Mod_Chikuatta\Audio\Mod BGM Tracks\BGM Tracks

Step 5
--------
Edit "bgm_tracks.json".
1. "id": Do not change it when overriding an existing track.
2. audio file/path fields: must point to an existing audio file.
3. volume/loop fields: edit carefully.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. Changing a BGM track does not automatically assign it to a stage.
2. Use BGM Stage Loadout for stage assignment.

File Dependency Chain
--------

Registering a file and making gameplay use it are separate steps. For example, audio usually flows like this: audio file -> clip ID -> classification/track/loadout -> gameplay reference.

