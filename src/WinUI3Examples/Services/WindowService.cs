using Microsoft.UI.Xaml;
using System.Collections.Generic;
using System.Linq;

namespace WinUI3Examples.Services;

public interface IWindowService
{
    List<Window> ActiveWindows { get; }

    Window CreateWindow();
    T CreateWindow<T>() where T : Window, new();
    void AddWindow(Window window);
    Window GetWindowForElement(UIElement element);
}

public class WindowService : IWindowService
{
    private List<Window> activeWindows = new();
    public List<Window> ActiveWindows => activeWindows;

    public Window CreateWindow()
    {
        var window = new Window();

        window.Closed += (s, e) => activeWindows.Remove(window);
        activeWindows.Add(window);

        return window;
    }

    public T CreateWindow<T>() where T : Window, new()
    {
        var window = new T();

        window.Closed += (s, e) => activeWindows.Remove(window);
        activeWindows.Add(window);

        return window;
    }

    public void AddWindow(Window window)
    {
        window.Closed += (s, e) => activeWindows.Remove(window);
        activeWindows.Add(window);
    }

    public Window GetWindowForElement(UIElement element)
    {
        if (element.XamlRoot == null)
            return null;

        return activeWindows.FirstOrDefault(w => w.Content.XamlRoot == element.XamlRoot);
    }
}
