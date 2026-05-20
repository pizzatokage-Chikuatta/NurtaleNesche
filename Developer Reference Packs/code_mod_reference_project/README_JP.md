# Code Modリファレンスプロジェクト

これはv1 experimental Code/DLL Mod用の、ビルド可能なリファレンスプロジェクトです。

メインのチュートリアルではありません。

先に以下を読んでください。

~~~text
../../_Mod_Tutorial/JP/チュートリアル項目５
~~~

このプロジェクトは、実際の`.csproj`、ソース配置、manifestテンプレートを見せるためのものです。

このプロジェクトが扱うもの
--------

ビルド対象の例:

1. Task Provider: `*.task_providers.json`。
2. Arrow/Potion Event Task Option: `*.event_task_options.json`。
3. Captive Treating Task Option: `*.captive_treating_task_options.json`。
4. Sense component: `*.senses.json`。
5. Status Effect Factory: `*.status_effect_factories.json`。
6. Item Selection Request Factory: `*.item_selection_request_factories.json`。

高度な参考ソースのみ:

1. Solo Action Task Option。
2. Chase Task Option。
3. Ability Task Option。
4. Collect Captive Task Option。

これらの高度なOptionカテゴリはRegistry Helperを使うため、通常のmanifest例と同じ初心者向け扱いにはしません。`.cs.txt`の参考ソースとして含めています。

ビルド準備
--------

対象ゲームバージョンと同じDLLが必要です。

公開パッケージ内では、以下の配置を想定しています。

~~~text
NurtaleNesche_Modding_v1_Public
  dlls
    NurtaleNesche.Modding.Abstractions.dll
    NurtaleNesche.Runtime.dll
    UnityEngine.CoreModule.dll
    Newtonsoft.Json.dll
  Developer Reference Packs
    code_mod_reference_project
      ExampleCodeMod.csproj
~~~

DLLが別の場所にある場合は以下のようにビルドします。

~~~text
dotnet build ExampleCodeMod.csproj /p:NurtaleNescheDllDir="C:/Path/To/Nurtale Nesche_Data/Managed"
~~~

`Newtonsoft.Json.dll`や`UnityEngine.CoreModule.dll`が別フォルダーの場合は、以下も指定します。

~~~text
/p:NewtonsoftDllDir="C:/Path/To/NewtonsoftFolder" /p:UnityDllDir="C:/Path/To/UnityEngineFolder"
~~~

出力
--------

ビルドされたDLLは以下です。

~~~text
bin/Debug/netstandard2.1/ExampleCodeMod.dll
~~~

ゲームはDLLを読み込みます。`Mods`内の`.cs`ファイルはコンパイルしません。

最終的なModフォルダー例
--------

~~~text
Mods
  Mod_YourName
    Code
      ExampleCodeMod
        ExampleCodeMod.dll
        example.task_providers.json
        example.event_task_options.json
        example.captive_treating_task_options.json
        example.senses.json
        example.status_effect_factories.json
        example.item_selection_request_factories.json
~~~

実際に使うmanifestだけをコピーしてリネームしてください。

使わないexample manifestを実際のModに同梱しないでください。

重要
--------

Code/DLL Modはv1 experimentalです。

対象ゲームバージョンと同じDLLを参照してビルドしてください。DLLを変更したらゲーム再起動が必要です。
