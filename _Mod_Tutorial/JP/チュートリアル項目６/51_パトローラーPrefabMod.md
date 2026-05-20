パトローラーPrefab Mod
================================

このチュートリアルでは、AssetBundle内のPrefabを新しいPatroller Dataへ接続する方法を説明します。

これは完全にゼロから新しい敵AIを設計するチュートリアルではありません。最初は、既に動作しているPatroller Prefab形状とPatroller Data JSONを改造するのが一番安全です。

始める前に、以下のサンプルPrefabパッケージをImportしてください。

~~~text
Mods\Developer Reference Packs\assetbundle_prefab_reference_pack
~~~

サンプルPatroller Prefabを開き、HierarchyとComponent構成を確認してください。そのサンプルをDuplicateし、Duplicateした側を編集します。ランタイムに必要なComponentを理解していない場合、空のPrefabから始めないでください。

目的
--------

AssetBundleからPrefabを読み込む新しいPatroller IDを作ります。

接続関係:

~~~text
Patroller Data id
  -> assetBundleFileName
  -> prefabAssetPathInsideBundle
  -> Patroller Spawn JSON
  -> Patroller Spawn Loadout JSON
~~~

必要なPrefabチェックリスト
--------

スポーンされるPrefabには、ゲームが期待するコンポーネントが必要です。

最低限、ランタイムスポーンパイプラインは以下を期待します。

1. Commons。
2. DungeonDwellerCommons。
3. IPatrollerControlを実装したコンポーネント。

実際の敵では、正しいAnimation関連コンポーネント、Hit Manager、Collider、Rigidbody、Renderer、Sense関連オブジェクト、Audio/Event関連コンポーネント、Controller固有の子オブジェクトも必要になることが多いです。

これらをまだ理解していない場合、完全オリジナルPrefabを作る前に、既に動作しているPatroller Prefabを改造してください。

ステップ1
--------

UnityエディターでPatroller PrefabからAssetBundleをビルドします。

出力例:

~~~text
my_shadow_bundle
my_shadow_bundle.manifest
~~~

"my_shadow_bundle.manifest"を開き、"Assets:"を探します。

例:

~~~text
Assets:
- Assets/MyMod/Patrollers/MyShadow.prefab
~~~

この場合:

1. "assetBundleFileName"は"my_shadow_bundle"。
2. "prefabAssetPathInsideBundle"は"Assets/MyMod/Patrollers/MyShadow.prefab"。

ステップ2
--------

以下のフォルダーを作ります。

~~~text
Mods\Mod_YourName\Patrollers\My AssetBundle Patroller\Patroller Data\Stage 6
~~~

Patroller JSONの横に実体AssetBundleファイルを置きます。

~~~text
Mods\Mod_YourName\Patrollers\My AssetBundle Patroller\Patroller Data\Stage 6\my_shadow_bundle
~~~

.manifestだけ置いても動きません。実体Bundleファイルが必要です。

ステップ3
--------

"Mods\mod.json templates"から"mod.template_folder_backed.json"をコピーします。

以下へ貼り付けます。

~~~text
Mods\Mod_YourName\Patrollers\My AssetBundle Patroller
~~~

名前を"mod.json"へ変更します。

"mod.json"を以下のように編集します。

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "patrollerData",
  "entry": "Patroller Data",
  "overwriteExisting": true
}
~~~

ステップ4
--------

Patroller Data JSONを作ります。

~~~text
Mods\Mod_YourName\Patrollers\My AssetBundle Patroller\Patroller Data\Stage 6\patroller.my_shadow.stage_06.json
~~~

最初のテストでは、既に動作しているPatroller Data JSONをコピーして編集してください。どのデフォルト値が安全か理解していない場合、極小JSONをゼロから書かない方が安全です。

重要なAssetBundleフィールドは以下です。

~~~json
{
  "id": "patroller.my_shadow.stage_06",
  "NameInAlphabet": "My Shadow",
  "assetBundleFileName": "my_shadow_bundle",
  "prefabAssetPathInsideBundle": "Assets/MyMod/Patrollers/MyShadow.prefab",
  "statusProfileId": "profile_status_effect.patroller.enemy_default",
  "actionRuleProfileId": "profile_action_rule.patroller.enemy_default",
  "dropTableProfileId": "drop_table.shadow_creature.stage_06",
  "shadowProfileId": "shadow_profile.shadow_creature",
  "animationSetId": "Shadow Creature_AC",
  "showUpStage": 6,
  "tasks": {
    "providers": [
      {
        "id": "task.warp_idle.shadow_creature",
        "enabled": true,
        "config": {
          "priority": "Idle",
          "providerCooldownMedian": 3
        }
      }
    ]
  }
}
~~~

