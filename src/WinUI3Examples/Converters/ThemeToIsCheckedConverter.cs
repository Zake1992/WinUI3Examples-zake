using Microsoft.UI.Xaml.Data;
using System;
using WinUI3Examples.Services;

namespace WinUI3Examples.Converters;

public class ThemeToIsCheckedConverter : IValueConverter
{
    private static IThemeService themeService;
    private static IThemeService ThemeService
    {
        get
        {
            if (themeService == null)
                themeService = App.Current.GetService<IThemeService>();

            return themeService;
        }
    }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return (parameter as string) == ThemeService.RootTheme.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
