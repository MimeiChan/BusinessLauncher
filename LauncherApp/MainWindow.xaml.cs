using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;
using System.Runtime.InteropServices;
using WinRT;
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
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            _appWindow = AppWindow.GetFromWindowId(windowId);

            // Set window title
            Title = "業務ランチャー";
        }

        private void SetWindowSize(int width, int height)
        {
            // Get handle to the core window
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            _appWindow = AppWindow.GetFromWindowId(windowId);

            var size = new Windows.Graphics.SizeInt32();
            size.Width = width;
            size.Height = height;
            _appWindow.Resize(size);
        }
    }

    // ネイティブウィンドウハンドル取得用のユーティリティクラス
    public static class WindowNative
    {
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto, PreserveSig = true, SetLastError = false)]
        public static extern IntPtr GetActiveWindow();

        public static IntPtr GetWindowHandle(object window)
        {
            var windowsBaseWindow = window as Microsoft.UI.Xaml.Window;
            if (windowsBaseWindow == null)
                return IntPtr.Zero;

            // Handle is an IntPtr value (HWND) cast to a int value
            dynamic interopWindowHandle = TypeHelper.As<IWindowNative>(windowsBaseWindow);
            return interopWindowHandle.WindowHandle;
        }
    }

    // 型ヘルパー (WinRT統合用)
    internal static class TypeHelper
    {
        public static T As<T>(object obj)
        {
            return (T)obj.As<T>();
        }
    }

    // COM インターフェース定義
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("EECDBF0E-BAE9-4CB6-A68E-9598E1CB57BB")]
    internal interface IWindowNative
    {
        IntPtr WindowHandle { get; }
    }
}
