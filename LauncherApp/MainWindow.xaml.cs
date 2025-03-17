using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;
using WinRT.Interop;

namespace LauncherApp
{
    public sealed partial class MainWindow : Window
    {
        private AppWindow _appWindow;

        public MainWindow()
        {
            InitializeComponent();

            // Set window size
            SetWindowSize(1200, 800);

            // Set custom title bar
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            // Get AppWindow object
            // 修正: As<T>() の呼び出し方法を変更
            var windowNative = WinRT.CastExtensions.As<IWindowNative>(this);
            var windowHandle = windowNative.WindowHandle;
            var windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
            _appWindow = AppWindow.GetFromWindowId(windowId);

            // Set window title
            Title = "業務ランチャー";
        }

        private void SetWindowSize(int width, int height)
        {
            // 修正: As<T>() の呼び出し方法を変更
            var windowNative = WinRT.CastExtensions.As<IWindowNative>(this);
            var windowHandle = windowNative.WindowHandle;
            var windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
            _appWindow = AppWindow.GetFromWindowId(windowId);

            var size = new Windows.Graphics.SizeInt32();
            size.Width = width;
            size.Height = height;
            _appWindow.Resize(size);
        }
    }

    // Interface for getting the window handle
    internal interface IWindowNative
    {
        IntPtr WindowHandle { get; }
    }
}
