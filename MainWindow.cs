using Microsoft.Identity.Client;
using Microsoft.VisualBasic.ApplicationServices;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Graph.Models;
using Microsoft.Graph;
using Azure.Identity;
using static System.Formats.Asn1.AsnWriter;
using static System.Windows.Forms.Design.AxImporter;
using System.Text.Json.Nodes;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using Azure.Core;

namespace MsalExample
{
    public partial class MainWindow : Form
    {
        private readonly HttpClient _httpClient = new();

        // In order to take advantage of token caching, your MSAL client singleton must
        // have a lifecycle that at least matches the lifecycle of the user's session in
        // the application. In this sample, the lifecycle of the MSAL client is tied to
        // the lifecycle of this form instance, which is the whole of the application.
        private readonly IPublicClientApplication msalPublicClientApp;

        public MainWindow()
        {
            InitializeComponent();

            // Configure your public client application
            msalPublicClientApp = PublicClientApplicationBuilder
                .CreateWithApplicationOptions(new PublicClientApplicationOptions
                {
                    // Enter the tenant ID obtained from the Microsoft Entra admin center
                    TenantId = textBox1.Text,

                    // Enter the client ID obtained from the Microsoft Entra admin center
                    ClientId = textBox2.Text
                })
                .WithDefaultRedirectUri() // http://localhost
                .Build();
        }


        // <summary>
        // Handle the "Sign In" button click. This will acquire an access token scoped to
        // Microsoft Graph, either from the cache or from an interactive session. It will
        // then use that access token in an HTTP request to Microsoft Graph and display
        // the results.
        // </summary>
        private async void SignInButton_Click(object sender, EventArgs e)
        {
            AuthenticationResult? msalAuthenticationResult = null;

            // Acquire a cached access token for Microsoft Graph if one is available from a prior
            // execution of this authentication flow.
            var accounts = await msalPublicClientApp.GetAccountsAsync();
            if (accounts.Any())
            {
                try
                {
                    // Will return a cached access token if available, refreshing if necessary.
                    msalAuthenticationResult = await msalPublicClientApp.AcquireTokenSilent(
                        new[] { "https://graph.microsoft.com/User.Read" },
                        accounts.First())
                        .ExecuteAsync();
                }
                catch (MsalUiRequiredException)
                {
                    // Nothing in cache for this account + scope, and interactive experience required.
                }
            }

            if (msalAuthenticationResult == null)
            {
                // This is likely the first authentication request since application start or after
                // Sign Out was clicked, so calling this will launch the user's default browser and
                // send them through a login flow. After the flow is complete, the rest of this method
                // will continue to execute.
                msalAuthenticationResult = await msalPublicClientApp.AcquireTokenInteractive(
                    new[] { "https://graph.microsoft.com/User.Read" })
                    .ExecuteAsync();
            }

            // Call Microsoft Graph using the access token acquired above.
            // Get all users in the tenant
            var usersRequest = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/users");
            usersRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", msalAuthenticationResult.AccessToken);
            var usersResponse = await _httpClient.SendAsync(usersRequest);
            usersResponse.EnsureSuccessStatusCode();
            var usersJson = await usersResponse.Content.ReadAsStringAsync();
            var users = JsonDocument.Parse(usersJson).RootElement.GetProperty("value");

            // Define the payload for the patch request
            var payload = new { passwordProfile = new { forceChangePasswordNextSignIn = true } };
            var payloadJSON = System.Text.Json.JsonSerializer.Serialize(payload);
            var patchContent = new StringContent(payloadJSON, Encoding.UTF8, "application/json");

            // Iterate through all users and apply the patch
            foreach (var user in users.EnumerateArray())
            {
                var userId = user.GetProperty("id").GetString();
                var graphRequest = new HttpRequestMessage(HttpMethod.Patch, $"https://graph.microsoft.com/v1.0/users/{userId}")
                {
                    Content = patchContent
                };
                graphRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", msalAuthenticationResult.AccessToken);
                var graphResponseMessage = await _httpClient.SendAsync(graphRequest);

                // Check for 204 No Content response
                if (graphResponseMessage.StatusCode == HttpStatusCode.NoContent)
                {
                    GraphResultsTextBox.Text += $"User {userId}: Password Successfully Expired\r\n";
                }
                else
                {
                    using var graphResponseJson = JsonDocument.Parse(await graphResponseMessage.Content.ReadAsStreamAsync());
                    GraphResultsTextBox.Text += $"User {userId}: " + System.Text.Json.JsonSerializer.Serialize(graphResponseJson, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }) + "\n";
                }
                var tokenWasFromCache = TokenSource.Cache == msalAuthenticationResult.AuthenticationResultMetadata.TokenSource;
                AccessTokenSourceLabel.Text = $"{(tokenWasFromCache ? "Cached" : "Newly Acquired")} (Expires: {msalAuthenticationResult.ExpiresOn:R})";
            }
            // Parsing HTTP response code var httpResponseCode = (int)graphResponseMessage.StatusCode; HttpResponseCodeLabel.Text = $"HTTP Response Code: {httpResponseCode}"
            // Hide the call to action and show the results.
            SignInCallToActionLabel.Hide();
            GraphResultsPanel.Show();
        }

