Type: equipmentDefinitions
Example file: equipment.example.training_dagger.json

What this data does:
It registers an equipment id that can be spawned by item boxes, drops, or inventory systems.

Fields shown in the example:

- `id`: canonical equipment id. `equipment.example.training_dagger` is the id used by `spawnEquipmentIds` in the item-box example.
- `NameInAlphabet`: internal/default display name.
- `showUpStage`: stage number where this equipment is allowed to appear in stage-filtered systems.
- `builtInPrefabAddress`: built-in prefab instantiated for this equipment. The example uses the built-in Dagger prefab.
- `assetBundleFileName` and `prefabAssetName`: external prefab source fields. They are null because this example uses a built-in prefab.
- `assemblyFileName` and `logicTypeName`: custom code-backed equipment logic fields. They are null here.
- `spriteIds`: sprite slots read by the equipment logic/UI. The dagger example maps `displayPristine`, `displayWorn`, and `displayBroken`.
- `extras`: additional equipment-specific data. The example leaves it empty.