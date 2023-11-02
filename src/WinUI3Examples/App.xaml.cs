using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading;
using Windows.ApplicationModel;
using WinUI3Examples.Helpers;
using WinUI3Examples.Navigation;
using WinUI3Examples.Pages;
using WinUI3Examples.Services;
using WinUI3Examples.ViewModels;

namespace WinUI3Examples;

public partial class App : Application
{
    public static new App Current => (App)Application.Current;

    private readonly Logger logger = LogManager.GetCurrentClassLogger();

    private readonly Mutex singleInstanceMutex;
    private IServiceProvider serviceProvider;

    private ISettingsService settings;

    private Window startupWindow;
    public Window StartupWindow => startupWindow;
    public XamlRoot MainRoot => StartupWindow.Content.XamlRoot;

    public T GetService<T>() => serviceProvider.GetService<T>();

    public App()
    {
        //
        // Don't allow multiple instances of the application
        //
        var s = Assembly.GetExecutingAssembly().GetType().GUID.ToString();
        singleInstanceMutex = new Mutex(true, s, out var isNew);

        if (!isNew)
            Environment.Exit(0);

        ConfigureServcies();
        InitializeComponent();
    }


    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        //
        // To start with an empty main window and set its page dynamically,
        // use the code below
        //
        //startupWindow = GetService<IWindowService>().CreateWindow();
        //SetRootPage();
        var navService = GetService<INavigationService>();
        startupWindow = (MainWindow)navService;
        GetService<IWindowService>().AddWindow(startupWindow);

        //
        // Set the initial theme
        //
        GetService<IThemeService>().Initialize();

        settings = GetService<ISettingsService>();

        startupWindow.SetWindowSize(settings.AppWindowSize);
        startupWindow.SizeChanged += StartupWindow_SizeChanged;
        StartupWindow.Activate();
        navService.Navigate(typeof(WelcomePage));
    }

    private void StartupWindow_SizeChanged(object sender, WindowSizeChangedEventArgs args)
    {
        settings.AppWindowSize = args.Size;
    }

    private void SetRootPage()
    {
        //
        // See OnLaunched for calling this method
        //
        var rootPage = StartupWindow.Content as NavigationRootPage;

        if (rootPage == null)
        {
            rootPage = new NavigationRootPage();
            var rootFrame = (Frame)rootPage.FindName("rootFrame") ?? throw new Exception("No root frame found");
            rootFrame.NavigationFailed += (s, e) => throw new Exception($"Failed to load page {e.SourcePageType.FullName}");
            StartupWindow.Content = rootPage;
        }
    }

    private void ConfigureServcies()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Package.Current.InstalledLocation.Path)
            .AddJsonFile("appsettings.json", false);

        var configuration = builder.Build();

        //
        // Configure logging with NLog
        //
        LogManager.Configuration = new NLogLoggingConfiguration(configuration.GetSection("NLog"));
        var services = new ServiceCollection();

        //
        // Bind appsettings.json to a settings model
        //
        var settings = new SettingsService();
        configuration.GetSection("AppSettings").Bind(settings);

        //
        // Services
        //
        services.AddSingleton<INavigationService, MainWindow>();
        services.AddSingleton<IWindowService, WindowService>();
        services.AddSingleton<ISettingsService>(settings);
        services.AddSingleton<IThemeService, ThemeService>();

        //
        // View Models
        //
        services.AddTransient<ISettingsPageViewModel, SettingsPageViewModel>();

        serviceProvider = services.BuildServiceProvider();
    }

    ~App()
    {
        singleInstanceMutex?.Dispose();
    }
}

