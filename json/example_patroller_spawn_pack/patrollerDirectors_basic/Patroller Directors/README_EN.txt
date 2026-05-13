Type: patrollerDirectors
Example file: patroller_director.example.stage_03.json

What this data does:
A patroller director is a spawner configuration for repeated/director-controlled enemy spawning.

Fields shown in the example:

- `id`: canonical director id.
- `patrolRegionIds`: region guids assigned to patrollers spawned by this director.
- `startActive`: true starts the director active.
- `autoActivation`: whether the director activates automatically after a delay.
- `autoActivationDelay`: delay used when auto activation is enabled.
- `maxAliveFromThisSpawner`: max alive patrollers from this director/spawner.
- `maxAliveOnFloor`: max alive patrollers allowed on the floor.
- `fastReduceCoolEnemyCount`: count threshold used by director cooldown reduction behavior.
- `spawnCooldown`: base seconds between spawn waves.
- `spawnJitterX`: random horizontal spawn offset range.
- `allowDuplicatePatrollersInSameWave`: whether the same patroller id may appear more than once in one wave.
- `spawnCountChoices`: possible counts for a spawn wave. The example always chooses 1.
- `patrollers`: weighted patroller choices.
- `patrollers[].patrollerId`: patroller data id to spawn.
- `patrollers[].weight`: relative chance for this patroller choice.