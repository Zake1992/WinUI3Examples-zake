# WinUI 3 Examples

* Create a single instance of the application
* Set up dependency injection, app settings and logging
* Set main window size and title

## Create single instance of the application

Add a Mutex class member to the App class.

Add code to the constructor to see if the mutex is new, and exit the application if it is not. Create a finalizer for the App class and use it to dispose of the mutex.

## Set up dependency injection, app settings and logging

Add the following NuGet packages

* Microsoft.Extensions.Configuration
* Microsoft.Extensions.Configuration.Binder
* Microsoft.Extensions.Configuration.FileExtensions
* Microsoft.Extensions.Configuration.Json
* Microsoft.Extensions.DependencyInjection
* NLog
* NLog.Extensions.Logging

Add an appsettings.json file to the project. Set its properties to copy if newer. Add some NLog targets and configuration rules. Add any desired application settings.

In App.OnLaunched, configure the DI and bind your settings.

Optionally, in the App class, set up a public Current member for easy access to the App class instance, a GetService<T> method, and a public member to enable easy access to the main window.

## Set main window size and title