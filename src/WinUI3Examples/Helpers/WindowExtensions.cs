using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using Windows.Foundation;
using Windows.Graphics;
using WinRT.Interop;

namespace WinUI3Examples.Helpers;
public static class WindowExtensions
{
    public static AppWindow GetAppWindow(this Window window) => GetAppWindowFromWindowHandle(window.GetWindowHandle());

    public static AppWindow GetAppWindowFromWindowHandle(IntPtr hwnd)
    {
        if (hwnd == IntPtr.Zero)
            throw new ArgumentNullException(nameof(hwnd));

        var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
        return AppWindow.GetFromWindowId(windowId);
    }

    public static IntPtr GetWindowHandle(this Window window)
    {
        if (window is null)
            throw new ArgumentNullException(nameof(window));

        return WindowNative.GetWindowHandle(window);
    }

    public static void SetWindowSize(this Window window, double width, double height)
        => window.GetAppWindow().Resize(new SizeInt32((int)width, (int)height));

    public static void SetWindowSize(this Window window, Size size)
        => window.GetAppWindow().Resize(new SizeInt32((int)size.Width, (int)size.Height));
}
