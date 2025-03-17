using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace LauncherApp.Services
{
    public interface IDialogService
    {
        Task<ContentDialogResult> ShowConfirmationAsync(string title, string message);
        Task ShowErrorAsync(string title, string message);
        Task<T?> ShowDialogAsync<T>(ContentDialog dialog) where T : class;
    }
}
