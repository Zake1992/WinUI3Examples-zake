using Microsoft.UI.Xaml.Controls;
using WinUI3Examples.ViewModels.Dialogs;

namespace WinUI3Examples.Pages.Dialogs;

public sealed partial class ContentDialogPage : Page
{
    private readonly ContentDialogViewModel viewModel;

    public ContentDialogPage()
    {
        InitializeComponent();
        viewModel = new ContentDialogViewModel();
    }
}
