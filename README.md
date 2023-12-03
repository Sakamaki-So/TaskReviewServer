# TaskReview サーバーアプリ
## このアプリについて
このアプリはモバイルアプリ開発Dチームの作成アプリである
[TaskReview](https://github.com/haruto0707/TaskReview)
のサーバーサイドのアプリです。
このアプリはFirebase内に格納されたやることリストと評価データを収集し、やることリストの優先度を算出して書き換えます。そのため、TaskReview単体でも動作しますが、優先度の算出を行い、書き換える場合にはこのアプリが動作している必要があります。

## 使用方法
このアプリは.NET環境が必要です。そのため、Windows以外のOSの場合は、.NET SDKをインストールする必要があります。

まず、ソースファイルを使用する場合はVisual Studioからビルドを行っいます。
リリースアプリケーションから実行する場合は、[ここからダウンロード](https://github.com/Sakamaki-So/TaskReviewServer/releases/tag/1.1)し、展開してからtaskreviewserver.exeが存在するディレクトリ上で以下のコマンドを実行してください。
```
taskreviewserver.exe
```

次に、APIキー、メールアドレス、パスコードを要求されるので、あらかじめ伝えてあるものを入力してください。認証に失敗した場合はそのままエラーを出力して終了します。アプリが正しく実行されている場合は、使用方法可能なコマンドが表示されます。

実行する場合は以下のコマンドを入力します。

```shell
# 1分ごとに集計、算出し、算出結果で上書きする
# Firebase内の集計データを表示する。
launch

# 何分ごとに実行するかを指定する場合(例は2分ごとに設定)
launch -t 2

# 集計データを非表示にする場合
launch --hide
```

終了する場合は以下のコマンドを使用します。
```
exit
```



## 使用ライブラリ
firebase-database-dotnet: https://github.com/step-up-labs/firebase-database-dotnet

firebase-authentication-dotnet: https://github.com/step-up-labs/firebase-authentication-dotnet