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
                // エラー時にはサンプルデータを読み込むよう変更
                return await LoadSampleLaunchItemsAsync();
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
        /// サンプルデータの読み込み方法を修正
        /// </summary>
        private async Task<IEnumerable<LaunchItem>> LoadSampleLaunchItemsAsync()
        {
            List<LaunchItem> sampleItems = new List<LaunchItem>();
            
            try
            {
                // 複数の方法を試して、サンプルデータを読み込む
                string jsonContent = null;
                
                // 方法1: ms-appx URI を使用
                try
                {
                    var uri = new Uri("ms-appx:///SampleData/sample_items.json");
                    var file = await StorageFile.GetFileFromApplicationUriAsync(uri);
                    jsonContent = await FileIO.ReadTextAsync(file);
                    _logger.LogInformation("サンプルデータをms-appx URIから読み込みました");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ms-appx URIからの読み込みに失敗しました");
                }
                
                // 方法2: インストールフォルダから直接読み込み
                if (string.IsNullOrEmpty(jsonContent))
                {
                    try
                    {
                        var installFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                        var file = await installFolder.GetFileAsync("SampleData\\sample_items.json");
                        jsonContent = await FileIO.ReadTextAsync(file);
                        _logger.LogInformation("サンプルデータをインストールフォルダから読み込みました");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "インストールフォルダからの読み込みに失敗しました");
                    }
                }
                
                // 方法3: 実行フォルダから読み込み
                if (string.IsNullOrEmpty(jsonContent))
                {
                    try
                    {
                        string executablePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                        string filePath = Path.Combine(executablePath, "SampleData", "sample_items.json");
                        jsonContent = File.ReadAllText(filePath);
                        _logger.LogInformation("サンプルデータを実行フォルダから読み込みました: {path}", filePath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "実行フォルダからの読み込みに失敗しました");
                    }
                }

                // データをデシリアライズ
                if (!string.IsNullOrEmpty(jsonContent))
                {
                    sampleItems = JsonSerializer.Deserialize<List<LaunchItem>>(jsonContent, _jsonOptions);
                    _logger.LogInformation("サンプルデータを正常に読み込みました: {count} アイテム", sampleItems?.Count ?? 0);
                }
                else
                {
                    // どの方法でも読み込めなかった場合は、ハードコードされたサンプルデータを使用
                    _logger.LogWarning("ファイルからサンプルデータを読み込めなかったため、デフォルトデータを使用します");
                    sampleItems = CreateDefaultSampleItems();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "サンプルデータ読み込み中にエラーが発生しました");
                sampleItems = CreateDefaultSampleItems();
            }
            
            return sampleItems;
        }
        
        /// <summary>
        /// デフォルトのサンプルアイテムを作成
        /// </summary>
        private List<LaunchItem> CreateDefaultSampleItems()
        {
            return new List<LaunchItem>
            {
                new LaunchItem
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "メモ帳",
                    IconPath = "",
                    ApplicationPath = "C:\\Windows\\notepad.exe",
                    CommandLineArgs = "",
                    WorkingDirectory = "C:\\Windows",
                    DisplayOrder = 0,
                    Category = "Windows標準",
                    Description = "テキストエディタ",
                    LastLaunched = DateTime.Now.AddDays(-1),
                    LaunchCount = 5
                },
                new LaunchItem
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "電卓",
                    IconPath = "",
                    ApplicationPath = "C:\\Windows\\System32\\calc.exe",
                    CommandLineArgs = "",
                    WorkingDirectory = "C:\\Windows\\System32",
                    DisplayOrder = 1,
                    Category = "Windows標準",
                    Description = "計算機",
                    LastLaunched = DateTime.Now.AddDays(-2),
                    LaunchCount = 8
                },
                new LaunchItem
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "コマンドプロンプト",
                    IconPath = "",
                    ApplicationPath = "C:\\Windows\\System32\\cmd.exe",
                    CommandLineArgs = "",
                    WorkingDirectory = "C:\\Windows\\System32",
                    DisplayOrder = 2,
                    Category = "開発ツール",
                    Description = "コマンドライン",
                    LastLaunched = DateTime.Now.AddHours(-5),
                    LaunchCount = 12
                }
            };
        }
    }
}
