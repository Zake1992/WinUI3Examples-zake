using ColorCode;
using ColorCode.Common;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.Storage;
using WinUI3Examples.Helpers;
using WinUI3Examples.Services;

namespace WinUI3Examples.Controls;

public sealed partial class CodeExample : UserControl
{
    public static readonly DependencyProperty CodeProperty =
        DependencyProperty.Register("Code", typeof(string), typeof(CodeExample), new PropertyMetadata("", OnDependencyPropertyChanged));
    public string Code
    {
        get => (string)GetValue(CodeProperty);
        set => SetValue(CodeProperty, value);
    }

    public static readonly DependencyProperty CodeSourceFileProperty =
        DependencyProperty.Register("CodeSourceFile", typeof(object), typeof(CodeExample), new PropertyMetadata(null, OnDependencyPropertyChanged));
    public Uri CodeSourceFile
    {
        get => (Uri)GetValue(CodeSourceFileProperty);
        set => SetValue(CodeSourceFileProperty, value);
    }

    public static readonly DependencyProperty SubstitutionsProperty =
        DependencyProperty.Register("Substitutions", typeof(IList<ControlExampleSubstitution>), typeof(CodeExample), new PropertyMetadata(null));
    public IList<ControlExampleSubstitution> Substitutions
    {
        get => (IList<ControlExampleSubstitution>)GetValue(SubstitutionsProperty);
        set => SetValue(SubstitutionsProperty, value);
    }

    public static readonly DependencyProperty IsCSharpExampleProperty =
        DependencyProperty.Register("IsCSharpExample", typeof(bool), typeof(CodeExample), new PropertyMetadata(false));
    public bool IsCSharpExample
    {
        get => (bool)GetValue(IsCSharpExampleProperty);
        set => SetValue(IsCSharpExampleProperty, value);
    }

    private static Regex SubstitutionPattern = new Regex(@"\$\(([^\)]+)\)");

    public CodeExample()
    {
        InitializeComponent();
    }

    private static void OnDependencyPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is CodeExample example)
            example.ReevaluateVisibility();
    }

    private void ReevaluateVisibility()
    {
        Visibility = (Code.Length == 0 && CodeSourceFile == null) ? Visibility.Collapsed : Visibility.Visible;
    }

    private void OnCodeExampleLoaded(object sender, RoutedEventArgs e)
    {
        ReevaluateVisibility();
        VisualStateManager.GoToState(this, IsCSharpExample ? "CSharpExample" : "XamlExample", false);

        foreach (var substitution in Substitutions)
            substitution.ValueChanged += (s, e) => GenerateSyntaxHighlightedContent();
    }

    private void OnCodePresenterLoaded(object sender, RoutedEventArgs e) => GenerateSyntaxHighlightedContent();

    private void OnActualThemeChanged(FrameworkElement sender, object args) => GenerateSyntaxHighlightedContent();

    private void GenerateSyntaxHighlightedContent()
    {
        if (!string.IsNullOrEmpty(Code))
            FormatAndRenderFromString(Code, CodePresenter, IsCSharpExample ? Languages.CSharp : Languages.Xml);
        else
            FormatAndRenderFromFile(CodeSourceFile, CodePresenter, IsCSharpExample ? Languages.CSharp : Languages.Xml);
    }

    private async void FormatAndRenderFromFile(Uri source, ContentPresenter presenter, ILanguage language)
    {
        if (source == null || !source.AbsolutePath.EndsWith("txt"))
        {
            presenter.Visibility = Visibility.Collapsed;
            return;
        }

        var file = await StorageFile.GetFileFromApplicationUriAsync(source);
        var codeString = await FileIO.ReadTextAsync(file);
        FormatAndRenderFromString(codeString, presenter, language);
    }

    private void FormatAndRenderFromString(string codeString, ContentPresenter presenter, ILanguage language)
    {
        codeString = SubstitutionPattern.Replace(codeString, match =>
        {
            foreach (var substitution in Substitutions.Where(substitution => substitution.Key == match.Groups[1].Value))
                return substitution.ValueAsString();

            throw new KeyNotFoundException(match.Groups[1].Value);
        });

        var rtb = new RichTextBlock { FontFamily = new FontFamily("Consolas") };
        var formatter = CreateRichTextFormatter();
        formatter.FormatRichTextBlock(codeString, language, rtb);
        presenter.Content = rtb;
    }

    private RichTextBlockFormatter CreateRichTextFormatter()
    {
        var theme = App.Current.GetService<IThemeService>().RootTheme;

        if (theme == ElementTheme.Default && Application.Current.RequestedTheme == ApplicationTheme.Dark)
            theme = ElementTheme.Dark;

        var formatter = new RichTextBlockFormatter(theme);

        if (theme == ElementTheme.Dark)
            FixHorribleDarkThemeColors(formatter);

        return formatter;
    }

    private void FixHorribleDarkThemeColors(RichTextBlockFormatter formatter)
    {
        formatter.Styles.Remove(formatter.Styles[ScopeName.XmlAttribute]);
        formatter.Styles.Remove(formatter.Styles[ScopeName.XmlAttributeQuotes]);
        formatter.Styles.Remove(formatter.Styles[ScopeName.XmlAttributeValue]);
        formatter.Styles.Remove(formatter.Styles[ScopeName.HtmlComment]);
        formatter.Styles.Remove(formatter.Styles[ScopeName.XmlDelimiter]);
        formatter.Styles.Remove(formatter.Styles[ScopeName.XmlName]);

        formatter.Styles.Add(new ColorCode.Styling.Style(ScopeName.XmlAttribute)
        {
            Foreground = "#FF87CEFA",
            ReferenceName = "xmlAttribute"
        });
        formatter.Styles.Add(new ColorCode.Styling.Style(ScopeName.XmlAttributeQuotes)
        {
            Foreground = "#FFFFA07A",
            ReferenceName = "xmlAttributeQuotes"
        });
        formatter.Styles.Add(new ColorCode.Styling.Style(ScopeName.XmlAttributeValue)
        {
            Foreground = "#FFFFA07A",
            ReferenceName = "xmlAttributeValue"
        });
        formatter.Styles.Add(new ColorCode.Styling.Style(ScopeName.HtmlComment)
        {
            Foreground = "#FF6B8E23",
            ReferenceName = "htmlComment"
        });
        formatter.Styles.Add(new ColorCode.Styling.Style(ScopeName.XmlDelimiter)
        {
            Foreground = "#FF808080",
            ReferenceName = "xmlDelimiter"
        });
        formatter.Styles.Add(new ColorCode.Styling.Style(ScopeName.XmlName)
        {
            Foreground = "#FF5F82E8",
            ReferenceName = "xmlName"
        });
    }
}
