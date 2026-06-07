SFX Audio File
================================
This tutorial imports a new sound effect file, such as .ogg or .wav.
This alone does not make the sound play. Use the SFX Classification ID tutorial to assign it to a sound event.

Step 1
--------
Create the following folder hierarchy under the "Mods" folder.
Mods\Mod_{YourName}\Audio\Sound Effects\Mod SFX\Audio Files\Nesche\Footsteps

Step 2
--------
Copy "mod.template_folder_backed.json" from "Mods\mod.json templates", paste it into "Mod SFX", and rename it to "mod.json".

Step 3
--------
Open mod.json and change its content to this:
```json
{
  "experimentalApiVersion": "v1",
  "type": "audioFiles",
  "entry": "Audio Files",
  "overwriteExisting": true
}
```
1. For this tutorial, "type" must be "audioFiles".
2. "entry" must match the folder name next to mod.json. In this case, it is "Audio Files".

Step 4
--------
Put your .ogg or .wav file here:
Mods\Mod_Chikuatta\Audio\Sound Effects\Mod SFX\Audio Files\Nesche\Footsteps

Example:
My_Run_1.ogg

Step 5
--------
Create a JSON file in the same folder, such as "sfx.clip.nesche_run_1.json".
```json
{
  "id": "sfx.clip.nesche_run_1",
  "file": "My_Run_1.ogg"
}
```
1. "id": SFX clip ID. If this matches an existing sfx.clip.* ID, it overrides that clip.
2. "file": actual .ogg/.wav file path. If the audio file is in the same folder, the file name is enough.
3. Beginners should always write "id" explicitly.

Finally, open the game main menu -> Mods -> Mod Report and check whether the mod was imported correctly.

Notes
--------
1. "audioFiles" registers sound clips only.
2. Which moment uses the sound is controlled by "classificationID".
3. To replace an existing sound clip, use the same existing sfx.clip.* ID.

File Dependency Chain
--------

Registering a file and making gameplay use it are separate steps. For example, audio usually flows like this: audio file -> clip ID -> classification/track/loadout -> gameplay reference.

