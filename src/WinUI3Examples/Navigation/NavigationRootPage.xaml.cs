using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using WinUI3Examples.Pages;

namespace WinUI3Examples.Navigation;

public sealed partial class NavigationRootPage : Page
{
    public NavigationRootPage()
    {
        InitializeComponent();

        var window = App.Current.StartupWindow;
        window.ExtendsContentIntoTitleBar = true;
        window.SetTitleBar(AppTitleBar);
    }

    public void Navigate(Type pageType, object targetPageArgs = null, NavigationTransitionInfo transInfo = null)
    {
        NavigationRootPageArgs args = new() { NavigationRootPage = this, Parameter = targetPageArgs };
        rootFrame.Navigate(pageType, args, transInfo);
    }

    private void OnNavigationSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected)
        {
            if (rootFrame.CurrentSourcePageType != typeof(SettingsPage))
                Navigate(typeof(SettingsPage));
        }
    }
}
