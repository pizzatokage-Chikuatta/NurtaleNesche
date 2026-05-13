Current type: animatorController

This folder is disabled by default because the metadata file is `mod.template.json`. Rename it to `mod.json` after replacing `target` with a real actor animation id.

The loader scans the `entry` folder for files ending with `_controller.json`.

Use the current controller field names in new files:

- `stateMotionBindings`, not `actions`
- `motionParameters`, not `params`
- `entryStateId`, not `entryState`
- `motionId`, not `actionId`
- `clipId` and `clipAssetPath`, not `mainClip` and `mainClipAssetPath`
- `fromStateId` and `toStateId`, not `from` and `to`

Strict runtime requires explicit `states`. A state motion binding by itself does not create a playable state.