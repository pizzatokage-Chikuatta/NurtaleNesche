Type: itemBoxSpawns
Example file: item_box_spawn.example.stage_01.001.json

What this data does:
It creates or configures one stage-initial item box.

Fields shown in the example:

- `id`: canonical item box spawn id. The loadout refers to this id.
- `enabled`: true allows this spawn to run.
- `boxPrefabId`: item-box prefab id to instantiate. The example uses `item_box_prefab.example_crate`.
- `spawnRegionId`: region guid where the box should spawn. Replace the placeholder with a real stage region guid.
- `xOffsetFromRegionCenter`: horizontal offset from the chosen region center/checkpoint.
- `useSpawnRegionCheckpointPosition`: true uses the spawn region checkpoint position when available.
- `yPosOffset`: vertical position offset.
- `dropYVelocity`: upward velocity applied to spawned drops from this box.
- `openSFXId`: SFX id played when the box opens.
- `uniqueDropTableIds`: extra drop tables attached to this box. The example rolls `drop_table.example.training_box`.
- `fixedRandomSpawnItemIds`: fixed random item ids placed in the box. The example includes `item.biscuit` and `item.lockpick`.
- `fixedRandomSpawnQuantityList`: quantities for `fixedRandomSpawnItemIds` by index. The example gives 1 biscuit and 1 lockpick.
- `spawnEquipmentIds`: equipment ids spawned by the box. The example adds `equipment.example.training_dagger`.
