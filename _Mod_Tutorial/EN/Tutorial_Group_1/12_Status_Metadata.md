Status Metadata
================================
This tutorial changes status effect metadata, such as restraints, curses, visibility effects, or other status settings.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_{YourName}\Status\Mod Status Metadata\Status Metadata

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Status Metadata", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "statusMetadata",
  "entry": "Status Metadata",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "statusMetadata".
2. "entry" must match the folder name next to mod.json. In this case, it is "Status Metadata".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "status_effect.eyemask.json" from:
StreamingAssets\Status\Status Metadata
Paste it into:
Mods\Mod_Chikuatta\Status\Mod Status Metadata\Status Metadata

Step 5
--------
Edit "status_effect.eyemask.json".
1. "id": do not change it when overriding an existing status effect.
2. "efficacies": effect IDs applied by this status. See Reference_Data.
3. visual/audio/behavior fields: leave unchanged unless you know what they do.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. Status metadata can affect visuals, controls, stamina, movement, or gameplay restrictions.
2. Wrong efficacy IDs can make a status do nothing or do the wrong thing.
