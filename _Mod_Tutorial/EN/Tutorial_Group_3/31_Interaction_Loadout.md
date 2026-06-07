Interaction Loadout
================================
This tutorial changes which interaction options are available on objects such as chainpoints or items.

This is an advanced JSON target. It is not recommended as your first mod.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_Chikuatta\Advanced\Mod Interaction Loadouts\Interaction Loadouts\ChainPoints

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Interaction Loadouts", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "interactionLoadouts",
  "entry": "Interaction Loadouts",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "interactionLoadouts".
2. "entry" must match the folder name next to mod.json. In this case, it is "Interaction Loadouts".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "ChainPoints\guillotine.json" from:
StreamingAssets\Interaction\Loadouts
Paste it into:
Mods\Mod_Chikuatta\Advanced\Mod Interaction Loadouts\Interaction Loadouts

Step 5
--------
Edit "ChainPoints\guillotine.json".
1. "id": loadout ID.
2. command/option IDs: interaction options included in this loadout.
3. condition fields: display/use conditions if present.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. Adding an option ID does not create a new behavior.
2. Missing option IDs can make nothing appear.

Replacement List Warning
--------

This file is a replacement list, not a small additive patch. If your mod overrides the same "id", the final list is the list written in your mod file. Keep built-in entries you still want to use.

Copy-First Rule
--------

For this advanced target, copy a built-in file first and change one field at a time. Creating this JSON from empty is risky because hidden links to runtime code or other registries can be easy to miss.

