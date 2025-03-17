using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace LauncherApp.Services
{
    public class DialogService : IDialogService
    {
        public async Task<ContentDialogResult> ShowConfirmationAsync(string title, string message)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                PrimaryButtonText = "OK",
                SecondaryButtonText = "キャンセル",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = App.MainWindow.Content.XamlRoot
            };

            return await dialog.ShowAsync();
        }

        public async Task ShowErrorAsync(string title, string message)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                PrimaryButtonText = "OK",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = App.MainWindow.Content.XamlRoot
            };

            await dialog.ShowAsync();
        }

        public async Task<T?> ShowDialogAsync<T>(ContentDialog dialog) where T : class
        {
            dialog.XamlRoot = App.MainWindow.Content.XamlRoot;
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                return dialog.Tag as T;
            }

            return null;
        }
    }
}
