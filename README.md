Implemented in 2023.10-2024.01
# Changing the projected image using mocopi
Enable gesture operation for projected evacuation guidance system.

# mocopiを用いた投影映像の変更
## 目的
既存の先行研究として「投影型避難誘導システム」がある。このシステムは、屋内での自己位置推定結果に基づき避難経路を動的に変更し、小型プロジェクターで投影することで避難を補助するものである（参考資料: [J-GLOBAL](https://jglobal.jst.go.jp/detail?JGLOBAL_ID=202402283745637006)）。
現状このシステムは使用者自身によるデータ入力機能を持たないが、より動的な経路選択を実現するためにはデータ入力が不可欠である。入力手段の一つとしてジェスチャー操作を実装する。
![image](https://github.com/RyutaAkashi/Changing-the-projected-image-using-mocopi/blob/main/result/research.png)

## 手法
モーションキャプチャデバイスmocopiを装着し、取得されるデータから姿勢解析を行う。特定のジェスチャーを認識した際に、プロジェクターが投影する映像が切り替わるようにする。

## 実装詳細
### システム構成
* **mocopiセンサー**: 使用者が身体に装着する。
* **mocopi App**: モバイル端末用アプリケーション。センサーとBluetooth通信を行い、センサー情報を受信して人間の動きを描画する。
* **Unity**: mocopi Appから姿勢情報を受け取り、姿勢解析（ジェスチャー判定）を実行する。
* **Raspberry Pi**: Unityから判定されたジェスチャー情報を受け取り、プロジェクターの投影映像を切り替える。
* **プロジェクター**: 映像を投影する。
![image](https://github.com/RyutaAkashi/Changing-the-projected-image-using-mocopi/blob/main/result/method.png)

### `server.cs` (Unity上で動作)
あらかじめ指定した5つのジョイント（関節）の絶対座標を抽出し、ジェスチャーを判定する。判定結果は、ソケット通信を用いてクライアント（Raspberry Pi）に文字列として送信する。

### `client.py` (Raspberry Pi上で動作)
サーバー（Unity）とソケット通信を確立し、ジェスチャーの判定結果を受信する。受信した結果に応じて、プロジェクターが投影する画面の映像を切り替える。

## 結果
身体の動き（ジェスチャー）に応じて、プロジェクターで投影する映像を切り替えるシステムを実装できた。
![image](https://github.com/RyutaAkashi/Changing-the-projected-image-using-mocopi/blob/main/result/result_no.png)
![image](https://github.com/RyutaAkashi/Changing-the-projected-image-using-mocopi/blob/main/result/result_right.png)
![image](https://github.com/RyutaAkashi/Changing-the-projected-image-using-mocopi/blob/main/result/result_batu.png)
![image](https://github.com/RyutaAkashi/Changing-the-projected-image-using-mocopi/blob/main/result/result_sos.png)

## 実行デモンストレーション
本プログラムの実際の動作については、以下の動画で確認できる。
* [mocopiによるデータ入力](https://youtu.be/0JK1nc627ek)

