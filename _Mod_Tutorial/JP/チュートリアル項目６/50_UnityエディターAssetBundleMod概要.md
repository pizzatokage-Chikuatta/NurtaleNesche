UnityエディターAssetBundle Mod概要
================================

この項目では、Unityエディターで作ったPrefabをAssetBundle化し、ゲームのModとして読み込ませる方法を説明します。

普通のJSON Modを理解してから触ってください。Prefab Modは強力ですが、Item、Drop Table、Patroller Data、Animation JSONなどより壊れやすいです。

必須サンプルPrefabパック
--------

空のGameObjectからPrefab Modを作り始めないでください。

PatrollerまたはChainpointのPrefabチュートリアルを進める前に、以下のサンプルPrefabパッケージをImportまたは確認してください。

~~~text
Mods\Developer Reference Packs\assetbundle_prefab_reference_pack
~~~

このReference Packには以下のサンプルPrefabを入れる予定です。

1. Patrollerサンプル: Goblin、Bandit、Shadow Creature。
2. Chainpointサンプル: Cuff、Mouthpiece Chain、Kabeshiri Front Group、Milk Potion Machine。
3. AssetBundleを作った時のサンプルパス説明。

このチュートリアル内のコンポーネント一覧は安全確認用です。動作するPrefabの階層とコンポーネント構造を確認することの代わりにはなりません。

この流れでやること
--------

ビルド済みゲームはModsフォルダーから生の.prefabファイルを読み込みません。

代わりに以下の流れになります。

1. UnityエディターでPrefabを作る、または編集する。
2. そのPrefabをAssetBundleにビルドする。
3. 実体のAssetBundleファイルをModsフォルダーへ入れる。
4. どのBundleファイルの中のどのPrefabを読むかをJSONで指定する。

重要なJSONフィールドは二つです。

1. "assetBundleFileName": 実体のAssetBundleファイル名。
2. "prefabAssetPathInsideBundle": そのBundle内に記録されているPrefabのパス。

新しいModでは"prefabAssetName"を使わないでください。これは古い互換用の別名です。

AssetBundle出力ファイルの意味
--------

Unityは以下のようなファイルを出力します。

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

意味:

1. "test shadow creature"や"test milk potion machine"が実体のAssetBundleファイルです。"assetBundleFileName"にはこれを書きます。
2. "test shadow creature.manifest"や"test milk potion machine.manifest"はビルド情報です。ランタイムでは普通不要ですが、Modderは中身を見る必要があります。
3. .manifest内の"Assets:"に"prefabAssetPathInsideBundle"へ入れる正しいパスが書かれています。
4. "AssetBundles"と"AssetBundles.manifest"はビルド全体のメタデータです。今回のような単体Prefab Modでは、普通"AssetBundles"を"assetBundleFileName"にしません。
5. ".meta"はUnityエディター用メタデータです。プレイヤーに配布する必要は基本ありません。

prefabAssetPathInsideBundleの調べ方
--------

Bundleの.manifestファイルを開きます。

以下のような場所を探します。

~~~text
Assets:
- Assets/Prefabs/Test Patroller For Asset Bundle/Shadow Creature.prefab
~~~

"- "の後ろをコピーします。

JSONでは以下のように使います。

~~~json
{
  "assetBundleFileName": "test shadow creature",
  "prefabAssetPathInsideBundle": "Assets/Prefabs/Test Patroller For Asset Bundle/Shadow Creature.prefab"
}
~~~

Modをダウンロードしたプレイヤーは元の.prefabファイルを持っている必要はありません。Prefab本体はAssetBundleの中に入っています。ただしUnityがBundle内のアセット名としてこのパスを使うため、JSONにはパスを書く必要があります。

Bundleファイルを置く場所
--------

AssetBundleファイルは、それを参照するJSONファイルの横に置きます。

Patroller Dataの場合:

~~~text
Mods\Mod_YourName\Patrollers\My AssetBundle Patroller\Patroller Data\Stage 6
  my_patroller_bundle
  patroller.my_patroller.stage_06.json
~~~

Chainpoint Definitionの場合:

~~~text
Mods\Mod_YourName\Chainpoints\My AssetBundle Chainpoint\Definitions
  my_chainpoint_bundle
  chainpoint.my_machine.json
~~~

ゲームは"assetBundleFileName"を、そのフィールドが書かれているJSONファイルからの相対位置として解決します。

mod.jsonとmod.template.json
--------

"mod.json"はゲームに読み込まれます。

"mod.template.json"はMod manifestとして読み込まれません。デフォルトで起動してほしくないサンプルは"mod.template.json"にしてください。

チュートリアルでModを有効化する場合は、テンプレートを"mod.json"へリネームします。

メインメニュー再インポートの限界
--------

メインメニューの再インポートボタンは、主にJSON、PNG、audio、dataの再読み込み用です。

AssetBundle Prefabを変更した場合、一番安全なテスト手順は以下です。

1. AssetBundleをビルドする。
2. Modsへコピーする。
3. ゲームを起動または再起動する。
4. テストする。
5. 同じBundleファイルを再ビルドした場合、信用できるテストをする前にゲームを再起動する。

再インポートはBundleを指すJSONを更新できますが、読み込み済みBundleやDLLはキャッシュされる可能性があります。

対応するチュートリアル対象
--------

この項目では以下を扱います。

1. Patroller Prefab Mod。
2. Chainpoint Prefab Mod。

ここでは扱わないもの:

1. 生の.prefab読み込み。対応していません。
2. あらゆるModder環境のUnityプロジェクト構築。
3. 全てのCustom MonoBehaviour形状。
4. AssetBundle依存関係チェーン。まずは単体Prefab Bundleから始めてください。

よくあるミス
--------

1. 実体Bundleファイルではなく"AssetBundles"を"assetBundleFileName"にしてしまう。
2. Bundleファイルを参照JSONとは別フォルダーに置く。
3. "prefabAssetPathInsideBundle"を打ち間違える。
4. .manifestだけ配布して、実体Bundleファイルを忘れる。
5. 生の.prefabがビルド済みゲームで読み込まれると思い込む。
6. Prefab Modがv1実験的機能であることを忘れる。

次に読む
--------

1. Patroller Prefab Modは51_パトローラーPrefabMod.md。
2. Chainpoint Prefab Modは52_チェインポイントPrefabMod.md。
