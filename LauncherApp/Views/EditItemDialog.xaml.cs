using LauncherApp.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace LauncherApp.Views
{
    public sealed partial class EditItemDialog : ContentDialog
    {
        private LaunchItem _item;

        public EditItemDialog()
        {
            InitializeComponent();
            _item = new LaunchItem();
        }

        public EditItemDialog(LaunchItem item)
        {
            InitializeComponent();
            _item = item;
            
            // Fill the form with existing data
            NameTextBox.Text = item.Name;
            IconPathTextBox.Text = item.IconPath;
            ApplicationPathTextBox.Text = item.ApplicationPath;
            CommandLineArgsTextBox.Text = item.CommandLineArgs;
            WorkingDirectoryTextBox.Text = item.WorkingDirectory;
            CategoryTextBox.Text = item.Category;
            DescriptionTextBox.Text = item.Description;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                args.Cancel = true;
                _ = ContentDialogHelper.ShowErrorAsync("エラー", "アプリケーション名を入力してください。");
                return;
            }

            if (string.IsNullOrWhiteSpace(ApplicationPathTextBox.Text))
            {
                args.Cancel = true;
                _ = ContentDialogHelper.ShowErrorAsync("エラー", "実行ファイルのパスを指定してください。");
                return;
            }

            // Update the item with form values
            _item.Name = NameTextBox.Text.Trim();
            _item.IconPath = IconPathTextBox.Text.Trim();
            _item.ApplicationPath = ApplicationPathTextBox.Text.Trim();
            _item.CommandLineArgs = CommandLineArgsTextBox.Text.Trim();
            _item.WorkingDirectory = WorkingDirectoryTextBox.Text.Trim();
            _item.Category = CategoryTextBox.Text.Trim();
            _item.Description = DescriptionTextBox.Text.Trim();

            // Set the updated item as the dialog result
            Tag = _item;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Cancel - do nothing
        }

        private async void BrowseIconButton_Click(object sender, RoutedEventArgs e)
        {
            // This is a prototype, so we'll just show a message
            await ContentDialogHelper.ShowErrorAsync("未実装", "この機能はプロトタイプでは未実装です。");
        }

        private async void BrowseAppButton_Click(object sender, RoutedEventArgs e)
        {
            // This is a prototype, so we'll just show a message
            await ContentDialogHelper.ShowErrorAsync("未実装", "この機能はプロトタイプでは未実装です。");
        }

        private async void BrowseWorkingDirButton_Click(object sender, RoutedEventArgs e)
        {
            // This is a prototype, so we'll just show a message
            await ContentDialogHelper.ShowErrorAsync("未実装", "この機能はプロトタイプでは未実装です。");
        }
    }

    // Helper class for showing dialogs in the context of the dialog itself
    internal static class ContentDialogHelper
    {
        public static async System.Threading.Tasks.Task<ContentDialogResult> ShowErrorAsync(string title, string message)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                PrimaryButtonText = "OK",
                XamlRoot = App.MainWindow.Content.XamlRoot
            };

            return await dialog.ShowAsync();
        }
    }
}
