Create Or Patch Animation Clip
================================

Important StreamingAssets Warning
--------

`Assets\StreamingAssets` contains many readable game data files, but `Assets\StreamingAssets\AnimationClips` is an edge case. Do not edit files inside `Assets\StreamingAssets\AnimationClips` directly and expect a safe/distributable animation mod.

Use `Assets\StreamingAssets\AnimationClips` only as reference data for built-in `clipId`, `trackId`, timing, and transform values. Your actual animation mod files must live under `Mods\...` with `type: "animation"` in `mod.json`.

This tutorial explains `type: "animation"` mods.

Use this tutorial when you want to create a new animation clip or safely patch one track of an existing clip. For most Nesche body/eyemask/armbinder mods, use `mergeMode: "patch"` so tracks you do not edit stay inherited from the built-in game data.

If your goal is to replace an enemy controller slot, use this tutorial for the clip JSON shape, then use `35_Animation_Clip_Override.md` for the controller slot map.

Read This First
--------

There are two different workflows.

1. Enemy replacement workflow: create a new clip id, then map a controller slot to that clip id with `animationClipOverride`.
2. Nesche patch workflow: keep the built-in `clipId`, set `mergeMode: "patch"`, and include only the track you want to change.

For Nesche, do not copy every track into one giant JSON unless you intentionally want to own every track. A small patch is safer and lets other mods patch other tracks.

Reference files:

~~~text
Mods\_Mod_Tutorial\EN\Reference_Data\Animation_Targets_And_Slots.md
Assets\StreamingAssets\AnimationClips
Assets\StreamingAssets\AnimationController_Detail
~~~

Working example packs:

~~~text
Mods\Mod_Chikuatta\Animations\Example_02_Nesche_Normal_Body_Patch
Mods\Mod_Chikuatta\Animations\Example_03_Nesche_Normal_Eyemask_Patch
Mods\Mod_Chikuatta\Animations\Example_04_Nesche_Armbinder_Body_Patch
Mods\Mod_Chikuatta\Animations\Example_05_Nesche_Armbinder_Eyemask_Patch
~~~

The examples are disabled by default. Rename `mod.template.json` to `mod.json` only when you want to test one example.

Step 1
--------

Create this folder hierarchy under `Mods`.

~~~text
Mods\Mod_YourName\Animations\Nesche Normal Eyemask Patch\Animations
~~~

Replace `Mod_YourName` with your own mod folder name.

Step 2
--------

Copy this template:

~~~text
Mods\mod.json templates\mod.template_actor_targeted.json
~~~

Paste it here:

~~~text
Mods\Mod_YourName\Animations\Nesche Normal Eyemask Patch
~~~

Rename the copied file to:

~~~text
mod.json
~~~

Step 3
--------

Open `mod.json` and use this shape for a safe track patch.

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "animation",
  "target": "Nesche",
  "variant": "",
  "entry": "Animations",
  "mergeMode": "patch"
}
~~~

Field meanings:

1. `type`: use `animation` for animation clip JSON files.
2. `target`: use the animation target id. For Nesche, use `Nesche`.
3. `variant`: empty string means normal/base Nesche.
4. `entry`: must match the folder beside `mod.json`.
5. `mergeMode`: use `patch` when you want omitted tracks to inherit.

Do not use a patroller id as the target. This is wrong:

~~~text
patroller.goblin.stage_06
~~~

Step 4
--------

Choose the exact built-in clip you want to patch.

For normal idle Nesche, use this reference file:

~~~text
Assets\StreamingAssets\AnimationClips\Nesche\Actions\Normal\Idle E_animations.json
~~~

Inside it, find this built-in `clipId`:

~~~text
nesche.actions.normal.nesche_normal_idle_e
~~~

For first tests, do not invent a new `clipId` for Nesche patches. Keep the built-in `clipId` so the game knows which existing clip you are patching.

Step 5
--------

Inside your `Animations` folder, create this file:

~~~text
my_nesche_normal_eyemask_animations.json
~~~

The filename must end with:

~~~text
_animations.json
~~~

If it does not end with `_animations.json`, the loader will not scan it.

Step 6
--------

Put this content into `my_nesche_normal_eyemask_animations.json`.

~~~json
{
  "clips": [
    {
      "name": "Nesche Normal Eyemask Patch",
      "clipId": "nesche.actions.normal.nesche_normal_idle_e",
      "tracks": [
        {
          "trackId": "eyemask",
          "spriteFrames": [
            {
              "spriteName": "My_Eyemask_0",
              "time": 0.0,
              "duration": 0.3
            },
            {
              "spriteName": "My_Eyemask_1",
              "time": 0.3,
              "duration": 0.3
            }
          ]
        }
      ]
    }
  ]
}
~~~

