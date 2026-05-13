Type: classificationID
Example file: sfx.example.ui.json

What this data does:
It registers a weighted SFX classification. Gameplay can request the classification id and the sound system chooses one of its clip entries by weight.

Fields shown in the example:

- `id`: canonical SFX classification id. The animation SFX example uses `sfx.example.ui`.
- `clips`: weighted SFX clip entries.
- `clipId`: SFX clip id registered by `audioFiles`.
- `weight`: relative chance for this clip inside the classification. The example has one clip with weight 1, so it is always chosen.