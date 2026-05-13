# Code Mod Registry Helper Guide

## Status

This guide covers v1 experimental `ModRegistryExtensions`.

These helpers are convenience wrappers over current runtime registries. They do not create a new registry layer and they do not make the runtime registries stable SDK abstractions.

Use these helpers from `NurtaleNesche.Modding.Registries` as the v1 experimental entry point. The registry parameters are still runtime types, so they are usable for v1 code mods but not stable SDK boundaries.

If the same code mod also uses public id attributes or manifest constants, reference `NurtaleNesche.Modding.Abstractions.dll` in addition to the current runtime assembly.

## Purpose

Without helpers, registrar code usually looks like this:

```csharp
registry.RegisterPlainProvider("task_provider.example.wait", () => new ExampleWaitTaskProvider(), allowOverride: true);
```

With helpers:

```csharp
registry.RegisterTaskProvider<ExampleWaitTaskProvider>("task_provider.example.wait", allowOverride: true);
```

For task providers, the helper registers a plain C# provider factory. It:

- creates the provider with `new T()`;
- keeps the existing id/override behavior;
- gives compile-time constraints for the expected type category.

Use `RegisterComponentTaskProvider<T>` only for deliberate legacy component-backed experiments.

## Task Provider And Task Option Helpers

Use these in registrar classes from `NurtaleNesche.Modding.Registries`:

```csharp
using NurtaleNesche.Modding.Registries;

public sealed class ExampleTaskRegistrar : ITaskProviderModRegistrar
{
    public void RegisterTaskProviders(TaskProviderRegistry registry)
    {
        registry.RegisterTaskProvider<ExampleWaitTaskProvider>("task_provider.example.wait");
    }
}
```

Available helpers:

- `RegisterTaskProvider<T>` for plain C# task providers.
- `RegisterComponentTaskProvider<T>` for explicit legacy Unity-component task providers.
- `RegisterCaptiveTreatingTaskOption<T>`.
- `RegisterCollectCaptiveTaskOption<T>`.
- `RegisterSoloActionTaskOption<T>`.
- `RegisterChaseTaskOption<T>`.
- `RegisterAbilityTaskOption<T>`.
- `RegisterArrowHitEventTaskOption<T>`.
- `RegisterPotionHitEventTaskOption<T>`.

## Sense Helper

Sense helpers register Unity components by id.

```csharp
using NurtaleNesche.Modding.Patrollers.Senses;
using NurtaleNesche.Modding.Registries;

public sealed class ExampleNoiseSense : Sense
{
}

public sealed class ExampleSenseRegistrar : ISenseModRegistrar
{
    public void RegisterSenses(SenseFactoryRegistry registry)
    {
        registry.RegisterSense<ExampleNoiseSense>("sense.example.noise");
    }
}
```

`RegisterSense<T>` requires `T` to be a `UnityEngine.Component`.

## Status Effect Helper

Status-effect helpers register factories that create new status-effect instances.

```csharp
using NurtaleNesche.Modding.Registries;
using NurtaleNesche.Modding.StatusEffects;

public sealed class ExampleStatusEffect : StatusEffect
{
    public override void OnEntry()
    {
        base.OnEntry();
    }
}

public sealed class ExampleStatusRegistrar : IStatusEffectFactoryModRegistrar
{
    public void RegisterStatusEffectFactories(StatusEffectFactoryRegistry registry)
    {
        registry.RegisterStatusEffect<ExampleStatusEffect>("status_effect.example");
    }
}
```

For profile-specific effects:

```csharp
registry.RegisterStatusEffect<ExampleStatusEffect>("status_effect.example", "profile.example");
```

`RegisterStatusEffect<T>` requires a public parameterless constructor.

## Item Selection Request Factory Helper

Item-selection helpers register factory instances by factory id.

```csharp
using NurtaleNesche.Modding.Items;
using NurtaleNesche.Modding.Registries;
using PlayerGroup;
using UnityEngine;

public sealed class ExampleItemSelectionFactory : IItemSelectionRequestFactory
{
    public bool TryCreate(string requestTypeId, Player player, GameObject itemObject, out IItemSelectionRequest request)
    {
        request = null;
        return false;
    }
}

public sealed class ExampleItemSelectionRegistrar : IItemSelectionRequestFactoryModRegistrar
{
    public void RegisterItemSelectionRequestFactories(ItemSelectionRequestFactoryRegistry registry)
    {
        registry.RegisterItemSelectionRequestFactory<ExampleItemSelectionFactory>("item_selection.example");
    }
}
```

`RegisterItemSelectionRequestFactory<T>` requires a public parameterless constructor.

## Current Experimental Coupling

Registrar method signatures still expose runtime registry types:

- `TaskProviderRegistry`.
- task-option registries.
- `SenseFactoryRegistry`.
- `StatusEffectFactoryRegistry`.
- `ItemSelectionRequestFactoryRegistry`.

This is allowed for v1 experimental code mods. It is not a stable SDK boundary yet.

## When To Prefer Manifest Files

Prefer manifest files when the mod only needs to map ids to types.

Prefer registrar classes when:

- the mod wants explicit registration code;
- one type should register multiple ids;
- registration needs profile-specific status factories;
- the mod wants startup validation before registering.
