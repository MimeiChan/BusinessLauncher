# Business Launcher

WinUI 3ベースの業務ランチャーアプリケーションプロトタイプです。このアプリケーションは、よく使用するアプリケーションやツールを一元管理し、効率的に起動するためのランチャーです。

![スクリーンショット](docs/screenshot.png)

## 機能

- Fluent Designに基づいたモダンなUI
- アプリケーションの追加、編集、削除機能
- グリッドレイアウト表示
- ドラッグ＆ドロップによる並べ替え
- カテゴリ分類と検索機能
- ライト/ダークテーマ対応
- 設定のJSON形式での保存と読み込み

## アーキテクチャ

- MVVM (Model-View-ViewModel)パターン
- 依存性注入によるサービスの管理
- JSON形式によるデータ永続化

## セットアップ

### 必要環境

- Windows 10 version 1809 (build 17763) 以上
- Visual Studio 2022
- .NET 8.0
- Windows App SDK 1.4以上

### 開発環境のセットアップ

1. リポジトリをクローン: `git clone https://github.com/MimeiChan/BusinessLauncher.git`
2. Visual Studioでソリューションを開く
3. ビルドして実行

## 使い方

### アプリケーションの起動

1. グリッド表示されているアプリケーションをクリックすると起動します
2. 検索ボックスで名前やカテゴリでフィルタリングできます

### アプリケーションの追加・編集

1. 「編集モード」ボタンをクリックして編集モードに切り替えます
2. 「追加」ボタンをクリックして新しいアプリケーションを追加します
3. 既存のアプリケーションをクリックして編集します
4. 編集が完了したら「表示モード」ボタンをクリックして通常モードに戻ります

### 設定の変更

1. 「設定」ボタンをクリックして設定画面を開きます
2. テーマやグリッドサイズなどを変更できます
3. 「保存」ボタンをクリックして設定を保存します

## プロジェクト構成

```
LauncherApp (WinUI 3プロジェクト)
├── App.xaml / App.xaml.cs
├── MainWindow.xaml / MainWindow.xaml.cs
├── Views/
│   ├── LauncherPage.xaml
│   ├── EditItemDialog.xaml
│   └── SettingsPage.xaml
├── ViewModels/
│   ├── LauncherViewModel.cs
│   ├── LaunchItemViewModel.cs
│   └── SettingsViewModel.cs
├── Models/
│   ├── LaunchItem.cs
│   └── AppSettings.cs
├── Services/
│   ├── ISettingsService.cs
│   ├── JsonSettingsService.cs
│   ├── ILauncherService.cs
│   ├── ProcessLauncherService.cs
│   └── IDialogService.cs
└── Helpers/
    ├── Logger.cs
    └── Extensions/
        ├── CollectionExtensions.cs
        └── UIExtensions.cs
```

## 今後の拡張予定

- ショートカットキーによる起動機能
- アイコンカスタマイズ機能の強化
- ドラッグ＆ドロップによる並べ替え実装
- クラウド同期機能

## 注意事項

このアプリケーションはプロトタイプであり、すべての機能が実装されているわけではありません。以下の機能は現在開発中です:

- ファイル選択ダイアログを使用した実行ファイル選択
- ドラッグ＆ドロップによる並べ替え
- カスタムアイコンの設定

## ライセンス

[MIT License](LICENSE)

## 謝辞

- [Windows UI Library (WinUI)](https://github.com/microsoft/microsoft-ui-xaml)
- [CommunityToolkit.WinUI.UI.Controls](https://github.com/windows-toolkit/WindowsCommunityToolkit)
- [Microsoft.Extensions.DependencyInjection](https://github.com/dotnet/runtime)
