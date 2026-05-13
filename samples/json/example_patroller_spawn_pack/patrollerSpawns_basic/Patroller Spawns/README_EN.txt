Type: patrollerSpawns
Example files: patroller_spawn.example.stage_01.shadow_creature.json, patroller_spawn.example.stage_02.shadow_creature.json

What this data does:
It places one patroller in a stage region.

Fields shown in the examples:

- `id`: canonical patroller spawn id. Spawn loadouts refer to this id.
- `patrollerId`: patroller data id to instantiate. Replace placeholder values with a real patroller id.
- `enabled`: true allows this spawn to run.
- `spawnRegionId`: region guid where the patroller starts.
- `patrolRegionIds`: region guids assigned to this spawned patroller for patrol behavior.
- `xOffsetFromRegionCenter`: horizontal offset from the spawn region center/checkpoint.
- `yOffsetFromGround`: vertical offset from the ground.
- `useSpawnRegionCheckpointPosition`: true uses the spawn region checkpoint position when available; false uses the region center plus offsets.