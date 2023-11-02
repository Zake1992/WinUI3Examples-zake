using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using System;
using System.Collections.Generic;
using WinUI3Examples.Helpers;

namespace WinUI3Examples.Controls;

[ContentProperty(Name = "Example")]
public sealed partial class ControlExample : UserControl
{
    public static readonly DependencyProperty HeaderTextProperty =
        DependencyProperty.Register("HeaderText", typeof(string), typeof(ControlExample), new PropertyMetadata(null));
    public string HeaderText
    {
        get => (string)GetValue(HeaderTextProperty);
        set => SetValue(HeaderTextProperty, value);
    }

    public static readonly DependencyProperty ExampleProperty =
        DependencyProperty.Register("Example", typeof(object), typeof(ControlExample), new PropertyMetadata(null));
    public object Example
    {
        get => GetValue(ExampleProperty);
        set => SetValue(ExampleProperty, value);
    }

    public static readonly DependencyProperty OutputProperty = DependencyProperty.Register("Output", typeof(object), typeof(ControlExample), new PropertyMetadata(null));
    public object Output
    {
        get => GetValue(OutputProperty);
        set => SetValue(OutputProperty, value);
    }

    public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register("Options", typeof(object), typeof(ControlExample), new PropertyMetadata(null));
    public object Options
    {
        get => GetValue(OptionsProperty);
        set => SetValue(OptionsProperty, value);
    }

    public static readonly DependencyProperty XamlSourceProperty =
        DependencyProperty.Register("XamlSource", typeof(object), typeof(ControlExample), new PropertyMetadata(null));
    public Uri XamlSource
    {
        get => (Uri)GetValue(XamlSourceProperty);
        set => SetValue(XamlSourceProperty, value);
    }

    public static readonly DependencyProperty XamlProperty =
        DependencyProperty.Register("Xaml", typeof(string), typeof(ControlExample), new PropertyMetadata(null));
    public string Xaml
    {
        get => (string)GetValue(XamlProperty);
        set => SetValue(XamlProperty, value);
    }

    public static readonly DependencyProperty CSharpSourceProperty =
        DependencyProperty.Register("CSharpSource", typeof(object), typeof(ControlExample), new PropertyMetadata(null));
    public Uri CSharpSource
    {
        get => (Uri)GetValue(CSharpSourceProperty);
        set => SetValue(CSharpSourceProperty, value);
    }

    public static readonly DependencyProperty CSharpProperty =
        DependencyProperty.Register("CSharp", typeof(string), typeof(ControlExample), new PropertyMetadata(null));
    public string CSharp
    {
        get => (string)GetValue(CSharpProperty);
        set => SetValue(CSharpProperty, value);
    }

    public static readonly DependencyProperty SubstitutionsProperty =
        DependencyProperty.Register("Substitutions", typeof(IList<ControlExampleSubstitution>), typeof(ControlExample), new PropertyMetadata(null));
    public IList<ControlExampleSubstitution> Substitutions
    {
        get => (IList<ControlExampleSubstitution>)GetValue(SubstitutionsProperty);
        set => SetValue(SubstitutionsProperty, value);
    }

    public ControlExample()
    {
        InitializeComponent();

        Substitutions = new List<ControlExampleSubstitution>();
    }
}
