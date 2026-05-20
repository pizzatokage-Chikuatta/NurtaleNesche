Nurtale Nesche Mod Tutorial
================================

This folder is a tutorial package for making mods. This folder itself is not a mod that should be loaded by the game.

Read This First
--------

1. Start with "Tutorial_Group_1".
2. Before choosing "type" in mod.json, read "Reference_Data/00_Mod_Manifest_And_Registry_Map.md".
3. If you need item IDs, equipment IDs, region UUIDs, or other IDs, check "Reference_Data".
4. Mods that are useful but easier to break are in "Tutorial_Group_2".
5. Advanced JSON targets such as Action Rules, Interaction, and Animation Track Layouts are in "Tutorial_Group_3".
6. Animation and language-file mods are in "Tutorial_Group_4".
7. Code/DLL mods are in "Tutorial_Group_5". Use these only when JSON/PNG mods are not enough.
8. Unity Editor AssetBundle prefab mods are in "Tutorial_Group_6". Use these only after you understand normal JSON mods.

Folder Guide
--------

1. "Tutorial_Group_1": safer beginner JSON/audio tutorials.
2. "Tutorial_Group_2": usable tutorials that need extra caution.
3. "Tutorial_Group_3": advanced JSON tutorials.
4. "Tutorial_Group_4": advanced animation and language-file tutorials.
5. "Tutorial_Group_5": experimental code/DLL mod tutorials.
6. "Tutorial_Group_6": experimental Unity Editor AssetBundle prefab tutorials.
7. "Reference_Data": mod.json type map, ID lists for items, equipment, patrollers, status effects, region UUIDs, and more.

Code/DLL Reading Order
--------

1. 40_Code_Mod_Overview.md
2. 41_Task_Provider_Code_Mod.md
3. 43_Built_In_Task_System_Architecture.md
4. 42_Code_Mod_Manifest_Reference.md

Unity Editor AssetBundle Reading Order
--------

1. 50_Unity_Editor_Asset_Mod_Overview.md
2. 51_Patroller_Prefab_Mod.md
3. 52_Chainpoint_Prefab_Mod.md

Basic Workflow
--------

1. Check "Reference_Data/00_Mod_Manifest_And_Registry_Map.md" if you are unsure which mod.json "type" to use.
2. Copy a template from "mod.json templates".
3. Create the folder hierarchy shown in the tutorial.
4. Copy the original JSON from StreamingAssets into your mod folder under Mods when the tutorial tells you to do so.
5. Edit the JSON, CSV, PNG, audio file, DLL-related file, or AssetBundle file shown in the tutorial.
6. Open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Global ID Rule
--------

If a field references another ID, do not invent it. Use Reference_Data, built-in JSON, or another JSON file you created in your mod. Only new top-level mod data IDs and generated GUIDs should be invented.

Code Mod Warning
--------

Code/DLL mods are trusted local code. The game does not sandbox them. They are v1 experimental and can break after game updates. DLL changes usually require restarting the game.

AssetBundle Prefab Warning
--------

Unity Editor AssetBundle prefab mods are advanced. Raw .prefab files are not loaded by the built game. You must build an AssetBundle and connect it with JSON. Start by importing the sample prefab package from "../Developer Reference Packs/assetbundle_prefab_reference_pack" and modifying a known working prefab shape before making a completely original one.

Notes
--------

1. The v1 mod API is experimental. Game updates can change mod data shape.
2. Some IDs must not be changed when you are overriding built-in data. Read each tutorial before editing IDs.
3. The files in "Reference_Data" are generated from the current game data.
4. This "_Mod_Tutorial" folder is documentation. Do not treat it as your actual mod folder.
