# Code Mod Manifest Guide

## Status

This guide covers v1 experimental code/DLL manifest files.

These manifest files are separate from normal folder-backed `mod.json` data mods. They are scanned by filename pattern under the root `Mods` folder and map public ids to C# types in already-loaded mod assemblies.

For implementation code, prefer documented `NurtaleNesche.Modding.*` aliases and helpers. The runtime still validates some requirements against underlying game types, but those underlying namespaces are not stable SDK promises.

## General Rules

- A matching `.dll` must exist somewhere under `Mods`.
- `experimentalApiVersion` should be `"v1"` at the root of each code manifest.
- Missing or unsupported `experimentalApiVersion` values are warning-only in v1 and do not block loading.
- `type` is the full C# type name, including namespace.
- Manifest files do not define behavior; they only map ids to compiled types.
- Unknown JSON fields are ignored by the runtime loaders, but the validator warns on unknown top-level fields and unknown registration entry fields.
- Code/DLL mods are trusted local code and are not sandboxed.
- Runtime DLL loading is desktop/Mono-first.

## Task Providers

Filename pattern:

```text
*.task_providers.json
```

Shape:

```json
{
  "experimentalApiVersion": "v1",
  "providers": [
    {
      "id": "task_provider.example.wait",
      "type": "ExampleMods.Tasks.ExampleWaitTaskProvider"
    }
  ]
}
```

Requirements:

- `type` must inherit `Patroller.TaskProviderCore`, or the public matching alias `NurtaleNesche.Modding.Patrollers.Tasks.TaskProviderCore`.
- Manifest registration currently allows override.

## Event Task Options

Filename pattern:

```text
*.event_task_options.json
```

Shape:

```json
{
  "experimentalApiVersion": "v1",
  "arrowOptions": [
    {
      "id": "task_option.arrow_hit.example",
      "type": "ExampleMods.Tasks.ExampleArrowHitOption",
      "overrideExisting": false
    }
  ],
  "potionOptions": [
    {
      "id": "task_option.potion_hit.example",
      "type": "ExampleMods.Tasks.ExamplePotionHitOption",
      "overrideExisting": false
    }
  ]
}
```

Requirements:

- `arrowOptions[].type` must implement `Patroller.IArrowHitEventTaskOption`.
- `potionOptions[].type` must implement `Patroller.IPotionHitEventTaskOption`.

## Captive Treating Task Options

Filename patterns:

```text
*.captive_treating_task_options.json
*.captive_treating_action_sets.json
```

The second filename is a legacy compatibility alias.

Current shape:

```json
{
  "experimentalApiVersion": "v1",
  "taskOptions": [
    {
      "id": "task_option.captive_treating.example",
      "type": "ExampleMods.Tasks.ExampleCaptiveTreatingOption",
      "overrideExisting": false
    }
  ]
}
```

Legacy shape:

```json
{
  "experimentalApiVersion": "v1",
  "actionSets": [
    {
      "id": "task_option.captive_treating.example",
      "type": "ExampleMods.Tasks.ExampleCaptiveTreatingOption",
      "overrideExisting": false
    }
  ]
}
```

Requirements:

- `type` must implement `Patroller.ICaptiveTreatingTaskOption`.
- Prefer `taskOptions`; `actionSets` exists only for older naming compatibility.

## Senses

Filename pattern:

```text
*.senses.json
```

Shape:

```json
{
  "experimentalApiVersion": "v1",
  "senses": [
    {
      "id": "sense.example.noise",
      "type": "ExampleMods.Senses.ExampleNoiseSense",
      "overrideExisting": false
    }
  ]
}
```

Requirements:

- `type` must be a `UnityEngine.Component`.
- In practice, custom senses should inherit `NurtaleNesche.Modding.Patrollers.Senses.Sense` or another compatible runtime sense component.

## Status Effect Factories

Filename pattern:

```text
*.status_effect_factories.json
```

Shape:

```json
{
  "experimentalApiVersion": "v1",
  "factories": [
    {
      "id": "status_effect.example",
      "profileId": "",
      "type": "ExampleMods.StatusEffects.ExampleStatusEffect"
    }
  ]
}
```

Profile-specific shape:

```json
{
  "experimentalApiVersion": "v1",
  "factories": [
    {
      "id": "status_effect.example",
      "profileId": "profile.example",
      "type": "ExampleMods.StatusEffects.ExampleStatusEffect"
    }
  ]
}
```

Requirements:

- `type` must implement `IStatusEffect`.
- `type` must have a public parameterless constructor.
- Empty or missing `profileId` registers a profile-agnostic factory.

## Item Selection Request Factories

Filename pattern:

```text
*.item_selection_request_factories.json
```

Shape:

```json
{
  "experimentalApiVersion": "v1",
  "factories": [
    {
      "id": "item_selection.example",
      "type": "ExampleMods.Items.ExampleItemSelectionFactory"
    }
  ]
}
```

Requirements:

- `type` must implement `IItemSelectionRequestFactory`.
- `type` must be constructible with `Activator.CreateInstance`.

## Registrar Alternative

For advanced code mods, registrar classes can replace or complement manifest files:

- `ITaskProviderModRegistrar`.
- `IEventTaskOptionModRegistrar`.
- `ICaptiveTreatingTaskOptionModRegistrar`.
- `ISenseModRegistrar`.
- `IStatusEffectFactoryModRegistrar`.
- `IItemSelectionRequestFactoryModRegistrar`.

Use registrar classes when one type needs to register multiple ids, when profile-specific registration is clearer in code, or when the mod wants custom startup validation.

## Validation Notes

The current v1 validator is a first-pass validator. It can report common manifest problems, but it does not compile DLLs and does not prove runtime behavior.

The Unity Editor validator enables code-manifest type-resolution diagnostics. It may load `.dll` files under `Mods` for validation-only type-name checks, but this does not change runtime loading behavior.

Current validator checks include:

- required registration arrays exist when expected;
- `id` and `type` are non-empty;
- deprecated `actionSets` usage is reported;
- `overrideExisting` is boolean when present;
- code-manifest `type` names can be resolved from currently loaded or validation-loaded assemblies when the Unity Editor validator is used.

Future validator work should check:

- status factories with `profileId` use normalized profile ids;
- manifests that reference missing DLL types are reported more clearly.
