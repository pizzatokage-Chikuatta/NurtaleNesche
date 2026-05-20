Animation Clip Override
================================

This tutorial makes an existing controller slot use a different animation clip.

This is the actual replacement step. If you created a new clip in 34_Create_New_Animation_Clip.md, this tutorial shows how to make Goblin use it.

What You Are Making
--------

You will replace this built-in Goblin idle slot:

~~~text
Goblin_Idling.main
~~~

With this clip id:

~~~text
mod.yourname.goblin_idle
~~~

Step 1
--------

Create this folder hierarchy under Mods.

~~~text
Mods\Mod_YourName\Animations\Goblin Idle Override\Animation Clip Overrides
~~~

Step 2
--------

Copy this template:

~~~text
Mods\mod.json templates\mod.template_actor_targeted.json
~~~

Paste it here:

~~~text
Mods\Mod_YourName\Animations\Goblin Idle Override
~~~

Rename it to:

~~~text
mod.json
~~~

Step 3
--------

Open mod.json and replace its content with this.

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "animationClipOverride",
  "target": "Goblin_AC Edit",
  "variant": "",
  "entry": "Animation Clip Overrides"
}
~~~

Field meanings:

1. "type": "animationClipOverride" tells the loader to read map.json.
2. "target": "Goblin_AC Edit" is the animation set id.
3. "entry": "Animation Clip Overrides" must match the folder beside mod.json.

Do not use patroller.goblin as target for animation override mods.

How to find target IDs for other actors:

1. Open StreamingAssets\AnimationController_Detail.
2. Find the folder for the actor you want to edit.
3. Use the controller filename without the _controller.json suffix as the target.
4. Example: Goblin_AC Edit_controller.json means target is Goblin_AC Edit.

The target is an animation set/controller id, not a patroller data id.

Step 4
--------

Inside Animation Clip Overrides, create this file:

~~~text
map.json
~~~

Put this content into map.json.

~~~json
{
  "priority": 100,
  "overrides": {
    "items": [
      {
        "clipSlot": "Goblin_Idling.main",
        "clipId": "mod.yourname.goblin_idle"
      }
    ]
  }
}
~~~

Step 5
--------

Understand map.json.

1. "priority": if multiple mods replace the same slot, higher priority wins.
2. "overrides.items": list of slot replacements.
3. "clipSlot": the controller slot to replace.
4. "clipId": the animation clip id that should play instead.

The clipSlot format is:

~~~text
stateId.slotName
~~~

So this:

~~~text
Goblin_Idling.main
~~~

Means:

1. stateId is Goblin_Idling.
2. slotName is main.

Step 6
--------

Find where Goblin_Idling.main comes from.

Open this built-in controller JSON:

~~~text
StreamingAssets\AnimationController_Detail\Goblin\Goblin_AC Edit_controller.json
~~~

Find "stateMotionBindings".

You will see a shape like this:

~~~json
{
  "stateId": "Goblin_Idling",
  "clips": {
    "main": "goblin.goblin_idling"
  },
  "tuning": {},
  "motionParameters": {}
}
~~~

This creates the clipSlot:

~~~text
Goblin_Idling.main
~~~

More examples:

1. "stateId": "Goblin_WalkRun" plus "walk" gives Goblin_WalkRun.walk.
2. "stateId": "Goblin_WalkRun" plus "run" gives Goblin_WalkRun.run.

Step 7
--------

Make sure the replacement clip exists.

The clip id in map.json must be loaded from somewhere.

Valid options:

1. Use a clip you created in 34_Create_New_Animation_Clip.md.
2. Put an _animations.json file and sprite folder inside this same Animation Clip Overrides folder.
3. Use another already-loaded built-in clip id.

For the simplest workflow, do tutorial 34 first and use the same clipId here:

~~~text
mod.yourname.goblin_idle
~~~

Step 8
--------

If you want this override folder to be self-contained, put the animation clip JSON and sprite folder inside the same folder as map.json.

Final folder shape:

~~~text
Mods\Mod_YourName\Animations\Goblin Idle Override
  mod.json
  Animation Clip Overrides
    map.json
    my_goblin_idle_animations.json
    mod.yourname.goblin_idle
      My_Goblin_Idle_0.png
      My_Goblin_Idle_1.png
~~~

The _animations.json shape is the same as tutorial 34.

Step 9
--------

Start the game and open:

~~~text
Main Menu -> Mods -> Mod Report
~~~

Then enter a stage with Goblins and check their idle animation.

Troubleshooting
--------

1. Mod imports but nothing changes: target is probably wrong, or another mod with higher priority wins.
2. Mod report says missing map.json: map.json is not directly inside the entry folder.
3. Animation source exists but cannot be built: clipId exists, but the PNG/spriteName/track data is wrong.
4. Slot does not change: clipSlot is wrong. Re-check stateId.slotName in the controller JSON.
5. Goblin disappears: replacement clip cannot render, usually because sprites or track IDs are missing.

Common Mistakes
--------

1. Using "clipSlot": "goblin.goblin_idling". Wrong. clipSlot is stateId.slotName.
2. Using "target": "patroller.goblin". Wrong. Use Goblin_AC Edit.
3. Writing a clipId in map.json that no animation mod provides.
4. Forgetting map.json. animationClipOverride requires map.json.
5. Lowering priority and losing to another mod that overrides the same slot.
