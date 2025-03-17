using LauncherApp.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace LauncherApp.Services
{
    public class ProcessLauncherService : ILauncherService
    {
        private readonly ILogger<ProcessLauncherService> _logger;

        public ProcessLauncherService(ILogger<ProcessLauncherService> logger)
        {
            _logger = logger;
        }

        public async Task<LaunchResult> LaunchApplicationAsync(LaunchItem item)
        {
            if (string.IsNullOrEmpty(item.ApplicationPath))
            {
                return new LaunchResult(false, "アプリケーションのパスが指定されていません。");
            }

            if (!File.Exists(item.ApplicationPath))
            {
                return new LaunchResult(false, $"指定されたアプリケーションが見つかりません: {item.ApplicationPath}");
            }

            try
            {
                _logger.LogInformation("起動: {AppName}, パス: {AppPath}", item.Name, item.ApplicationPath);

                var startInfo = new ProcessStartInfo
                {
                    FileName = item.ApplicationPath,
                    Arguments = item.CommandLineArgs,
                    UseShellExecute = true
                };

                if (!string.IsNullOrEmpty(item.WorkingDirectory) && Directory.Exists(item.WorkingDirectory))
                {
                    startInfo.WorkingDirectory = item.WorkingDirectory;
                }

                var process = Process.Start(startInfo);
                
                if (process == null)
                {
                    return new LaunchResult(false, "プロセスの起動に失敗しました。");
                }

                // 非同期操作をシミュレート（実際のプロセスの終了を待つわけではない）
                await Task.Delay(100);

                return new LaunchResult(true, $"{item.Name} を起動しました。");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "アプリケーション起動エラー: {AppName}, パス: {AppPath}", item.Name, item.ApplicationPath);
                return new LaunchResult(false, $"アプリケーション起動エラー: {ex.Message}", ex);
            }
        }
    }
}
