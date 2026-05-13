Type: equipmentUi
Example file: training_dagger.json

What this data does:
It tells the equipment UI and input layer how an equipment id behaves in the HUD.

Fields shown in the example:

- `equipmentId`: equipment definition id receiving this UI/input metadata.
- `isTrinket`: false means this is treated as normal equipment rather than trinket equipment.
- `hudKeyIndex`: HUD/input slot index used by the equipment.
- `quickUseAction`: action triggered by quick use. The dagger example uses `Parry`.
- `longPressAction`: action triggered by long press. The dagger example also uses `Parry`.
- `escapeToolId`: id used when this equipment acts as an escape tool.