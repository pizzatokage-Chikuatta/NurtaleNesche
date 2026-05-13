# Example Audio Pack

This pack shows the JSON records used by the audio registries.

The data flow is:

1. `audioFiles` registers an SFX clip id and points it to an audio file path.
2. `classificationID` groups one or more SFX clip ids with weights.
3. `animationSFX` or other gameplay data can play the classification id.
4. `bgmTracks` registers a BGM track id and file path.
5. `bgmStageLoadouts` assigns a BGM track to a stage.

These examples are data shapes. Before enabling them, provide the referenced `.ogg` files at the paths used by the JSON.