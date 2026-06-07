Unity Editor AssetBundle Mod Overview
================================

This group explains prefab mods made with Unity Editor and AssetBundles.

Use this group only after you understand normal JSON mods. Prefab mods are powerful, but they are easier to break than item, drop table, patroller data, or animation JSON mods.

Required Sample Prefab Pack
--------

Do not start prefab modding from a blank GameObject.

Before following the patroller or chainpoint prefab tutorials, import or inspect the sample prefab package in:

~~~text
Mods\Developer Reference Packs\assetbundle_prefab_reference_pack
~~~

That reference pack should contain a UnityPackage with these sample prefabs:

1. Patroller samples: Goblin, Bandit, Shadow Creature.
2. Chainpoint samples: Cuff, Mouthpiece Chain, Kabeshiri Front Group, Milk Potion Machine.
3. Notes about which sample paths were used to build AssetBundles.

The text checklists in these tutorials are safety checks. They are not a replacement for studying the hierarchy and components of a working prefab.

What This Workflow Does
--------

The built game cannot load a raw .prefab file from Mods.

Instead, the modder does this:

1. Make or edit a prefab in Unity Editor.
2. Build that prefab into an AssetBundle.
3. Put the actual AssetBundle file into the Mods folder.
4. Write JSON that tells the game which bundle file and which prefab path inside that bundle should be loaded.

The two important JSON fields are:

1. "assetBundleFileName": the actual AssetBundle file name.
2. "prefabAssetPathInsideBundle": the prefab path recorded inside the bundle.

Do not use "prefabAssetName" in new tutorials or new mods. It is only a legacy compatibility alias.

What AssetBundle Output Files Mean
--------

Unity can output files like this:

~~~text
AssetBundles
AssetBundles.manifest
test shadow creature
test shadow creature.manifest
test milk potion machine
test milk potion machine.manifest
test shadow creature.meta
test milk potion machine.meta
~~~

Meaning:

1. "test shadow creature" and "test milk potion machine" are actual AssetBundle files. These are the files used by "assetBundleFileName".
2. "test shadow creature.manifest" and "test milk potion machine.manifest" are build reports. Runtime usually does not need them, but modders should read them.
3. The "Assets:" section inside a .manifest file shows the correct "prefabAssetPathInsideBundle".
4. "AssetBundles" and "AssetBundles.manifest" are root build metadata files. For these simple prefab mods, do not use "AssetBundles" as "assetBundleFileName".
5. ".meta" files are Unity Editor metadata. Players usually do not need them.

How To Find prefabAssetPathInsideBundle
--------

Open the bundle .manifest file.

Look for a section like this:

~~~text
Assets:
- Assets/Prefabs/Test Patroller For Asset Bundle/Shadow Creature.prefab
~~~

Copy the path after "- ".

Use it like this:

~~~json
{
  "assetBundleFileName": "test shadow creature",
  "prefabAssetPathInsideBundle": "Assets/Prefabs/Test Patroller For Asset Bundle/Shadow Creature.prefab"
}
~~~

The player who downloads the mod does not need the original .prefab file. The prefab is already inside the AssetBundle. The JSON still needs the path because Unity uses that path as the asset name inside the bundle.

Where To Put The Bundle File
--------

Put the AssetBundle file next to the JSON file that references it.

For patroller data:

~~~text
Mods\Mod_YourName\Patrollers\My AssetBundle Patroller\Patroller Data\Stage 6
  my_patroller_bundle
  patroller.my_patroller.stage_06.json
~~~

For chainpoint definitions:

~~~text
Mods\Mod_YourName\Chainpoints\My AssetBundle Chainpoint\Definitions
  my_chainpoint_bundle
  chainpoint.my_machine.json
~~~

The game resolves "assetBundleFileName" relative to the JSON file that contains that field.

mod.json vs mod.template.json
--------

"mod.json" is loaded by the game.

"mod.template.json" is not loaded as a mod manifest. Use "mod.template.json" for examples that should not activate by default.

If a tutorial says to activate the mod, rename the template to "mod.json".

Main Menu Reimport Limitation
--------

The Main Menu reimport button is mainly for JSON, PNG, audio, and data reimport.

For AssetBundle prefab changes, the safest test flow is:

1. Build the AssetBundle.
2. Copy it into Mods.
3. Start or restart the game.
4. Test.
5. If you rebuild the same bundle file, restart the game again before trusting the result.

Reimport can refresh the JSON that points to the bundle, but already-loaded bundles or script assemblies can remain cached.

Supported Tutorial Targets
--------

This group documents:

1. Patroller prefab mods.
2. Chainpoint prefab mods.

Not covered here:

1. Raw .prefab loading. It is not supported.
2. Full Unity project setup for every possible modder environment.
3. Every possible custom MonoBehaviour shape.
4. AssetBundle dependency chains. Start with one simple prefab bundle.

Common Mistakes
--------

1. Using "AssetBundles" as "assetBundleFileName" instead of the real bundle file.
2. Putting the bundle file in a different folder from the JSON that references it.
3. Typing a wrong "prefabAssetPathInsideBundle".
4. Shipping only the .manifest file and forgetting the actual bundle file.
5. Expecting raw .prefab files to load in the built game.
6. Forgetting that prefab mods are v1 experimental.

Next
--------

Read:

1. 51_Patroller_Prefab_Mod.md for patroller prefab mods.
2. 52_Chainpoint_Prefab_Mod.md for chainpoint prefab mods.