This patch owns only `trackId: "eyemask"`.

Because `mergeMode` is `patch`, the body, mouthpiece, armbinder, leg shackles, and other tracks remain inherited.

Step 7
--------

Create a sprite folder beside the JSON file. For patch mods, use the same folder name as the `clipId`.

~~~text
Mods\Mod_YourName\Animations\Nesche Normal Eyemask Patch
  mod.json
  Animations
    my_nesche_normal_eyemask_animations.json
    nesche.actions.normal.nesche_normal_idle_e
      My_Eyemask_0.png
      My_Eyemask_1.png
~~~

Important rules:

1. The sprite folder name must match the `clipId`.
2. PNG filenames must match `spriteName`.
3. Do not write `.png` inside `spriteName`.
4. `My_Eyemask_0` means the file must be `My_Eyemask_0.png`.

Step 8
--------

Understand track names.

Common track ids include:

1. `body`: main body sprite track.
2. `cloth`: clothing overlay track.
3. `eyemask`: eyemask overlay track.
4. `mouthpiece`: mouthpiece overlay track.
5. `armbinder`: armbinder/restraint overlay track.
6. `yoke`: yoke-related overlay track, depending on the clip.

Do not guess track ids. Open a similar built-in file under `Assets\StreamingAssets\AnimationClips` and copy the exact `trackId`.

Step 9
--------

Understand `spriteFrames`.

Each frame needs:

1. `spriteName`: PNG filename without `.png`.
2. `time`: when this sprite starts, in seconds.
3. `duration`: how long this sprite stays visible, in seconds.

Use the built-in file as timing reference. For first mods, copy the built-in `time` and `duration` values and only change `spriteName` plus PNG files.

Step 10
--------

Understand `transforms`.

A transform key uses this shape:

~~~json
{
  "time": 0.0,
  "value": 0.637
}
~~~

So this is valid:

~~~json
{
  "transforms": {
    "position": {
      "x": [ { "time": 0.0, "value": 0.0 } ],
      "y": [ { "time": 0.0, "value": 0.0 } ],
      "z": []
    }
  }
}
~~~

This is wrong:

~~~text
"position": { "x": [0.5], "y": [0.3], "z": [0] }
~~~

For simple patch mods, you can omit `transforms`. The game will inherit transforms from the built-in/full replacement track.

Step 11
--------

Understand variants.

Nesche has multiple animation variants. The same track name can exist under different variants.

Examples:

1. Normal/base Nesche uses `variant: ""`.
2. Armbinder Nesche uses `variant: "armbinder"`.
3. Cuff/collar/chain style clips can use `variant: "ccc"`.
4. Yoke clips can use `variant: "yoke_bondage"`.

A normal eyemask patch does not affect armbinder clips. An armbinder eyemask patch does not affect normal clips.

If you want an eyemask skin to work for normal and armbinder, make two separate patch mods or two patch JSON files:

1. one for `variant: ""` and the normal `clipId`;
2. one for `variant: "armbinder"` and the armbinder `clipId`.

Step 12
--------

Use the working examples as templates.

Normal body patch:

~~~text
Mods\Mod_Chikuatta\Animations\Example_02_Nesche_Normal_Body_Patch
~~~

Normal eyemask patch:

~~~text
Mods\Mod_Chikuatta\Animations\Example_03_Nesche_Normal_Eyemask_Patch
~~~

Armbinder body patch:

~~~text
Mods\Mod_Chikuatta\Animations\Example_04_Nesche_Armbinder_Body_Patch
~~~

Armbinder eyemask patch:

~~~text
Mods\Mod_Chikuatta\Animations\Example_05_Nesche_Armbinder_Eyemask_Patch
~~~

These examples intentionally include only one track each. That is the mod-friendly shape.

Step 13
--------

Start the game and open:

~~~text
Main Menu -> Mods -> Mod Report
~~~

Then test in gameplay.

Expected results:

1. Mod Report shows the animation mod as imported.
2. Only the intended clip/track changes.
3. Other tracks still appear normally.
4. Other variants stay unchanged unless you patched that variant too.

Common Mistakes
--------

1. Editing `Assets\StreamingAssets\AnimationClips` directly. Use it as reference only.
2. Using a patroller id as `target`. Use an animation target id such as `Nesche` or `Orc_AC Edit`.
3. Forgetting `mergeMode: "patch"` for track-only Nesche mods.
4. Using `variant: ""` and expecting armbinder clips to change.
5. Including every track when you only meant to change `body` or `eyemask`.
6. Writing transform arrays as raw numbers instead of `{ "time": ..., "value": ... }` objects.
7. Mismatching `spriteName` and PNG filename.

