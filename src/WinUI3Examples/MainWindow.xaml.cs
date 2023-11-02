using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using WinUI3Examples.Pages;
using WinUI3Examples.Services;

namespace WinUI3Examples;

public sealed partial class MainWindow : Window, INavigationService
{
    private List<NavigationViewItem> _navigationViewItems;
    private List<NavigationViewItem> navigationViewItems
    {
        get
        {
            if (_navigationViewItems == null)
            {
                _navigationViewItems = new List<NavigationViewItem>();
                var items = navigationView.MenuItems.Select(i => (NavigationViewItem)i).ToList();
                items.AddRange(navigationView.FooterMenuItems.Select(i => (NavigationViewItem)i));
                _navigationViewItems.AddRange(items);

                foreach (NavigationViewItem mainItem in items)
                    _navigationViewItems.AddRange(mainItem.MenuItems.Select(i => (NavigationViewItem)i));
            }

            return _navigationViewItems;
        }
    }

    public MainWindow()
    {
        InitializeComponent();

        ExtendsContentIntoTitleBar = true;
        SetTitleBar(appTitleBar);
    }

    private void OnWindowActivated(object sender, WindowActivatedEventArgs args)
    {
        if (args.WindowActivationState == WindowActivationState.Deactivated)
            appTitle.Foreground = (SolidColorBrush)App.Current.Resources["WindowCaptionForegroundDisabled"];
        else
            appTitle.Foreground = (SolidColorBrush)App.Current.Resources["WindowCaptionForeground"];
    }

    private void OnNavigationSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected)
            Navigate(typeof(SettingsPage));
        else
            Navigate(Type.GetType(args.SelectedItemContainer.Tag?.ToString()));
    }

    public void Navigate(Type pageType, object targetPageArgs = null, NavigationTransitionInfo transInfo = null)
    {
        rootFrame.Navigate(pageType, targetPageArgs, transInfo);

        if (pageType == typeof(SettingsPage))
        {
            navigationView.Header = "Settings";
            return;
        }

        var item = GetNavigationViewItemForPage(pageType);

        if (item == null)
        {
            Navigate(typeof(WelcomePage));
            return;
        }

        navigationView.Header = item.Content.ToString();
        navigationView.SelectedItem = item;
    }

    private NavigationViewItem GetNavigationViewItemForPage(Type pageType)
    {
        return navigationViewItems.FirstOrDefault(i => i.Tag?.ToString() == pageType.FullName);
    }

    private async void myButton_Click(object sender, RoutedEventArgs e)
    {
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Current.StartupWindow);
        var picker = new Windows.Storage.Pickers.FolderPicker();

        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

        picker.FileTypeFilter.Add("*");
        var file = await picker.PickSingleFolderAsync();


    }
}
