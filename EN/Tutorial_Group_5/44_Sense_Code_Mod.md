Sense Code Mod
================================

This tutorial creates a real Code/DLL Sense mod.

A Sense is a patroller detection component. Task providers decide what to do, but Senses decide what the patroller can perceive: captives, items, chainpoints, suspicious checkpoints, sounds, or other patrollers.

Read this after:

1. `40_Code_Mod_Overview.md`
2. `41_Task_Provider_Code_Mod.md`
3. `43_Built_In_Task_System_Architecture.md`

Sense mods are more fragile than simple task providers. A bad Sense can make enemies detect through rooms, keep chasing stale targets, or stop detecting anything.

What You Are Building
--------

You are building one C# class:

~~~text
ExampleMods.Senses.ExampleNearbyPlayerSense
~~~

The game will know it by this Sense ID:

~~~text
sense.example.nearby_player
~~~

Patroller Data will later reference that ID in:

~~~json
"senses": {
  "providers": [
    {
      "id": "sense.example.nearby_player",
      "enabled": true,
      "config": {
        "distance": 4.5,
        "priority": 2
      }
    }
  ]
}
~~~

Important: registering a Sense ID is not enough. A patroller must also reference that Sense ID in `senses.providers`.

How Sense Loading Works
--------

The runtime pipeline is:

1. The game loads DLLs under `Mods`.
2. The game reads `*.senses.json`.
3. The manifest maps a Sense ID to a C# type inside the DLL.
4. `SenseFactoryRegistry` stores that ID and type.
5. When a patroller initializes, Patroller Data `senses.providers` chooses which Senses to attach.
6. The runtime adds the Sense component to the patroller's Sense object.
7. If the component implements `IJsonConfigurable`, its `ApplyConfig` method receives the JSON `config`.
8. `SenseManager` finds detector interfaces like `ICaptiveDetector`, `IItemDetector`, and `IChainpointDetector`.
9. During gameplay, `SenseManager` calls `UpdateList`.
10. The Sense writes perception into `PatrollerMemory`.

Step 1
--------

Prepare a C# Class Library project.

Recommended easiest route:

1. Open `Developer Reference Packs/code_mod_reference_project`.
2. Open `ExampleCodeMod.csproj` in Visual Studio, Visual Studio Code, or another C# IDE.
3. Use the existing `src\Senses\ExampleNearbyPlayerSense.cs` as the starting file.
4. Build the project.
5. Copy the built DLL and the Sense manifest into your real mod folder.

If you do not use the reference project, create a normal C# Class Library project. Do not create a Unity project for this tutorial. The required result is a `.dll` file.

Required references:

1. `NurtaleNesche.Modding.Abstractions.dll`
2. `NurtaleNesche.Runtime.dll`
3. `UnityEngine.CoreModule.dll`
4. `Newtonsoft.Json.dll`
5. `netstandard.dll` if your compiler asks for it

Do not build against DLLs from a different game version.

Where to find these DLLs in a built game:

~~~text
Nurtale Nesche_Data\Managed
~~~

If you are using the public SDK package, the DLLs may also be in:

~~~text
dlls
~~~

Step 2
--------

Create this C# source file:

~~~text
ExampleNearbyPlayerSense.cs
~~~

Full source:

~~~csharp
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NurtaleNesche.Modding.Patrollers.Senses;
using Patroller;
using UnityEngine;

namespace ExampleMods.Senses
{
    public sealed class ExampleNearbyPlayerSense : Sense, ICaptiveDetector, global::IJsonConfigurable
    {
        const string SourceId = "sense.example.nearby_player";

        readonly List<CaptiveInfoOnOthers> cache = new();
        BaseController controller;
        PatrollerMemory memory;
        float distance = 4.5f;
        int priority = 2;

        public void ApplyConfig(JObject cfg)
        {
            if (cfg == null) return;

            distance = cfg["distance"]?.Value<float>() ?? distance;
            priority = cfg["priority"]?.Value<int>() ?? priority;
            distance = Mathf.Max(0.1f, distance);
        }

        public override bool Initialize()
        {
            if (!base.Initialize()) return false;

            controller = GetComponentInParent<BaseController>(true);
            memory = GetComponentInParent<PatrollerMemory>(true);
            return true;
        }

        public override bool UpdateList()
        {
            if (!base.UpdateList()) return false;

            memory?.StageForgetCaptivesBySource(SourceId);

            var region = controller?.Commons?.CurrentRegion;
            if (region?.CaptivesStaying == null)
                return true;

            foreach (var captive in region.CaptivesStaying)
            {
                TryObserve(captive);
            }

            return true;
        }

