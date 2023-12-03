# TaskReview サーバーアプリ
## このアプリについて
このアプリはモバイルアプリ開発Dチームの作成アプリである
[TaskReview](https://github.com/haruto0707/TaskReview)
のサーバーサイドのアプリです。
このアプリはFirebase内に格納されたやることリストと評価データを収集し、やることリストの優先度を算出して書き換えます。そのため、TaskReview単体でも動作しますが、優先度の算出を行い、書き換える場合にはこのアプリが動作している必要があります。

## 使用方法
このアプリは.NET環境が必要です。そのため、Windows以外のOSの場合は、.NET SDKをインストールする必要があります。

まず、ソースファイルを使用する場合はVisual Studioからビルドを行ってください。

リリースアプリケーションから実行する場合は、[ここからダウンロード](https://github.com/Sakamaki-So/TaskReviewServer/releases/tag/1.0)し、展開してからtaskreviewserver.exeが存在するディレクトリ上で以下のコマンドを実行してください。
```
taskreviewserver.exe
```

次に、APIキー、メールアドレス、パスコードを要求されるので、あらかじめ伝えてあるものを入力してください。認証に失敗した場合はそのままエラーを出力して終了します。

アプリが正しく実行されている場合は、1分おきにFirebaseから取得したデータを画面上に出力します。アプリを終了するには、何かキーを押すと終了します。

## 使用ライブラリ
firebase-database-dotnet: https://github.com/step-up-labs/firebase-database-dotnet

firebase-authentication-dotnet: https://github.com/step-up-labs/firebase-authentication-dotnet