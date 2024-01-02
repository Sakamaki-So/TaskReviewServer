# TaskReview サーバーアプリ
## このアプリについて
モバイルアプリ開発Dチームの開発している
[TaskReview](https://github.com/haruto0707/TaskReview)
のサーバーサイドのアプリです。
このアプリはFirebase内に格納されたやることリストと評価データを収集し、やることリストの優先度を算出して書き換えます。

## 使用方法
このアプリは.NET6.0環境が必要です。

まず、ソースファイルを使用する場合はVisual Studioからビルドを行います。
リリースアプリケーションから実行する場合は、[ここからダウンロード](https://github.com/Sakamaki-So/TaskReviewServer/releases/tag/1.1)して、展開してからtaskreviewserver.exeが存在するディレクトリ上で以下のコマンドを実行します。
```
taskreviewserver.exe
```

次に、画面の指示に従い、APIキー、メールアドレス、パスコードを入力します。認証に失敗した場合はそのままエラーを出力して終了します。アプリが正しく実行されている場合は、使用可能なコマンドが表示されます。

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
