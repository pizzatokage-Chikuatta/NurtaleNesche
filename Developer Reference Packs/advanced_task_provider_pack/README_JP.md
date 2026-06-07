# 高度なTask Providerリファレンスパック

このフォルダーは、Code/DLL Mod制作者向けのリファレンスソースです。

メインのチュートリアルではありません。

先に以下のチュートリアルを読んでください。

~~~text
../../_Mod_Tutorial/JP/チュートリアル項目５
~~~

このパックは、DLL -> manifest -> Patroller Data の流れを理解した後に参照するためのものです。

このパックの目的
--------

このパックは、Task Provider Code Modの最小サンプルソースです。

説明しているもの:

1. Plain C#のTask Providerクラス。
2. `TaskProviderIdAttribute`。
3. Patroller DataのJSON configを読む`ApplyConfig`。
4. `TaskSequenceBuilder`。
5. 任意のRegistrarクラス。
6. Task Provider manifestのテンプレート。

このパックに含まれないもの
--------

このパックは完全なビルド可能プロジェクトではありません。

含まれないもの:

1. `.csproj`。
2. IDE固有のセットアップ。
3. ビルド済みDLL。
4. 完全なPatroller Data Mod。
5. 完全なCode Modチュートリアル。

完全な手順は`_Mod_Tutorial/JP/チュートリアル項目５`に集約します。チュートリアルを複数箇所に分散させないためです。

ファイル
--------

1. `ExampleTaskProvider.cs.txt`: 簡単な待機Task Providerのソース。
2. `ExampleTaskProviderRegistrar.cs.txt`: 任意の明示的な登録ヘルパー。
3. `example.task_providers.template.json`: provider IDとC# typeを結びつけるmanifestテンプレート。
4. `BUILD_NOTES_EN.txt`: 短いビルド/参照メモ。
5. `README_EN.md`: 英語説明。
6. `README_JP.md`: このファイル。

最終的なModフォルダー例
--------

DLLをビルドした後、プレイヤーへ配布するModは大体以下の形になります。

~~~text
Mods
  Mod_YourName
    Code
      ExampleTaskMod
        ExampleTaskMod.dll
        example.task_providers.json
    Patrollers
      Your Patroller Data Folder
        mod.json
        Patroller Data
          Stage 6
            patroller.your_test.json
~~~

DLLにはC#の挙動が入ります。

`example.task_providers.json`はprovider IDを登録します。

Patroller Data JSONは`tasks.providers`でそのprovider IDを参照する必要があります。

重要なルール
--------

ゲームは`Mods`内の`.cs`ファイルをコンパイルしません。

プレイヤーへ配布する必要があるのはビルド済みDLLです。

このフォルダー内の`.cs.txt`はMod制作者用の参考ソースです。

例のProvider ID
--------

このパックでは以下を使っています。

~~~text
task_provider.example.wait
~~~

実際に公開するModでは、自分用の一意なIDを使ってください。

~~~text
task_provider.your_name.wait
~~~

Patroller Data側の例
--------

ProviderはPatroller Dataで参照されて初めて有効になります。

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

`config.seconds`は`ExampleTaskProvider.cs.txt`の`ApplyConfig`で読み取られます。

互換性
--------

Code/DLL Modはv1 experimentalです。

対象ゲームバージョンと同じDLLを参照してビルドしてください。ゲーム更新後は再ビルドと再テストが必要です。
