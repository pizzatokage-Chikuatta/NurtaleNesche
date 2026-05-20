Code Modマニフェストリファレンス
================================

このファイルは現在のCode/DLL Mod登録口をまとめたものです。

まずは41_タスクプロバイダーコードMod.mdを終わらせてから読んでください。Task Provider、Task Set、Task Option、Event Task Optionの違いが分からない場合は43_内蔵タスクシステム構造.mdも読んでください。

共通ルール
--------

1. DLL内にC#型がすでに存在している必要がある。
2. マニフェストはIDとそのC#型を結びつける。
3. マニフェストは挙動そのものを定義しない。
4. `type`にはnamespace込みの完全なC#型名を書く。
5. Code Modは実験的機能である。
6. DLL変更後はゲームを再起動する。
7. 型を登録するだけでは半分。別のJSON/データ側からそのIDを参照する必要がある。

登録したIDをどこで使うか
--------

1. Task Provider IDはPatroller Dataの`tasks.providers`で使う。
2. Arrow/Potion Event Task Option IDはEvent Task Setの`options`で使う。
3. Captive Treating Task Option IDはhandling-captives task setの`options`で使う。
4. Sense IDはPatroller Dataの`senses.providers`で使う。
5. Status Effect Factory IDはそのStatus Effect IDが有効化される時に使われる。通常は対応するStatus Metadata JSONも必要。
6. Item Selection Request Factory IDはアイテム操作データ側からそのrequest typeが要求される時に使われる。

Task Provider
--------

マニフェストファイル名:

~~~text
*.task_providers.json
~~~

ルート配列:

~~~text
providers
~~~

例:

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

型の条件:

1. `TaskSetCore`または`TaskProviderCore`を継承する。
2. `TaskProviderIdAttribute`またはマニフェストで登録する。
3. `GetNewTask`を実装する。
4. Plain C# Task Providerにはpublicな引数なしコンストラクタが必要。

実行時の挙動:

スケジューラーがProviderにタスクを要求します。Providerが`null`を返すと今は何もしません。`PatrollerTaskInfoContainer`を返すと、Priorityと現在状態に応じて採用されます。

Event Task Option
--------

Event Task OptionはPatrollerが矢やポーションに当たった時の処理を変えます。

マニフェストファイル名:

~~~text
*.event_task_options.json
~~~

ルート配列:

1. arrowOptions
2. potionOptions

例:

~~~json
{
  "experimentalApiVersion": "v1",
  "arrowOptions": [
    {
      "id": "event_task_option.arrow_hit.example_ignore",
      "type": "ExampleMods.Events.ExampleArrowIgnore",
      "overrideExisting": false
    }
  ],
  "potionOptions": [
    {
      "id": "event_task_option.potion_hit.example_ignore",
      "type": "ExampleMods.Events.ExamplePotionIgnore",
      "overrideExisting": false
    }
  ]
}
~~~

最小Arrow skeleton:

~~~csharp
using NurtaleNesche.Modding.Patrollers.Tasks;

namespace ExampleMods.Events
{
    [ArrowHitEventTaskOptionId("event_task_option.arrow_hit.example_ignore")]
    public sealed class ExampleArrowIgnore : IArrowHitEventTaskOption
    {
        public int Priority => 0;
        public string OptionId => "event_task_option.arrow_hit.example_ignore";

        public bool CanHandle(Patroller.ArrowHitEventContext context)
        {
            return false;
        }

        public Patroller.ArrowHitEventResult Handle(Patroller.ArrowHitEventContext context)
        {
            return Patroller.ArrowHitEventResult.NotHandled;
        }
    }
}
~~~

最小Potion skeleton:

~~~csharp
using NurtaleNesche.Modding.Patrollers.Tasks;

namespace ExampleMods.Events
{
    [PotionHitEventTaskOptionId("event_task_option.potion_hit.example_ignore")]
    public sealed class ExamplePotionIgnore : IPotionHitEventTaskOption
    {
        public int Priority => 0;
        public string OptionId => "event_task_option.potion_hit.example_ignore";

        public bool CanHandle(Patroller.PotionHitEventContext context)
        {
            return false;
        }

        public Patroller.PotionHitEventResult Handle(Patroller.PotionHitEventContext context)
        {
            return Patroller.PotionHitEventResult.NotHandled;
        }
    }
}
~~~

重要:

このskeletonは形だけで、何もしません。`CanHandle = true`を返すのは本当にそのイベントを処理すべき時だけにしてください。handledな結果を返すと後続の通常ヒット処理を止める可能性があります。

Captive Treating Task Option
--------

Captive Treating Task Optionはhandling-captives task setが使う子オプションです。

マニフェストファイル名:

~~~text
*.captive_treating_task_options.json
~~~

ルート配列:

~~~text
taskOptions
~~~

古い互換用ファイル名/配列:

~~~text
*.captive_treating_action_sets.json
actionSets
~~~

古い名前は互換用に読めますが、新規Modでは使わないでください。

例:

~~~json
{
  "experimentalApiVersion": "v1",
  "taskOptions": [
    {
      "id": "task_option.example.captive_treating",
      "type": "ExampleMods.Captives.ExampleCaptiveTreatingOption",
      "overrideExisting": false
    }
  ]
}
~~~

最小skeleton:

