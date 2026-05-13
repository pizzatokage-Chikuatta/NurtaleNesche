Type: dropTableLoadouts
Example file: drop_table_loadout.example.stage_common.stage_01.json

What this data does:
A drop table loadout groups several drop table ids under one loadout id. Systems that ask for a stage/common loadout receive the listed table ids.

Fields shown in the example:

- `dropTableLoadoutId`: canonical loadout id. `drop_table_loadout.example.stage_common.stage_01` is the example stage-common loadout id.
- `dropTableIds`: drop table ids included in the loadout. The example includes `drop_table.example.training_box` and the built-in `drop_table.lockpick`.