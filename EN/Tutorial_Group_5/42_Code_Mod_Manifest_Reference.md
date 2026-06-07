Code Mod Manifest Reference
================================

This file lists the current Code/DLL mod registration surfaces.

Read this after finishing 41_Task_Provider_Code_Mod.md. Read 43_Built_In_Task_System_Architecture.md if you do not understand the difference between task providers, task sets, task options, and event task options.

Universal Rules
--------

1. A DLL must already contain the C# type.
2. A manifest maps an ID to that C# type.
3. The manifest does not define behavior.
4. The `type` field is the full C# type name, including namespace.
5. Code mods are experimental.
6. Restart the game after changing DLLs.
7. Registering a type is only half of the work. Some other JSON/data surface must reference the registered ID.

Which JSON Uses The Registered ID
--------

1. Task provider IDs are used by Patroller Data `tasks.providers`.
2. Arrow/potion event task option IDs are used by event task set `options`.
3. Captive treating task option IDs are used by handling-captives task set `options`.
4. Sense IDs are used by Patroller Data `senses.providers`.
5. Status effect factory IDs are used when that status effect ID is activated; normally a matching status metadata JSON should exist.
6. Item selection request factory IDs are used by item interaction data that asks for that request type.

Task Providers
--------

Manifest filename:

~~~text
*.task_providers.json
~~~

Root list:

~~~text
providers
~~~

Example:

~~~json
{
  "experimentalApiVersion": "v1",
  "providers": [
    {
      "id": "task_provider.example.wait",
      "type": "ExampleMods.Tasks.ExampleWaitTaskProvider"
    }
  ]
}
~~~

Type requirement:

1. Inherit `TaskSetCore` or `TaskProviderCore`.
2. Use `TaskProviderIdAttribute` or manifest registration.
3. Implement `GetNewTask`.
4. Plain C# task providers need a public parameterless constructor.

Runtime behavior:

The scheduler asks the provider for a task. If the provider returns `null`, it declines for now. If it returns a `PatrollerTaskInfoContainer`, the scheduler can accept it based on priority and current state.

Event Task Options
--------

Event task options change how a patroller reacts to arrow hits or potion hits.

Manifest filename:

~~~text
*.event_task_options.json
~~~

Root lists:

1. arrowOptions
2. potionOptions

Example:

~~~json
{
  "experimentalApiVersion": "v1",
  "arrowOptions": [
    {
      "id": "event_task_option.arrow_hit.example_ignore",
      "type": "ExampleMods.Events.ExampleArrowIgnore",
      "overrideExisting": false
    }
  ],
  "potionOptions": [
    {
      "id": "event_task_option.potion_hit.example_ignore",
      "type": "ExampleMods.Events.ExamplePotionIgnore",
      "overrideExisting": false
    }
  ]
}
~~~

Minimal arrow skeleton:

~~~csharp
using NurtaleNesche.Modding.Patrollers.Tasks;

namespace ExampleMods.Events
{
    [ArrowHitEventTaskOptionId("event_task_option.arrow_hit.example_ignore")]
    public sealed class ExampleArrowIgnore : IArrowHitEventTaskOption
    {
        public int Priority => 0;
        public string OptionId => "event_task_option.arrow_hit.example_ignore";

        public bool CanHandle(Patroller.ArrowHitEventContext context)
        {
            return false;
        }

        public Patroller.ArrowHitEventResult Handle(Patroller.ArrowHitEventContext context)
        {
            return Patroller.ArrowHitEventResult.NotHandled;
        }
    }
}
~~~

Minimal potion skeleton:

~~~csharp
using NurtaleNesche.Modding.Patrollers.Tasks;

namespace ExampleMods.Events
{
    [PotionHitEventTaskOptionId("event_task_option.potion_hit.example_ignore")]
    public sealed class ExamplePotionIgnore : IPotionHitEventTaskOption
    {
        public int Priority => 0;
        public string OptionId => "event_task_option.potion_hit.example_ignore";

        public bool CanHandle(Patroller.PotionHitEventContext context)
        {
            return false;
        }

        public Patroller.PotionHitEventResult Handle(Patroller.PotionHitEventContext context)
        {
            return Patroller.PotionHitEventResult.NotHandled;
        }
    }
}
~~~

Important:

These skeletons compile the shape, but they intentionally do nothing. Return `CanHandle = true` only when your option really should handle the event. Returning a handled result can block later/general hit behavior.

Captive Treating Task Options
--------

Captive treating task options are child options used by the handling-captives task set.

Manifest filename:

~~~text
*.captive_treating_task_options.json
~~~

Root list:

~~~text
taskOptions
~~~

Legacy filename/list:

~~~text
*.captive_treating_action_sets.json
actionSets
~~~

The legacy name is readable for old compatibility, but do not use it for new mods.

Example:

