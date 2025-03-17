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

            // 先にウィンドウを初期化
            try
            {
                // カスタムタイトルバーを設定
                ExtendsContentIntoTitleBar = true;
                SetTitleBar(AppTitleBar);

                // ウィンドウタイトルを設定
                Title = "業務ランチャー";

                // ウィンドウハンドルを取得 - 標準のWinRT.Interopを使用
                var hwnd = WindowNative.GetWindowHandle(this);
                
                // WindowIdを取得
                var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
                
                // AppWindowオブジェクトを取得
                _appWindow = AppWindow.GetFromWindowId(windowId);
                
                // ウィンドウサイズを設定
                if (_appWindow != null)
                {
                    var size = new Windows.Graphics.SizeInt32
                    {
                        Width = 1200,
                        Height = 800
                    };
                    _appWindow.Resize(size);
                }
            }
            catch (Exception ex)
            {
                // デバッグ用：例外情報を出力
                System.Diagnostics.Debug.WriteLine($"Window initialization failed: {ex.Message}");
            }
        }
    }

    // ウィンドウハンドル取得のためのネイティブメソッド
    public static class WindowNative
    {
        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("EECDBF0E-BAE9-4CB6-A68E-9598E1CB57BB")]
        internal interface IWindowNative
        {
            IntPtr WindowHandle { get; }
        }

        public static IntPtr GetWindowHandle(Window window)
        {
            return ((IWindowNative)window).WindowHandle;
        }
    }
}
