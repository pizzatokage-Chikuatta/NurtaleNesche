Type: patrollerData
Example file: patroller.example.shadow_creature.training.json

What this data does:
It registers a patroller id and configures the runtime object that will be spawned.

Fields shown in the example:

- `id`: canonical patroller id. Spawn records use this as `patrollerId`.
- `NameInAlphabet`: internal/default readable name.
- `builtInPrefabAddress`: built-in prefab used to instantiate the patroller. The example uses the Stage 2 ShadowCreature prefab.
- `statusProfileId`: status effect profile assigned to the patroller.
- `actionRuleProfileId`: action rule profile assigned to the patroller.
- `dropTableProfileId`: standard drop table id/profile used by the patroller death flow.
- `shadowProfileId`: shadow visual profile id used by this patroller.
- `animationActorId`: animation actor/controller id used by the animation runtime.
- `showUpStage`: stage number where this patroller is allowed to appear in stage-filtered systems.
- `positions.yPosOffset`: vertical spawn/presentation offset.
- `flags.isCloseDoorByDefault`: default door-closing behavior flag copied into the patroller.
- `flags.canCheckTrap`: whether this patroller can check traps.
- `stats`: runtime stat values such as HP, damage, walk/run/chase speed, and rotation speed.
- `inventories`: initial gold, items, and equipment carried by the patroller.
- `combat`: attack range, cooldown, prepare time, and recovery time.
- `senses.providers`: sense providers added to the patroller. The example uses sight and manual sense.
- `tasks.providers`: task provider entries used by the scheduler. The example uses built-in `sneak` and `chase` provider types.
- `extras`: extra patroller-specific data. The example leaves it empty.
- `speechProfileId`: speech profile id used by speech systems.