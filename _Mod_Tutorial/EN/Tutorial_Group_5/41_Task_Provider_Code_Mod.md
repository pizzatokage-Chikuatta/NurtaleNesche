Task Provider Code Mod
================================

This tutorial creates a minimal Code/DLL task provider.

The example provider makes a patroller wait briefly by emitting an idle task. This is intentionally simple. The goal is to prove the full DLL -> manifest -> Patroller Data pipeline.

What You Are Building
--------

You are building one C# class:

~~~text
ExampleMods.Tasks.ExampleWaitTaskProvider
~~~

The game will know it by this task provider ID:

~~~text
task_provider.example.wait
~~~

Patroller Data will later reference that ID.

Step 1
--------

Prepare a C# Class Library project in your C# IDE.

The required result is a DLL file. The exact IDE is up to you.

Example project name:

~~~text
ExampleTaskMod
~~~

Do not create this as a Unity game project. A normal C# class library is enough.

Step 2
--------

Add references to the same-version game DLLs.

For this tutorial, use:

1. NurtaleNesche.Modding.Abstractions.dll
2. NurtaleNesche.Runtime.dll
3. UnityEngine.CoreModule.dll
4. Newtonsoft.Json.dll
5. netstandard.dll if your compiler asks for it

Where to find them in a built game:

~~~text
Nurtale Nesche_Data\Managed
~~~

Do not use DLLs from a different game version.

Step 3
--------

Create this C# source file in your project:

~~~text
ExampleWaitTaskProvider.cs
~~~

Full source:

~~~csharp
using Newtonsoft.Json.Linq;
using NurtaleNesche.Modding.Patrollers.Tasks;

namespace ExampleMods.Tasks
{
    [TaskProviderId("task_provider.example.wait")]
    public sealed class ExampleWaitTaskProvider : TaskSetCore
    {
        float seconds = 1.5f;

        public override void ApplyConfig(JObject cfg)
        {
            seconds = cfg?["seconds"]?.Value<float?>() ?? seconds;
        }

        public override Patroller.PatrollerTaskInfoContainer GetNewTask()
        {
            if (!IsConditionMetForPriority(TaskPriorities.MiscTaskLow))
                return null;

            if (controller == null || controller.baseIdleState == null)
                return null;

            return new TaskSequenceBuilder()
                .Then(controller.baseIdleState, new global::StateArgs_Idle(seconds))
                .Build(this, TaskPriorities.MiscTaskLow, "ExampleWait");
        }
    }
}
~~~

What each part means:

1. `TaskProviderId` declares an ID that the game can register from the DLL.
2. The ID is `task_provider.example.wait`.
3. `TaskSetCore` is the plain C# task-provider base class used by current v1 code mods.
4. `seconds` is custom data for this example provider.
5. `ApplyConfig` is called when JSON config is applied from Patroller Data.
6. `GetNewTask` is called by the scheduler when the provider can propose behavior.
7. `return null` means "do not propose any task right now." It is not an error.
8. `IsConditionMetForPriority` prevents the provider from interrupting stronger tasks.
9. `controller` is the owner patroller.
10. `controller.baseIdleState` is the built-in idle state.
11. `StateArgs_Idle(seconds)` tells the idle state how long to idle.
12. `TaskSequenceBuilder` builds the task container the current runtime expects.

Why global::StateArgs_Idle Is Used
--------

`StateArgs_Idle` is currently a runtime type in the global namespace.

The `global::` prefix prevents C# from searching for `StateArgs_Idle` inside your own namespace first. It is not always required, but it avoids confusion in tutorials.

Step 4
--------

Build the project.

The output should be:

~~~text
ExampleTaskMod.dll
~~~

The game will not compile ExampleWaitTaskProvider.cs from Mods. Only the compiled DLL is loaded.

Step 5
--------

Create this folder:

~~~text
Mods\Mod_YourName\Code\ExampleTaskMod
~~~

Put the DLL here:

