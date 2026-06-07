Code/DLL Mod Overview
================================

This group explains Code/DLL mods.

Use Code/DLL mods only when JSON, PNG, audio, animation, or prefab data mods are not enough. Code mods are powerful, but they are also the easiest way to break gameplay.

What a Code Mod Is
--------

A Code Mod is a compiled C# DLL.

The game does not compile raw .cs files from the Mods folder. The modder writes C# source code in a normal C# project, builds it into a .dll file, then places that DLL somewhere under Mods.

The game scans for DLL files under Mods. Then it scans known code-manifest files such as:

1. *.task_providers.json
2. *.event_task_options.json
3. *.captive_treating_task_options.json
4. *.senses.json
5. *.status_effect_factories.json
6. *.item_selection_request_factories.json

The manifest maps an ID to a C# type inside a loaded DLL. The manifest does not define behavior by itself.

Desktop Runtime Warning
--------

Code/DLL mods are a desktop runtime feature.

They are designed for the current desktop build workflow where the game can load managed DLLs from the local file system. Do not assume this works on IL2CPP-only builds, consoles, or locked-down platforms.

Security Warning
--------

Code mods are trusted local code.

They are not sandboxed. A DLL can run arbitrary C# code with the same trust level as the game process. Only install Code/DLL mods from sources you trust.

Version Warning
--------

Build code mods against DLLs from the same game version you are targeting.

If the game updates, rebuild your code mod against the new game DLLs. Even if the mod still loads, runtime-coupled types such as task states, state args, task containers, status effects, and registries are v1 experimental and can change.

Required DLL References
--------

A typical v1 task-provider Code Mod needs references to:

1. NurtaleNesche.Modding.Abstractions.dll
2. NurtaleNesche.Runtime.dll
3. UnityEngine.CoreModule.dll
4. Newtonsoft.Json.dll if your code uses JObject or reads JSON.
5. netstandard.dll if your compiler asks for it.

Where to find them in a built game:

~~~text
Nurtale Nesche_Data\Managed\NurtaleNesche.Modding.Abstractions.dll
Nurtale Nesche_Data\Managed\NurtaleNesche.Runtime.dll
Nurtale Nesche_Data\Managed\UnityEngine.CoreModule.dll
Nurtale Nesche_Data\Managed\Newtonsoft.Json.dll
~~~

If you are using the public SDK zip, these DLLs may also be included there. The important rule is that the DLLs must match the game version being modded.

Why NurtaleNesche.Runtime.dll Is Still Needed
--------

NurtaleNesche.Modding.Abstractions.dll contains public modding-facing names, attributes, and helper aliases.

Task provider mods still use runtime-coupled game types in v1, such as:

1. Patroller.PatrollerTaskInfoContainer
2. State
3. StateArgs and concrete StateArgs_* classes
4. BaseController and built-in states exposed by the controller
5. task registries and event contexts

Those live in NurtaleNesche.Runtime.dll. This is why task-provider code mods usually need both DLLs.

Main Menu Reimport Is Not DLL Hot Reload
--------

The Main Menu mod reimport button is for JSON, PNG, audio, and data reimport.

Do not treat it as reliable DLL hot reload. If a DLL was already loaded from the same path, the game can keep using the already-loaded assembly. The clean testing flow is:

1. Build the DLL.
2. Copy it into Mods.
3. Start or restart the game.
4. Test.
5. If you change the DLL, restart the game again.

Advanced test workaround:

~~~text
ExampleTaskMod_v2.dll
~~~

Using a new DLL filename/path can force a new assembly path, but restarting the game is still cleaner.

What Code Mods Can Currently Add
--------

1. Task providers.
2. Arrow-hit and potion-hit event task options.
3. Captive treating task options.
4. Senses.
5. Status effect factories.
6. Item selection request factories.

Recommended Reading Order
--------

1. Read 41_Task_Provider_Code_Mod.md first.
2. Read 43_Built_In_Task_System_Architecture.md to understand the built-in task/task-set/task-option structure.
3. Read 42_Code_Mod_Manifest_Reference.md when you need non-task-provider manifest shapes.

Recommended Starting Point
--------

Start with a task provider.

Task providers are the best first Code Mod target because they have a clear lifecycle:

1. The provider is registered by ID.
2. Patroller Data references that provider ID.
3. The provider emits a task when its conditions are met.

Other code-mod surfaces are supported, but they are more advanced because they touch damage events, status lifecycle, senses, or UI interaction systems.

Where to Put Code Mods
--------

Example:

~~~text
Mods\Mod_YourName\Code\ExampleTaskMod\ExampleTaskMod.dll
Mods\Mod_YourName\Code\ExampleTaskMod\example.task_providers.json
~~~

The exact folder name is flexible. The important part is that the DLL and the manifest are somewhere under Mods.

What This Group Does Not Cover
--------

Unity Editor asset mods, such as custom patroller prefab mods or custom chainpoint prefab mods, are not Code/DLL mods. Those require a separate Unity Editor asset workflow and will be covered separately in Tutorial Group 6.