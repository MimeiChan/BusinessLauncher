using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LauncherApp.Models;
using LauncherApp.Services;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using System;
using System.Threading.Tasks;

namespace LauncherApp.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly ISettingsService _settingsService;
        private readonly IDialogService _dialogService;
        private readonly ILogger<SettingsViewModel> _logger;

        [ObservableProperty]
        private bool _darkMode;

        [ObservableProperty]
        private int _gridItemSize = 120;

        [ObservableProperty]
        private int _columnsCount = 5;

        [ObservableProperty]
        private bool _showCategories = true;

        [ObservableProperty]
        private bool _showLastLaunched = false;

        [ObservableProperty]
        private string _settingsFilePath = string.Empty;

        [ObservableProperty]
        private LogLevel _logLevel = LogLevel.Information;

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        [ObservableProperty]
        private bool _isLoading;

        public SettingsViewModel(
            ISettingsService settingsService,
            IDialogService dialogService,
            ILogger<SettingsViewModel> logger)
        {
            _settingsService = settingsService;
            _dialogService = dialogService;
            _logger = logger;
        }

        [RelayCommand]
        private async Task LoadSettingsAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "設定を読み込んでいます...";

                var settings = await _settingsService.LoadAppSettingsAsync();
                
                DarkMode = settings.DarkMode;
                GridItemSize = settings.GridItemSize;
                ColumnsCount = settings.ColumnsCount;
                ShowCategories = settings.ShowCategories;
                ShowLastLaunched = settings.ShowLastLaunched;
                SettingsFilePath = settings.SettingsFilePath;
                LogLevel = settings.LogLevel;

                // Apply theme
                ApplyTheme();

                StatusMessage = "設定を読み込みました";
                _logger.LogInformation("Settings loaded");
            }
            catch (Exception ex)
            {
                StatusMessage = "設定の読み込み中にエラーが発生しました";
                _logger.LogError(ex, "Error loading settings");
                await _dialogService.ShowErrorAsync("エラー", $"設定の読み込み中にエラーが発生しました: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task SaveSettingsAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "設定を保存しています...";

                var settings = new AppSettings
                {
                    DarkMode = DarkMode,
                    GridItemSize = GridItemSize,
                    ColumnsCount = ColumnsCount,
                    ShowCategories = ShowCategories,
                    ShowLastLaunched = ShowLastLaunched,
                    SettingsFilePath = SettingsFilePath,
                    LogLevel = LogLevel
                };

                await _settingsService.SaveAppSettingsAsync(settings);
                
                // Apply theme
                ApplyTheme();

                StatusMessage = "設定を保存しました";
                _logger.LogInformation("Settings saved");
            }
            catch (Exception ex)
            {
                StatusMessage = "設定の保存中にエラーが発生しました";
                _logger.LogError(ex, "Error saving settings");
                await _dialogService.ShowErrorAsync("エラー", $"設定の保存中にエラーが発生しました: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void ApplyTheme()
        {
            var app = Application.Current as App;
            if (app is null) return;

            // Apply theme to application
            if (App.MainWindow.Content is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = DarkMode ? ElementTheme.Dark : ElementTheme.Light;
                _logger.LogInformation("Theme applied: {Theme}", DarkMode ? "Dark" : "Light");
            }
        }

        [RelayCommand]
        private async Task BrowseSettingsFilePathAsync()
        {
            // This is a placeholder - actual implementation would use Windows.Storage.Pickers
            // which requires additional setup in WinUI 3
            await _dialogService.ShowErrorAsync("未実装", "この機能はプロトタイプでは未実装です。");
        }
    }
}
