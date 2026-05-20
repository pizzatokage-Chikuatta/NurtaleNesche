Create New Animation Clip
================================

This tutorial creates and registers a new Goblin animation clip from JSON and PNG files.

Important: this tutorial does not replace an in-game animation by itself. It only creates a new clip id that the game can load. To make Goblin actually use this clip, use 35_Animation_Clip_Override.md after this.

What You Are Making
--------

You will make a new animation clip with this id:

~~~text
mod.yourname.goblin_idle
~~~

The clip will use two PNG files:

~~~text
My_Goblin_Idle_0.png
My_Goblin_Idle_1.png
~~~

Step 1
--------

Create this folder hierarchy under Mods.

~~~text
Mods\Mod_YourName\Animations\Goblin Idle Clip\Animations
~~~

Replace Mod_YourName with your own mod folder name.

Step 2
--------

Copy this template:

~~~text
Mods\mod.json templates\mod.template_actor_targeted.json
~~~

Paste it here:

~~~text
Mods\Mod_YourName\Animations\Goblin Idle Clip
~~~

Rename the copied file to:

~~~text
mod.json
~~~

Step 3
--------

Open mod.json and replace its content with this.

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "animation",
  "target": "Goblin_AC Edit",
  "variant": "",
  "entry": "Animations"
}
~~~

Field meanings:

1. "experimentalApiVersion": v1 experimental mod API marker.
2. "type": "animation" tells the loader to scan animation clip JSON files.
3. "target": "Goblin_AC Edit" is the animation set id. It is not the patroller id.
4. "variant": "" means the base/default variant.
5. "entry": "Animations" must match the folder beside mod.json.

Do not use this as the target:

~~~text
patroller.goblin
~~~

That is a patroller data id, not an animation set id.

Step 4
--------

Inside the Animations folder, create this JSON file:

~~~text
my_goblin_idle_animations.json
~~~

The filename must end with:

~~~text
_animations.json
~~~

If it does not end with _animations.json, the loader will not scan it.

Safer workflow for real mods:

1. Open a similar built-in file under StreamingAssets\AnimationClips.
2. Copy that file into your mod folder.
3. Rename it so it still ends with _animations.json.
4. Change the clipId, sprite folder name, spriteName values, and PNG files.
5. Keep the JSON shape the same until your first test passes.

This is safer than writing a large animation JSON from zero.

Step 5
--------

Put this content into my_goblin_idle_animations.json.

~~~json
{
  "clips": [
    {
      "name": "mod.yourname.goblin_idle",
      "clipId": "mod.yourname.goblin_idle",
      "frameRate": 100.0,
      "pixelsPerUnit": 54.0,
      "loopTime": true,
      "tracks": [
        {
          "trackId": "body",
          "spriteFrames": [
            {
              "spriteName": "My_Goblin_Idle_0",
              "time": 0.0,
              "duration": 0.3
            },
            {
              "spriteName": "My_Goblin_Idle_1",
              "time": 0.3,
              "duration": 0.3
            }
          ],
          "transforms": {
            "position": { "x": [], "y": [], "z": [] },
            "rotation": { "x": [], "y": [], "z": [] },
            "scale": { "x": [], "y": [], "z": [] },
            "color": { "r": [], "g": [], "b": [], "a": [] }
          }
        }
      ],
      "transforms": {
        "position": { "x": [], "y": [], "z": [] },
        "rotation": { "x": [], "y": [], "z": [] },
        "scale": { "x": [], "y": [], "z": [] },
        "color": { "r": [], "g": [], "b": [], "a": [] }
      },
      "events": []
    }
  ]
}
~~~

Step 6
--------

Inside the Animations folder, create a sprite folder with exactly this name:

~~~text
mod.yourname.goblin_idle
~~~

Put your PNG files inside that folder:

~~~text
My_Goblin_Idle_0.png
My_Goblin_Idle_1.png
~~~

Your final folder should look like this:

~~~text
Mods\Mod_YourName\Animations\Goblin Idle Clip
  mod.json
  Animations
    my_goblin_idle_animations.json
    mod.yourname.goblin_idle
      My_Goblin_Idle_0.png
      My_Goblin_Idle_1.png
~~~

Important rules:

1. The sprite folder name must match "clipId" or "name".
2. The PNG filenames must match "spriteName".
3. Do not write .png inside "spriteName".
4. "My_Goblin_Idle_0" means the file must be My_Goblin_Idle_0.png.

Step 7
--------

Check the basic animation fields.

