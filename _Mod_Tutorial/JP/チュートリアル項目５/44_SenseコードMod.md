SenseコードMod
================================

このチュートリアルでは、実際に動くCode/DLL Sense Modを作ります。

Senseはパトローラーの検知コンポーネントです。Task Providerは「何をするか」を決めますが、Senseは「何を見つけられるか」を決めます。例えばプレイヤー、捕虜、アイテム、チェインポイント、疑念地点、音、他のパトローラーなどです。

先に以下を読んでください。

1. `40_コードMod概要.md`
2. `41_タスクプロバイダーコードMod.md`
3. `43_内蔵タスクシステム構造.md`

Sense Modは単純なTask Providerより壊れやすいです。間違えると敵が部屋越しに検知したり、存在しない対象を追い続けたり、逆に何も検知しなくなります。

作るもの
--------

以下のC#クラスを作ります。

~~~text
ExampleMods.Senses.ExampleNearbyPlayerSense
~~~

ゲーム側で使うSense ID:

~~~text
sense.example.nearby_player
~~~

Patroller Dataでは以下のように`senses.providers`から参照します。

~~~json
"senses": {
  "providers": [
    {
      "id": "sense.example.nearby_player",
      "enabled": true,
      "config": {
        "distance": 4.5,
        "priority": 2
      }
    }
  ]
}
~~~

重要: Sense IDを登録するだけでは動きません。Patroller Dataの`senses.providers`にもそのIDを書く必要があります。

Sense読み込みの流れ
--------

ランタイムでは以下の順番で動きます。

1. ゲームが`Mods`内のDLLを読み込む。
2. ゲームが`*.senses.json`を読む。
3. ManifestがSense IDとDLL内のC#型を結びつける。
4. `SenseFactoryRegistry`がIDと型を保存する。
5. パトローラー初期化時、Patroller Dataの`senses.providers`が使用するSenseを決める。
6. ランタイムがパトローラーのSenseオブジェクトへSenseコンポーネントを追加する。
7. コンポーネントが`IJsonConfigurable`を実装していれば、`config`が`ApplyConfig`へ渡される。
8. `SenseManager`が`ICaptiveDetector`や`IItemDetector`などのDetector Interfaceを探す。
9. ゲーム中、`SenseManager`が`UpdateList`を呼ぶ。
10. Senseが`PatrollerMemory`へ検知情報を書く。

ステップ1
--------

C# Class Libraryプロジェクトを用意します。

一番簡単な方法:

1. `Developer Reference Packs/code_mod_reference_project`を開く。
2. `ExampleCodeMod.csproj`をVisual Studio、Visual Studio Code、またはC# IDEで開く。
3. 既存の`src\Senses\ExampleNearbyPlayerSense.cs`を出発点にする。
4. プロジェクトをビルドする。
5. 出力されたDLLとSense Manifestを自分のModフォルダーへコピーする。

このリファレンスプロジェクトを使わない場合は、通常のC# Class Libraryプロジェクトを作ってください。このチュートリアルではUnityプロジェクトを新規作成する必要はありません。必要なのは`.dll`ファイルです。

必要な参照:

1. `NurtaleNesche.Modding.Abstractions.dll`
2. `NurtaleNesche.Runtime.dll`
3. `UnityEngine.CoreModule.dll`
4. `Newtonsoft.Json.dll`
5. コンパイラに要求された場合は`netstandard.dll`

対象ゲームと違うバージョンのDLLを参照しないでください。

ビルド済みゲーム内では以下にあります。

~~~text
Nurtale Nesche_Data\Managed
~~~

Public SDK packageを使っている場合は、以下に入っている場合もあります。

~~~text
dlls
~~~

ステップ2
--------

以下のC#ファイルを作ります。

~~~text
ExampleNearbyPlayerSense.cs
~~~

全文:

~~~csharp
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NurtaleNesche.Modding.Patrollers.Senses;
using Patroller;
using UnityEngine;

