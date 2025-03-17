using LauncherApp.Models;
using System;
using System.Threading.Tasks;

namespace LauncherApp.Services
{
    public interface ILauncherService
    {
        Task<LaunchResult> LaunchApplicationAsync(LaunchItem item);
    }

    public record LaunchResult(bool Success, string Message, Exception? Exception = null);
}