重要:

1. "id"は新しいPatroller IDです。
2. "assetBundleFileName"は実体Bundleファイル名と一致させる。
3. "prefabAssetPathInsideBundle"はBundleの.manifest内"Assets:"からコピーする。
4. "animationSetId"または"animationActorId"はゲームが解決できるAnimation Setにする。
5. "tasks.providers"には有効なTask Provider IDを入れる。間違えるとFallback Idleになる可能性がある。
6. "senses.providers"は検知を制御する。間違えると何も検知しない可能性がある。

ステップ5
--------

新しいPatrollerをステージ開始時に出したい場合、Patroller Spawn Modを作ります。

以下のフォルダーを作ります。

~~~text
Mods\Mod_YourName\Patrollers\My AssetBundle Patroller Spawn\Initial Spawns\Stage 6
~~~

"Initial Spawns"の横に"mod.json"を作ります。

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "patrollerSpawns",
  "entry": "Initial Spawns",
  "overwriteExisting": true
}
~~~

以下のSpawn JSONを作ります。

~~~text
Mods\Mod_YourName\Patrollers\My AssetBundle Patroller Spawn\Initial Spawns\Stage 6\patroller_spawn.my_shadow.stage_06.001.json
~~~

例:

~~~json
{
  "id": "patroller_spawn.my_shadow.stage_06.001",
  "patrollerId": "patroller.my_shadow.stage_06",
  "enabled": true,
  "spawnRegionId": "0294cdd3-1dba-4ef8-b24a-fc7a5add47d0",
  "patrolRegionIds": [
    "4e270969-c8a4-4ade-8d50-83c82409e62d"
  ],
  "useSpawnRegionCheckpointPosition": false,
  "xOffsetFromRegionCenter": 0,
  "yOffsetFromGround": 0
}
~~~

対象ステージのRegion UUIDは"Reference_Data"から選んでください。

ステップ6
--------

ステージがそのSpawn IDを使うよう、Patroller Spawn Loadout Modを作ります。

以下のフォルダーを作ります。

~~~text
Mods\Mod_YourName\Patrollers\My AssetBundle Patroller Spawn Loadout\Initial Spawn Loadouts
~~~

"Initial Spawn Loadouts"の横に"mod.json"を作ります。

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "patrollerSpawnLoadouts",
  "entry": "Initial Spawn Loadouts",
  "overwriteExisting": true
}
~~~

ステージロードアウトJSONを作る、またはコピーします。

~~~text
Mods\Mod_YourName\Patrollers\My AssetBundle Patroller Spawn Loadout\Initial Spawn Loadouts\patroller_spawn_loadout.stage_06.json
~~~

例:

~~~json
{
  "patrollerSpawnLoadoutId": "patroller_spawn_loadout.stage_06",
  "patrollerSpawnIds": [
    "patroller_spawn.my_shadow.stage_06.001"
  ],
  "commonPatrolRegionIds": [
    "4e270969-c8a4-4ade-8d50-83c82409e62d"
  ],
  "commonPatrolRegionExpansions": []
}
~~~

既存の内蔵Spawnも残したい場合、StreamingAssetsから元ロードアウトをコピーし、リストを丸ごと置き換えるのではなく自分のSpawn IDを追加してください。

ステップ7
--------

ゲームを起動して確認します。

1. Main Menu -> Mods -> Mod Report。
2. 対象ステージへ入る。
3. Patrollerがスポーンするか確認する。
4. Animation、移動、検知、被弾、最低一つのTask実行を確認する。

トラブルシューティング
--------

1. "AssetBundle not found": BundleファイルがPatroller Data JSONの横にない、または"assetBundleFileName"が間違っている。
2. "Prefab not found in bundle": "prefabAssetPathInsideBundle"が.manifest内"Assets:"のパスと一致していない。
3. スポーンするが何もしない: Task Provider IDまたはSenseが間違っている。
4. 見えない: Animation Set、Renderer、JSON animation bindingが間違っている。
5. 攻撃が当たらない: Hit ManagerまたはCollider設定が足りない。
6. Spawn ID警告が出る: Spawn Loadoutが存在しないSpawn IDを参照している。
7. Bundleを変更したのに結果が変わらない: 再ビルドしたBundleをテストする前にゲームを再起動する。
