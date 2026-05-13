Type: patrollerSpawnLoadouts
Example files: patroller_spawn_loadout.example.stage_01.json, patroller_spawn_loadout.example.stage_02.json

What this data does:
It groups patroller spawn ids for a stage/loadout.

Fields shown in the examples:

- `patrollerSpawnLoadoutId`: canonical loadout id.
- `patrollerSpawnIds`: spawn ids included in this loadout.
- `commonPatrolRegionIds`: shared patrol region guids passed along with the loadout. These are useful when several spawned patrollers should know the same patrol-region pool in addition to their per-spawn `patrolRegionIds`.