namespace ExampleMods.Senses
{
    public sealed class ExampleNearbyPlayerSense : Sense, ICaptiveDetector, global::IJsonConfigurable
    {
        const string SourceId = "sense.example.nearby_player";

        readonly List<CaptiveInfoOnOthers> cache = new();
        BaseController controller;
        PatrollerMemory memory;
        float distance = 4.5f;
        int priority = 2;

        public void ApplyConfig(JObject cfg)
        {
            if (cfg == null) return;

            distance = cfg["distance"]?.Value<float>() ?? distance;
            priority = cfg["priority"]?.Value<int>() ?? priority;
            distance = Mathf.Max(0.1f, distance);
        }

        public override bool Initialize()
        {
            if (!base.Initialize()) return false;

            controller = GetComponentInParent<BaseController>(true);
            memory = GetComponentInParent<PatrollerMemory>(true);
            return true;
        }

        public override bool UpdateList()
        {
            if (!base.UpdateList()) return false;

            memory?.StageForgetCaptivesBySource(SourceId);

            var region = controller?.Commons?.CurrentRegion;
            if (region?.CaptivesStaying == null)
                return true;

            foreach (var captive in region.CaptivesStaying)
            {
                TryObserve(captive);
            }

            return true;
        }

        void TryObserve(ICaptive captive)
        {
            if (controller == null || memory == null) return;
            if (captive == null) return;
            if (!captive.IsPlayer()) return;
            if (captive.GetTransform() == null) return;
            if (captive.GetCheckpoint() == null) return;
            if (captive.GetCheckpoint().Commons.CurrentRegion != controller.Commons.CurrentRegion) return;
            if (Vector2.Distance(controller.transform.position, captive.GetTransform().position) > distance) return;

            memory.StageCaptiveObservation(SourceId, captive, priority);
        }

        public void Sweep()
        {
            memory?.StageForgetCaptivesBySource(SourceId);
        }

        public List<CaptiveInfoOnOthers> GetCaptivesDetected()
        {
            if (memory == null)
                cache.Clear();
            else
                memory.CopyCaptiveObservationsTo(SourceId, cache, clone: false);

            return cache;
        }
    }
}
~~~

各部分の意味:

1. `Sense`はUnity Component型のSenseにするための基底クラスです。
2. `ICaptiveDetector`は「捕虜を検知するSense」であることを`SenseManager`へ知らせます。
3. `IJsonConfigurable`により、Patroller Dataの`config`から`distance`と`priority`を受け取れます。
4. `SourceId`は`PatrollerMemory`内でこのSenseの検知情報を区別するためのIDです。
5. `Initialize`ではシーン上にコンポーネントが追加された後に必要な参照を取得します。
6. `UpdateList`は`SenseManager`から呼ばれます。
7. `base.UpdateList()`は同じフレームで二重更新しないためのガードです。
8. `StageForgetCaptivesBySource`は古い検知情報を消します。
9. `TryObserve`はプレイヤー限定、同じRegion、Checkpoint有効、距離内かを確認します。
10. `StageCaptiveObservation`は検知結果を`PatrollerMemory`へ書きます。
11. `Sweep`はこのSenseの検知情報をリセットします。
12. `GetCaptivesDetected`は`SenseManager`互換のため、現在の検知結果を返します。

PatrollerMemoryの簡単な説明
--------

`PatrollerMemory`はパトローラーの検知記憶です。

Code Modでは`NurtaleNesche.Runtime.dll`から使用できます。

以下のnamespaceを使います。

~~~csharp
using Patroller;
~~~

このnamespaceから、Sense Modでよく使う以下の型へアクセスできます。

1. `PatrollerMemory`
2. `BaseController`
3. `CaptiveInfoOnOthers`

Senseは基本的に`PatrollerMemory`を以下の流れで使います。

1. このSenseが前回書いた古い検知情報を消す。
2. 現在有効な対象を探す。
3. 固有の`SourceId`で新しい検知情報を書く。
4. `SenseManager`に求められた時、現在の検知情報を返す。

捕虜検知でよく使う呼び出し:

