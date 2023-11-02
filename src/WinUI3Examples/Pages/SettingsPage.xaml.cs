using Microsoft.UI.Xaml.Controls;
using System;
using WinUI3Examples.ViewModels;

namespace WinUI3Examples.Pages;

public sealed partial class SettingsPage : Page
{
    private readonly ISettingsPageViewModel viewModel;

    public SettingsPage()
    {
        InitializeComponent();

        viewModel = App.Current.GetService<ISettingsPageViewModel>() ??
            throw new ArgumentNullException("ISettingsPageViewModel");
    }
}
