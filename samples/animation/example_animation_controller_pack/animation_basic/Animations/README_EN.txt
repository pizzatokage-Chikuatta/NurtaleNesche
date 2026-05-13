Type: animation
Example file: example_idle_animations.json

What this data does:
It registers one or more animation clips for the actor selected by this folder's `mod.json` target.

Example behavior:
`example_idle_animations.json` defines `example.goblin_idle`, a looping body-track idle clip with two sprite frames.

Fields shown in the example:

- `clips`: list of animation clips in this file.
- `name`: readable clip name.
- `clipId`: canonical clip id. Animator controllers or clip overrides refer to this value.
- `frameRate`: frame rate used by the generated animation source.
- `pixelsPerUnit`: pixel scale used when building sprite frames.
- `loopTime`: true loops the clip.
- `tracks`: animated sprite/transform tracks.
- `trackId`: track name. `body` is the main required visible track in this example.
- `spriteFrames`: ordered sprite frame list for the track.
- `spriteName`: PNG frame name without `.png`. The loader looks for a matching PNG in the clip's sibling image folder.
- `time`: start time of this frame in seconds.
- `duration`: how long this frame stays visible.
- `transforms`: optional animated transform/color curves. Empty arrays mean no authored curve values.
- `events`: animation event list. The example leaves it empty.