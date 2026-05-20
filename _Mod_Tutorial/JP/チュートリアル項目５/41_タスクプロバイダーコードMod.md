タスクプロバイダーコードMod
================================

このチュートリアルでは最小構成のCode/DLL Task Providerを作ります。

例のProviderは、Patrollerに短いIdleタスクを発行します。とても単純ですが、DLL -> マニフェスト -> Patroller Dataの流れを確認するには十分です。

今回作るもの
--------

作るC#クラス:

~~~text
ExampleMods.Tasks.ExampleWaitTaskProvider
~~~

ゲーム側で使うTask Provider ID:

~~~text
task_provider.example.wait
~~~

後でPatroller DataがこのIDを参照します。

ステップ1
--------

C# IDEでC# Class Libraryプロジェクトを作成してください。

必要な成果物はDLLファイルです。IDEの種類は問いません。

プロジェクト名の例:

~~~text
ExampleTaskMod
~~~

Unityゲームプロジェクトとして作る必要はありません。通常のC# Class Libraryで十分です。

ステップ2
--------

対象ゲームと同じバージョンのDLLを参照に追加してください。

このチュートリアルでは以下を使います。

1. NurtaleNesche.Modding.Abstractions.dll
2. NurtaleNesche.Runtime.dll
3. UnityEngine.CoreModule.dll
4. Newtonsoft.Json.dll
5. コンパイラに要求された場合はnetstandard.dll

ビルド済みゲーム内での場所:

~~~text
Nurtale Nesche_Data\Managed
~~~

別バージョンのゲームDLLを使わないでください。

ステップ3
--------

プロジェクト内に以下のC#ファイルを作ります。

~~~text
ExampleWaitTaskProvider.cs
~~~

中身:

~~~csharp
using Newtonsoft.Json.Linq;
using NurtaleNesche.Modding.Patrollers.Tasks;

namespace ExampleMods.Tasks
{
    [TaskProviderId("task_provider.example.wait")]
    public sealed class ExampleWaitTaskProvider : TaskSetCore
    {
        float seconds = 1.5f;

        public override void ApplyConfig(JObject cfg)
        {
            seconds = cfg?["seconds"]?.Value<float?>() ?? seconds;
        }

        public override Patroller.PatrollerTaskInfoContainer GetNewTask()
        {
            if (!IsConditionMetForPriority(TaskPriorities.MiscTaskLow))
                return null;

            if (controller == null || controller.baseIdleState == null)
                return null;

            return new TaskSequenceBuilder()
                .Then(controller.baseIdleState, new global::StateArgs_Idle(seconds))
                .Build(this, TaskPriorities.MiscTaskLow, "ExampleWait");
        }
    }
}
~~~

各部分の意味:

1. `TaskProviderId`はゲームへ登録するIDを宣言する。
2. このIDは`task_provider.example.wait`。
3. `TaskSetCore`はv1 Code Modで使うPlain C#のTask Provider基底クラス。
4. `seconds`は今回の例で使うカスタム値。
5. `ApplyConfig`はPatroller DataのJSON config適用時に呼ばれる。
6. `GetNewTask`はスケジューラーがこのProviderに挙動提案を求める時に呼ばれる。
7. `return null`は「今はタスクを提案しない」という意味。エラーではない。
8. `IsConditionMetForPriority`は強いタスクを邪魔しないためのチェック。
9. `controller`はこのProviderを持つPatroller。
10. `controller.baseIdleState`は内蔵Idle State。
11. `StateArgs_Idle(seconds)`はIdle Stateへ待機時間を渡す。
12. `TaskSequenceBuilder`は現在のランタイムが必要とするTaskContainerを作る。

なぜglobal::StateArgs_Idleなのか
--------

`StateArgs_Idle`は現在ゲーム本体側のグローバル名前空間にあります。

`global::`を付けると、自分のnamespace内から探そうとして失敗する事故を避けられます。必須ではない場合もありますが、チュートリアルでは混乱防止のため付けています。

ステップ4
--------

プロジェクトをビルドします。

出力結果の例:

~~~text
ExampleTaskMod.dll
~~~

ゲームはMods内のExampleWaitTaskProvider.csをコンパイルしません。読み込まれるのはコンパイル済みDLLだけです。

ステップ5
--------

以下のフォルダーを作ります。

~~~text
Mods\Mod_YourName\Code\ExampleTaskMod
~~~

DLLをここに置きます。

~~~text
Mods\Mod_YourName\Code\ExampleTaskMod\ExampleTaskMod.dll
~~~

ステップ6
--------

DLLの横に以下のマニフェストファイルを作ります。

~~~text
example.task_providers.json
~~~

