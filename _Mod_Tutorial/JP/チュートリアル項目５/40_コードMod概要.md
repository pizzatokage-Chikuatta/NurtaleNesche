Code/DLL Mod概要
================================

この項目はCode/DLL Modの説明です。

JSON、PNG、音声、アニメーション、Prefab系のデータModだけでは足りない場合に使ってください。Code Modは強力ですが、ゲームプレイを壊しやすい上級機能です。

Code Modとは
--------

Code Modはコンパイル済みC# DLLです。

ゲームはModsフォルダー内の生の.csファイルをコンパイルしません。Mod制作者は通常のC#プロジェクトでコードを書き、.dllへビルドし、そのDLLをMods配下へ置きます。

ゲームはMods配下のDLLを探し、その後以下のようなコードMod用マニフェストを読みます。

1. *.task_providers.json
2. *.event_task_options.json
3. *.captive_treating_task_options.json
4. *.senses.json
5. *.status_effect_factories.json
6. *.item_selection_request_factories.json

マニフェストはIDとDLL内のC#型を結びつけるだけです。マニフェストだけでは挙動は作られません。

デスクトップ実行環境の注意
--------

Code/DLL Modはデスクトップ版向けの機能です。

現在のゲームはローカルファイルから管理DLLを読み込めるデスクトップ実行環境を前提にしています。IL2CPP専用ビルド、コンソール、制限された環境で動くとは考えないでください。

セキュリティ注意
--------

Code Modは信頼済みのローカルコードとして実行されます。

サンドボックス化されません。DLLはゲームプロセスと同じ権限で任意のC#コードを実行できます。信頼できる配布元のCode/DLL Modだけを使ってください。

バージョン注意
--------

Code Modは対象ゲームと同じバージョンのDLLを参照してビルドしてください。

ゲームが更新された場合、Code Modも新しいゲームDLLを参照して再ビルドする必要があります。ロード自体はできても、State、StateArgs、TaskContainer、StatusEffect、Registryなどのランタイム寄りAPIはv1実験的APIなので変更される可能性があります。

必要なDLL参照
--------

典型的なv1 Task Provider Code Modでは以下を参照します。

1. NurtaleNesche.Modding.Abstractions.dll
2. NurtaleNesche.Runtime.dll
3. UnityEngine.CoreModule.dll
4. JObjectやJSONを使うならNewtonsoft.Json.dll
5. コンパイラに要求された場合はnetstandard.dll

ビルド済みゲーム内での場所:

~~~text
Nurtale Nesche_Data\Managed\NurtaleNesche.Modding.Abstractions.dll
Nurtale Nesche_Data\Managed\NurtaleNesche.Runtime.dll
Nurtale Nesche_Data\Managed\UnityEngine.CoreModule.dll
Nurtale Nesche_Data\Managed\Newtonsoft.Json.dll
~~~

公開SDK zipに含めている場合はそちらを使っても構いません。重要なのは、対象ゲームと同じバージョンのDLLを使うことです。

なぜNurtaleNesche.Runtime.dllも必要なのか
--------

NurtaleNesche.Modding.Abstractions.dllにはMod制作者向けの名前、属性、ヘルパーが入っています。

しかしv1のTask Provider Modはまだ以下のようなゲーム本体側の型を使います。

1. Patroller.PatrollerTaskInfoContainer
2. State
3. StateArgsとStateArgs_*系クラス
4. BaseControllerとcontrollerが持つ内蔵State
5. Task RegistryやEvent Context

これらはNurtaleNesche.Runtime.dll側にあります。なのでTask Provider Code Modは基本的に両方のDLLが必要です。

メインメニューの再インポートはDLLホットリロードではない
--------

メインメニューのMod再インポートはJSON、PNG、音声、データ再読み込み用です。

DLLのホットリロードとして信用しないでください。同じパスからすでに読み込まれたDLLは、ゲームが読み込み済みAssemblyを使い続ける場合があります。安全なテスト手順は以下です。

1. DLLをビルドする。
2. Modsへコピーする。
3. ゲームを起動、または再起動する。
4. テストする。
5. DLLを変更したらまたゲームを再起動する。

上級者向けのテスト回避策:

~~~text
ExampleTaskMod_v2.dll
~~~

DLL名やパスを変えると別Assemblyとして読める場合がありますが、基本はゲーム再起動が安全です。

現在追加できるCode Mod
--------

1. Task Provider。
2. Arrow Hit / Potion Hit Event Task Option。
3. Captive Treating Task Option。
4. Sense。
5. Status Effect Factory。
6. Item Selection Request Factory。

おすすめの読む順番
--------

1. まず41_タスクプロバイダーコードMod.mdを読む。
2. 内蔵のTask/TaskSet/TaskOption構造を知りたい場合は43_内蔵タスクシステム構造.mdを読む。
3. Task Provider以外のマニフェスト形状が必要なら42_コードModマニフェストリファレンス.mdを読む。

最初に作るならTask Provider
--------

最初のCode ModにはTask Providerが向いています。

理由:

1. ProviderがIDで登録される。
2. Patroller DataがそのProvider IDを参照する。
3. 条件を満たすとProviderがタスクを発行する。

他のCode Modも対応していますが、ダメージイベント、ステータスライフサイクル、Sense、UI操作に触るため上級者向けです。

配置場所
--------

例:

~~~text
Mods\Mod_YourName\Code\ExampleTaskMod\ExampleTaskMod.dll
Mods\Mod_YourName\Code\ExampleTaskMod\example.task_providers.json
~~~

フォルダー名はある程度自由です。重要なのはDLLとマニフェストがMods配下にあることです。

この項目で扱わないもの
--------

カスタムPatroller Prefab ModやカスタムChainpoint Prefab ModのようなUnity EditorアセットModはCode/DLL Modではありません。別のUnity Editorアセット制作手順が必要なので、チュートリアル項目6で扱う予定です。