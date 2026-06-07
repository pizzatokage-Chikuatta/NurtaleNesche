Animation SFX
================================

This tutorial attaches sound events to a JSON animation clip.

This does not use Unity .anim Animation Events. Do not add receiver methods to old Animator components for this. The JSON animation system reads animation SFX data by animationClipId.

`animationClipId` is the clip id from an `_animations.json` file. It is not `target`, not `variant`, and not a controller `stateId`. If you add SFX to a custom Orc override clip, use your custom clip id. If you add SFX to a Nesche patch target, use the built-in Nesche clip id you are patching.

Step 1
--------

Create this folder hierarchy under Mods.

~~~text
Mods\Mod_YourName\Animations\Goblin Idle SFX\Animation SFX
~~~

Step 2
--------

Copy this template:

~~~text
Mods\mod.json templates\mod.template_folder_backed.json
~~~

Paste it here:

~~~text
Mods\Mod_YourName\Animations\Goblin Idle SFX
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
  "type": "animationSFX",
  "entry": "Animation SFX",
  "overwriteExisting": true
}
~~~

Step 4
--------

Inside Animation SFX, create this file:

~~~text
goblin_idle_sfx.json
~~~

Put this content inside it.

~~~json
{
  "animationClipId": "goblin.goblin_idling",
  "events": [
    {
      "normalizedTime": 0.2,
      "playbackAction": "play",
      "sfxClassId": "sfx.voice.akanai",
      "sfxClipId": "",
      "volumeMultiplier": 1.0,
      "pitchMultiplier": 1.0,
      "soundStrength": 10.0,
      "noiseLevel": 0.0,
      "raiseSuspicion": false,
      "soundSource": "Enemy",
      "loop": false,
      "alert": false
    }
  ]
}
~~~

Step 5
--------

Understand the top-level field.

1. "animationClipId" is the clip id that owns these sound events.
2. It is not a controller state id.
3. For Goblin idle, use goblin.goblin_idling.
4. For a custom clip, use your custom clip id such as mod.yourname.goblin_idle.

Do not use this here:

~~~text
Goblin_Idling
~~~

That is a controller state id, not an animation clip id.

Step 6
--------

Understand event timing.

~~~text
"normalizedTime": 0.2
~~~

Means the event plays at 20 percent of the animation clip.

Use:

1. 0.0 for the start.
2. 0.5 for the middle.
3. 1.0 for the end.

Step 7
--------

Understand SFX selection.

Use "sfxClassId" when you want the sound system to choose a weighted clip from a classification:

~~~text
"sfxClassId": "sfx.voice.akanai",
"sfxClipId": ""
~~~

Use "sfxClipId" only when you want one exact clip:

~~~text
"sfxClassId": "",
"sfxClipId": "sfx.clip.example"
~~~

Advanced alternating pattern:

~~~text
"sfxClassIds": [
  "sfx.voice.h_normal_loop_1",
  "sfx.voice.h_normal_loop_2"
],
"sfxPatternId": "my_loop_pattern",
"sfxPatternMode": "alternate"
~~~

For first mods, use only "sfxClassId".

Use one main selection path per event:

1. Use "sfxClassId" for normal weighted random playback.
2. Use "sfxClipId" only for one exact clip.
3. Do not fill both unless you are intentionally testing advanced behavior.
4. If both are filled by accident, debugging becomes harder because it is unclear which sound you expected.

Step 8
--------

Understand gameplay sound fields.

1. "volumeMultiplier": loudness multiplier.
2. "pitchMultiplier": pitch multiplier.
3. "soundStrength": propagation strength.
4. "noiseLevel": value used by suspicion/perception systems.
5. "raiseSuspicion": true can raise suspicion.
6. "soundSource": logical source such as Enemy, Player, Environment.
7. "loop": true means looping playback.
8. "playbackAction": "play" plays sound. Some loop setups can use "stop".
9. "alert": true can push SoundToScreenFX.

Policy:

1. Enemy and trap alert sounds can use "alert": true.
2. Player and friendly captive sounds should use "alert": false.
3. If unsure, use "alert": false.

Step 9
--------

Start the game and open:

~~~text
Main Menu -> Mods -> Mod Report
~~~

Then enter a stage with Goblins and wait for Goblin idle to play.

How To Use Your Own OGG
--------

If you want your own .ogg sound:

1. Create an Audio Files mod for the .ogg file.
2. That creates or registers an SFX clip id.
3. Create an SFX Classification mod that points to that SFX clip id.
4. Put that classification id into "sfxClassId" here.

Dependency chain:

~~~text
.ogg file -> sfxClipId -> sfxClassId -> animationSFX event
~~~

If one link is wrong, the animation event can import but no sound will play.

Common Mistakes
--------

1. Using Goblin_Idling as animationClipId. Use goblin.goblin_idling.
2. Setting normalizedTime to 2.0. Use 0.0 to 1.0.
3. Using alert true for player/friendly sounds.
4. Typing a missing sfxClassId.
5. Expecting old Unity .anim events to control this system.

