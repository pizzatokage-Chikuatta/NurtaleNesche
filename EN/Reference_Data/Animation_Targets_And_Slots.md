Animation Targets And Slots
================================

Use this file when an animation tutorial asks for `target`, `variant`, `clipSlot`, `clipId`, or `trackId`.

Important StreamingAssets Warning
--------

`Assets\StreamingAssets` contains many readable game data files, but `Assets\StreamingAssets\AnimationClips` is an edge case. Do not edit files inside `Assets\StreamingAssets\AnimationClips` directly and expect a safe/distributable animation mod.

Use `Assets\StreamingAssets\AnimationClips` only as reference data for built-in `clipId`, `trackId`, timing, and transform values. Your actual animation mod files must live under `Mods\...`.

Target Is Not Patroller ID
--------

Do not use gameplay ids such as:

~~~text
patroller.goblin.stage_06
patroller.orc.stage_01
~~~

Animation mods use animation target ids such as:

~~~text
Orc_AC Edit
Goblin_AC Edit
Nesche
~~~

Common Targets
--------

| Purpose | target | variant |
| --- | --- | --- |
| Orc base animation/controller | `Orc_AC Edit` | `""` |
| Goblin base animation/controller | `Goblin_AC Edit` | `""` |
| Nesche normal/base action clips | `Nesche` | `""` |
| Nesche armbinder action clips | `Nesche` | `"armbinder"` |
| Nesche cuff-collar-chain / CWC clips | `Nesche` | `"ccc"` |
| Nesche yoke bondage clips | `Nesche` | `"yoke_bondage"` |

Common Clip Slots
--------

`clipSlot` is used by `type: "animationClipOverride"`.

To build a `clipSlot`, combine:

~~~text
stateId.clipSlotName
~~~

Example from Orc:

| Controller | State | Slot | Final clipSlot | Built-in clipId |
| --- | --- | --- | --- | --- |
| `Orc_AC Edit` | `Orc_WalkRun` | `walk` | `Orc_WalkRun.walk` | `orc.orc_walk` |
| `Orc_AC Edit` | `Orc_WalkRun` | `run` | `Orc_WalkRun.run` | `orc.orc_run` |

Example from Nesche:

| Target | Variant | State | Slot | Final clipSlot | Built-in clipId |
| --- | --- | --- | --- | --- | --- |
| `Nesche` | `""` | `Idle` | `main` | `Idle.main` | `nesche.actions.normal.nesche_normal_idle_e` |
| `Nesche` | `""` | `Walk_Run` | `walk` | `Walk_Run.walk` | `nesche.actions.normal.walk_edit.nesche_normal_walk_edit2` |
| `Nesche` | `"armbinder"` | `Idle` | `main` | `Idle.main` | `nesche.actions.armbinder.nesche_armbinder_idle_e` |
| `Nesche` | `"armbinder"` | `Walk_Run` | `walk` | `Walk_Run.walk` | `nesche.actions.armbinder.nesche_armbinder_walk` |

Recommended Nesche Patch Shape
--------

For Nesche body/overlay track edits, prefer `type: "animation"` plus `mergeMode: "patch"`.

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "animation",
  "target": "Nesche",
  "variant": "armbinder",
  "entry": "Animations",
  "mergeMode": "patch"
}
~~~

Patch only the track you want to change:

~~~json
{
  "clips": [
    {
      "clipId": "nesche.actions.armbinder.nesche_armbinder_idle_e",
      "tracks": [
        {
          "trackId": "eyemask",
          "spriteFrames": [
            { "spriteName": "MyEyemask_0", "time": 0.0, "duration": 0.24 }
          ]
        }
      ]
    }
  ]
}
~~~

Omitted tracks inherit from the built-in/full replacement clip.

Variant Rule
--------

A patch for `variant: ""` affects only base/normal player clips.

A patch for `variant: "armbinder"` affects only armbinder player clips.

If you want an eyemask skin to cover normal, armbinder, yoke, and other variants, make separate patches for each variant and clip that should show it.