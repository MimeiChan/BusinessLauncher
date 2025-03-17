using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Threading.Tasks;

namespace LauncherApp.Helpers.Extensions
{
    public static class UIExtensions
    {
        /// <summary>
        /// Finds a child control in the visual tree
        /// </summary>
        public static T FindChild<T>(this DependencyObject parent, string childName = null) where T : DependencyObject
        {
            if (parent == null)
                return null;

            T foundChild = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T typedChild)
                {
                    if (childName == null || (child is FrameworkElement element && element.Name == childName))
                    {
                        foundChild = typedChild;
                        break;
                    }
                }

                foundChild = FindChild<T>(child, childName);
                if (foundChild != null)
                    break;
            }

            return foundChild;
        }

        /// <summary>
        /// Shows a simple content dialog
        /// </summary>
        public static async Task<ContentDialogResult> ShowDialogAsync(this Page page, string title, string content, string primaryButtonText = "OK", string secondaryButtonText = null)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                PrimaryButtonText = primaryButtonText,
                SecondaryButtonText = secondaryButtonText,
                XamlRoot = page.XamlRoot
            };

            return await dialog.ShowAsync();
        }

        /// <summary>
        /// Updates the element theme for a specific element and its children
        /// </summary>
        public static void SetElementTheme(this FrameworkElement element, ElementTheme theme)
        {
            if (element is null)
                return;

            element.RequestedTheme = theme;
        }

        /// <summary>
        /// Gets the actual theme of the application
        /// </summary>
        public static ElementTheme GetActualTheme(this Application app)
        {
            if (app is null)
                return ElementTheme.Default;

            if (app.Resources.ThemeDictionaries.TryGetValue("Default", out var themeValue))
            {
                return ElementTheme.Default;
            }
            else if (app.Resources.ThemeDictionaries.TryGetValue("Light", out themeValue))
            {
                return ElementTheme.Light;
            }
            else if (app.Resources.ThemeDictionaries.TryGetValue("Dark", out themeValue))
            {
                return ElementTheme.Dark;
            }

            return ElementTheme.Default;
        }
    }
}
