using Microsoft.UI.Xaml.Media.Animation;
using System;

namespace WinUI3Examples.Services;

public interface INavigationService
{
    void Navigate(Type pageType, object targetPageArgs = null, NavigationTransitionInfo transInfo = null);
}
