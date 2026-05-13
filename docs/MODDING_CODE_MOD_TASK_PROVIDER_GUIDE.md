# Code Mod Task Provider Guide

## Status

This guide covers v1 experimental code/DLL task providers.

Nothing in this guide is stable API yet. `NurtaleNesche.Modding.*` is the v1 experimental authoring entry point, but the current task system still bridges into runtime types.

## Required References

A code mod assembly that implements task providers currently needs references to:

- `NurtaleNesche.Modding.Abstractions.dll` from the target game build for public modding constants and id attributes.
- `NurtaleNesche.Runtime.dll` from the target game build for experimental runtime-coupled task APIs.
- `UnityEngine.CoreModule.dll`.
- `Newtonsoft.Json.dll` if the provider reads JSON config through `JObject`.
- `netstandard.dll`, depending on the compiler/toolchain.
- Any Unity package or third-party DLL that the chosen runtime-coupled API path exposes through compilation.

`NurtaleNesche.Modding.Abstractions.dll` is the separated public modding assembly. Runtime-coupled task helpers such as `TaskSetCore`, `TaskInfo`, and `TaskSequenceBuilder` are exposed through the experimental `NurtaleNesche.Modding.Patrollers.Tasks` namespace, but still bridge into `NurtaleNesche.Runtime.dll` in v1.

This is desktop/Mono-first. Runtime DLL loading is trusted local code and is not sandboxed.

## Recommended Authoring Shape

Use the public experimental namespace first:

```csharp
using Newtonsoft.Json.Linq;
using NurtaleNesche.Modding.Patrollers.Tasks;
```

For simple task providers, inherit `TaskSetCore` or `TaskProviderCore`:

```csharp
[TaskProviderId("task_provider.example.wait")]
public sealed class ExampleWaitTaskProvider : TaskSetCore
{
    public override void ApplyConfig(JObject cfg)
    {
        // Optional: read custom provider config here.
    }

    public override Patroller.PatrollerTaskInfoContainer GetNewTask()
    {
        if (!IsConditionMetForPriority(TaskPriorities.MiscTaskLow)) return null;
        if (controller == null || controller.baseIdleState == null) return null;

        return new TaskSequenceBuilder()
            .Then(controller.baseIdleState, new StateArgs_Idle(1.5f))
            .Build(this, TaskPriorities.MiscTaskLow, "ExampleWait");
    }
}
```

## Registration Options

### Attribute Discovery

The loader can discover task providers with:

```csharp
[TaskProviderId("task_provider.example.wait")]
```

This is the preferred v1 path for simple code mods.

### Manifest Discovery

Add a `*.task_providers.json` file in the mod folder:

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

The manifest maps an id to a type already present in a loaded DLL. It does not define task behavior.

### Registrar Discovery

Registrar classes may implement:

```csharp
using NurtaleNesche.Modding.Registries;

public sealed class ExampleTaskProviderRegistrar : ITaskProviderModRegistrar
{
    public void RegisterTaskProviders(TaskProviderRegistry registry)
    {
        registry.RegisterTaskProvider<ExampleWaitTaskProvider>("task_provider.example.wait", allowOverride: true);
    }
}
```

`TaskProviderRegistry` is still a runtime type in v1. This is allowed for experimental code mods, but it is not a stable SDK abstraction.
`RegisterTaskProvider<T>` is an experimental convenience extension method over that runtime registry.

The same helper pattern exists for current task-option registries:

- `RegisterCaptiveTreatingTaskOption<T>`.
- `RegisterCollectCaptiveTaskOption<T>`.
- `RegisterSoloActionTaskOption<T>`.
- `RegisterChaseTaskOption<T>`.
- `RegisterAbilityTaskOption<T>`.
- `RegisterArrowHitEventTaskOption<T>`.
- `RegisterPotionHitEventTaskOption<T>`.

The helper pattern also exists for other current code-mod factory registries:

- `RegisterSense<T>`.
- `RegisterStatusEffect<T>`.
- `RegisterItemSelectionRequestFactory<T>`.

## Current Experimental Coupling

These are still runtime-coupled and may change. Avoid depending on undocumented runtime types unless this guide explicitly needs them:

- `TaskProviderRegistry`.
- `Patroller.PatrollerTaskInfoContainer`.
- `State`.
- `StateArgs` and concrete `StateArgs_*` classes.
- concrete built-in states exposed through `controller`.
- `priority` as a protected runtime field.
- `Patroller.CommonTaskPriorityEnum`.

Prefer `TaskPriorities`, `TaskInfo`, and `TaskSequenceBuilder` where they fit, but do not treat them as stable until the SDK is promoted beyond experimental.

## Data Connection

After the provider id is registered, patroller JSON can reference it in that patroller's task loadout.

Keep at least one safe fallback task provider in the patroller data. A code mod provider that fails to load should not leave the patroller with no valid behavior.

## Validation

Run the editor validator after adding or changing code mod manifests:

```text
Tools > Mod Data > Validate V1 Experimental Mods
```

The current validator is not a compiler. It can catch common manifest problems, but it cannot prove that a DLL type compiles or behaves correctly.