中身:

~~~json
{
  "experimentalApiVersion": "v1",
  "providers": [
    {
      "id": "task_provider.example.wait",
      "type": "ExampleMods.Tasks.ExampleWaitTaskProvider"
    }
  ]
}
~~~

フィールドの意味:

1. `experimentalApiVersion`は`"v1"`にする。
2. `providers`はこのマニフェストで登録するTask Provider一覧。
3. `id`はPatroller Dataから参照するTask Provider ID。
4. `type`はnamespaceとclass名を含む完全なC#型名。
5. マニフェストは挙動を作らない。挙動を作るのはDLL内のC#型。

AttributeとManifest
--------

このチュートリアルでは両方使っています。

1. `[TaskProviderId("task_provider.example.wait")]`
2. `example.task_providers.json`

これは安全ですが、少し重複しています。

AttributeだけでもDLLスキャン時に登録できます。Manifestだけでも読み込まれたDLL内のtypeを指定して登録できます。初心者向けには、コード側にもディスク上にもIDが見えるため両方書いています。上級者は片方に寄せても構いません。

ステップ7
--------

Patroller Data Modを作り、対象PatrollerのProvider一覧にこのProvider IDを追加します。

Provider entryの例:

~~~json
{
  "id": "task_provider.example.wait",
  "priority": "MiscTaskLow",
  "providerCooldownMedian": 1,
  "config": {
    "seconds": 1.5
  }
}
~~~

重要:

1. `id`はマニフェストIDまたはTaskProviderIdと一致していないといけない。
2. `priority`は現在動いている他タスクと比べた強さを決める。
3. `providerCooldownMedian`は提案が採用された後、再提案までの待ち時間。
4. `config.seconds`は`ApplyConfig`へ渡される。
5. Patroller Dataには安全なfallback providerを残すこと。
6. 最初のCode Modテストで既存挙動を全部消さないこと。

実際の配置場所はコピーしたPatroller Data JSONによって変わります。対象Patroller JSON内の`tasks.providers`一覧を探し、そこに追加してください。

Priorityの注意
--------

この例では以下を使います。

~~~text
MiscTaskLow
~~~

待機するだけのProviderなので、Chase、Attack、TakeDamage、Event、Dieなどを邪魔するべきではありません。ActionScheduler2の挙動を理解するまでは高いPriorityを使わないでください。

Provider Cooldownの注意
--------

`providerCooldownMedian`はIdle時間とは別物です。

`seconds`は発行されたIdleタスクの長さです。`providerCooldownMedian`は、このProviderが採用後に次の提案まで待つ時間です。

ステップ8
--------

ゲームを起動または再起動します。

以下を開きます。

~~~text
Main Menu -> Mods -> Mod Report
~~~

マニフェスト警告やtype解決警告がないか確認します。

ステップ9
--------

ゲーム内でテストします。

Patrollerが普通に動き、type警告が出ていなければDLLは読み込まれ、Providerも登録されています。Providerが動作しているか明確に見たい場合は、他の強いProviderと競合しないPatroller Dataで一時的にテストしてください。

一時デバッグ例:

~~~csharp
UnityEngine.Debug.Log("ExampleWaitTaskProvider proposed a wait task.");
~~~

これはテスト中だけ使ってください。Mod配布前にはうるさいログを消すのを推奨します。

再起動ルール
--------

DLLを変更したらゲームを再起動してください。

ゲームは読み込んだAssemblyをフルパスでキャッシュします。メインメニューのMod再インポートは、同じパスの変更済みDLLを再読み込みする用途として信用しないでください。

よくあるミス
--------

1. Mods内に.csファイルを置けばゲームがコンパイルしてくれると思っている。
2. マニフェストファイルを置き忘れる。
3. マニフェスト名が違う。`.task_providers.json`で終わる必要がある。
4. type名が違う。typeにはnamespace込みの完全な型名が必要。
5. 違うゲームバージョンのDLLを参照してビルドしている。
6. Patroller Dataからfallback task providerを消してしまう。
7. 再インポートボタンでDLLホットリロードできると思っている。
8. Patroller Dataにconfigを書いたのに`ApplyConfig`で読んでいない。
9. 高すぎるpriorityで重要なゲームプレイを割り込んでしまう。

最終フォルダー形
--------

~~~text
Mods
|-- Mod_YourName
    |-- Code
        |-- ExampleTaskMod
            |-- ExampleTaskMod.dll
            |-- example.task_providers.json
~~~

次に読むもの
--------

TaskSet、TaskOption、EventTaskSet、内蔵サンプルを理解したい場合は43_内蔵タスクシステム構造.mdを読んでください。