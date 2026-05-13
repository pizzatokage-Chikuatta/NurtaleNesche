# Advanced Task Provider Reference

This folder is intentionally separated from `Example Mod Packs` because it is not a normal user JSON pack.

Code/DLL mods are v1 experimental. They are trusted local code, desktop/Mono-first, and have no stability guarantee yet.

What actually loads:

- `TaskProviderModLoader` scans `Mods` for `*.dll` files.
- It can discover task providers through `NurtaleNesche.Modding.Patrollers.Tasks.TaskProviderIdAttribute`.
- It can also read optional `*.task_providers.json` manifests.
- A manifest only maps a provider id to a C# task provider type that already exists in a loaded assembly.
- The manifest does not describe task behavior, schedules, states, movement, or sensing.
- `TaskSequenceBuilder` is the recommended v1 helper for simple task sequences; direct `State` / `StateArgs` construction is still experimental runtime coupling.

For normal JSON mods, do not use this folder. Configure patroller data to reference task provider ids already registered by the game, and keep at least one fallback provider in the patroller's task provider loadout.

Files in this folder:

- `example.task_providers.template.json`: manifest shape only. Do not rename it to `*.task_providers.json` unless the matching code assembly is present.
- `ExampleTaskProvider.cs.txt`: source reference snippet distributed as text. The game does not compile or load this file.
- `ExampleTaskProviderRegistrar.cs.txt`: optional registrar source reference snippet.
