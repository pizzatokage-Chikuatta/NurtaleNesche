Type: playerStats
Example file: player_stats.example.training_runner.json

What this data does:
It defines a player stat profile. This can change vitals, movement, interaction timing, inventory limits, and related profile ids.

Fields shown in the example:

- `id`: canonical player stat profile id.
- `displayName`: readable name for the profile.
- `actionRuleProfileId`: action rule profile used by the player.
- `shadowProfileId`: shadow profile used by the player.
- `vitals.maxHealth`: starting maximum HP.
- `vitals.healthCap`: cap for maximum HP increases.
- `vitals.maxStamina`: starting maximum stamina.
- `vitals.staminaCap`: cap for maximum stamina increases.
- `vitals.staminaRestoreAmount`: stamina restored by the configured restore tick/effect.
- `movement.walkSpeed`: base walk speed.
- `movement.sprintMultiplier`: sprint multiplier applied over walk speed.
- `interaction.interactIntervalSeconds`: minimum interaction interval.
- `inventory.maxItemSpeciesHoldable`: maximum distinct item species the inventory can hold.
- `inventory.combinationLoadoutId`: item combination loadout available to this player profile.
- `sexualHeat.decreasePerSecond`: heat decrease rate.
- `extras`: extra profile data. The example leaves it empty.