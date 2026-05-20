Player Stats
================================
This tutorial changes the player base stats.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_Chikuatta\Player\Mod Player Stats\Player Stats

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Player Stats", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "playerStats",
  "entry": "Player Stats",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "playerStats".
2. "entry" must match the folder name next to mod.json. In this case, it is "Player Stats".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "player_stats.nesche_default.json" from:
StreamingAssets\Player\Stats
Paste it into:
Mods\Mod_Chikuatta\Player\Mod Player Stats\Player Stats

Step 5
--------
Edit "player_stats.nesche_default.json".
1. "id": Do not change it when overriding the built-in player stats.
2. HP/stamina/speed-like fields: edit carefully and test in-game.
3. Unknown fields should be left unchanged.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. This can affect game balance strongly.
2. Change one value at a time and test.
