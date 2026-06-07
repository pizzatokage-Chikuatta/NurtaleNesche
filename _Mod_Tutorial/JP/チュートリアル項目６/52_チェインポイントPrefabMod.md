チェインポイントPrefab Mod
================================

このチュートリアルでは、AssetBundle内のPrefabをランタイムスポーンされるChainpointへ接続する方法を説明します。

Chainpoint Prefab Modは上級者向けです。Chainpointは単なる見た目オブジェクトではなく、ゲームプレイスクリプト、Status Effect、Checkpoint identity、Interaction Data、Animation State Binding、Save/Restore identityを持つことがあります。

始める前に、以下のサンプルPrefabパッケージをImportしてください。

~~~text
Mods\Developer Reference Packs\assetbundle_prefab_reference_pack
~~~

サンプルChainpoint Prefabを開き、HierarchyとComponent構成を確認してください。そのサンプルをDuplicateし、Duplicateした側を編集します。Chainpoint、Checkpoint、State Animator Binding、Status/Interaction接続を理解していない場合、空のPrefabから始めないでください。

目的
--------

以下を作ります。

1. Chainpoint Definition。
2. Chainpoint Spawn。
3. Chainpoint Spawn Loadout。

接続関係:

~~~text
chainPointDefinitions id
  -> assetBundleFileName
  -> prefabAssetPathInsideBundle
  -> chainPointSpawns chainPointId
  -> chainPointSpawnLoadouts chainPointSpawnIds
~~~

必要なPrefabチェックリスト
--------

スポーンされるPrefabには以下が必要です。

1. Prefab階層内のどこかにChainpoint派生コンポーネント。
2. Chainpoint.EnsureSetupが解決できるCheckpointコンポーネント。
3. そのChainpoint種類が要求する追加コンポーネント。
4. Spawn Dataが必要なIChainpointSpawnDataReceiverコンポーネント。
5. IInitializableコンポーネントがある場合、Chainpointスポーン制御順で安全に初期化できること。

初めてChainpoint Prefab Modを作る場合、既に動作しているChainpoint Prefab形状から始めてください。

ステップ1
--------

UnityエディターでChainpoint PrefabからAssetBundleをビルドします。

出力例:

~~~text
my_chainpoint_bundle
my_chainpoint_bundle.manifest
~~~

"my_chainpoint_bundle.manifest"を開き、"Assets:"を探します。

例:

~~~text
Assets:
- Assets/MyMod/Chainpoints/MyMachine.prefab
~~~

この場合:

1. "assetBundleFileName"は"my_chainpoint_bundle"。
2. "prefabAssetPathInsideBundle"は"Assets/MyMod/Chainpoints/MyMachine.prefab"。

ステップ2
--------

Chainpoint Definition Modフォルダーを作ります。

~~~text
Mods\Mod_YourName\Chainpoints\My AssetBundle Chainpoint\Definitions
~~~

Definition JSONの横に実体AssetBundleファイルを置きます。

~~~text
Mods\Mod_YourName\Chainpoints\My AssetBundle Chainpoint\Definitions\my_chainpoint_bundle
~~~

"Definitions"の横に"mod.json"を作ります。

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "chainPointDefinitions",
  "entry": "Definitions",
  "overwriteExisting": true
}
~~~

ステップ3
--------

Chainpoint Definition JSONを作ります。

~~~text
Mods\Mod_YourName\Chainpoints\My AssetBundle Chainpoint\Definitions\chainpoint.my_machine.json
~~~

例:

~~~json
{
  "id": "chainpoint.my_machine",
  "statusId": "status_effect.breast_milk_potion_machine",
  "typeEnum": "Able_To_Play_With",
  "accessScope": "Public",
  "isNeedLockPickToEscape": false,
  "runtimeSpawnable": true,
  "interactionLoadout": "profile.milk_machine",
  "assetBundleFileName": "my_chainpoint_bundle",
  "prefabAssetPathInsideBundle": "Assets/MyMod/Chainpoints/MyMachine.prefab",
  "stateAnimatorBindings": [
    {
      "chainpointStateId": "empty",
      "mainAnimatorId": "body",
      "entryMode": "play",
      "entryStateName": "New State",
      "monitorStateNames": [
        "New State"
      ]
    },
    {
      "chainpointStateId": "captured_idle",
      "mainAnimatorId": "body",
      "entryMode": "play",
      "entryStateName": "Idle",
      "monitorStateNames": [
        "Idle"
      ]
    }
  ]
}
~~~

重要:

