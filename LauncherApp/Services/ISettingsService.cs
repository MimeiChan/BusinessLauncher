using LauncherApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LauncherApp.Services
{
    public interface ISettingsService
    {
        Task<IEnumerable<LaunchItem>> LoadLaunchItemsAsync();
        Task SaveLaunchItemsAsync(IEnumerable<LaunchItem> items);
        Task<AppSettings> LoadAppSettingsAsync();
        Task SaveAppSettingsAsync(AppSettings settings);
    }
}
