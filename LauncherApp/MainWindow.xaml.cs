using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;
using System.Runtime.InteropServices;
using WinRT.Interop;

namespace LauncherApp
{
    public sealed partial class MainWindow : Window
    {
        private AppWindow _appWindow;

        public MainWindow()
        {
            InitializeComponent();

            // Set custom title bar
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            // Get window handle using the WinRT Interop helper
            IntPtr hWnd = GetWindowHandle();
            
            // Get AppWindow object
            var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            _appWindow = AppWindow.GetFromWindowId(windowId);

            // Set window title
            Title = "業務ランチャー";
            
            // Set window size
            SetWindowSize(1200, 800);
        }

        private IntPtr GetWindowHandle()
        {
            var windowHandle = WindowNative.GetWindowHandle(this);
            return windowHandle;
        }

        private void SetWindowSize(int width, int height)
        {
            if (_appWindow != null)
            {
                var size = new Windows.Graphics.SizeInt32
                {
                    Width = width,
                    Height = height
                };
                _appWindow.Resize(size);
            }
        }
    }

    // P/Invoke for getting the window handle
    internal class WindowNative
    {
        [DllImport("Microsoft.ui.xaml.dll")]
        public static extern IntPtr GetWindowHandle(Window window);
    }
}
