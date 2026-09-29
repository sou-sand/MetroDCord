using MetroDCord.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;

// Explicit global routing aliases to prevent namespace collisions with 'MetroDCord.Windows'
using ApplicationData = global::Windows.Storage.ApplicationData;
using NavigationEventArgs = global::Windows.UI.Xaml.Navigation.NavigationEventArgs;
using Page = global::Windows.UI.Xaml.Controls.Page;
using RoutedEventArgs = global::Windows.UI.Xaml.RoutedEventArgs;
using ElementTheme = global::Windows.UI.Xaml.ElementTheme;
using UICommand = global::Windows.UI.Popups.UICommand;
using UICommandInvokedHandler = global::Windows.UI.Popups.UICommandInvokedHandler;
using MessageDialog = global::Windows.UI.Popups.MessageDialog;

namespace MetroDCord.Windows
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.RequestedTheme = ElementTheme.Dark;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Check if the user session token already exists locally
            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("UserToken"))
            {
                string savedToken = localSettings.Values["UserToken"] as string;
                if (!string.IsNullOrEmpty(savedToken))
                {
                    // Route immediately towards the horizontal chat interface
                    this.Frame.Navigate(typeof(ChatPage), savedToken);
                }
            }
        }

        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            await PromptAppExitAsync();
        }

        private async Task PromptAppExitAsync()
        {
            var dialog = new MessageDialog("Are you sure you want to exit MetroDCord?", "You didn't even login yet.");

            dialog.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler((cmd) => {
                global::Windows.UI.Xaml.Application.Current.Exit();
            })));

            dialog.Commands.Add(new UICommand("No", null));

            dialog.DefaultCommandIndex = 1;
            dialog.CancelCommandIndex = 1;

            await dialog.ShowAsync();
        }

        private void login_token(object sender, RoutedEventArgs e)
        {
            // Route views directly towards the token validation pane
            this.Frame.Navigate(typeof(TokenLogin));
        }

        private void login_webview(object sender, RoutedEventArgs e)
        {
            // Redirect view hierarchy context towards WebAuthPage injecting the standard endpoint URI node string parameter
            this.Frame.Navigate(typeof(WebAuthPage), "https://discord.com/login");
        }

        private void sign_up_webview(object sender, RoutedEventArgs e)
        {
            // Redirect view hierarchy context towards WebAuthPage injecting the registration endpoint URI node string parameter
            this.Frame.Navigate(typeof(WebAuthPage), "https://discord.com/register");
        }


        private async void token_help_git(object sender, RoutedEventArgs e)
        {
            var uri = new Uri("https://github.com"); // UPDATE
            await global::Windows.System.Launcher.LaunchUriAsync(uri);
        }

        private async void open_socials(object sender, RoutedEventArgs e)
        {
            var uri = new Uri("https://github.com"); // UPDATE
            await global::Windows.System.Launcher.LaunchUriAsync(uri);
        }

    }
}
