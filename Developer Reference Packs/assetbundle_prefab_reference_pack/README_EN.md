# AssetBundle Prefab Reference Pack

This reference pack is for Tutorial Group 6.

Group 6 prefab mods are not friendly enough with text-only instructions. Modders need to inspect working prefab hierarchy and components before making custom patroller or chainpoint prefabs.

## Required Public Artifact

This folder should include a UnityPackage such as:

~~~text
NurtaleNesche_AssetBundlePrefabSamples_v1.unitypackage
~~~

If that UnityPackage is missing, the public Group 6 prefab tutorials are incomplete.

## Recommended Contents

Export a UnityPackage from the game Unity project with dependencies included.

Sample prefabs included in the intended public pack:

Patroller samples:

1. Goblin.
2. Bandit.
3. Shadow Creature.

Patroller sample prefabs must preserve their H-scene setup if they can play in-game H scenes. In particular, check the BaseController "baseInGameHState" field and the related HSceneControl list or controller-specific equivalent. Without that inspector data, a mod patroller can catch the player but fail to start the H scene animation.

Chainpoint samples:

1. Cuff.
2. Mouthpiece Chain.
3. Kabeshiri Front Group.
4. Milk Potion Machine.

Recommended optional support file:

1. The AssetBundle build script used by the project, if it is safe to redistribute.

## Export Rule

Recommended project helper:

~~~text
Tools -> Mod Data -> Export AssetBundle Prefab Sample Package
~~~

Manual Unity Editor path:

~~~text
Assets -> Export Package...
~~~

Enable:

~~~text
Include dependencies
~~~

Do not ship loose .prefab files alone. Loose prefabs without dependencies are usually not enough for modders.

## Licensing Rule

Only include assets you are allowed to redistribute publicly.

If a prefab dependency uses outsourced art, licensed third-party assets, or any source file that should not be public, make a stripped sample prefab instead.

## Modder Workflow

1. Import the UnityPackage into a Unity project.
2. Open the sample prefab.
3. Study the hierarchy and components.
4. Duplicate the sample prefab.
5. Modify the duplicate.
6. Build an AssetBundle from the duplicate.
7. Connect it to JSON by following Tutorial Group 6.

For patrollers, do not delete inspector-assigned state data while editing the duplicate. H-scene capable prefabs especially need their BaseController "baseInGameHState" and HSceneControl setup preserved.
