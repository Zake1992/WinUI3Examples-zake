using Microsoft.UI.Xaml;

namespace WinUI3Examples.Services;

public interface IThemeService
{
    ElementTheme RootTheme { get; set; }
    void Initialize();
}

public class ThemeService : IThemeService
{
    private readonly IWindowService windowSvc;
    private readonly ISettingsService settingsSvc;

    public ThemeService(IWindowService windowService, ISettingsService settingsService)
    {
        windowSvc = windowService;
        settingsSvc = settingsService;
    }

    public ElementTheme RootTheme
    {
        get
        {
            foreach (var window in windowSvc.ActiveWindows)
                if (window.Content is FrameworkElement rootElement)
                    return rootElement.RequestedTheme;

            return ElementTheme.Default;
        }
        set
        {
            foreach (var window in windowSvc.ActiveWindows)
                if (window.Content is FrameworkElement rootElement)
                    rootElement.RequestedTheme = value;

            settingsSvc.SelectedAppTheme = value;
        }
    }

    public void Initialize()
    {
        try
        {
            RootTheme = settingsSvc.SelectedAppTheme;
        }
        catch
        {
            RootTheme = ElementTheme.Default;
        }
    }
}
