# Developer Reference Packs

このフォルダーは通常のJSON Mod例ではありません。

すでにチュートリアルを読んだ上級Mod制作者向けのリファレンス資料です。

通常のJSON/PNG Mod制作者はまず以下から始めてください。

~~~text
../_Mod_Tutorial/JP/START_HERE.md
~~~

Code/DLL Mod制作者はまず以下を読んでください。

~~~text
../_Mod_Tutorial/JP/チュートリアル項目５
~~~

Unity Editor / AssetBundle Prefab Mod制作者は以下を読んでください。

~~~text
../_Mod_Tutorial/JP/チュートリアル項目６
~~~

パック一覧
--------

1. `code_mod_reference_project`
   - ビルド可能なexperimental Code/DLLリファレンスプロジェクト。
   - チュートリアル項目５で説明しているmanifest-backedなCode Mod面を扱います。
   - 実際の`.csproj`とソース配置を見たい場合に使います。

2. `advanced_task_provider_pack`
   - Task Providerのソース断片リファレンス。
   - 小さい参考資料として残しています。
   - 完全なチュートリアルでも、ビルド可能プロジェクトでもありません。

3. `assetbundle_prefab_reference_pack`
   - AssetBundle Prefab Mod用のUnity Prefabサンプルパッケージ/リファレンス。
   - チュートリアル項目６で使います。

ルール
--------

このフォルダー内に別のチュートリアルを増やさないでください。

チュートリアルの唯一の正本は常に`../_Mod_Tutorial`です。
