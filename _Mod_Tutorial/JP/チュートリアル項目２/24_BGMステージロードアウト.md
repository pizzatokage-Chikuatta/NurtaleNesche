BGMステージロードアウト
================================
どのステージでどのBGMを流すかを変更するModを作ります。

この対象は「Use_With_Caution」です。便利ですが壊しやすい項目もあるため、まずは既存データの上書きから始めるのを推奨します。

ステップ1
--------
'Mods'の中に以下のフォルダー階層を作る。
Mods\Mod_Chikuatta\Audio\Mod BGM Stage Loadouts\BGM Stage Loadouts

①"Mod_Chikuatta"は例です。個人のニックネームを使用してください。

ステップ2
--------
Mods\mod.json templatesから'mod.template_folder_backed.json'をコピーし、'Mod BGM Stage Loadouts'の中に張り付け、名前を'mod.json'へ変更する。

ステップ3
--------
mod.jsonを開いて中身を以下のように変更する。

```json
{
  "experimentalApiVersion": "v1",
  "type": "bgmStageLoadouts",
  "entry": "BGM Stage Loadouts",
  "overwriteExisting": true
}
```

①今回はBGMステージロードアウトのModなので"type"は"bgmStageLoadouts"である。
②"entry"は横のフォルダー名と一致しないといけない。今回なら"BGM Stage Loadouts"である。

ステップ4
--------
以下のフォルダーから'bgm_stage_loadouts.json'をコピーし、
StreamingAssets\Audio\BGM\Stage Loadouts
以下へ張り付ける。
Mods\Mod_Chikuatta\Audio\Mod BGM Stage Loadouts\BGM Stage Loadouts

①'bgm_stage_loadouts.json'は例として選んでおり、同じ種類の他ファイルを選んでも手順は同じです。

ステップ5
--------
'bgm_stage_loadouts.json'の中身を修正する。
①"stageId": 対象ステージid。例: "stage_01"。
②"bgmTrackId": 流すBGMトラックid。BGM Tracks側の"id"と一致させる。

最後にゲームのメインメニュー -> Mods -> Mod Reportで正しく認識されているか確認することで完了。

備考
--------
①存在しないbgmTrackIdを入れるとBGMが流れない。
②新規BGMを使う場合は、先にBGM Tracks Modでトラックidを登録する。

置き換えリストの注意
--------

このファイルは「少し追加するパッチ」ではなく、リスト全体の置き換えです。同じ"id"を上書きする場合、最終的に使われるリストはMod側に書いたリストになります。残したいbuilt-in項目は消さずに残してください。

ファイル参照の流れ
--------

ファイルを登録することと、ゲーム内で使わせることは別です。例えば音声は、audio file -> clip ID -> classification/track/loadout -> gameplay reference のように参照がつながります。

