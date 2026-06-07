Animator Controller
================================

This tutorial explains the safest way to edit an animator controller JSON.

Use this only when animationClipOverride is not enough. For normal animation replacement, use 35_Animation_Clip_Override.md instead.

Before editing, check:

~~~text
Mods\_Mod_Tutorial\EN\Reference_Data\Animation_Targets_And_Slots.md
~~~

`target` must be an animation target/controller id such as `Goblin_AC Edit` or `Orc_AC Edit`. It is not a patroller data id. The actor-targeted template may contain a different sample target, so always change it to the controller you are actually editing.

What This Mod Type Does
--------

"animatorController" changes the controller definition for an animation set.

It can change:

1. which clip id is bound to a state;
2. blend tree children;
3. parameters;
4. layers;
5. states;
6. transitions.

This is powerful and easy to break.

Step 1
--------

Create this folder hierarchy under Mods.

~~~text
Mods\Mod_YourName\Animations\Goblin Controller\Animator Controllers
~~~

Step 2
--------

Copy this template:

~~~text
Mods\mod.json templates\mod.template_actor_targeted.json
~~~

Paste it here:

~~~text
Mods\Mod_YourName\Animations\Goblin Controller
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
  "type": "animatorController",
  "target": "Goblin_AC Edit",
  "variant": "",
  "entry": "Animator Controllers"
}
~~~

Step 4
--------

Copy this built-in controller file:

~~~text
StreamingAssets\AnimationController_Detail\Goblin\Goblin_AC Edit_controller.json
~~~

Paste it into:

~~~text
Mods\Mod_YourName\Animations\Goblin Controller\Animator Controllers
~~~

The filename must end with:

~~~text
_controller.json
~~~

Otherwise the loader will not scan it.

Important: animatorController mods are full controller replacements. They are not partial patches. If you copy the built-in controller and delete sections you do not understand, those sections are gone from the final controller.

Step 5
--------

For the first test, change only one clip id.

Find this entry:

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

Change only "goblin.goblin_idling":

~~~json
{
  "stateId": "Goblin_Idling",
  "clips": {
    "main": "mod.yourname.goblin_idle"
  },
  "tuning": {},
  "motionParameters": {}
}
~~~

Step 6
--------

If the controller also has a detailed "states" entry for Goblin_Idling, change its "clipId" too.

Example:

~~~json
{
  "id": "Goblin_Idling",
  "clipId": "mod.yourname.goblin_idle"
}
~~~

Do not delete the other fields from the state.

Step 7
--------

Make sure the new clip id exists.

The controller mod does not create animation clips. If you use:

~~~text
mod.yourname.goblin_idle
~~~

You also need an animation clip mod that provides that clip id.

Step 8
--------

Understand the important controller fields.

1. "id": controller id.
2. "entryStateId": first state when this controller starts.
3. "stateMotionBindings": practical state-to-clip-slot bindings.
4. "stateId": state key used by runtime code and bindings.
5. "clips": slot name to clipId map.
6. "parameters": runtime values such as speed.
7. "layers": state grouping.
8. "states": detailed state definitions.
9. "blendTree": blend state data for walk/run style states.
10. "transitions": automatic state changes.

Step 9
--------

Start the game and open:

~~~text
Main Menu -> Mods -> Mod Report
~~~

Then enter a stage with Goblins.

Safe Editing Rule
--------

For the first controller mod:

1. Do not rename state ids.
2. Do not delete parameters.
3. Do not delete transitions.
4. Do not delete Goblin_WalkRun.
5. Change one clip id only.

Do Not Edit Yet
--------

Avoid these fields until you already have a working controller mod:

1. "parameters": runtime code can depend on these names.
2. "layers": deleting a layer can remove whole state groups.
3. "transitions": deleting or changing them can make an actor stuck forever.
4. "blendTree": walk/run blending can break if child clips or thresholds are wrong.
5. "entryStateId": wrong entry state can make the actor start invisible or frozen.

Common Breakage Symptoms
--------

1. Goblin disappears: controller references a clip id that cannot be built.
2. Goblin never walks: walk/run state or speed parameter was broken.
3. Goblin stays idle forever: transitions or runtime-called state ids were broken.
4. Mod imports but no change appears: wrong target or another mod wins later.

