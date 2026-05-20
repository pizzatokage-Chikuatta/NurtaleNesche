BGMトラック
================================
BGMトラックidと音声ファイル参照を追加/変更するModを作ります。

この対象は「Use_With_Caution」です。便利ですが壊しやすい項目もあるため、まずは既存データの上書きから始めるのを推奨します。

ステップ1
--------
'Mods'の中に以下のフォルダー階層を作る。
Mods\Mod_Chikuatta\Audio\Mod BGM Tracks\BGM Tracks

①"Mod_Chikuatta"は例です。個人のニックネームを使用してください。

ステップ2
--------
Mods\mod.json templatesから'mod.template_folder_backed.json'をコピーし、'Mod BGM Tracks'の中に張り付け、名前を'mod.json'へ変更する。

ステップ3
--------
mod.jsonを開いて中身を以下のように変更する。

```json
{
  "experimentalApiVersion": "v1",
  "type": "bgmTracks",
  "entry": "BGM Tracks",
  "overwriteExisting": true
}
```

①今回はBGMトラックのModなので"type"は"bgmTracks"である。
②"entry"は横のフォルダー名と一致しないといけない。今回なら"BGM Tracks"である。

ステップ4
--------
以下のフォルダーから'bgm_tracks.json'をコピーし、
StreamingAssets\Audio\BGM\Tracks
以下へ張り付ける。
Mods\Mod_Chikuatta\Audio\Mod BGM Tracks\BGM Tracks

①'bgm_tracks.json'は例として選んでおり、同じ種類の他ファイルを選んでも手順は同じです。

ステップ5
--------
'bgm_tracks.json'の中身を修正する。
①"id": BGMトラックid。既存BGMを上書きする場合は同じid、新規なら固有idにする。
②"addressableKey": BGM音声ファイルへの参照。Mod音声を使う場合はModフォルダー内の音声ファイルと対応させる。
③"volumeMultiplier": 音量倍率。1が標準、0.5なら半分くらい。

最後にゲームのメインメニュー -> Mods -> Mod Reportで正しく認識されているか確認することで完了。

備考
--------
①BGMトラックを追加しただけではステージで流れない。BGM Stage Loadouts側でstageIdに割り当てる必要がある。
②音声ファイル形式は.ogg推奨。ファイル名やパスのズレに注意。
③BGMは長い音声なので、音量が大きすぎるとプレイ体験に直撃する。

ファイル参照の流れ
--------

ファイルを登録することと、ゲーム内で使わせることは別です。例えば音声は、audio file -> clip ID -> classification/track/loadout -> gameplay reference のように参照がつながります。