1. "clips": list of animation clips in this JSON file.
2. "name": readable clip name. For first mods, make it the same as clipId.
3. "clipId": the real id used by override maps and controller data.
4. "frameRate": runtime frame rate metadata.
5. "pixelsPerUnit": sprite scale. Goblin body clips should start with 54.0.
6. "loopTime": true makes the clip loop.
7. "tracks": visual renderer tracks inside this animation.
8. "trackId": the renderer track this timeline controls.
9. "spriteFrames": the frame timeline for that track.
10. "time": frame start time in seconds.
11. "duration": how long that sprite stays visible.
12. "events": old-style animation event receiver calls. Leave empty for a first clip.

Track IDs are not arbitrary names. They must match renderer tracks that exist on the target animation set.

Common trackId candidates:

~~~text
body
cloth
eyemask
mouthpiece
armbinder
leg_shackles
rotor
shadow
~~~

Important examples:

1. For this Goblin tutorial, use "body". Goblin normally has a body renderer track, not player restraint overlay tracks.
2. For Player or friendly captive girl animation clips, "cloth", "eyemask", "mouthpiece", "armbinder", and similar overlay tracks can be valid if that target's animation runtime has those tracks.
3. For other patrollers, only use the tracks that their animation set actually defines.
4. If you add "eyemask" to a Goblin clip but Goblin has no eyemask renderer track, the game cannot render that track correctly.

Think of "clipId" as the animation's id, and "trackId" as the visual layer inside that animation.

How to find valid trackId values:

1. Open a built-in animation JSON for the same actor or a similar actor.
2. Search for "trackId".
3. Copy only track IDs that already appear in that actor's working animation files.
4. For Player/friendly captive overlay tracks, also check the matching animation track layout data in the reference files.

If the Mod Report says a track sequence is missing, first check the trackId spelling and the spriteName/PNG names.

Step 8
--------

Understand transform arrays before editing them.

This shape is valid and means "do not animate position":

~~~text
"position": { "x": [], "y": [], "z": [] }
~~~

This shape is invalid:

~~~text
"position": { "x": [0.5], "y": [0.3], "z": [0] }
~~~

The arrays must contain key objects, not raw numbers.

Correct shape:

~~~text
"position": {
  "x": [
    { "time": 0.0, "value": 0.0 },
    { "time": 0.3, "value": 0.05 }
  ],
  "y": [
    { "time": 0.0, "value": 0.0 }
  ],
  "z": []
}
~~~

Each key object has:

1. "time": time in seconds.
2. "value": value at that time.
3. "inSlope": optional. Do not use for first mods.
4. "outSlope": optional. Do not use for first mods.

Step 9
--------

Understand what each transform channel means.

1. "position": local Unity units.
2. "rotation": local Euler rotation in degrees.
3. "scale": local scale multiplier.
4. "color": sprite color, from 0.0 to 1.0.

Example: move the body track slightly right after 0.3 seconds:

~~~text
"transforms": {
  "position": {
    "x": [
      { "time": 0.0, "value": 0.0 },
      { "time": 0.3, "value": 0.05 }
    ],
    "y": [],
    "z": []
  },
  "rotation": { "x": [], "y": [], "z": [] },
  "scale": { "x": [], "y": [], "z": [] },
  "color": { "r": [], "g": [], "b": [], "a": [] }
}
~~~

Example: make the body track half transparent at the start:

~~~text
"color": {
  "r": [],
  "g": [],
  "b": [],
  "a": [
    { "time": 0.0, "value": 0.5 }
  ]
}
~~~

Empty arrays keep the default runtime/prefab value.

Step 10
--------

Understand top-level transforms versus track transforms.

Track-level transforms are inside each track:

~~~text
"tracks": [
  {
    "trackId": "body",
    "transforms": {}
  }
]
~~~

They affect that track renderer only.

Top-level transforms are directly under the clip:

~~~text
"transforms": {}
~~~

They affect the animation host/root presentation object. For first Goblin body mods, prefer track-level transforms or leave all transforms empty.

Step 11
--------

Start the game and open:

~~~text
Main Menu -> Mods -> Mod Report
~~~

The animation mod should be imported without errors.

Important: Goblin will not visually change yet. This clip is only registered. Use 35_Animation_Clip_Override.md to make Goblin use this clip.

Common Mistakes
--------

1. Using patroller.goblin as target. Use Goblin_AC Edit.
2. Naming the JSON my_goblin_idle.json. It must end with _animations.json.
3. Putting PNG files directly beside the JSON. Put them in a folder named like the clipId.
4. Writing "spriteName": "My_Goblin_Idle_0.png". Do not include .png.
5. Filling transform arrays with raw numbers. Use key objects with "time" and "value".
6. Using a trackId that does not exist on the target animation set, such as "eyemask" on a Goblin body-only clip.
7. Expecting this clip to replace Goblin idle by itself. Use the override tutorial next.
