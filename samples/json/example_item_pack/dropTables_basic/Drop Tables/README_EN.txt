Type: dropTables
Example file: drop_table.example.training_box.json

What this data does:
A drop table decides which items/equipment are produced when a box opens or a monster death state rolls drops.

Example behavior:
`drop_table.example.training_box` always creates `item.example.training_arrow`. After that, it rolls one weighted random entry from the group:

- weight 3: `itemId: null`, so this roll produces nothing.
- weight 2: drops `item.lockpick`.
- weight 1: drops `item.potion.example.training_tonic` as a finished item because `hasBrokenChance` is false.

With total weight 6, the one roll is 3/6 no extra item, 2/6 lockpick, and 1/6 training tonic.

Fields shown in the example:

- `id`: canonical drop table id. Other JSON refers to this value. `drop_table.example.training_box` means this is the training-box example table.
- `guaranteedItem`: item ids that are always added before random rolls. The example always spawns `item.example.training_arrow`.
- `guaranteedEquipment`: equipment ids that are always added before random rolls. The example leaves it empty.
- `groups`: list of random roll groups. Each group is rolled independently.
- `rolls`: number of random picks made from that group. `1` means pick one entry once. It is not priority.
- `entries`: weighted choices available to that group.
- `itemId`: item id to drop when this entry wins. `null` means the entry intentionally drops nothing.
- `weight`: relative chance inside the group. Bigger weight means more likely compared with the other entries in the same group.
- `hasBrokenChance`: controls whether the spawned item uses its item definition `brokenChance`. `false` means the drop is forced to be a finished/non-broken item.