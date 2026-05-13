# Advanced Animation Modding Guide

This guide is updated for the current folder-backed animation loader.

Use these mod types:

- `animation`: add new `*_animations.json` clips and sibling PNG sprite folders.
- `animationClipOverride`: override existing controller slots with `map.json` plus `*_animations.json` clips.
- `animatorController`: add or replace `*_controller.json` controller definitions.

Do not use legacy `animationClip` or `animationController` type names. Do not use `file` in animation mod metadata; use `entry` and point it at the folder to scan.

Current field names for new files:

- Animation clip roots use `clips`.
- Clips use `clipId` and `loopTime`.
- Tracks use `trackId` and `spriteFrames`.
- Override maps use `priority`, `overrides.items`, `clipSlot`, and `clipId`.
- Controllers use `stateMotionBindings`, `motionParameters`, `entryStateId`, `motionId`, `clipId`, `clipAssetPath`, `fromStateId`, and `toStateId`.

Older names such as `animations`, `id`, `loop`, `actions`, `params`, `slot`, and `animName` may still be accepted as aliases, but do not use them in new public examples.

The examples in this folder are disabled with `mod.template.json`. Rename a template to `mod.json` only after replacing `target` with a real actor animation id.