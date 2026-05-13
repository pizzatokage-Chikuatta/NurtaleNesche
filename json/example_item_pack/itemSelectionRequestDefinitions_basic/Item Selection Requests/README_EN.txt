Type: itemSelectionRequestDefinitions
Example file: item_selection_request.example_lockpick.json

What this data does:
It defines an item-selection request that can be referenced by an item or interaction flow.

Fields shown in the example:

- `id`: canonical request id.
- `loadoutId`: UI/profile loadout used by the request.
- `factoryId`: factory that builds the request behavior. The example uses the built-in lockpick factory.
- `allowedTokens`: token names accepted by this request. The example accepts `bind_id`.