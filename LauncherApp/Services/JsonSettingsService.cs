using LauncherApp.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace LauncherApp.Services
{
    public class JsonSettingsService : ISettingsService
    {
        private readonly ILogger<JsonSettingsService> _logger;
        private readonly string _launchItemsFileName = "launchItems.json";
        private readonly string _appSettingsFileName = "appSettings.json";
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true
        };

        public JsonSettingsService(ILogger<JsonSettingsService> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<LaunchItem>> LoadLaunchItemsAsync()
        {
            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var file = await localFolder.TryGetItemAsync(_launchItemsFileName) as StorageFile;

                if (file == null)
                {
                    _logger.LogInformation("Launch items file not found. Loading sample data.");
                    return await LoadSampleLaunchItemsAsync();
                }

                var json = await FileIO.ReadTextAsync(file);
                var items = JsonSerializer.Deserialize<List<LaunchItem>>(json, _jsonOptions);

                _logger.LogInformation("Loaded {Count} launch items", items?.Count ?? 0);
                return items ?? new List<LaunchItem>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading launch items");
                return new List<LaunchItem>();
            }
        }

        public async Task SaveLaunchItemsAsync(IEnumerable<LaunchItem> items)
        {
            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var file = await localFolder.CreateFileAsync(_launchItemsFileName, CreationCollisionOption.ReplaceExisting);

                var json = JsonSerializer.Serialize(items, _jsonOptions);
                await FileIO.WriteTextAsync(file, json);

                _logger.LogInformation("Saved launch items successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving launch items");
                throw;
            }
        }

        public async Task<AppSettings> LoadAppSettingsAsync()
        {
            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var file = await localFolder.TryGetItemAsync(_appSettingsFileName) as StorageFile;

                if (file == null)
                {
                    _logger.LogInformation("App settings file not found. Returning default settings.");
                    return new AppSettings();
                }

                var json = await FileIO.ReadTextAsync(file);
                var settings = JsonSerializer.Deserialize<AppSettings>(json, _jsonOptions);

                _logger.LogInformation("Loaded app settings");
                return settings ?? new AppSettings();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading app settings");
                return new AppSettings();
            }
        }

        public async Task SaveAppSettingsAsync(AppSettings settings)
        {
            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var file = await localFolder.CreateFileAsync(_appSettingsFileName, CreationCollisionOption.ReplaceExisting);

                var json = JsonSerializer.Serialize(settings, _jsonOptions);
                await FileIO.WriteTextAsync(file, json);

                _logger.LogInformation("Saved app settings successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving app settings");
                throw;
            }
        }

        /// <summary>
        /// Load sample data for demonstration purposes
        /// </summary>
        private async Task<IEnumerable<LaunchItem>> LoadSampleLaunchItemsAsync()
        {
            try
            {
                // Load sample data from embedded resource or file
                var sampleFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///SampleData/sample_items.json"));
                var json = await FileIO.ReadTextAsync(sampleFile);
                
                var items = JsonSerializer.Deserialize<List<LaunchItem>>(json, _jsonOptions);
                _logger.LogInformation("Loaded {Count} sample items", items?.Count ?? 0);
                
                return items ?? new List<LaunchItem>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading sample data");
                return new List<LaunchItem>
                {
                    new LaunchItem
                    {
                        Name = "メモ帳",
                        ApplicationPath = "C:\\Windows\\notepad.exe",
                        Category = "Windows標準",
                        Description = "テキストエディタ"
                    }
                };
            }
        }
    }
}
