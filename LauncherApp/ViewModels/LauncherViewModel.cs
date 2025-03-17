using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LauncherApp.Models;
using LauncherApp.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LauncherApp.ViewModels
{
    public partial class LauncherViewModel : ObservableObject
    {
        private readonly ISettingsService _settingsService;
        private readonly ILauncherService _launcherService;
        private readonly IDialogService _dialogService;
        private readonly ILogger<LauncherViewModel> _logger;

        [ObservableProperty]
        private ObservableCollection<LaunchItemViewModel> _launchItems = new();

        [ObservableProperty]
        private bool _isEditMode;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private string _selectedCategory = string.Empty;

        [ObservableProperty]
        private ObservableCollection<string> _categories = new();

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        public LauncherViewModel(
            ISettingsService settingsService,
            ILauncherService launcherService,
            IDialogService dialogService,
            ILogger<LauncherViewModel> logger)
        {
            _settingsService = settingsService;
            _launcherService = launcherService;
            _dialogService = dialogService;
            _logger = logger;
        }

        [RelayCommand]
        public async Task LoadItemsAsync() // private から public に変更
        {
            try
            {
                IsLoading = true;
                StatusMessage = "アイテムを読み込んでいます...";

                var items = await _settingsService.LoadLaunchItemsAsync();
                
                LaunchItems.Clear();
                
                foreach (var item in items.OrderBy(i => i.DisplayOrder))
                {
                    var vmItem = new LaunchItemViewModel(
                        item,
                        _launcherService,
                        _dialogService,
                        App.Current.Services.GetService(typeof(ILogger<LaunchItemViewModel>)) as ILogger<LaunchItemViewModel>);
                    
                    LaunchItems.Add(vmItem);
                }

                // Update categories
                UpdateCategories();

                StatusMessage = $"{LaunchItems.Count} アイテムを読み込みました";
                _logger.LogInformation("Loaded {Count} items", LaunchItems.Count);
            }
            catch (Exception ex)
            {
                StatusMessage = "アイテムの読み込み中にエラーが発生しました";
                _logger.LogError(ex, "Error loading items");
                await _dialogService.ShowErrorAsync("エラー", $"アイテムの読み込み中にエラーが発生しました: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task SaveItemsAsync() // private から public に変更
        {
            try
            {
                IsLoading = true;
                StatusMessage = "アイテムを保存しています...";

                var items = LaunchItems.Select(vm => vm.Model).ToList();
                await _settingsService.SaveLaunchItemsAsync(items);

                StatusMessage = $"{items.Count} アイテムを保存しました";
                _logger.LogInformation("Saved {Count} items", items.Count);
            }
            catch (Exception ex)
            {
                StatusMessage = "アイテムの保存中にエラーが発生しました";
                _logger.LogError(ex, "Error saving items");
                await _dialogService.ShowErrorAsync("エラー", $"アイテムの保存中にエラーが発生しました: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void ToggleEditMode()
        {
            IsEditMode = !IsEditMode;

            foreach (var item in LaunchItems)
            {
                item.IsEditable = IsEditMode;
            }

            StatusMessage = IsEditMode ? "編集モードに切り替えました" : "表示モードに切り替えました";
        }

        private void UpdateCategories()
        {
            var categorySet = new HashSet<string>();
            
            foreach (var item in LaunchItems)
            {
                if (!string.IsNullOrEmpty(item.Category))
                {
                    categorySet.Add(item.Category);
                }
            }

            Categories.Clear();
            Categories.Add(string.Empty); // "All" category
            
            foreach (var category in categorySet.OrderBy(c => c))
            {
                Categories.Add(category);
            }
        }

        // Filter items based on search text and selected category
        public IEnumerable<LaunchItemViewModel> FilteredItems
        {
            get
            {
                var query = LaunchItems.AsEnumerable();

                if (!string.IsNullOrEmpty(SearchText))
                {
                    var searchText = SearchText.ToLower();
                    query = query.Where(item =>
                        item.Name.ToLower().Contains(searchText) ||
                        item.Description.ToLower().Contains(searchText));
                }

                if (!string.IsNullOrEmpty(SelectedCategory))
                {
                    query = query.Where(item => item.Category == SelectedCategory);
                }

                return query;
            }
        }
    }
}
