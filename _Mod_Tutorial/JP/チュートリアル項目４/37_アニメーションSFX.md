アニメーションSFX
================================

このチュートリアルではJSONアニメーションクリップにSFXイベントを追加します。Unityの.anim Animation Eventではありません。古いAnimatorへreceiver methodを追加する方式ではありません。

ステップ1
--------

Modsの中に以下のフォルダー階層を作る。

~~~text
Mods\Mod_YourName\Animations\Goblin Idle SFX\Animation SFX
~~~

ステップ2
--------

mod.template_folder_backed.jsonをコピーして、mod.jsonへ変更する。

mod.jsonの中身：

~~~json
{
  "experimentalApiVersion": "v1",
  "type": "animationSFX",
  "entry": "Animation SFX",
  "overwriteExisting": true
}
~~~

ステップ3
--------

Animation SFXフォルダーの中に以下を作る。

~~~text
goblin_idle_sfx.json
~~~

中身：

~~~json
{
  "animationClipId": "goblin.goblin_idling",
  "events": [
    {
      "normalizedTime": 0.2,
      "playbackAction": "play",
      "sfxClassId": "sfx.voice.akanai",
      "sfxClipId": "",
      "volumeMultiplier": 1.0,
      "pitchMultiplier": 1.0,
      "soundStrength": 10.0,
      "noiseLevel": 0.0,
      "raiseSuspicion": false,
      "soundSource": "Enemy",
      "loop": false,
      "alert": false
    }
  ]
}
~~~

ステップ4
--------

animationClipIdを理解する。

animationClipIdはsound eventを付けるclipIdです。controller stateIdではありません。

正しい例：

~~~text
goblin.goblin_idling
~~~

間違い：

~~~text
Goblin_Idling
~~~

ステップ5
--------

normalizedTimeを理解する。

~~~text
"normalizedTime": 0.2
~~~

これはアニメーション全体の20%地点で再生するという意味です。

1. 0.0: 開始地点。
2. 0.5: 中間。
3. 1.0: 終了地点。

ステップ6
--------

SFX選択を理解する。

"sfxClassId"は分類IDから重み付きで音を選ぶ通常方式です。

~~~text
"sfxClassId": "sfx.voice.akanai",
"sfxClipId": ""
~~~

"sfxClipId"は特定の音だけを直接指定したい場合に使います。

~~~text
"sfxClassId": "",
"sfxClipId": "sfx.clip.example"
~~~

最初はsfxClassIdだけを使ってください。両方埋めると、どちらを期待していたのか分かりづらくなります。

ステップ7
--------

独自の.oggを使う場合の流れ：

~~~text
.ogg file -> sfxClipId -> sfxClassId -> animationSFX event
~~~

1. .ogg用のAudio Files modを作る。
2. それに対応するSFX clip idを作る。
3. SFX Classification modでsfxClassIdを作り、clip idを登録する。
4. そのsfxClassIdをanimationSFXへ書く。

どれか一つでも間違うと、animationSFXは読み込まれても音が鳴らないことがあります。

ステップ8
--------

重要な音フィールド：

1. "volumeMultiplier": 音量倍率。
2. "pitchMultiplier": ピッチ倍率。
3. "soundStrength": 音の伝播強度。
4. "noiseLevel": suspicion/perception用。
5. "raiseSuspicion": trueで疑念上昇に使う。
6. "soundSource": Enemy, Player, Environmentなど。
7. "loop": trueでループ。
8. "playbackAction": "play"で再生。
9. "alert": trueでSoundToScreenFXを押す可能性がある。

alert方針：

1. 敵や罠の警戒音はtrueでよい。
2. Player/friendly captiveの音はfalse。
3. 迷ったらfalse。

よくあるミス
--------

1. animationClipIdにGoblin_Idlingを書く。
2. normalizedTimeに2.0を書く。
3. Player/friendly音でalert trueにする。
4. 存在しないsfxClassIdを書く。
5. Unity .animイベントと混同する。
