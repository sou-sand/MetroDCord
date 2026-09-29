using System;
using System.Diagnostics;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Explicit global aliases targeting native WinRT references to fix CS0234 namespace problems
using ApplicationData = global::Windows.Storage.ApplicationData;
using NavigationEventArgs = global::Windows.UI.Xaml.Navigation.NavigationEventArgs;
using WebViewNavigationCompletedEventArgs = global::Windows.UI.Xaml.Controls.WebViewNavigationCompletedEventArgs;

namespace MetroDCord.Windows
{
    public sealed partial class WebAuthPage : Page
    {
        public WebAuthPage()
        {
            this.InitializeComponent();
            this.RequestedTheme = ElementTheme.Dark;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Expecting target authentication URL string passed as navigation parameter
            string targetUrl = e.Parameter as string;
            if (!string.IsNullOrEmpty(targetUrl))
            {
                // Update header label dynamically based on destination context target
                if (targetUrl.Contains("register"))
                {
                    TxtPageHeader.Text = "create account";
                }
                else
                {
                    TxtPageHeader.Text = "account authentication";
                }

                LoadingOverlay.Visibility = Visibility.Visible;
                PrgLoading.IsActive = true;

                // Direct the WebView container towards the secure node endpoint uri target location
                AuthWebView.Navigate(new Uri(targetUrl));
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Safely drop runtime tasks and cycle layout visibility frame back to home hub panel
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        private async void AuthWebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            // Drop loading splash animations framework layout screens layer
            LoadingOverlay.Visibility = Visibility.Collapsed;
            PrgLoading.IsActive = false;

            // Trigger interception loop tracking once the user hits the Discord dashboard context route node
            if (args.Uri.AbsoluteUri.Contains("/channels/"))
            {
                try
                {
                    // JavaScript execution script array statement string to query the client LocalStorage token payload
                    string script = "(function() { return window.localStorage.getItem('token'); })();";
                    string token = await AuthWebView.InvokeScriptAsync("eval", new string[] { script });

                    if (!string.IsNullOrEmpty(token))
                    {
                        // Strip residual parsing elements or quote wrappers out of raw string text
                        token = token.Trim('"');

                        // Bind token securely inside the operating system local app storage registry framework layout settings
                        var localSettings = ApplicationData.Current.LocalSettings;
                        localSettings.Values["UserToken"] = token;

                        // Force application routing system context view layout forward onto chat dashboard layout instances
                        this.Frame.Navigate(typeof(ChatPage), token);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[Script Injection Execution Exception Triggered]: " + ex.Message);
                }
            }
        }
    }
}
