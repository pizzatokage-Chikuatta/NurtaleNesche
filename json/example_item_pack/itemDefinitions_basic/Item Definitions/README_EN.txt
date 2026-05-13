Type: itemDefinitions
Example files: item.example.training_arrow.json, item.potion.example.training_tonic.json, item.your_mod.test_item.json

What this data does:
It registers item ids that the inventory, drops, item boxes, and potion/equipment systems can reference.

Fields shown in the examples:

- `id`: canonical item id. `item.example.training_arrow` is the id used by drop tables and item boxes to spawn the training arrow.
- `NameInAlphabet`: internal/default English-style name used by the item definition.
- `showUpStage`: stage number where this item is allowed to appear in stage-filtered systems.
- `brokenChance`: chance used by item spawning when the drop entry allows broken-item rolls. `0.2` on the training arrow means the item can use a 20% broken chance when spawned through a drop that keeps broken chance enabled.
- `itemSelectionRequestId`: connects an item to an item-selection UI/request flow. The tonic uses `item_selection_request.potion`; the arrow leaves it empty.
- `builtInPrefabAddress`: built-in prefab used to instantiate the item. The arrow uses the built-in Arrow prefab; the tonic uses the Potion Base prefab.
- `assetBundleFileName` and `prefabAssetName`: external prefab source fields. They are null in this example because the built-in prefab is used.
- `assemblyFileName` and `logicTypeName`: custom code-backed item logic fields. They are null here.
- `spriteIds`: named sprite slots read by item logic or UI. The arrow defines `flying`, `ground`, and `groundMultiple`; the tonic defines `default`.
- `extras`: item-specific extra data. The arrow uses `dropSFXId` and `damageAmount: 20`; the tonic uses `dropSFXId`.