Type: itemNames
Example files: item.example.training_arrow.json, item.potion.example.training_tonic.json

What this data does:
It gives display names to item ids already registered by `itemDefinitions`.

Fields shown in the examples:

- `itemId`: item definition id that receives these names.
- `unknownNames`: names shown before the game treats the item as identified. `White Tonic` is the unidentified name for the training tonic.
- `knownNames`: names shown after identification or when the item is already known. `Training Tonic` is the known name.
- Language keys such as `en`: localized text for that language.