        // Find USERS by EMAIL

        private async void FindEmailButton_Click(object sender, EventArgs e)
        {
            AuthenticationResult? msalAuthenticationResult = null;

            // Acquire a cached access token for Microsoft Graph if one is available from a prior
            // execution of this authentication flow.
            var accounts = await msalPublicClientApp.GetAccountsAsync();
            if (accounts.Any())
            {
                try
                {
                    // Will return a cached access token if available, refreshing if necessary.
                    msalAuthenticationResult = await msalPublicClientApp.AcquireTokenSilent(
                        new[] { "https://graph.microsoft.com/User.Read" },
                        accounts.First())
                        .ExecuteAsync();
                }
                catch (MsalUiRequiredException)
                {
                    // Nothing in cache for this account + scope, and interactive experience required.
                }
            }

            if (msalAuthenticationResult == null)
            {
                // This is likely the first authentication request since application start or after
                // Sign Out was clicked, so calling this will launch the user's default browser and
                // send them through a login flow. After the flow is complete, the rest of this method
                // will continue to execute.
                msalAuthenticationResult = await msalPublicClientApp.AcquireTokenInteractive(
                    new[] { "https://graph.microsoft.com/User.Read" })
                    .ExecuteAsync();
            }

            // Call Microsoft Graph using the access token acquired above.
            using var graphRequest = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me");
            graphRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", msalAuthenticationResult.AccessToken);
            var graphResponseMessage = await _httpClient.SendAsync(graphRequest);
            graphResponseMessage.EnsureSuccessStatusCode();

            // Present the results to the user (formatting the json for readability)
            using var graphResponseJson = JsonDocument.Parse(await graphResponseMessage.Content.ReadAsStreamAsync());
            GraphResultsTextBox.Text = System.Text.Json.JsonSerializer.Serialize(graphResponseJson, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
            var tokenWasFromCache = TokenSource.Cache == msalAuthenticationResult.AuthenticationResultMetadata.TokenSource;
            AccessTokenSourceLabel.Text = $"{(tokenWasFromCache ? "Cached" : "Newly Acquired")} (Expires: {msalAuthenticationResult.ExpiresOn:R})";

            // Hide the call to action and show the results.
            SignInCallToActionLabel.Hide();
            GraphResultsPanel.Show();
        }
        /// <summary>
        /// Handle the "Sign Out" button click. This will remove all cached tokens from
        /// the MSAL client, resulting in any future usage requiring a reauthentication
        /// experience.
        /// </summary>
        private async void SignOutButton_Click(object sender, EventArgs e)
        {
            // Signing out is removing all cached tokens, meaning the next token request will
            // require the user to sign in.
            foreach (var account in (await msalPublicClientApp.GetAccountsAsync()).ToList())
            {
                await msalPublicClientApp.RemoveAsync(account);
            }

            // Show the call to action and hide the results.
            GraphResultsPanel.Hide();
            GraphResultsTextBox.Clear();
            SignInCallToActionLabel.Show();
        }

        private void button1_Click(ApplicationOptions applicationOptions)
        {
            applicationOptions.TenantId = textBox1.Text;
        }

        private void button2_Click(ApplicationOptions applicationOptions)
        {
            applicationOptions.ClientId = textBox2.Text;
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "2fdbaf70-405c-420d-81e6-0d5391cd6245";
            textBox2.Text = "1be0f404-8ead-476c-bc75-72a6bd2ac06d";
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void GraphResultsTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void SignInCallToActionLabel_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
