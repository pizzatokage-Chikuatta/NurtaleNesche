# Example Animation Controller Pack

This pack shows the JSON pieces used by the tested monster skin/animation mod.

The working flow is:

1. `animation` registers JSON clips and PNG frame folders for one actor target.
2. `animatorController` maps controller states and slots to clip ids.
3. `animationClipOverride` can replace specific controller slots without replacing the whole controller.
4. `animationSFX` attaches sounds to animation clip timing.
5. `sprites` registers ordinary sprite metadata for UI/item use, not animation frame folders.

For `animation`, the current loader scans the entry folder for `*_animations.json`. For `animatorController`, it scans `*_controller.json`. This is a loader scan pattern for those two types, not a rule for every JSON file.