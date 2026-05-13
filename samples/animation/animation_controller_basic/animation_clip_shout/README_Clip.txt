Current type: animationClipOverride

This folder is disabled by default because the metadata file is `mod.template.json`. Rename it to `mod.json` after replacing `target` with a real actor animation id.

The loader scans the `entry` folder for `map.json` and files ending with `_animations.json`.

`map.json` must be an object with this shape:

{
  "priority": 100,
  "overrides": {
    "items": [
      { "clipSlot": "Orc_Shout.main", "clipId": "example.orc_shout" }
    ]
  }
}

`clipSlot` is `<stateId>.<slotName>`. `clipId` must match a `clipId` inside a `*_animations.json` file in this same entry folder.

For the animation file itself, use root `clips`, clip `clipId`, clip `loopTime`, track `trackId`, and track `spriteFrames`. Older names are still loaded as aliases, but new examples should use the current names.