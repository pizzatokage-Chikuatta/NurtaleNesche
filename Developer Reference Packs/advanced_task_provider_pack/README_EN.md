# Advanced Task Provider Reference Pack

This folder is reference source for advanced code/DLL modders.

It is not the main tutorial.

Start with the full tutorials first:

~~~text
../../_Mod_Tutorial/EN/Tutorial_Group_5/40_Code_Mod_Overview.md
../../_Mod_Tutorial/EN/Tutorial_Group_5/41_Task_Provider_Code_Mod.md
../../_Mod_Tutorial/EN/Tutorial_Group_5/42_Code_Mod_Manifest_Reference.md
../../_Mod_Tutorial/EN/Tutorial_Group_5/43_Built_In_Task_System_Architecture.md
~~~

Use this pack only after you understand the DLL -> manifest -> Patroller Data pipeline.

What This Pack Is
--------

This pack is a small copy-paste source reference for one task-provider code mod.

It demonstrates:

1. A plain C# task provider class.
2. `TaskProviderIdAttribute`.
3. `ApplyConfig` reading JSON config from Patroller Data.
4. `TaskSequenceBuilder`.
5. An optional registrar class.
6. A task-provider manifest template.

What This Pack Is Not
--------

This pack is not a full buildable project.

It does not include:

1. A `.csproj`.
2. IDE-specific setup.
3. DLL build output.
4. A full Patroller Data mod.
5. A complete code-mod tutorial.

Those are intentionally covered by `_Mod_Tutorial/EN/Tutorial_Group_5` so there is only one maintained tutorial source.

Files
--------

1. `ExampleTaskProvider.cs.txt`: source for a simple wait task provider.
2. `ExampleTaskProviderRegistrar.cs.txt`: optional explicit registry helper.
3. `example.task_providers.template.json`: manifest template that maps provider ID to C# type.
4. `BUILD_NOTES_EN.txt`: short build/reference notes.
5. `README_EN.md`: this file.
6. `README_JP.md`: Japanese summary.

Minimal Final Mod Shape
--------

After compiling your DLL, the final player-facing mod folder usually looks like this:

~~~text
Mods
  Mod_YourName
    Code
      ExampleTaskMod
        ExampleTaskMod.dll
        example.task_providers.json
    Patrollers
      Your Patroller Data Folder
        mod.json
        Patroller Data
          Stage 6
            patroller.your_test.json
~~~

The DLL contains the C# behavior.

The `example.task_providers.json` file registers the provider ID.

The Patroller Data JSON must reference that provider ID in `tasks.providers`.

Important Runtime Rule
--------

The game does not compile `.cs` files from `Mods`.

Players must receive the compiled DLL.

The `.cs.txt` files in this folder are only reference source for mod authors.

Example Provider ID
--------

This pack uses:

~~~text
task_provider.example.wait
~~~

If you publish a real mod, use your own unique ID, for example:

~~~text
task_provider.your_name.wait
~~~

Example Patroller Data Entry
--------

The provider is only active when a patroller data file references it:

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

The `config.seconds` value is read by `ExampleTaskProvider.cs.txt` through `ApplyConfig`.

Compatibility
--------

Code/DLL mods are v1 experimental.

Build against DLLs from the same game version you are targeting. If the game updates, rebuild and retest the mod.
