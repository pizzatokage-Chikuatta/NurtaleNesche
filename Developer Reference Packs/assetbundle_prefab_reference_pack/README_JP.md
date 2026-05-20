# AssetBundle Prefab Reference Pack

このReference Packはチュートリアル項目６用です。

Group 6のPrefab Modは、文章だけでは十分に親切ではありません。ModderはCustom PatrollerやCustom Chainpoint Prefabを作る前に、動作するPrefabのHierarchyとComponent構成を確認する必要があります。

## 公開時に必要なもの

このフォルダーには以下のUnityPackageを入れてください。

~~~text
NurtaleNesche_AssetBundlePrefabSamples_v1.unitypackage
~~~

このUnityPackageが無い場合、公開用Group 6 Prefabチュートリアルは未完成です。

## 収録予定Prefab

Patrollerサンプル:

1. Goblin。
2. Bandit。
3. Shadow Creature。

Chainpointサンプル:

1. Cuff。
2. Mouthpiece Chain。
3. Kabeshiri Front Group。
4. Milk Potion Machine。

任意の補助ファイル:

1. 再配布して問題なければ、プロジェクトで使用したAssetBundleビルドスクリプト。

## Exportルール

推奨プロジェクトヘルパー:

~~~text
Tools -> Mod Data -> Export AssetBundle Prefab Sample Package
~~~

手動で行う場合はUnity Editorで以下を使います。

~~~text
Assets -> Export Package...
~~~

以下を有効にします。

~~~text
Include dependencies
~~~

生の.prefabファイルだけを配布しないでください。依存関係が無いPrefabだけでは、Modderにとって基本的に不十分です。

## ライセンスルール

公開再配布してよいAssetだけを含めてください。

Prefabの依存関係に外注アート、ライセンス付きサードパーティAsset、公開すべきでないソースファイルが含まれる場合、代わりに公開用の簡略サンプルPrefabを作ってください。

## Modder側の流れ

1. UnityPackageをUnityプロジェクトへImportする。
2. サンプルPrefabを開く。
3. HierarchyとComponent構成を確認する。
4. サンプルPrefabをDuplicateする。
5. DuplicateしたPrefabを編集する。
6. DuplicateしたPrefabからAssetBundleをビルドする。
7. チュートリアル項目６に従ってJSONへ接続する。
