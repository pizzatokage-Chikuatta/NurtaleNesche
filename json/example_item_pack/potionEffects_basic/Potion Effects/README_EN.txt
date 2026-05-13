Type: potionEffects
Example file: item.potion.example.training_tonic.json

What this data does:
It gives potion behavior to an item id. The tested tonic heals when drunk and also applies a smaller effect when it hits a target.

Fields shown in the example:

- `potionId`: item id that receives this potion behavior. It matches `item.potion.example.training_tonic` from itemDefinitions.
- `onDrink`: actions when the player drinks the potion.
- `consumeItem`: true removes one potion from inventory after drinking.
- `actions`: ordered effects. The example uses `Heal` amount 20 and `RecoverStamina` amount 15 on drink.
- `vfxId`: visual effect id played for this potion event.
- `onHit`: behavior when the thrown potion hits a target.
- `stunTime`: stun duration applied on hit.
- `onHit.actions`: effects on hit. The example heals 10 on hit.