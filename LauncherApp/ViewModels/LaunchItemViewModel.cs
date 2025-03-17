using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LauncherApp.Models;
using LauncherApp.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LauncherApp.ViewModels
{
    public partial class LaunchItemViewModel : ObservableObject
    {
        private readonly ILauncherService _launcherService;
        private readonly IDialogService _dialogService;
        private readonly ILogger<LaunchItemViewModel> _logger;

        public LaunchItemViewModel(
            LaunchItem model,
            ILauncherService launcherService,
            IDialogService dialogService,
            ILogger<LaunchItemViewModel> logger)
        {
            Model = model;
            _launcherService = launcherService;
            _dialogService = dialogService;
            _logger = logger;
        }

        public LaunchItem Model { get; }

        public string Id => Model.Id;
        public string Name => Model.Name;
        public string IconPath => Model.IconPath;
        public string ApplicationPath => Model.ApplicationPath;
        public string CommandLineArgs => Model.CommandLineArgs;
        public string WorkingDirectory => Model.WorkingDirectory;
        public int DisplayOrder => Model.DisplayOrder;
        public string Category => Model.Category;
        public string Description => Model.Description;
        public DateTime LastLaunched => Model.LastLaunched;
        public int LaunchCount => Model.LaunchCount;
        
        [ObservableProperty]
        private bool _isEditable;

        [RelayCommand]
        private async Task LaunchAsync()
        {
            try
            {
                _logger.LogInformation("Launching application: {Name}", Name);
                var result = await _launcherService.LaunchApplicationAsync(Model);

                if (!result.Success)
                {
                    await _dialogService.ShowErrorAsync("起動エラー", result.Message);
                }
                else
                {
                    // Update statistics
                    Model.LastLaunched = DateTime.Now;
                    Model.LaunchCount++;
                    OnPropertyChanged(nameof(LastLaunched));
                    OnPropertyChanged(nameof(LaunchCount));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error launching application: {Name}", Name);
                await _dialogService.ShowErrorAsync("起動エラー", $"アプリケーションの起動中にエラーが発生しました: {ex.Message}");
            }
        }
    }
}
