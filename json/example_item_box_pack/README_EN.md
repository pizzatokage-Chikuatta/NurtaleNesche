# Example Item Box Pack

This pack shows how a stage creates item boxes from JSON.

The tested flow is:

1. `itemBoxPrefabs` registers a box prefab id.
2. `itemBoxSpawns` creates a box instance in a scene region and gives it drop data.
3. `itemBoxSpawnLoadouts` lists which spawn ids should be used by a stage/loadout.

The example spawn creates a crate variant, attaches `drop_table.example.training_box`, adds fixed random items, and spawns `equipment.example.training_dagger` from the box.