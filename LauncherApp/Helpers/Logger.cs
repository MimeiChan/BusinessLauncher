using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace LauncherApp.Helpers
{
    /// <summary>
    /// ファイルベースのログ記録サポート機能を提供するヘルパークラス
    /// </summary>
    public static class Logger
    {
        private const string LogFileName = "launcher_app.log";
        private static readonly Queue<string> _pendingLogs = new();
        private static bool _isWriting = false;

        /// <summary>
        /// アプリケーションログファイルにログエントリを追加します
        /// </summary>
        public static void Log(LogLevel level, string message, Exception? exception = null)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var logEntry = $"[{timestamp}] [{level}] {message}";
            
            if (exception != null)
            {
                logEntry += $"\r\nException: {exception.Message}\r\n{exception.StackTrace}";
            }

            lock (_pendingLogs)
            {
                _pendingLogs.Enqueue(logEntry);
            }

            _ = WriteLogsAsync();
        }

        /// <summary>
        /// キュー内のログを非同期的にファイルに書き込みます
        /// </summary>
        private static async Task WriteLogsAsync()
        {
            if (_isWriting)
            {
                return;
            }

            _isWriting = true;

            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var logFile = await localFolder.CreateFileAsync(LogFileName, CreationCollisionOption.OpenIfExists);

                using var stream = await logFile.OpenAsync(FileAccessMode.ReadWrite);
                stream.Seek(stream.Size);
                using var writer = new StreamWriter(stream.AsStream());

                while (true)
                {
                    string? logEntry = null;

                    lock (_pendingLogs)
                    {
                        if (_pendingLogs.Count > 0)
                        {
                            logEntry = _pendingLogs.Dequeue();
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (logEntry != null)
                    {
                        await writer.WriteLineAsync(logEntry);
                    }
                }
            }
            catch
            {
                // ログ書き込み失敗時は静かに失敗
                // 一般的にはログシステムのエラーをユーザーに表示しない
            }
            finally
            {
                _isWriting = false;
            }

            // もしキューに新しいエントリが追加されていた場合は再度書き込み
            lock (_pendingLogs)
            {
                if (_pendingLogs.Count > 0)
                {
                    _ = WriteLogsAsync();
                }
            }
        }
    }
}