1. "id"はChainpoint Definition ID。
2. ステージ初期スポーンで出したい場合、"runtimeSpawnable"はtrueにする。
3. "statusId"は実在するStatus Effect IDにする。
4. "interactionLoadout"は実在するInteraction Loadout IDにする。
5. "stateAnimatorBindings"はPrefab内のAnimator IDとState名に合わせる。
6. "assetBundleFileName"は実体Bundleファイル名にする。
7. "prefabAssetPathInsideBundle"は.manifest内"Assets:"からコピーする。

ステップ4
--------

Chainpoint Spawn Modフォルダーを作ります。

~~~text
Mods\Mod_YourName\Chainpoints\My AssetBundle Chainpoint Spawns\Initial Spawns
~~~

"Initial Spawns"の横に"mod.json"を作ります。

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "chainPointSpawns",
  "entry": "Initial Spawns",
  "overwriteExisting": true
}
~~~

Spawn JSONを作ります。

~~~text
Mods\Mod_YourName\Chainpoints\My AssetBundle Chainpoint Spawns\Initial Spawns\chainpoint_spawn.my_machine.stage_06.001.json
~~~

例:

~~~json
{
  "id": "chainpoint_spawn.my_machine.stage_06.001",
  "checkpointUuid": "b0cfc6bb-46c5-4998-8181-80c34f36aeca",
  "enabled": true,
  "regionId": "4e270969-c8a4-4ade-8d50-83c82409e62d",
  "offsetX": 0,
  "heightFromGround": 2,
  "rotationY": 0,
  "chainPointId": "chainpoint.my_machine"
}
~~~

重要:

1. "checkpointUuid"は新しく生成したGUIDにする。他のChainpointのUUIDをコピーしてはいけない。
2. "regionId"はReference_Data内の実在するRegion UUIDにする。
3. "chainPointId"はステップ3のDefinition IDと一致させる。

ステップ5
--------

Chainpoint Spawn Loadout Modフォルダーを作ります。

~~~text
Mods\Mod_YourName\Chainpoints\My AssetBundle Chainpoint Loadout\Initial Spawn Loadouts
~~~

"Initial Spawn Loadouts"の横に"mod.json"を作ります。

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "chainPointSpawnLoadouts",
  "entry": "Initial Spawn Loadouts",
  "overwriteExisting": true
}
~~~

ステージロードアウトJSONを作る、またはコピーします。

~~~text
Mods\Mod_YourName\Chainpoints\My AssetBundle Chainpoint Loadout\Initial Spawn Loadouts\chainpoint_spawn_loadout.stage_06.json
~~~

例:

~~~json
{
  "chainPointSpawnLoadoutId": "chainpoint_spawn_loadout.stage_06",
  "chainPointSpawnIds": [
    "chainpoint_spawn.my_machine.stage_06.001"
  ]
}
~~~

既にそのステージに内蔵Chainpoint Spawnがある場合、StreamingAssetsから元ロードアウトをコピーし、リストを丸ごと置き換えるのではなく自分のIDを追加してください。

ステップ6
--------

ゲームを起動して確認します。

1. Main Menu -> Mods -> Mod Report。
2. 対象ステージへ入る。
3. Chainpointがスポーンしたか確認する。
4. Interactionが動くか確認する。
5. 保存/ロードが必要なChainpointならSave/Loadも確認する。

トラブルシューティング
--------

1. "Unknown chainpoint spawn id": Loadoutが存在しないSpawn IDを参照している。
2. "Unknown chainpoint id": Spawn JSONが存在しないDefinition IDを参照している。
3. "AssetBundle not found": BundleファイルがDefinition JSONの横にない、または"assetBundleFileName"が間違っている。
4. "Prefab not found in bundle": "prefabAssetPathInsideBundle"が.manifest内のパスと一致していない。
5. Chainpointがスポーンしない: "runtimeSpawnable"がfalse、regionIdが間違い、またはLoadoutにSpawn IDが入っていない可能性がある。
6. スポーンするがInteractionに失敗する: interactionLoadout、statusId、Chainpoint派生スクリプト、Checkpoint設定が間違っている可能性がある。
7. Save/Restoreがおかしい: checkpointUuidが重複している、またはリリース後に変更された可能性がある。

GUIDルール
--------

ランタイムスポーンされるChainpointには、安定した一意の"checkpointUuid"が必要です。

GUID生成器などで以下のような値を作ってください。

~~~text
b0cfc6bb-46c5-4998-8181-80c34f36aeca
~~~

二つのChainpointに同じGUIDを使ってはいけません。Mod公開後にGUIDを変えると、古いセーブがそのChainpointと一致しなくなる可能性があります。
