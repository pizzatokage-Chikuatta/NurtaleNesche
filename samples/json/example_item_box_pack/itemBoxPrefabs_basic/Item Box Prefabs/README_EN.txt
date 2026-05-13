Type: itemBoxPrefabs
Example file: item_box_prefab.example_crate.json

What this data does:
It registers an item-box prefab id that item box spawn records can instantiate.

Fields shown in the example:

- `id`: canonical item-box prefab id. The spawn example refers to `item_box_prefab.example_crate` through `boxPrefabId`.
- `builtInPrefabAddress`: built-in Unity prefab address for the box. The example uses the Stage 1 crate variant prefab.
- `assetBundleFileName` and `prefabAssetName`: external prefab source fields. They are null because the example uses a built-in prefab.