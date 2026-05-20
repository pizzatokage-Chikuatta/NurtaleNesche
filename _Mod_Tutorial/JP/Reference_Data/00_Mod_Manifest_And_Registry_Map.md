mod.json マニフェストとレジストリマップ
================================

このファイルは、mod.json の "type" に何を書くかを確認するための全体マップです。

実際にModを作る時は各チュートリアルを見てください。全体像だけを見たい時はこのファイルを見てください。

v1実験版ルール
--------

mod.json には必ずこれを書きます。

~~~json
{
  "experimentalApiVersion": "v1"
}
~~~

v1は実験版です。JSON、PNG、音声、アニメーション、AssetBundle、Code/DLL Mod はゲーム更新で仕様が変わる可能性があります。

mod.json の3つの形
--------

非DLL Mod の多くは、以下の3つの形のどれかを使います。

形1: フォルダー型レジストリMod
--------

1つのフォルダー内に1つ以上のJSONファイルを入れる時に使います。

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "dropTables",
  "entry": "Drop Tables",
  "overwriteExisting": true
}
~~~

各フィールド:

1. "type" は、どのゲーム内レジストリへ読み込むかを決めます。
2. "entry" は、mod.json の横にあるフォルダー名です。
3. "overwriteExisting": true は、同じ id の内蔵データをこのModで上書きできるという意味です。

ほとんどのJSON Modはこの形です。

形2: アクター指定型アニメーション/コントローラーMod
--------

Goblin のコントローラーやアニメーションセットなど、特定のアクターを対象にする時に使います。

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "animatorController",
  "target": "patroller.goblin",
  "variant": "",
  "entry": "AnimationController"
}
~~~

各フィールド:

1. "type" は "animation"、"animationClipOverride"、"animatorController" のどれかです。
2. "target" は対象アクター/コントローラーのIDです。
3. "variant" はチュートリアルで指定されない限り、基本 "" または "Base" です。
4. "entry" は、mod.json の横にあるフォルダー名です。

形3: 単体ファイル型Mod
--------

1つの単体ファイルを指定する時に使います。

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "language",
  "file": "language.json"
}
~~~

現在は主に "language" で使います。

GameServices レジストリマップ
--------

これは「mod.json の type が、ゲーム内のどのレジストリへ入るか」の対応表です。

これはMod制作用の対応表です。Code Mod がすべてのレジストリを直接使ってよいという意味ではありません。

| ゲーム内レジストリ / 項目 | mod.json の "type" |
| --- | --- |
| ActionRule | actionRules |
| AnimationMods | animation |
| AnimationClipOverrideMods | animationClipOverride |
| AnimationControllerMods | animatorController |
| AnimationSfx | animationSFX |
| AnimationTrackLayouts | animationTrackLayouts |
| BgmStageLoadouts | bgmStageLoadouts |
| BgmTracks | bgmTracks |
| ChainPoints | chainPointDefinitions |
| ChainPointSpawnLoadouts | chainPointSpawnLoadouts |
| ChainPointSpawns | chainPointSpawns |
| CombinationLoadouts | itemCombinationLoadouts |
| DropTableLoadouts | dropTableLoadouts |
| DropTables | dropTables |
| EquipmentUi | equipmentUi |
| Equipments | equipmentDefinitions |
| InteractionLoadouts | interactionLoadouts |
| InteractionOptionMeta | interactionOptionMeta |
| ItemBoxPrefabs | itemBoxPrefabs |
| ItemBoxSpawnLoadouts | itemBoxSpawnLoadouts |
| ItemBoxSpawns | itemBoxSpawns |
| ItemCombinations | itemCombinations |
| ItemNames | itemNames |
| ItemSelectionRequestDefinitions | itemSelectionRequestDefinitions |
| Items | itemDefinitions |
| Language | language |
| PatrollerDirectorStageLoadouts | patrollerDirectorStageLoadouts |
| PatrollerDirectors | patrollerDirectors |
| PatrollerSpawnLoadouts | patrollerSpawnLoadouts |
| PatrollerSpawns | patrollerSpawns |
| Patrollers | patrollerData |
| PlayerStats | playerStats |
| PotionEffects | potionEffects |
| SfxClassifications | classificationID |
| SfxClips | audioFiles |
| ShadowProfiles | shadowProfiles |
| Sprites | sprites |
| StatusEffectMetaRegistry | statusMetadata |
| StatusLoadouts | statusLoadouts |

フォルダー型の type
--------

以下は基本的に "entry" を使うフォルダー型です。

~~~text
actionRules
animationSFX
animationTrackLayouts
audioFiles
bgmStageLoadouts
bgmTracks
chainPointDefinitions
chainPointSpawnLoadouts
chainPointSpawns
classificationID
dropTableLoadouts
dropTables
equipmentDefinitions
equipmentUi
interactionLoadouts
interactionOptionMeta
itemBoxPrefabs
itemBoxSpawnLoadouts
itemBoxSpawns
itemCombinationLoadouts
itemCombinations
itemDefinitions
itemNames
itemSelectionRequestDefinitions
patrollerData
patrollerDirectorStageLoadouts
patrollerDirectors
patrollerSpawnLoadouts
patrollerSpawns
playerStats
potionEffects
shadowProfiles
sprites
statusLoadouts
statusMetadata
~~~

アクター指定型の type
--------

以下は "target"、"variant"、"entry" を使います。

~~~text
animation
animationClipOverride
animatorController
~~~

単体ファイル型の type
--------

以下は "file" を使います。

~~~text
language
~~~

重要メモ
--------

1. "type" の文字列は完全一致が必要です。大文字小文字も区別されます。
2. "entry" は mod.json があるフォルダーから見た相対フォルダー名です。
3. "file" は mod.json があるフォルダーから見た相対ファイル名です。
4. 内蔵データを上書きする時は、JSON内の "id" を変えてはいけません。
5. 新しいデータを追加する時は、自分用のユニークな id を使います。
6. Mod編集後は、ゲームのメインメニュー -> Mods -> Mod Report で読み込み結果を確認してください。
