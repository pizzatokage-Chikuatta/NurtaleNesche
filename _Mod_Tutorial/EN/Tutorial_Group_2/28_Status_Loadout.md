Status Loadout
================================
This tutorial changes a list of status effects available to a player or patroller profile.

This is a caution target because missing or wrong status IDs can break restraint/status behavior.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_Chikuatta\Status\Mod Status Loadouts\Status Loadouts

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Status Loadouts", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "statusLoadouts",
  "entry": "Status Loadouts",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "statusLoadouts".
2. "entry" must match the folder name next to mod.json. In this case, it is "Status Loadouts".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "profile_status_effect.player.json" from:
StreamingAssets\Status\Status Loadouts
Paste it into:
Mods\Mod_Chikuatta\Status\Mod Status Loadouts\Status Loadouts

Step 5
--------
Edit "profile_status_effect.player.json".
1. "id": Do not change it when overriding an existing loadout.
2. status effect IDs: statuses included in this loadout.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. Status effect IDs are in Reference_Data.
2. The loadout only lists available statuses. The status metadata defines what each status does.

Replacement List Warning
--------

This file is a replacement list, not a small additive patch. If your mod overrides the same "id", the final list is the list written in your mod file. Keep built-in entries you still want to use.

