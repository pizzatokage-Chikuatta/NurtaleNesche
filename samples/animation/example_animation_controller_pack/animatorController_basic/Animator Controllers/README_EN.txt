Type: animatorController
Example file: example_actor_controller.json

What this data does:
It defines an advanced animation controller record. The runtime uses it to choose which clip source should play for each state/slot.

Example behavior:
`Example_Goblin_AC` starts in `Goblin_Idling`. `Goblin_Idling` plays `example.goblin_idle` in the `main` slot. `Goblin_WalkRun` is a direct blend tree using the built-in `goblin.goblin_walk` and `goblin.goblin_run` clips.

Fields shown in the example:

- `schemaVersion`: schema version number for the controller record.
- `mode`: controller mode. The example uses `advanced`.
- `id`: controller/actor id.
- `displayName`: readable controller name.
- `entryStateId`: state id used when the controller first enters.
- `common.clips`: shared clip slot map. The example leaves it empty.
- `stateMotionBindings`: runtime state-to-clip slot bindings.
- `stateMotionBindings[].stateId`: state receiving the slot bindings.
- `stateMotionBindings[].clips`: slot-to-clipId map. `main: example.goblin_idle` makes the idle state use the example idle clip.
- `tuning`: blend/runtime tuning values for that state.
- `motionParameters`: controller parameter names used by the motion source.
- `parameters`: animator parameters exposed to the runtime.
- `layers`: layer definitions and which state ids belong to each layer.
- `states`: detailed state definitions.
- `states[].id`: state id used by bindings and transitions.
- `motionType`: `Clip` for a single clip state, `BlendTree` for a blended state.
- `clipId`: clip used by a `Clip` state.
- `blendTree`: blend tree definition for a `BlendTree` state.
- `blendTree.children[].clipSlot`: child slot name such as `walk` or `run`.
- `blendTree.children[].clipId`: clip id used by that child.
- `transitions`: transition records. The example leaves it empty.