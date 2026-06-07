Potion Effect
================================
This tutorial changes what a potion does when used or thrown.

This is a caution target because potion effects can apply statuses or gameplay effects.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_Chikuatta\Lootings\Mod Potion Effects\Potion Effects

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Potion Effects", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "potionEffects",
  "entry": "Potion Effects",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "potionEffects".
2. "entry" must match the folder name next to mod.json. In this case, it is "Potion Effects".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "item.potion.aphrodisia.json" from:
StreamingAssets\Lootings\Items And Equipments\Potions
Paste it into:
Mods\Mod_Chikuatta\Lootings\Mod Potion Effects\Potion Effects

Step 5
--------
Edit "item.potion.aphrodisia.json".
1. "id": Do not change it when overriding an existing potion effect.
2. status/effect IDs: must point to valid status/effect data.
3. duration/value fields: change carefully.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. Status effect IDs and efficacy IDs are in Reference_Data.
2. Potion item data and potion effect data are separate.
