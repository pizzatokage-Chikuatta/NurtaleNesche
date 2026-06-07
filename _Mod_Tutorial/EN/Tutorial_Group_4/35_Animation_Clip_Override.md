Animation Clip Override
================================

Important StreamingAssets Warning
--------

`Assets\StreamingAssets` contains many readable game data files, but `Assets\StreamingAssets\AnimationClips` is an edge case. Do not edit files inside `Assets\StreamingAssets\AnimationClips` directly and expect a safe/distributable animation mod.

Use `Assets\StreamingAssets\AnimationClips` only as reference data for built-in `clipId`, `trackId`, timing, and transform values. Your actual animation mod files must live under `Mods\...` with `type: "animation"` or `type: "animationClipOverride"` in `mod.json`.

This tutorial explains `type: "animationClipOverride"` mods.

Use this when you want an existing animator controller slot to play a different clip id. This is the normal enemy animation replacement flow.

If you are patching one Nesche bodypart/accessory track, use `34_Create_New_Animation_Clip.md` with `mergeMode: "patch"` instead.

What You Are Making
--------

This tutorial replaces the Orc walk slot:

~~~text
Orc_WalkRun.walk
~~~

With this new clip id:

~~~text
mod.chikuatta.example.orc_walk_body
~~~

Working example pack:

~~~text
Mods\Mod_Chikuatta\Animations\Example_01_Orc_Body_Walk_Override
~~~

The example is disabled by default. Rename `mod.template.json` to `mod.json` only when you want to test it.

Step 1
--------

Create this folder hierarchy under `Mods`.

~~~text
Mods\Mod_YourName\Animations\Orc Walk Override\Animation Clip Overrides
~~~

Step 2
--------

Copy this template:

~~~text
Mods\mod.json templates\mod.template_actor_targeted.json
~~~

Paste it here:

~~~text
Mods\Mod_YourName\Animations\Orc Walk Override
~~~

Rename it to:

~~~text
mod.json
~~~

Step 3
--------

Open `mod.json` and replace its content with this.

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "animationClipOverride",
  "target": "Orc_AC Edit",
  "variant": "",
  "entry": "Animation Clip Overrides"
}
~~~

Field meanings:

1. `type`: use `animationClipOverride` for controller slot replacement maps.
2. `target`: use the animation target/controller id. For Orc, use `Orc_AC Edit`.
3. `variant`: empty string means the base/default controller variant.
4. `entry`: must match the folder beside `mod.json`.

Do not use a patroller data id as target. This is wrong:

~~~text
patroller.orc.stage_06
~~~

Step 4
--------

Inside `Animation Clip Overrides`, create this file:

~~~text
map.json
~~~

Put this content into `map.json`.

~~~json
{
  "priority": 100,
  "overrides": {
    "items": [
      {
        "clipSlot": "Orc_WalkRun.walk",
        "clipId": "mod.yourname.orc_walk_body"
      }
    ]
  }
}
~~~

Field meanings:

1. `priority`: if multiple mods replace the same slot, higher priority wins.
2. `overrides.items`: list of slot replacements.
3. `clipSlot`: controller slot to replace.
4. `clipId`: animation clip id that should play instead.

Step 5
--------

Understand `clipSlot`.

The format is:

~~~text
stateId.slotName
~~~

So this:

~~~text
Orc_WalkRun.walk
~~~

Means:

1. `stateId` is `Orc_WalkRun`.
2. `slotName` is `walk`.

To find slots, open the built-in controller JSON:

~~~text
Assets\StreamingAssets\AnimationController_Detail\Orc\Orc_AC Edit_controller.json
~~~

Find `stateMotionBindings`. A shape like this creates `Orc_WalkRun.walk` and `Orc_WalkRun.run`:

~~~json
{
  "stateId": "Orc_WalkRun",
  "clips": {
    "walk": "orc.orc_walk",
    "run": "orc.orc_run"
  }
}
~~~

Step 6
--------

Create the replacement animation clip in the same entry folder.

Inside `Animation Clip Overrides`, create this file:

~~~text
my_orc_walk_body_animations.json
~~~

The filename must end with `_animations.json`.

Put a clip with the same `clipId` used by `map.json`:

~~~json
{
  "clips": [
    {
      "name": "mod.yourname.orc_walk_body",
      "clipId": "mod.yourname.orc_walk_body",
      "frameRate": 100.0,
      "pixelsPerUnit": 54.0,
      "loopTime": true,
      "tracks": [
        {
          "trackId": "body",
          "spriteFrames": [
            {
              "spriteName": "My_Orc_Walk_0",
              "time": 0.0,
              "duration": 0.15
            },
            {
              "spriteName": "My_Orc_Walk_1",
              "time": 0.15,
              "duration": 0.15
            }
          ]
        }
      ],
      "events": []
    }
  ]
}
~~~

Step 7
--------

Create a sprite folder matching the new `clipId`.

~~~text
Mods\Mod_YourName\Animations\Orc Walk Override
  mod.json
  Animation Clip Overrides
    map.json
    my_orc_walk_body_animations.json
    mod.yourname.orc_walk_body
      My_Orc_Walk_0.png
      My_Orc_Walk_1.png
~~~

Important rules:

1. The sprite folder name must match `clipId`.
2. PNG filenames must match `spriteName`.
3. Do not write `.png` inside `spriteName`.
4. The clip id in `map.json` must match the clip id in `_animations.json`.

Step 8
--------

For real Orc body replacement, copy timing from the built-in Orc walk reference:

~~~text
Assets\StreamingAssets\AnimationClips\Orc\Orc_Walk_animations.json
~~~

Keep the `body` track timing first. Replace PNGs after the copied version imports correctly.

Step 9
--------

Start the game and open:

~~~text
Main Menu -> Mods -> Mod Report
~~~

Then enter a stage with Orcs and watch the walking animation.

Expected results:

1. Mod Report shows the clip override import.
2. Orc walk uses your replacement sprites.
3. Orc run/idle/attack stay unchanged unless you override those slots too.

Common Mistakes
--------

1. Using `target: "patroller.orc.stage_06"`. Use `Orc_AC Edit`.
2. Using a built-in clip id as `clipSlot`. `clipSlot` is `stateId.slotName`.
3. Writing `clipSlot: "orc.orc_walk"`. Wrong. Use `Orc_WalkRun.walk`.
4. Writing a `clipId` in `map.json` but not providing that clip anywhere.
5. Forgetting `map.json`. `animationClipOverride` requires it.
6. Expecting one slot override to change every Orc animation. Each slot must be mapped separately.
7. Using this flow for a one-track Nesche eyemask patch. Use `mergeMode: "patch"` from tutorial 34 instead.
