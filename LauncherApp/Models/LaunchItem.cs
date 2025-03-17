using System;

namespace LauncherApp.Models
{
    public record LaunchItem
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string IconPath { get; set; } = string.Empty;
        public string ApplicationPath { get; set; } = string.Empty;
        public string CommandLineArgs { get; set; } = string.Empty;
        public string WorkingDirectory { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime LastLaunched { get; set; }
        public int LaunchCount { get; set; }
    }
}
