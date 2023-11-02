using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace WinUI3Examples.ViewModels.Dialogs;

public class ContentDialogViewModel : ObservableObject
{
    private string outputText = "Click the button";
    public string OutputText
    {
        get => outputText;
        set => SetProperty(ref outputText, value);
    }

    public AsyncRelayCommand ShowDialogCommand { get; set; }

    public ContentDialogViewModel()
    {
        ShowDialogCommand = new AsyncRelayCommand(ShowDialog, () => true);
    }

    private async Task ShowDialog()
    {
        OutputText = "Wailing for result";

        var dialog = new ContentDialog
        {
            Title = "My little dialog",
            Content = "Do you like my little dialog window?",
            CloseButtonText = "No",
            PrimaryButtonText = "Yes"
        };

        dialog.XamlRoot = App.Current.MainRoot;
        var result = await dialog.ShowAsync();

        OutputText = result switch
        {
            ContentDialogResult.None => "You selected No",
            ContentDialogResult.Primary => "You selected Yes",
            ContentDialogResult.Secondary => "Not sure how this got selected",
            _ => "Hmm...not sure what happened",
        };
    }
}
