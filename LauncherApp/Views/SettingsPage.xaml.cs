using LauncherApp.Services;
using LauncherApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace LauncherApp.Views
{
    public sealed partial class SettingsPage : Page
    {
        private SettingsViewModel _viewModel;
        private readonly ILogger<SettingsPage> _logger;

        public SettingsPage()
        {
            InitializeComponent();

            // Get dependencies
            var services = App.Current.Services;
            _logger = services.GetService<ILogger<SettingsPage>>();
            
            // Initialize ViewModel
            _viewModel = new SettingsViewModel(
                services.GetService<ISettingsService>(),
                services.GetService<IDialogService>(),
                services.GetService<ILogger<SettingsViewModel>>());

            // Bind ViewModel
            DataContext = _viewModel;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            try
            {
                // Load settings
                StatusTextBlock.Text = "設定を読み込んでいます...";
                LoadingIndicator.IsActive = true;
                await _viewModel.LoadSettingsAsync();
                
                // Update UI from ViewModel
                UpdateUIFromViewModel();
                
                StatusTextBlock.Text = "設定を読み込みました";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading settings");
                StatusTextBlock.Text = "設定の読み込み中にエラーが発生しました";
            }
            finally
            {
                LoadingIndicator.IsActive = false;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Update ViewModel from UI
                UpdateViewModelFromUI();
                
                // Save settings
                StatusTextBlock.Text = "設定を保存しています...";
                LoadingIndicator.IsActive = true;
                await _viewModel.SaveSettingsAsync();
                
                StatusTextBlock.Text = "設定を保存しました";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving settings");
                StatusTextBlock.Text = "設定の保存中にエラーが発生しました";
            }
            finally
            {
                LoadingIndicator.IsActive = false;
            }
        }

        private async void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _viewModel.BrowseSettingsFilePathCommand.ExecuteAsync(null);
                SettingsFilePathTextBox.Text = _viewModel.SettingsFilePath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error browsing for settings file");
                StatusTextBlock.Text = "ファイル選択中にエラーが発生しました";
            }
        }

        private void UpdateUIFromViewModel()
        {
            // Appearance
            DarkModeToggle.IsOn = _viewModel.DarkMode;
            
            // Grid Layout
            GridItemSizeSlider.Value = _viewModel.GridItemSize;
            ColumnsCountSlider.Value = _viewModel.ColumnsCount;
            
            // Display Options
            ShowCategoriesToggle.IsOn = _viewModel.ShowCategories;
            ShowLastLaunchedToggle.IsOn = _viewModel.ShowLastLaunched;
            
            // Advanced Settings
            SettingsFilePathTextBox.Text = _viewModel.SettingsFilePath;
            
            // Log Level
            int logLevelIndex = (int)_viewModel.LogLevel;
            if (logLevelIndex >= 0 && logLevelIndex < LogLevelComboBox.Items.Count)
            {
                LogLevelComboBox.SelectedIndex = logLevelIndex;
            }
            
            // Status
            StatusTextBlock.Text = _viewModel.StatusMessage;
            LoadingIndicator.IsActive = _viewModel.IsLoading;
        }

        private void UpdateViewModelFromUI()
        {
            // Appearance
            _viewModel.DarkMode = DarkModeToggle.IsOn;
            
            // Grid Layout
            _viewModel.GridItemSize = (int)GridItemSizeSlider.Value;
            _viewModel.ColumnsCount = (int)ColumnsCountSlider.Value;
            
            // Display Options
            _viewModel.ShowCategories = ShowCategoriesToggle.IsOn;
            _viewModel.ShowLastLaunched = ShowLastLaunchedToggle.IsOn;
            
            // Advanced Settings
            _viewModel.SettingsFilePath = SettingsFilePathTextBox.Text;
            
            // Log Level
            _viewModel.LogLevel = (LogLevel)LogLevelComboBox.SelectedIndex;
        }
    }
}
