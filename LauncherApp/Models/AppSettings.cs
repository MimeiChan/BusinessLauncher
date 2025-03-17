using Microsoft.Extensions.Logging;

namespace LauncherApp.Models
{
    public record AppSettings
    {
        public bool DarkMode { get; set; }
        public int GridItemSize { get; set; } = 120;
        public int ColumnsCount { get; set; } = 5;
        public bool ShowCategories { get; set; } = true;
        public bool ShowLastLaunched { get; set; } = false;
        public string SettingsFilePath { get; set; } = string.Empty;
        public LogLevel LogLevel { get; set; } = LogLevel.Information;
    }
}
