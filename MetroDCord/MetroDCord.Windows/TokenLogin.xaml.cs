using System;
using System.Diagnostics;
using System.Threading.Tasks;

// Explicit global aliases to prevent any CS0234 namespace collision with 'MetroDCord.Windows'
using Page = global::Windows.UI.Xaml.Controls.Page;
using RoutedEventArgs = global::Windows.UI.Xaml.RoutedEventArgs;
using NavigationEventArgs = global::Windows.UI.Xaml.Navigation.NavigationEventArgs;
using MessageDialog = global::Windows.UI.Popups.MessageDialog;
using ApplicationData = global::Windows.Storage.ApplicationData;
using HttpClient = global::Windows.Web.Http.HttpClient;
using HttpMethod = global::Windows.Web.Http.HttpMethod;
using HttpRequestMessage = global::Windows.Web.Http.HttpRequestMessage;
using HttpResponseMessage = global::Windows.Web.Http.HttpResponseMessage;

namespace MetroDCord
{
    public sealed partial class TokenLogin : Page
    {
        public TokenLogin()
        {
            this.InitializeComponent();
            this.RequestedTheme = global::Windows.UI.Xaml.ElementTheme.Dark;
        }

        // Handles the top Metro back arrow click to return to MainPage
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        // Action triggered when the user clicks the [Connect] or [Login] button
        private async void login_token(object sender, RoutedEventArgs e)
        {
            // 1. Grab text from your TextBox
            string rawToken = tokenbox.Password;
            string cleanToken = rawToken.Trim('"', ' ');

            if (string.IsNullOrEmpty(cleanToken))
            {
                var warnDialog = new MessageDialog("Please insert a valid token string before connecting.", "Empty Token");
                await warnDialog.ShowAsync();
                return;
            }

            // Disable button to prevent multi-click spamming
            login_btn.IsEnabled = false;

            // 2. Validate the token against the official Discord API endpoint
            bool isTokenValid = await ValidateDiscordTokenAsync(cleanToken);

            if (isTokenValid)
            {
                // 3. Save the validated token into the Windows 8.1 LocalSettings registry vault
                var localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["UserToken"] = cleanToken;

                // 4. Navigate forward straight into the main chat viewport infrastructure
                this.Frame.Navigate(typeof(ChatPage), cleanToken);
            }
            else
            {
                // Validation failed (Token is expired, fake, or rate-limited)
                var errorDialog = new MessageDialog("The provided token is invalid or expired. Double-check your input and telemetry headers.", "Authentication Failed");
                await errorDialog.ShowAsync();

                // FIX: Restored missing variable call context target
                login_btn.IsEnabled = true;
            }
        }

        private async void summon_help(object sender, RoutedEventArgs e)
        {
            var uri = new Uri("https://github.com"); // UPDATE
            await global::Windows.System.Launcher.LaunchUriAsync(uri);
        }

        /// <summary>
        /// Contacts the Discord API securely to verify user token authentication state.
        /// </summary>
        private async Task<bool> ValidateDiscordTokenAsync(string token)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // FIX: Changed destination target node to the proper user metadata profile endpoint API location
                    var request = new HttpRequestMessage(HttpMethod.Get, new Uri("https://discord.com"));

                    // Setup human-like authorization headers (CRITICAL to bypass automated bot detection)
                    request.Headers.Add("Authorization", token);

                    // Emulates a standard desktop client environment agent block
                    request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Discord/1.0.9001 Chrome/91.0.4472.164 Electron/13.1.6 Safari/537.36");

                    // Injects raw base64 super-properties payload stating we are an official stable release desktop node
                    request.Headers.Add("X-Super-Properties", "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiRGlzY29yZCBDbGllbnQiLCJyZWxlYXNlX2NoYW5uZWwiOiJzdGFibGUiLCJjbGllbnRfdmVyc2lvbiI6IjEuMC45MDAxIiwib3NfdmVyc2lvbiI6IjYuMy45NjAwIiwib3NfYXJjaCI6Ing2NCJ9");

                    HttpResponseMessage response = await client.SendRequestAsync(request);

                    // If HTTP Status Code is 200 (OK), authorization is confirmed successful
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[Token Validation Network Exception]: " + ex.Message);
                return false;
            }
        }
    }

}
