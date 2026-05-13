Type: animationClipOverride
Example files: map.json, example_override_animations.json

What this data does:
It replaces one or more controller clip slots with clips supplied by this mod folder.

Example behavior:
`map.json` replaces `Goblin_Idling.main` with `example.goblin_idle_override`. The replacement clip is defined in `example_override_animations.json`.

Fields shown in `map.json`:

- `priority`: winner value when multiple mods override the same slot. Higher priority wins.
- `overrides.items`: override entry list.
- `clipSlot`: controller state slot to replace, written as `<stateId>.<slotName>`. `Goblin_Idling.main` means the `main` slot in the `Goblin_Idling` state.
- `clipId`: replacement clip id. It must match a clip supplied by a scanned animation JSON file in this same override folder.

Fields shown in `example_override_animations.json` are the same clip fields described in the `animation` example.