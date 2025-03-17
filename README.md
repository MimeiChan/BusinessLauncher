# Business Launcher

WinUI 3ベースの業務ランチャーアプリケーションプロトタイプです。このアプリケーションは、よく使用するアプリケーションやツールを一元管理し、効率的に起動するためのランチャーです。

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
```