~~~text
Mods\Mod_YourName\Code\ExampleTaskMod\ExampleTaskMod.dll
~~~

Step 6
--------

Create this manifest file next to the DLL:

~~~text
example.task_providers.json
~~~

Put this content inside it:

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

Field meanings:

1. `experimentalApiVersion` should be `"v1"`.
2. `providers` is the list of task providers this manifest registers.
3. `id` is the task provider ID used by Patroller Data.
4. `type` is the full C# type name, including namespace and class name.
5. The manifest does not create behavior. The DLL type creates behavior.

Attribute And Manifest
--------

This tutorial uses both:

1. `[TaskProviderId("task_provider.example.wait")]`
2. `example.task_providers.json`

This is safe but partly redundant.

Attribute discovery can register the provider by scanning the DLL. Manifest discovery can also register the provider by mapping `id` to `type`. For beginners, using both makes the ID obvious in code and on disk. For advanced mods, you can choose one clean registration path.

Step 7
--------

Make a Patroller Data mod and add the provider ID to a patroller's provider list.

Example provider entry:

~~~json
{
  "id": "task_provider.example.wait",
  "priority": "MiscTaskLow",
  "providerCooldownMedian": 1,
  "config": {
    "seconds": 1.5
  }
}
~~~

Important:

1. `id` must match the manifest ID and/or `TaskProviderId`.
2. `priority` controls how strong the proposed task is compared with other current tasks.
3. `providerCooldownMedian` controls how often this provider can propose tasks after it succeeds.
4. `config.seconds` is passed to `ApplyConfig`.
5. Keep at least one safe fallback provider in the patroller data.
6. Do not remove all existing behavior while testing your first Code Mod.

The exact placement depends on the copied Patroller Data JSON. Search for the existing `tasks.providers` list in the target patroller JSON and add this entry there.

Priority Notes
--------

For this tutorial, use:

~~~text
MiscTaskLow
~~~

The example only waits, so it should not interrupt chase, attack, damage, event, or die tasks. Do not use high priorities until you understand ActionScheduler2 behavior.

Provider Cooldown Notes
--------

`providerCooldownMedian` is not the same as the idle duration.

`seconds` controls how long the emitted idle task lasts. `providerCooldownMedian` controls how long the provider waits before proposing again after an accepted proposal.

Step 8
--------

Start or restart the game.

Open:

~~~text
Main Menu -> Mods -> Mod Report
~~~

Check for manifest warnings or type-resolution warnings.

Step 9
--------

Test in gameplay.

If the patroller still behaves normally and no type warning appears, the DLL was loaded and the provider was registered. If you want to prove the provider is active, temporarily test with a patroller data loadout where this provider has a visible effect and does not compete with stronger providers.

Temporary debug option:

~~~csharp
UnityEngine.Debug.Log("ExampleWaitTaskProvider proposed a wait task.");
~~~

Use this only while testing. Remove noisy logs before sharing a mod.

Restart Rule
--------

When you change the DLL, restart the game.

The game caches loaded assemblies by full path. Reimporting mods from the main menu is not a reliable way to reload a changed DLL from the same path.

Common Mistakes
--------

1. Putting .cs files in Mods and expecting the game to compile them.
2. Forgetting the manifest file.
3. Using the wrong manifest filename. It must end with `.task_providers.json`.
4. Using the wrong type name. The `type` field must include the namespace.
5. Building against DLLs from a different game version.
6. Removing fallback task providers from Patroller Data.
7. Expecting DLL hot reload to work after pressing reimport.
8. Writing config in Patroller Data but not reading it in `ApplyConfig`.
9. Returning a task with too high priority and interrupting important gameplay.

Minimal Final Shape
--------

~~~text
Mods
|-- Mod_YourName
    |-- Code
        |-- ExampleTaskMod
            |-- ExampleTaskMod.dll
            |-- example.task_providers.json
~~~

What to Read Next
--------

Read 43_Built_In_Task_System_Architecture.md next if you want to understand task sets, task options, event task sets, and built-in examples.