        void TryObserve(ICaptive captive)
        {
            if (controller == null || memory == null) return;
            if (captive == null) return;
            if (!captive.IsPlayer()) return;
            if (captive.GetTransform() == null) return;
            if (captive.GetCheckpoint() == null) return;
            if (captive.GetCheckpoint().Commons.CurrentRegion != controller.Commons.CurrentRegion) return;
            if (Vector2.Distance(controller.transform.position, captive.GetTransform().position) > distance) return;

            memory.StageCaptiveObservation(SourceId, captive, priority);
        }

        public void Sweep()
        {
            memory?.StageForgetCaptivesBySource(SourceId);
        }

        public List<CaptiveInfoOnOthers> GetCaptivesDetected()
        {
            if (memory == null)
                cache.Clear();
            else
                memory.CopyCaptiveObservationsTo(SourceId, cache, clone: false);

            return cache;
        }
    }
}
~~~

What each part means:

1. `Sense` makes this a Unity component-style Sense.
2. `ICaptiveDetector` tells `SenseManager` this component can detect captives.
3. `IJsonConfigurable` allows Patroller Data `config` to change `distance` and `priority`.
4. `SourceId` identifies this Sense's observations in `PatrollerMemory`.
5. `Initialize` caches runtime references after the component exists in the scene.
6. `UpdateList` is called by `SenseManager`.
7. `base.UpdateList()` prevents duplicate work in the same frame.
8. `StageForgetCaptivesBySource` clears stale observations before adding current observations.
9. `TryObserve` checks player-only, same-region, valid checkpoint, and distance.
10. `StageCaptiveObservation` writes detection into `PatrollerMemory`.
11. `Sweep` clears this Sense's observations when another system asks for a reset.
12. `GetCaptivesDetected` returns the current observations for compatibility with `SenseManager`.

PatrollerMemory Quick Explanation
--------

`PatrollerMemory` is the patroller's perception memory.

For Code Mods, it is available from `NurtaleNesche.Runtime.dll`.

Use this namespace:

~~~csharp
using Patroller;
~~~

That namespace exposes runtime types used by Sense mods, including:

1. `PatrollerMemory`
2. `BaseController`
3. `CaptiveInfoOnOthers`

A Sense normally uses `PatrollerMemory` like this:

1. Clear this Sense's old observations.
2. Scan the current valid targets.
3. Stage new observations with a unique `SourceId`.
4. Return the observations when `SenseManager` asks for them.

Common captive-memory calls:

~~~csharp
memory.StageForgetCaptivesBySource(SourceId);
memory.StageCaptiveObservation(SourceId, captive, priority);
memory.CopyCaptiveObservationsTo(SourceId, cache, clone: false);
~~~

The same source-scoped pattern exists for other perception categories:

1. `StageSuspicionCheckpointObservation` / `StageForgetSuspicionCheckpointsBySource`
2. `StageNearChainpointObservation` / `StageForgetNearChainpointsBySource`
3. `StageNearItemObservation` / `StageForgetNearItemsBySource`
4. `StageNearPatrollerObservation` / `StageForgetNearPatrollersBySource`

Important rule: do not keep old observations forever. If your Sense does not forget stale observations, patrollers may chase targets that left the room, vanished, or were destroyed.

Reference file:

~~~text
Developer Reference Packs\code_mod_reference_project\reference\PatrollerMemory_Public_Surface.cs.txt
~~~

Use that file to inspect the public methods most commonly used by Sense mods.

Step 3
--------

Build the project.

The output should be:

~~~text
ExampleCodeMod.dll
~~~

The game loads DLLs. It does not compile raw `.cs` files from `Mods`.

Step 4
--------

Put the DLL somewhere under `Mods`.

Example:

~~~text
Mods\Mod_YourName\Code\ExampleCodeMod\ExampleCodeMod.dll
~~~

Step 5
--------

Create this manifest file next to the DLL:

~~~text
example.senses.json
~~~

Content:

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

What each field means:

1. `"experimentalApiVersion": "v1"` marks this as a v1 experimental mod file.
2. `"senses"` is the list read by `SenseModLoader`.
3. `"id"` is the Sense ID used from Patroller Data.
4. `"type"` must exactly match the C# namespace and class name.
5. `"overrideExisting": false` means this will not replace a built-in Sense ID.

Step 6
--------

Edit Patroller Data so a patroller uses your Sense.

For a first test, override the Stage 1 Orc Patroller Data.

Create this folder:

~~~text
Mods\Mod_YourName\Patrollers\Mod Patroller Data\Patroller Data\Stage 1
~~~

Copy this template:

~~~text
Mods\mod.json templates\mod.template_folder_backed.json
~~~

Paste it here and rename it to `mod.json`:

