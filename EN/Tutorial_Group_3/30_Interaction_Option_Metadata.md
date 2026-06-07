Interaction Option Metadata
================================
This tutorial changes display text/order for interaction options. It does not create new behavior by itself.

This is an advanced JSON target. It is not recommended as your first mod.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_Chikuatta\Advanced\Mod Interaction Option Metadata\Interaction Option Metadata

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Interaction Option Metadata", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "interactionOptionMeta",
  "entry": "Interaction Option Metadata",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "interactionOptionMeta".
2. "entry" must match the folder name next to mod.json. In this case, it is "Interaction Option Metadata".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "item_interaction_options.json" from:
StreamingAssets\Interaction\Option Metadata
Paste it into:
Mods\Mod_Chikuatta\Advanced\Mod Interaction Option Metadata\Interaction Option Metadata

Step 5
--------
Edit "item_interaction_options.json".
1. "id": interaction option ID.
2. language text fields: displayed option text.
3. "order": display order.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. This is display metadata only.
2. The actual command/behavior must already exist.

Copy-First Rule
--------

For this advanced target, copy a built-in file first and change one field at a time. Creating this JSON from empty is risky because hidden links to runtime code or other registries can be easy to miss.

