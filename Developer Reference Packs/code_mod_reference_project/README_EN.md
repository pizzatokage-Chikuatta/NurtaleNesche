# Code Mod Reference Project

This is a buildable reference project for experimental v1 Code/DLL mods.

It is not the main tutorial.

Read the tutorials first:

~~~text
../../_Mod_Tutorial/EN/Tutorial_Group_5
~~~

This project exists so code-modders can see a real `.csproj`, source layout, and manifest template set.

What This Project Covers
--------

Buildable examples:

1. Task provider: `*.task_providers.json`.
2. Arrow/potion event task options: `*.event_task_options.json`.
3. Captive-treating task option: `*.captive_treating_task_options.json`.
4. Sense component: `*.senses.json`.
5. Status effect factory: `*.status_effect_factories.json`.
6. Item-selection request factory: `*.item_selection_request_factories.json`.

Advanced source snippets only:

1. Solo action task option.
2. Chase task option.
3. Ability task option.
4. Collect-captive task option.

Those advanced option categories use registry helpers and are more runtime-coupled. They are included as `.cs.txt` snippets, not as beginner-safe manifest examples.

Build Setup
--------

This project expects same-version game DLLs.

In the public package, the default project path expects this layout:

~~~text
NurtaleNesche_Modding_v1_Public
  dlls
    NurtaleNesche.Modding.Abstractions.dll
    NurtaleNesche.Runtime.dll
    UnityEngine.CoreModule.dll
    Newtonsoft.Json.dll
  Developer Reference Packs
    code_mod_reference_project
      ExampleCodeMod.csproj
~~~

If your DLLs are somewhere else, build with:

~~~text
dotnet build ExampleCodeMod.csproj /p:NurtaleNescheDllDir="C:/Path/To/Nurtale Nesche_Data/Managed"
~~~

If `Newtonsoft.Json.dll` or `UnityEngine.CoreModule.dll` is in a different folder, also pass:

~~~text
/p:NewtonsoftDllDir="C:/Path/To/NewtonsoftFolder" /p:UnityDllDir="C:/Path/To/UnityEngineFolder"
~~~

Output
--------

The built DLL is:

~~~text
bin/Debug/netstandard2.1/ExampleCodeMod.dll
~~~

The game loads DLLs. It does not compile `.cs` files from `Mods`.

Final Mod Folder Example
--------

~~~text
Mods
  Mod_YourName
    Code
      ExampleCodeMod
        ExampleCodeMod.dll
        example.task_providers.json
        example.event_task_options.json
        example.captive_treating_task_options.json
        example.senses.json
        example.status_effect_factories.json
        example.item_selection_request_factories.json
~~~

Copy and rename only the manifests you actually use.

Do not ship unused example manifests in a real mod.

Important
--------

Code/DLL mods are v1 experimental.

Build against DLLs from the same game version you target. Restart the game after changing DLLs.
