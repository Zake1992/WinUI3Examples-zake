using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using System;
using Windows.ApplicationModel;
using WinUI3Examples.Services;

namespace WinUI3Examples.ViewModels;

public interface ISettingsPageViewModel
{
    string CurrentTheme { get; }
    string AppVersion { get; }
    RelayCommand<string> ThemeSelectedCommand { get; set; }
}

public class SettingsPageViewModel : ObservableObject, ISettingsPageViewModel
{
    private readonly IThemeService themeService;

    public string CurrentTheme => themeService.RootTheme.ToString();

    public string AppVersion
    {
        get
        {
            var v = Package.Current.Id.Version;
            return $"{v.Major}.{v.Minor}.{v.Build}.{v.Revision}";
        }
    }

    public RelayCommand<string> ThemeSelectedCommand { get; set; }

    public SettingsPageViewModel(IThemeService themeService)
    {
        this.themeService = themeService;

        ThemeSelectedCommand = new RelayCommand<string>(
            execute: (s) => themeService.RootTheme = (ElementTheme)Enum.Parse(typeof(ElementTheme), s),
            canExecute: (s) => true);
    }
}
