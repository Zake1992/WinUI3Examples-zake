using Microsoft.UI.Xaml;
using System;
using Windows.Foundation;
using Windows.Storage;

namespace WinUI3Examples.Services;

public interface ISettingsService
{
    ElementTheme SelectedAppTheme { get; set; }

    Size AppWindowSize { get; set; }
    string MyString { get; }
    int MyInt { get; }
}

public class SettingsService : ISettingsService
{
    private const string SelectedAppThemeKey = "SelectedAppTheme";
    private const string AppWindowWidth = "AppWindowWidth";
    private const string AppWindowHeight = "AppWindowHeight";

    //
    // Settings from LocalSettings
    //
    public ElementTheme SelectedAppTheme
    {
        get
        {
            try
            {
                return (ElementTheme)Enum.Parse(typeof(ElementTheme), GetSetting(SelectedAppThemeKey)?.ToString());
            }
            catch
            {
                SetSetting(SelectedAppThemeKey, ElementTheme.Default.ToString());
                return ElementTheme.Default;
            }
        }
        set => SetSetting(SelectedAppThemeKey, value.ToString());
    }

    public Size AppWindowSize
    {
        get
        {
            var w = GetSetting<double>(AppWindowWidth);
            var h = GetSetting<double>(AppWindowHeight);

            return new Size(w > 0 ? w : 700, h > 0 ? h : 500);
        }
        set
        {
            SetSetting(AppWindowWidth, value.Width);
            SetSetting(AppWindowHeight, value.Height);
        }
    }

    //
    // Settings from appsettings.json
    //
    public string MyString { get; set; } = string.Empty;

    public int MyInt { get; set; }

    private T GetSetting<T>(string key)
    {
        ApplicationData.Current.LocalSettings.Values.TryGetValue(key, out var value);
        return value is T ? (T)value : default;
    }

    private object GetSetting(string key)
    {
        return ApplicationData.Current.LocalSettings.Values[key];
    }

    private void SetSetting(string key, object value)
    {
        ApplicationData.Current.LocalSettings.Values[key] = value;
    }
}

