Animation Track Layout
================================
This tutorial changes how JSON animation tracks map to SpriteRenderers.

This is an advanced JSON target. It is not recommended as your first mod.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_Chikuatta\Advanced\Mod Animation Track Layouts\Animation Track Layouts

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Animation Track Layouts", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "animationTrackLayouts",
  "entry": "Animation Track Layouts",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "animationTrackLayouts".
2. "entry" must match the folder name next to mod.json. In this case, it is "Animation Track Layouts".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "nesche_player_track_layout.json" from:
StreamingAssets\AnimationTrackLayouts
Paste it into:
Mods\Mod_Chikuatta\Advanced\Mod Animation Track Layouts\Animation Track Layouts

Step 5
--------
Edit "nesche_player_track_layout.json".
1. "id": track layout ID.
2. "tracks": track definitions.
3. "trackId": ID used by animation JSON.
4. renderer path fields: path to the SpriteRenderer.
5. visibility fields: default track visibility.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. Wrong renderer paths can make sprites invisible.
2. Do not casually copy layouts between different characters.
3. This is usually not the first animation-related file to edit.

Copy-First Rule
--------

For this advanced target, copy a built-in file first and change one field at a time. Creating this JSON from empty is risky because hidden links to runtime code or other registries can be easy to miss.