~~~csharp
memory.StageForgetCaptivesBySource(SourceId);
memory.StageCaptiveObservation(SourceId, captive, priority);
memory.CopyCaptiveObservationsTo(SourceId, cache, clone: false);
~~~

同じSource単位の考え方は、他の検知カテゴリにもあります。

1. `StageSuspicionCheckpointObservation` / `StageForgetSuspicionCheckpointsBySource`
2. `StageNearChainpointObservation` / `StageForgetNearChainpointsBySource`
3. `StageNearItemObservation` / `StageForgetNearItemsBySource`
4. `StageNearPatrollerObservation` / `StageForgetNearPatrollersBySource`

重要: 古い検知情報を残し続けないでください。古い情報を消さないSenseは、部屋から出た対象、消えた対象、破壊された対象を追い続ける原因になります。

参照ファイル:

~~~text
Developer Reference Packs\code_mod_reference_project\reference\PatrollerMemory_Public_Surface.cs.txt
~~~

Sense Modでよく使う`PatrollerMemory`の公開メソッドはこのファイルで確認できます。

ステップ3
--------

プロジェクトをビルドします。

出力されるDLL例:

~~~text
ExampleCodeMod.dll
~~~

ゲームはDLLを読み込みます。`Mods`内の`.cs`ファイルはコンパイルされません。

ステップ4
--------

DLLを`Mods`内に置きます。

例:

~~~text
Mods\Mod_YourName\Code\ExampleCodeMod\ExampleCodeMod.dll
~~~

ステップ5
--------

DLLの横に以下のManifestを作ります。

~~~text
example.senses.json
~~~

中身:

~~~json
{
  "experimentalApiVersion": "v1",
  "senses": [
    {
      "id": "sense.example.nearby_player",
      "type": "ExampleMods.Senses.ExampleNearbyPlayerSense",
      "overrideExisting": false
    }
  ]
}
~~~

各フィールドの意味:

1. `"experimentalApiVersion": "v1"`はv1実験的Modファイルであることを示します。
2. `"senses"`は`SenseModLoader`が読むリストです。
3. `"id"`はPatroller Dataから参照するSense IDです。
4. `"type"`はC#のnamespaceとclass名に完全一致する必要があります。
5. `"overrideExisting": false`は内蔵Sense IDを上書きしないという意味です。

ステップ6
--------

Patroller Dataを編集し、パトローラーがこのSenseを使うようにします。

最初のテストでは、Stage 1 OrcのPatroller Dataを上書きします。

以下のフォルダーを作ります。

~~~text
Mods\Mod_YourName\Patrollers\Mod Patroller Data\Patroller Data\Stage 1
~~~

以下のテンプレートをコピーします。

~~~text
Mods\mod.json templates\mod.template_folder_backed.json
~~~

以下に貼り付け、`mod.json`へリネームします。

~~~text
Mods\Mod_YourName\Patrollers\Mod Patroller Data\mod.json
~~~

`mod.json`の中身を以下にします。

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "patrollerData",
  "entry": "Patroller Data",
  "overwriteExisting": true
}
~~~

以下の内蔵Patroller Dataをコピーします。

~~~text
StreamingAssets\Patrollers\Data\Stage 1\patroller.orc.stage_01.json
~~~

以下へ貼り付けます。

~~~text
Mods\Mod_YourName\Patrollers\Mod Patroller Data\Patroller Data\Stage 1\patroller.orc.stage_01.json
~~~

コピーした`patroller.orc.stage_01.json`を開き、以下を探します。

