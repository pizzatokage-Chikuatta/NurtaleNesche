Type: animationSFX
Example file: example_attack_sfx.json

What this data does:
It attaches sound events to an animation clip id.

Example behavior:
When `example.goblin_attack` reaches normalized time `0.2`, it plays the weighted SFX classification `sfx.example.ui` with volume and pitch multipliers of 1.0.

Fields shown in the example:

- `animationClipId`: animation clip id that owns these SFX events.
- `events`: list of timed SFX events.
- `normalizedTime`: event time within the clip, from 0 to 1.
- `playbackAction`: event action. The example uses `play`.
- `sfxClassId`: weighted SFX classification id to play.
- `sfxClipId`: exact SFX clip id override. Empty means use the classification instead.
- `volumeMultiplier`: volume multiplier for this event.
- `pitchMultiplier`: pitch multiplier for this event.
- `soundStrength`: sound propagation strength.
- `noiseLevel`: noise level for suspicion/perception systems.
- `raiseSuspicion`: whether this sound should raise suspicion.
- `soundSource`: logical source of the sound. The example uses `Enemy`.
- `loop`: whether the sound loops.
- `alert`: whether alert presentation should be pushed.