Chainpoint Spawn
================================
This tutorial creates a chainpoint spawn that is used when a stage starts.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_{YourName}\Chainpoints\Mod Initial Spawns\Initial Spawns

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod Initial Spawns", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "chainPointSpawns",
  "entry": "Initial Spawns",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "chainPointSpawns".
2. "entry" must match the folder name next to mod.json. In this case, it is "Initial Spawns".

Step 4
--------
If you have an existing chainpoint spawn JSON, copy it. If you are adding a new spawn, create a JSON like this:
```json
{
  "id": "chainpoint_spawn.kabe_manguri.stage_03.001",
  "checkpointUuid": "write a new GUID here",
  "enabled": true,
  "regionId": "5e944ee6-e478-4e41-ba0c-ece73dd377f9",
  "offsetX": 0,
  "heightFromGround": 2.8,
  "rotationY": 0,
  "chainPointId": "chainpoint.kabe_manguri"
}
```

Step 5
--------
Edit the chainpoint spawn JSON.
1. "id": spawn data ID. Use a unique ID when adding a new spawn.
2. "checkpointUuid": GUID used by this placed chainpoint. Use a new GUID for new spawns.
3. "enabled": if false, this chainpoint will not spawn.
4. "regionId": region UUID where this chainpoint spawns. See Reference_Data.
5. "offsetX": horizontal offset from the region center.
6. "heightFromGround": height from the ground.
7. "rotationY": Y rotation. Use this when you need to flip direction.
8. "chainPointId": chainpoint definition ID, such as "chainpoint.kabe_manguri".
9. "statusId", "typeEnum", and "isNeedLockPickToEscape": advanced optional fields. Usually omit them.

How To Generate A GUID
--------
Open Windows PowerShell and run this:
```powershell
[guid]::NewGuid().ToString()
```
Put the generated text into "checkpointUuid".

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. Creating a chainpoint spawn JSON alone may not spawn it. Add its "id" to a Chainpoint Spawn Loadout too.
2. Valid "chainPointId" values can be checked in StreamingAssets\ChainPoints\Definitions.
3. Start with normal runtime-spawnable chainpoints such as "chainpoint.kabe_manguri".
