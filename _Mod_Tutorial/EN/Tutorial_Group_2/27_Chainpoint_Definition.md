Chainpoint Definition
================================
This tutorial changes chainpoint definition data.

This is a caution target because chainpoints can depend on scene/prefab/runtime behavior.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_Chikuatta\Chainpoints\Mod Chainpoint Definitions\Chainpoint Definitions

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Chainpoint Definitions", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "chainPointDefinitions",
  "entry": "Chainpoint Definitions",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "chainPointDefinitions".
2. "entry" must match the folder name next to mod.json. In this case, it is "Chainpoint Definitions".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "chainpoint.kabe_manguri.json" from:
StreamingAssets\Chainpoints\Definitions
Paste it into:
Mods\Mod_Chikuatta\Chainpoints\Mod Chainpoint Definitions\Chainpoint Definitions

Step 5
--------
Edit "chainpoint.kabe_manguri.json".
1. "id": Do not change it when overriding an existing chainpoint definition.
2. prefab/behavior/status fields: must match built-in supported chainpoint behavior.
3. unknown runtime fields: leave unchanged.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. Not every chainpoint type is safe to spawn at runtime.
2. For spawning, see Chainpoint Spawn and Chainpoint Spawn Loadout.
