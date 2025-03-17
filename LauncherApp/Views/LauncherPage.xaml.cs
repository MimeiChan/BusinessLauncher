using LauncherApp.Services;
using LauncherApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LauncherApp.Views
{
    public sealed partial class LauncherPage : Page
    {
        private LauncherViewModel _viewModel;
        private readonly ILogger<LauncherPage> _logger;

        public LauncherPage()
        {
            InitializeComponent();

            // Get dependencies
            var services = App.Current.Services;
            _logger = services.GetRequiredService<ILogger<LauncherPage>>();
            
            try
            {
                // Initialize ViewModel
                _viewModel = new LauncherViewModel(
                    services.GetRequiredService<ISettingsService>(),
                    services.GetRequiredService<ILauncherService>(),
                    services.GetRequiredService<IDialogService>(),
                    services.GetRequiredService<ILogger<LauncherViewModel>>());

                // Explicitly set DataContext
                this.DataContext = _viewModel;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing ViewModel: {ex.Message}");
                if (_logger != null)
                {
                    _logger.LogError(ex, "Error initializing ViewModel");
                }
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            try
            {
                // Load data
                StatusTextBlock.Text = "データを読み込んでいます...";
                LoadingIndicator.IsActive = true;
                
                // Load items
                await _viewModel.LoadItemsAsync();
                
                // Initialize UI
                SetupUI();
                
                // Update status
                StatusTextBlock.Text = $"{_viewModel.LaunchItems.Count} アイテムを読み込みました";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "エラー: ページ読み込み中");
                StatusTextBlock.Text = "データ読み込み中にエラーが発生しました";
            }
            finally
            {
                LoadingIndicator.IsActive = false;
            }
        }
        
        private void SetupUI()
        {
            // Bind the GridView to the filtered items
            ItemsGridView.ItemsSource = _viewModel.FilteredItems;
            
            // Bind the search box
            SearchBox.Text = _viewModel.SearchText;
            
            // Setup the category combo box
            CategoryComboBox.ItemsSource = _viewModel.Categories;
            
            // Debugging: Print loaded items to debug output
            foreach (var item in _viewModel.LaunchItems)
            {
                System.Diagnostics.Debug.WriteLine($"Loaded item: {item.Name}, Path: {item.ApplicationPath}");
            }
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StatusTextBlock.Text = "更新中...";
                LoadingIndicator.IsActive = true;
                await _viewModel.LoadItemsAsync();
                SetupUI();
                UpdateUIFromViewModel();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "エラー: 更新中");
                StatusTextBlock.Text = "更新中にエラーが発生しました";
            }
            finally
            {
                LoadingIndicator.IsActive = false;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // To be implemented - will open the EditItemDialog
            StatusTextBlock.Text = "この機能はプロトタイプでは未実装です";
        }

        private void EditModeButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ToggleEditModeCommand.Execute(null);
            UpdateUIFromViewModel();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to settings page
            Frame.Navigate(typeof(SettingsPage));
        }

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                _viewModel.SearchText = sender.Text;
                UpdateFilteredItems();
            }
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            _viewModel.SelectedCategory = comboBox?.SelectedItem as string ?? string.Empty;
            UpdateFilteredItems();
        }

        private async void ItemsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is LaunchItemViewModel item)
            {
                try
                {
                    StatusTextBlock.Text = $"{item.Name} を起動中...";
                    LoadingIndicator.IsActive = true;
                    await item.LaunchCommand.ExecuteAsync(null);
                    StatusTextBlock.Text = $"{item.Name} を起動しました";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "エラー: アイテムの起動中");
                    StatusTextBlock.Text = "アイテムの起動中にエラーが発生しました";
                }
                finally
                {
                    LoadingIndicator.IsActive = false;
                }
            }
        }

        private void UpdateUIFromViewModel()
        {
            // Update status
            StatusTextBlock.Text = _viewModel.StatusMessage;
            LoadingIndicator.IsActive = _viewModel.IsLoading;

            // Update edit mode
            EditModeButton.Label = _viewModel.IsEditMode ? "表示モード" : "編集モード";
            EditModeButton.Icon = new SymbolIcon(_viewModel.IsEditMode ? Symbol.View : Symbol.Edit);

            // Update items source (refresh filtered items)
            UpdateFilteredItems();
        }

        private void UpdateFilteredItems()
        {
            // Update GridView
            var filteredItems = _viewModel.FilteredItems?.ToList();
            System.Diagnostics.Debug.WriteLine($"Filtered items count: {filteredItems?.Count ?? 0}");
            
            ItemsGridView.ItemsSource = null;
            ItemsGridView.ItemsSource = filteredItems;
        }
    }
}
