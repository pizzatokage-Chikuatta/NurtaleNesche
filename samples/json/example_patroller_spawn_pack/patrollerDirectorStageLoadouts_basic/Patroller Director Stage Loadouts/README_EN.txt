Type: patrollerDirectorStageLoadouts
Example file: patroller_director_loadout.example.stage_03.json

What this data does:
It assigns director ids to scene spawn point ids for a stage.

Fields shown in the example:

- `id`: canonical director stage loadout id. The sample id includes `.stage_03` because this loadout is meant for Stage 03.
- `assignments`: list of spawn point to director links.
- `assignments[].spawnPointId`: scene spawn point id to control.
- `assignments[].directorId`: patroller director id assigned to that spawn point.