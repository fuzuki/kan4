簡単官報管理官。(2017.02.16)
=============

インターネット版「官報」(<https://kanpou.npb.go.jp/>)のPDFファイルをダウンロードして、手元で管理するソフトです。

## 機能  
* pdf官報ダウンロード
* pdf連結
* 見出し検索

## インストール
不要。

## 使い方
解凍したディレクトリ内の、kan4.exeを実行してください。  
インターネットに接続した状態で「Download」ボタンを押すと、官報のダウンロードを開始します。  
「Cancel」ボタンを押すと、現在ダウンロード中の官報の処理が終わり次第、それ以上のダウンロードを中止します。  
テキストボックスに検索文字列を入力して、検索期間を指定して「search」ボタンを押すと、ダウンロード済み官報の目次を検索します。「本紙」「特別号外」といった文字列での検索も可能です。  
検索結果のリスト項目をダブルクリックすると、対象のpdfを開きます。  

## アンインストール
解凍したディレクトリを削除してください。  

## 動作環境  
* Windows8以降(.NET Framework 4.5 以降)

## 開発環境
* Visual Studio 2015

## ライセンス
* GPLv3(<https://ja.osdn.net/projects/opensource/wiki/licenses%252FGNU_General_Public_License_version_3.0>)

## ソースコード
kan4-src.zip  
または<https://github.com/fuzuki/kan4>。

## 利用ライブラリ
* iTextSharp 5.5.10(<https://www.nuget.org/packages/iTextSharp/>)
* System.Data.SQLite (x86/x64) 1.0.104(<https://www.nuget.org/packages/System.Data.SQLite/>)

## アイコン
Polaris UI and Linecons(<https://designmodo.com/linecons-free/>)

## 作成
ふづき(fuzuki@hiruandon.net)

