Action Rules
================================
This tutorial changes condition rules that allow or block ActionScheduler2 behavior.

This is an advanced JSON target. It is not recommended as your first mod.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_Chikuatta\Advanced\Mod Action Rules\Action Rules

1. "Mod_Chikuatta" is only an example. Use your own mod folder name.

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Action Rules", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "actionRules",
  "entry": "Action Rules",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "actionRules".
2. "entry" must match the folder name next to mod.json. In this case, it is "Action Rules".
3. "overwriteExisting": true means your mod can override built-in data with the same "id".

Step 4
--------
Copy "profile.player.action_rules.json" from:
StreamingAssets\Action\Action Rules
Paste it into:
Mods\Mod_Chikuatta\Advanced\Mod Action Rules\Action Rules

Step 5
--------
Edit "profile.player.action_rules.json".
1. "profileId": target action rule profile ID.
2. "rules": list of rule definitions.
3. "passCheckId": condition checked by this rule.
4. "blockIfAny"/"blockIfMissing"/"blockIfNeither": blocking conditions.
5. "forcePassIfAny": conditions that force the rule to pass.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. This directly affects AI/action behavior.
2. Change one rule at a time and test.
3. Wrong action rules can make valid tasks never run.

Replacement List Warning
--------

This file is a replacement list, not a small additive patch. If your mod overrides the same "id", the final list is the list written in your mod file. Keep built-in entries you still want to use.

Copy-First Rule
--------

For this advanced target, copy a built-in file first and change one field at a time. Creating this JSON from empty is risky because hidden links to runtime code or other registries can be easy to miss.