~~~json
{
  "experimentalApiVersion": "v1",
  "taskOptions": [
    {
      "id": "task_option.example.captive_treating",
      "type": "ExampleMods.Captives.ExampleCaptiveTreatingOption",
      "overrideExisting": false
    }
  ]
}
~~~

Minimal skeleton:

~~~csharp
using NurtaleNesche.Modding.Patrollers.Tasks;

namespace ExampleMods.Captives
{
    [CaptiveTreatingTaskOptionId("task_option.example.captive_treating")]
    public sealed class ExampleCaptiveTreatingOption : ICaptiveTreatingTaskOption
    {
        public Patroller.PatrollerTaskForTreatingCaptive GetCaptiveTreatmentTask(
            Patroller.BaseController controller,
            Patroller.PatrollerMemory memory)
        {
            return null;
        }
    }
}
~~~

Important:

Returning `null` means this option declines to treat a captive right now. A real option must return a valid captive-treatment task. This surface is advanced because captive treatment can move captives, apply restraints, start H scenes, send the player to prison, or change chainpoint ownership.

Senses
--------

Sense mods add or replace sense components used by patrollers.

Manifest filename:

~~~text
*.senses.json
~~~

Root list:

~~~text
senses
~~~

Example:

~~~json
{
  "experimentalApiVersion": "v1",
  "senses": [
    {
      "id": "sense.example.nearby_player",
      "type": "ExampleMods.Senses.ExampleNearbyPlayerSense",
      "overrideExisting": false
    }
  ]
}
~~~

Important:

Sense types are Unity component-style types. Registering a sense ID is not enough. Patroller Data must reference the sense ID in `senses.providers` so the runtime knows to use it.

Do not use an empty no-op Sense as a learning sample. A useful Sense should implement a detector interface such as `ICaptiveDetector`, `IItemDetector`, `IChainpointDetector`, `ISuspicionCheckpointDetector`, or `IPatrollerDetector`. See `44_Sense_Code_Mod.md` for a full working Sense example.

Sense mods are gameplay-sensitive. They can affect detection, memory, suspicion, and performance. Do not use this as your first Code Mod.

Status Effect Factories
--------

Status effect factory mods map status effect IDs to custom status effect classes.

Manifest filename:

~~~text
*.status_effect_factories.json
~~~

Root list:

~~~text
factories
~~~

Example:

~~~json
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
~~~

Minimal skeleton:

~~~csharp
using NurtaleNesche.Modding.StatusEffects;

namespace ExampleMods.StatusEffects
{
    public sealed class ExampleStatusEffect : StatusEffect
    {
        public override void OnEntry()
        {
            base.OnEntry();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
~~~

Important:

1. The type must implement `IStatusEffect`.
2. The type must have a public parameterless constructor.
3. A matching status metadata JSON is normally required so the game knows the status ID, efficacy, visual rules, stack rules, and other metadata.
4. `overrideExisting` is not part of the current runtime manifest entry for status factories. Use `id`, optional `profileId`, and `type`.
5. Always test activation, deactivation, save, load, and scene transition.

Item Selection Request Factories
--------

Item selection request factories create custom item interaction menu requests.

Manifest filename:

~~~text
*.item_selection_request_factories.json
~~~

Root list:

~~~text
factories
~~~

Example:

~~~json
{
  "experimentalApiVersion": "v1",
  "factories": [
    {
      "id": "item_selection_request.example",
      "type": "ExampleMods.Items.ExampleItemSelectionRequestFactory"
    }
  ]
}
~~~

Minimal skeleton:

~~~csharp
using NurtaleNesche.Modding.Items;
using PlayerGroup;
using UnityEngine;

namespace ExampleMods.Items
{
    public sealed class ExampleItemSelectionRequestFactory : IItemSelectionRequestFactory
    {
        public bool TryCreate(
            string requestTypeId,
            Player player,
            GameObject itemObject,
            out IItemSelectionRequest request)
        {
            request = null;
            return false;
        }
    }
}
~~~

Important:

Returning `false` means the factory declines to create a request. A real factory must create an `IItemSelectionRequest` instance and return `true`.

Registration Options
--------

Most code-mod surfaces support one or more of these paths:

1. Attribute on the C# class.
2. Manifest file mapping ID to type.
3. Registrar class.

For beginner mods, prefer manifest plus attribute where available. Registrar classes are useful for advanced mods, but they expose more runtime registry details.

Troubleshooting Table
--------

1. Type not found: the manifest type does not match the DLL namespace/class name, or the DLL was not loaded.
2. DLL loaded but nothing registered: missing manifest, wrong manifest filename, or missing attribute/registrar.
3. Works in old build but not new build: rebuild against the new game DLLs.
4. Changed DLL but result did not change: restart the game or use a new DLL path/name.
5. Validator is green but behavior fails: validator checks manifest shape, not gameplay correctness.
6. Registered ID exists but nothing happens: the relevant Patroller Data, status metadata, item interaction data, or task-set options probably do not reference that ID.
