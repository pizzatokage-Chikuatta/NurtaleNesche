Type: shadowProfiles
Example file: shadow_profile.example.shadow_creature.json

What this data does:
It defines shadow visual settings for an actor and optional per-animation-state overrides.

Fields shown in the example:

- `id`: canonical shadow profile id. Patroller data references this value through `shadowProfileId`.
- `default`: fallback shadow settings when no state-specific entry matches.
- `scale`: x/y shadow scale array.
- `offset`: x/y shadow offset array.
- `alpha`: shadow opacity.
- `smooth`: smoothing time/value used when changing shadow settings.
- `states`: per-state shadow settings keyed by animation/state name.
- `states.ShadowCreature_Idle`: shadow settings while that state is active.
- `states.ShadowCreature_Dash`: different settings for dash, with larger scale and alpha.