~~~csharp
using NurtaleNesche.Modding.Patrollers.Tasks;

namespace ExampleMods.Captives
{
    [CaptiveTreatingTaskOptionId("task_option.example.captive_treating")]
    public sealed class ExampleCaptiveTreatingOption : ICaptiveTreatingTaskOption
    {
        public Patroller.PatrollerTaskForTreatingCaptive GetCaptiveTreatmentTask(
            Patroller.BaseController controller,
            Patroller.PatrollerMemory memory)
        {
            return null;
        }
    }
}
~~~

重要:

`null`を返すと「今は捕虜処理をしない」という意味です。本物のOptionでは有効なCaptive Treatment Taskを返す必要があります。この領域は捕虜移動、拘束付与、H開始、牢屋送り、Chainpoint所有者変更などに関わるため上級者向けです。

Sense
--------

Sense ModはPatrollerの検知用コンポーネントを追加または差し替えます。

マニフェストファイル名:

~~~text
*.senses.json
~~~

ルート配列:

~~~text
senses
~~~

例:

~~~json
{
  "experimentalApiVersion": "v1",
  "senses": [
    {
      "id": "sense.example",
      "type": "ExampleMods.Senses.ExampleSense",
      "overrideExisting": false
    }
  ]
}
~~~

最小skeleton:

~~~csharp
using NurtaleNesche.Modding.Patrollers.Senses;

namespace ExampleMods.Senses
{
    public sealed class ExampleSense : Sense
    {
    }
}
~~~

重要:

Sense型はUnity Component系の型です。Sense IDを登録するだけでは足りません。Patroller Dataの`senses.providers`からそのIDを参照しないと、ランタイムは使いません。

Sense Modは検知、記憶、疑念、パフォーマンスに影響します。最初のCode Modとしてはおすすめしません。

Status Effect Factory
--------

Status Effect Factory ModはStatus Effect IDとカスタムStatus Effectクラスを結びつけます。

マニフェストファイル名:

~~~text
*.status_effect_factories.json
~~~

ルート配列:

~~~text
factories
~~~

例:

~~~json
{
  "experimentalApiVersion": "v1",
  "factories": [
    {
      "id": "status_effect.example",
      "profileId": "",
      "type": "ExampleMods.StatusEffects.ExampleStatusEffect"
    }
  ]
}
~~~

最小skeleton:

~~~csharp
using NurtaleNesche.Modding.StatusEffects;

namespace ExampleMods.StatusEffects
{
    public sealed class ExampleStatusEffect : StatusEffect
    {
        public override void OnEntry()
        {
            base.OnEntry();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
~~~

重要:

1. 型は`IStatusEffect`を実装する必要がある。
2. 型にはpublicな引数なしコンストラクタが必要。
3. 通常は同じIDのStatus Metadata JSONも必要。ゲームはそこからefficacy、見た目、スタック、表示ルールなどを読む。
4. 現在のStatus Factory manifest entryは`overrideExisting`をランタイム側で扱わない。`id`、任意の`profileId`、`type`を使う。
5. 必ず付与、解除、保存、ロード、シーン移動をテストすること。

Item Selection Request Factory
--------

Item Selection Request Factoryはアイテム操作メニュー用のrequestを作ります。

マニフェストファイル名:

~~~text
*.item_selection_request_factories.json
~~~

ルート配列:

~~~text
factories
~~~

例:

~~~json
{
  "experimentalApiVersion": "v1",
  "factories": [
    {
      "id": "item_selection_request.example",
      "type": "ExampleMods.Items.ExampleItemSelectionRequestFactory"
    }
  ]
}
~~~

最小skeleton:

~~~csharp
using NurtaleNesche.Modding.Items;
using PlayerGroup;
using UnityEngine;

namespace ExampleMods.Items
{
    public sealed class ExampleItemSelectionRequestFactory : IItemSelectionRequestFactory
    {
        public bool TryCreate(
            string requestTypeId,
            Player player,
            GameObject itemObject,
            out IItemSelectionRequest request)
        {
            request = null;
            return false;
        }
    }
}
~~~

重要:

`false`を返すと「このFactoryはrequestを作らない」という意味です。本物のFactoryでは`IItemSelectionRequest`を作り、`true`を返す必要があります。

登録方法
--------

多くのCode Modは以下のいずれかで登録できます。

1. C# classにつけるAttribute。
2. IDとtypeを対応させるマニフェスト。
3. Registrar class。

初心者は、使える場所ではAttribute + Manifestを優先してください。Registrarはより上級者向けです。

トラブルシューティング
--------

1. Type not found: マニフェストのtypeがDLL内のnamespace/class名と一致していない、またはDLLが読み込まれていない。
2. DLLはあるが何も登録されない: マニフェストがない、ファイル名が違う、Attribute/Registrarがない。
3. 古いビルドでは動いたが新ビルドで動かない: 新しいゲームDLLを参照して再ビルドする。
4. DLLを変更したのに結果が変わらない: ゲーム再起動、または新しいDLL名/パスを使う。
5. Validatorは通るが挙動がおかしい: Validatorはマニフェスト形状を確認するだけで、ゲームプレイの正しさは保証しない。
6. 登録IDは存在するのに何も起きない: 対応するPatroller Data、Status Metadata、Item Interaction Data、TaskSet options側がそのIDを参照していない可能性が高い。