~~~json
"senses": {
  "providers": [
~~~

既存の`providers`リスト内に以下を追加してください。

~~~json
{
  "id": "sense.example.nearby_player",
  "enabled": true,
  "config": {
    "distance": 4.5,
    "priority": 2
  }
}
~~~

最初のテストでは、他の内蔵Senseを削除しないでください。`senses.providers`全体を自作Senseだけにすると、アイテム検知、疑念検知、チェインポイント検知、通常の視覚/嗅覚検知などが消える可能性があります。

最終的なフォルダー形:

~~~text
Mods
  Mod_YourName
    Code
      ExampleCodeMod
        ExampleCodeMod.dll
        example.senses.json
    Patrollers
      Mod Patroller Data
        mod.json
        Patroller Data
          Stage 1
            patroller.orc.stage_01.json
~~~

ステップ7
--------

テストします。

1. DLLを変更した後はゲームを再起動する。
2. Main Menu -> Mods -> Mod Reportを開く。
3. DLLと`*.senses.json`が読み込まれているか確認する。
4. 編集したパトローラーがいるステージに入る。
5. `Unknown senseId`のような警告が出ていないか確認する。

Main MenuのReimportはDLL Hot Reloadとしては信用しないでください。DLL変更後はゲーム再起動が基本です。

内蔵例1: ItemDetector
--------

内蔵ID:

~~~text
sense.item_detector
~~~

内蔵ソース:

~~~text
Assets/Scripts/Patroller/Base/Sense/ItemDetector.cs
~~~

Patroller Data例:

~~~json
{
  "id": "sense.item_detector",
  "enabled": true,
  "config": {
    "sightDelayTime": 1,
    "detectDistance": 7
  }
}
~~~

この例から分かること:

1. `Sense`を継承しています。
2. `IItemDetector`を実装しています。
3. `SenseManager`はこれをアイテム検知Senseとして見つけます。
4. `IJsonConfigurable`を実装しています。
5. `sightDelayTime`と`detectDistance`はJSONの`config`から来ます。
6. シーン全体ではなく`controller.Commons.CurrentRegion.ItemsStaying`を見ています。
7. 検知したアイテムを`PatrollerMemory`へ書きます。
8. null、inactive、doomed、Region外のアイテムをキャッシュから消しています。

重要: Detectorは古い検知情報を掃除する必要があります。破壊済みObjectを覚え続けると、敵が存在しないものへ反応する可能性があります。

内蔵例2: AdjacentCaptiveDetector
--------

内蔵ID:

~~~text
sense.captive_detector_adjacent
~~~

内蔵ソース:

~~~text
Assets/Scripts/Patroller/Base/Sense/AdjacentCaptiveDetector.cs
~~~

Patroller Data例:

~~~json
{
  "id": "sense.captive_detector_adjacent",
  "enabled": true,
  "config": {
    "priority": 3,
    "playerOnly": true,
    "distance": 5
  }
}
~~~

この例から分かること:

1. `ICaptiveDetector`を実装しています。
2. 現在Region内の捕虜だけを見ています。
3. 同じRegionでない捕虜を拒否しています。
4. 距離チェックをしてから検知情報を追加しています。
5. `priority`により、より強い検知情報が勝てます。
6. 現在の検知情報を作り直す前に、自分のSourceの古い情報を消しています。

重要: Regionチェックは必須です。無視すると壁越し/部屋越し検知が起きます。

よくあるミス
--------

1. DLLを変更したのにゲームを再起動していない。
2. Manifestファイル名が`*.senses.json`になっていない。
3. Manifestの`type`がC#のnamespace/class名と一致していない。
4. Sense IDは登録したが、Patroller Dataの`senses.providers`へ追加していない。
5. Patroller Data上書き時に必要な内蔵Senseを消してしまった。
6. `UpdateList`で毎フレームシーン全体を探して重くしている。
7. 破壊済みUnity Objectを保持して古い検知が残っている。
8. Regionチェックをしていないため、壁越し検知が起きる。

Sense Modを作るべき時
--------

パトローラーの「検知方法」を増やしたい時にSense Modを作ります。

向いている例:

1. 新しい種類のObjectを検知させたい。
2. 特殊な距離/Region条件で検知させたい。
3. 自作Gameplay Objectへ反応させたい。

向いていない例:

1. 新しい行動をさせたいだけ。Task Providerを使ってください。
2. HPや移動速度を変えたいだけ。Patroller Dataを使ってください。
3. スプライトを変えたいだけ。Animation/Sprite系Modを使ってください。
