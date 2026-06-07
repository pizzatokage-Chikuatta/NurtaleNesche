アニメーションターゲットとスロット
================================

アニメーションチュートリアルで `target`、`variant`、`clipSlot`、`clipId`、`trackId` が必要になった時に見るファイルです。

重要：StreamingAssetsの注意
--------

`Assets\StreamingAssets`には直接読めるゲームデータが多く入っていますが、`Assets\StreamingAssets\AnimationClips`は例外です。`Assets\StreamingAssets\AnimationClips`内のファイルを直接編集しても、安全に配布できるアニメーションModにはなりません。

`Assets\StreamingAssets\AnimationClips`は、内蔵データの`clipId`、`trackId`、タイミング、位置情報などを確認するための参照用として使ってください。実際のアニメーションModファイルは、必ず`Mods\...`の中に置きます。

targetはPatroller IDではない
--------

以下のようなゲームプレイ用IDを使ってはいけません。

~~~text
patroller.goblin.stage_06
patroller.orc.stage_01
~~~

アニメーションModでは以下のようなアニメーションターゲットIDを使います。

~~~text
Orc_AC Edit
Goblin_AC Edit
Nesche
~~~

よく使うtarget
--------

| 目的 | target | variant |
| --- | --- | --- |
| Orcの基本アニメーション/コントローラー | `Orc_AC Edit` | `""` |
| Goblinの基本アニメーション/コントローラー | `Goblin_AC Edit` | `""` |
| Nesche通常アクション | `Nesche` | `""` |
| Nescheアームバインダー | `Nesche` | `"armbinder"` |
| Nesche cuff-collar-chain / CWC | `Nesche` | `"ccc"` |
| Nescheヨーク拘束 | `Nesche` | `"yoke_bondage"` |

よく使うclipSlot
--------

`clipSlot`は`type: "animationClipOverride"`で使います。

`clipSlot`は以下をつなげます。

~~~text
stateId.clipSlotName
~~~

Orcの例:

| Controller | State | Slot | 最終clipSlot | 内蔵clipId |
| --- | --- | --- | --- | --- |
| `Orc_AC Edit` | `Orc_WalkRun` | `walk` | `Orc_WalkRun.walk` | `orc.orc_walk` |
| `Orc_AC Edit` | `Orc_WalkRun` | `run` | `Orc_WalkRun.run` | `orc.orc_run` |

Nescheの例:

| Target | Variant | State | Slot | 最終clipSlot | 内蔵clipId |
| --- | --- | --- | --- | --- | --- |
| `Nesche` | `""` | `Idle` | `main` | `Idle.main` | `nesche.actions.normal.nesche_normal_idle_e` |
| `Nesche` | `""` | `Walk_Run` | `walk` | `Walk_Run.walk` | `nesche.actions.normal.walk_edit.nesche_normal_walk_edit2` |
| `Nesche` | `"armbinder"` | `Idle` | `main` | `Idle.main` | `nesche.actions.armbinder.nesche_armbinder_idle_e` |
| `Nesche` | `"armbinder"` | `Walk_Run` | `walk` | `Walk_Run.walk` | `nesche.actions.armbinder.nesche_armbinder_walk` |

Nescheで推奨するPatch形
--------

Nescheのbody/overlayトラックを差し替える場合は、`type: "animation"` と `mergeMode: "patch"` を使うのが安全です。

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "animation",
  "target": "Nesche",
  "variant": "armbinder",
  "entry": "Animations",
  "mergeMode": "patch"
}
~~~

変更したいトラックだけを書きます。

~~~json
{
  "clips": [
    {
      "clipId": "nesche.actions.armbinder.nesche_armbinder_idle_e",
      "tracks": [
        {
          "trackId": "eyemask",
          "spriteFrames": [
            { "spriteName": "MyEyemask_0", "time": 0.0, "duration": 0.24 }
          ]
        }
      ]
    }
  ]
}
~~~

書かなかったトラックは内蔵データ、または現在勝っているフル置換データから引き継がれます。

variantルール
--------

`variant: ""` のPatchは通常/baseのPlayerクリップだけに効きます。

`variant: "armbinder"` のPatchはアームバインダー用Playerクリップだけに効きます。

eyemaskスキンを通常、アームバインダー、ヨークなど全部に反映したい場合は、それぞれのvariantとclipごとにPatchを作る必要があります。