~~~text
Mods\Mod_YourName\Patrollers\Mod Patroller Data\mod.json
~~~

Put this content in `mod.json`:

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "patrollerData",
  "entry": "Patroller Data",
  "overwriteExisting": true
}
~~~

Copy this built-in Patroller Data file:

~~~text
StreamingAssets\Patrollers\Data\Stage 1\patroller.orc.stage_01.json
~~~

Paste it here:

~~~text
Mods\Mod_YourName\Patrollers\Mod Patroller Data\Patroller Data\Stage 1\patroller.orc.stage_01.json
~~~

Open the copied `patroller.orc.stage_01.json`, find:

~~~json
"senses": {
  "providers": [
~~~

Add this entry inside the existing `providers` list.

Example entry:

~~~json
{
  "id": "sense.example.nearby_player",
  "enabled": true,
  "config": {
    "distance": 4.5,
    "priority": 2
  }
}
~~~

Do not delete the other built-in Senses during the first test. If you replace the whole `senses.providers` list with only your custom Sense, the patroller may lose item detection, suspicion detection, chainpoint detection, or normal captive sight/scent detection.

Final folder shape:

~~~text
Mods
  Mod_YourName
    Code
      ExampleCodeMod
        ExampleCodeMod.dll
        example.senses.json
    Patrollers
      Mod Patroller Data
        mod.json
        Patroller Data
          Stage 1
            patroller.orc.stage_01.json
~~~

Step 7
--------

Test.

1. Restart the game after changing the DLL.
2. Open Main Menu -> Mods -> Mod Report.
3. Check that the DLL and `*.senses.json` are imported.
4. Enter a stage with the edited patroller.
5. Confirm there is no warning like `Unknown senseId`.

Main Menu reimport is not reliable DLL hot reload. Restart the game after changing the DLL.

Built-In Example 1: ItemDetector
--------

Built-in ID:

~~~text
sense.item_detector
~~~

Built-in source:

~~~text
Assets/Scripts/Patroller/Base/Sense/ItemDetector.cs
~~~

Example Patroller Data entry:

~~~json
{
  "id": "sense.item_detector",
  "enabled": true,
  "config": {
    "sightDelayTime": 1,
    "detectDistance": 7
  }
}
~~~

What this teaches:

1. It inherits `Sense`.
2. It implements `IItemDetector`.
3. `SenseManager` discovers it as an item detector.
4. It implements `IJsonConfigurable`.
5. `sightDelayTime` and `detectDistance` come from JSON `config`.
6. It scans `controller.Commons.CurrentRegion.ItemsStaying`, not the whole scene.
7. It stages item observations into `PatrollerMemory`.
8. It removes null, inactive, doomed, and out-of-region items from its cache.

Important lesson: detectors must clean stale observations. If a Sense remembers destroyed objects, enemies can react to things that no longer exist.

Built-In Example 2: AdjacentCaptiveDetector
--------

Built-in ID:

~~~text
sense.captive_detector_adjacent
~~~

Built-in source:

~~~text
Assets/Scripts/Patroller/Base/Sense/AdjacentCaptiveDetector.cs
~~~

Example Patroller Data entry:

~~~json
{
  "id": "sense.captive_detector_adjacent",
  "enabled": true,
  "config": {
    "priority": 3,
    "playerOnly": true,
    "distance": 5
  }
}
~~~

What this teaches:

1. It implements `ICaptiveDetector`.
2. It checks captives in the current region.
3. It rejects captives outside the same region.
4. It checks distance before adding an observation.
5. It uses `priority` so stronger observations can win.
6. It clears its source before rebuilding current observations.

Important lesson: region checks matter. If a custom Sense ignores regions, patrollers can detect through walls or across room transitions.

Common Mistakes
--------

1. The DLL was changed but the game was not restarted.
2. The manifest filename is not `*.senses.json`.
3. The manifest `type` does not match the C# namespace and class name.
4. The Sense ID is registered but not added to Patroller Data `senses.providers`.
5. The Patroller Data override accidentally removed required built-in Senses.
6. `UpdateList` scans the whole scene every frame and hurts performance.
7. The custom Sense stores destroyed Unity objects and creates stale detection.
8. The custom Sense detects across regions and creates through-wall behavior.

When To Make A Sense Mod
--------

Make a Sense mod when you need a new way for patrollers to perceive the world.

Good reasons:

1. A patroller should detect a new object category.
2. A patroller should use a special detection distance or region rule.
3. A patroller should react to a custom gameplay object.

Bad reasons:

1. You only want the patroller to perform a new action. Use a Task Provider.
2. You only want different HP or speed. Use Patroller Data.
3. You only want different sprites. Use animation/sprite mods.
