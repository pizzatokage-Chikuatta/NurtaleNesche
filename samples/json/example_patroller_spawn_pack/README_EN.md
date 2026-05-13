# Example Patroller Spawn Pack

This pack shows how a patroller can be defined and placed into a stage from JSON.

The tested monster flow uses the same pieces:

1. `patrollerData` registers the patroller id, prefab, stats, senses, task providers, drop table profile, and animation actor id.
2. `patrollerSpawns` places that patroller into a region.
3. `patrollerSpawnLoadouts` chooses which spawn ids are active for a stage/loadout and can provide shared patrol regions.
4. `dropTables` or `dropTableProfileId` control what the patroller drops when its death flow rolls drops.

Director records are for repeated/director-controlled spawning rather than a single stage-initial spawn.