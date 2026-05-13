# Drop Table Field Guide

Use this guide with `drop_table.example.training_box.json`.

## What The Example Drops

The example always drops one `item.example.training_arrow`. Then it makes one random roll:

- `itemId: null`, `weight: 3`: no extra item.
- `item.lockpick`, `weight: 2`: one lockpick.
- `item.potion.example.training_tonic`, `weight: 1`, `hasBrokenChance: false`: one finished Training Tonic.

The weights are relative. With 3 + 2 + 1 total weight, that single roll gives 50% no extra item, about 33.3% lockpick, and about 16.7% tonic.

## Fields

- `id`: the id other records use to request this table.
- `guaranteedItem`: items added every time the table is used.
- `guaranteedEquipment`: equipment added every time the table is used.
- `groups`: random roll sections.
- `rolls`: how many times to pick from that group's `entries`.
- `entries`: the choices for one group.
- `itemId`: the item to create when the entry wins. `null` creates no item for that roll.
- `weight`: relative chance against sibling entries.
- `hasBrokenChance`: false skips item broken chance for that drop; omitted/true allows normal broken chance behavior.