# Developer Reference Packs

These folders are not normal public JSON mod examples.

They are reference material for advanced modders who already read the tutorials.

Normal JSON/PNG mod authors should start with:

~~~text
../_Mod_Tutorial/EN/START_HERE.md
~~~

Code/DLL mod authors should read the full tutorials first:

~~~text
../_Mod_Tutorial/EN/Tutorial_Group_5
~~~

Unity Editor / AssetBundle prefab mod authors should read:

~~~text
../_Mod_Tutorial/EN/Tutorial_Group_6
~~~

Packs
--------

1. `code_mod_reference_project`
   - Buildable experimental Code/DLL reference project.
   - Covers the manifest-backed code-mod surfaces documented in Tutorial Group 5.
   - Use this when you need a real `.csproj` and source layout.

2. `advanced_task_provider_pack`
   - Source-only task-provider reference snippets.
   - Kept as compact reference material.
   - Not a full tutorial and not a buildable project.

3. `assetbundle_prefab_reference_pack`
   - Unity prefab sample package/reference for AssetBundle prefab mods.
   - Used by Tutorial Group 6.

Rule
--------

Do not maintain separate tutorials inside these packs.

The tutorial source of truth is always `../_Mod_Tutorial`.
