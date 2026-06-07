Sprite
================================
This tutorial changes normal sprite references, such as item/equipment sprites.

This is a caution target because sprite paths and IDs must match exactly.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_Chikuatta\Sprites\Mod Sprites\Sprites\Equipment

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Sprites", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "sprites",
  "entry": "Sprites",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "sprites".
2. "entry" must match the folder name next to mod.json. In this case, it is "Sprites".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "sprite.equipment.dagger_healthy.json" from:
StreamingAssets\Sprites\Equipment
Paste it into:
Mods\Mod_Chikuatta\Sprites\Mod Sprites\Sprites\Equipment

Step 5
--------
Edit "sprite.equipment.dagger_healthy.json".
1. "id": Do not change it when overriding an existing sprite reference.
2. file/path fields: must point to your PNG file.
3. pixels-per-unit or size fields: keep consistent with built-in data.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. This is for normal sprite references, not JSON animation frame sprites.
2. Animation sprites are more complicated and are in Tutorial_Group_4.

File Dependency Chain
--------

Registering a file and making gameplay use it are separate steps. For example, audio usually flows like this: audio file -> clip ID -> classification/track/loadout -> gameplay reference.

