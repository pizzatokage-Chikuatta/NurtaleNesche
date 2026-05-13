# Example Item Pack

This pack shows the item-side data flow used by the tested item mods.

A usable item usually needs several records:

- `itemDefinitions`: creates the runtime item id and points it to a prefab/sprites/extra gameplay data.
- `itemNames`: gives the item a visible name.
- `potionEffects`: gives a potion item behavior when drunk or when it hits a target.
- `dropTables`: decides what drops from a box or monster.
- `dropTableLoadouts`: groups drop table ids so a stage/common source can request several tables.

The example `drop_table.example.training_box` always drops `item.example.training_arrow`, then performs one weighted random roll that may drop nothing, a lockpick, or `item.potion.example.training_tonic`.