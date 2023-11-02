using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;
using Windows.Foundation;

namespace WinUI3Examples.Helpers;

public class ControlExampleSubstitution : DependencyObject
{
    public event TypedEventHandler<ControlExampleSubstitution, object> ValueChanged;

    public string Key { get; set; }

    private object _value = null;
    public object Value
    {
        get => _value;
        set
        {
            _value = value;
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool isEnabled = true;
    public bool IsEnabled
    {
        get => isEnabled;
        set
        {
            isEnabled = value;
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public string ValueAsString()
    {
        if (!isEnabled)
            return string.Empty;

        var value = Value;

        if (value is SolidColorBrush brush)
            value = brush.Color;

        return value == null ? string.Empty : value.ToString();
    }
}
