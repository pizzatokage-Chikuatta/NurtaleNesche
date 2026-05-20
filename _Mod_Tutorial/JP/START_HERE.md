Nurtale Nesche Mod Tutorial
================================

このフォルダーはMod制作のためのチュートリアル集です。このフォルダー自体は、ゲームに読み込ませるModではありません。

最初に読む
--------

1. まずは「チュートリアル項目１」から始めてください。
2. mod.json の "type" を選ぶ前に「Reference_Data/00_Mod_Manifest_And_Registry_Map.md」を確認してください。
3. Item ID、Equipment ID、Region UUID などが必要な時は「Reference_Data」を確認してください。
4. 便利だが壊しやすいModは「チュートリアル項目２」にあります。
5. Action Rules、Interaction、Animation Track Layouts などの高度なJSONは「チュートリアル項目３」にあります。
6. Animation と言語ファイルのModは「チュートリアル項目４」にあります。
7. Code/DLL Mod は「チュートリアル項目５」にあります。JSON/PNG Mod だけでは足りない時だけ使ってください。
8. Unity Editor AssetBundle Prefab Mod は「チュートリアル項目６」にあります。通常のJSON Modを理解してから使ってください。

フォルダー案内
--------

1. 「チュートリアル項目１」: 初心者向けで比較的安全なJSON/audioチュートリアル。
2. 「チュートリアル項目２」: 使えるが注意点が多いチュートリアル。
3. 「チュートリアル項目３」: 高度なJSONチュートリアル。
4. 「チュートリアル項目４」: 高度なアニメーションと言語ファイルのチュートリアル。
5. 「チュートリアル項目５」: 実験的なCode/DLL Modチュートリアル。
6. 「チュートリアル項目６」: 実験的なUnity Editor AssetBundle Prefabチュートリアル。
7. 「Reference_Data」: mod.json type マップ、アイテム、装備、パトローラー、ステータス効果、Region UUIDなどのID一覧。

Code/DLL 読む順番
--------

1. 40_コードMod概要.md
2. 41_タスクプロバイダーコードMod.md
3. 43_内蔵タスクシステム構造.md
4. 42_コードModマニフェストリファレンス.md

Unity Editor AssetBundle 読む順番
--------

1. 50_UnityエディターAssetBundleMod概要.md
2. 51_パトローラーPrefabMod.md
3. 52_チェインポイントPrefabMod.md

基本の流れ
--------

1. どの mod.json "type" を使うか迷う場合は「Reference_Data/00_Mod_Manifest_And_Registry_Map.md」を確認します。
2. 「mod.json templates」からテンプレートをコピーします。
3. チュートリアルに書かれたフォルダー階層を作ります。
4. チュートリアルで指示されている場合は、StreamingAssets から元のJSONをコピーして Mods 内の自分のModフォルダーへ入れます。
5. JSON、CSV、PNG、音声ファイル、DLL関連ファイル、AssetBundleファイルなど、チュートリアルで指定されたファイルを編集します。
6. ゲームのメインメニュー -> Mods -> Mod Report で、Modが正しく読み込まれているか確認します。

IDの基本ルール
--------

他のIDを参照するフィールドでは、適当なIDを作らないでください。Reference_Data、内蔵JSON、または自分のModで作った別JSONのIDを使ってください。新しいトップレベルデータのIDと生成GUIDだけは、自分で作ってよいIDです。

Code Mod 注意
--------

Code/DLL Mod は信頼されたローカルコードです。ゲームはサンドボックス化しません。v1では実験的で、ゲーム更新後に壊れる可能性があります。DLL変更後は基本的にゲーム再起動が必要です。

AssetBundle Prefab Mod 注意
--------

Unity Editor AssetBundle Prefab Mod は高度なModです。ビルド済みゲームは raw .prefab ファイルを Mods に入れても読み込みません。AssetBundleをビルドしてJSONと接続する必要があります。完全に新しいPrefabを作る前に、まず "../Developer Reference Packs/assetbundle_prefab_reference_pack" のサンプルPrefabパッケージをインポートして、既に動くPrefab形状を改造するところから始めてください。

メモ
--------

1. v1 Mod API は実験版です。ゲーム更新でModデータ形状が変わる可能性があります。
2. 内蔵データを上書きする時、変更してはいけないIDがあります。編集前に各チュートリアルを読んでください。
3. 「Reference_Data」のファイルは、現在のゲームデータから生成されています。
4. この「_Mod_Tutorial」フォルダーはドキュメントです。自分の実際のModフォルダーとして扱わないでください。
