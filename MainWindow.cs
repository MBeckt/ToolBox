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
                    TenantId = "2fdbaf70-405c-420d-81e6-0d5391cd6245",

                    // Enter the client ID obtained from the Microsoft Entra admin center
                    ClientId = "1be0f404-8ead-476c-bc75-72a6bd2ac06d"
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
            /*
            using var graphRequest = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/users");
            graphRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", msalAuthenticationResult.AccessToken);
            var graphResponseMessage = await _httpClient.SendAsync(graphRequest);
            graphResponseMessage.EnsureSuccessStatusCode();
            */
            /*Dictionary<string, string> jsonValues = new Dictionary<string, string>();
            jsonValues.Add("passwordProfile", "forceChangePasswordNextSignIn");*/




            /*using StringContent payload = new(
                System.Text.Json.JsonSerializer.Serialize(new
                {
                    passwordProfile = new
                    {
                        forceChangePasswordNextSignIn = true,
                    }
                }),
                Encoding.UTF8,
                "application/json");
            var payloadJSON = JsonConvert.SerializeObject(payload);
            var graphRequest = new HttpRequestMessage(HttpMethod.Patch, "https://graph.microsoft.com/v1.0/user/66895798-577b-4998-9326-b82a6d092aa4")
            {
                Content = new StringContent($"{payloadJSON}", Encoding.UTF8, "application/json")
            }; // Figure out how to add my fucking content

            graphRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", msalAuthenticationResult.AccessToken);


            var graphResponseMessage = await _httpClient.SendAsync(graphRequest);
            graphResponseMessage.EnsureSuccessStatusCode(); // ERROR 405 | NEED MORE PERMISSIONS // Now ERROR 400!!!!
           */






            /*
            var payload = new { passwordProfile = new { forceChangePasswordNextSignIn = true, } }; 
            var payloadJSON = System.Text.Json.JsonSerializer.Serialize(payload); 
            var graphRequest = new HttpRequestMessage(HttpMethod.Patch, $"https://graph.microsoft.com/v1.0/users/66895798-577b-4998-9326-b82a6d092aa4") 
                { Content = new StringContent(payloadJSON, Encoding.UTF8, "application/json") }; graphRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", msalAuthenticationResult.AccessToken); var graphResponseMessage = await _httpClient.SendAsync(graphRequest); graphResponseMessage.EnsureSuccessStatusCode();

            // Check for 204 No Content response
            if (graphResponseMessage.StatusCode == HttpStatusCode.NoContent) 
                { GraphResultsTextBox.Text = "HTTP Response Code: 204 No Content"; }
            else 
                { using var graphResponseJson = JsonDocument.Parse(await graphResponseMessage.Content.ReadAsStreamAsync()); 
                GraphResultsTextBox.Text = System.Text.Json.JsonSerializer.Serialize(graphResponseJson, new JsonSerializerOptions 
                    { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });} 
                var tokenWasFromCache = TokenSource.Cache == msalAuthenticationResult.AuthenticationResultMetadata.TokenSource; 
                AccessTokenSourceLabel.Text = $"{(tokenWasFromCache ? "Cached" : "Newly Acquired")} (Expires: {msalAuthenticationResult.ExpiresOn:R})"; 
            */


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
                    GraphResultsTextBox.Text += $"User {userId}: Password Successfully Expired\n";
                }
                else
                {
                    using var graphResponseJson = JsonDocument.Parse(await graphResponseMessage.Content.ReadAsStreamAsync());
                    GraphResultsTextBox.Text += $"User {userId}: " + System.Text.Json.JsonSerializer.Serialize(graphResponseJson, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }) + "\n";
                }

                var tokenWasFromCache = TokenSource.Cache == msalAuthenticationResult.AuthenticationResultMetadata.TokenSource;
                AccessTokenSourceLabel.Text = $"{(tokenWasFromCache ? "Cached" : "Newly Acquired")} (Expires: {msalAuthenticationResult.ExpiresOn:R})";
            }

            // Parsing HTTP response code
            /*var httpResponseCode = (int)usersResponse.StatusCode;
            GraphResultsTextBox.Text = $"HTTP Response Code: {httpResponseCode}";*/












            // Parsing HTTP response code var httpResponseCode = (int)graphResponseMessage.StatusCode; HttpResponseCodeLabel.Text = $"HTTP Response Code: {httpResponseCode}"

            /* using var graphResponseJson = JsonDocument.Parse(await graphResponseMessage.Content.ReadAsStreamAsync());
            var httpResponseCode = (int)graphResponseMessage.StatusCode; GraphResultsTextBox.Text = $"HTTP Response Code: {httpResponseCode}";
            var tokenWasFromCache = TokenSource.Cache == msalAuthenticationResult.AuthenticationResultMetadata.TokenSource;
            AccessTokenSourceLabel.Text = $"{(tokenWasFromCache ? "Cached" : "Newly Acquired")} (Expires: {msalAuthenticationResult.ExpiresOn:R})";
            */
            // Hide the call to action and show the results.
            SignInCallToActionLabel.Hide();
            GraphResultsPanel.Show();

            // Code snippets are only available for the latest version. Current version is 5.x

            // Dependencies

            /*
            var scopes = new[] { "User.Read" };

            var tenantId = "2fdbaf70-405c-420d-81e6-0d5391cd6245";

            // Value from app registration
            var clientId = "1be0f404-8ead-476c-bc75-72a6bd2ac06d";

            // using Azure.Identity;
            var options = new DeviceCodeCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
                ClientId = clientId,
                TenantId = tenantId,

                // Callback function that receives the user prompt
                // Prompt contains the generated device code that user must
                // enter during the auth process in the browser
                DeviceCodeCallback = (code, cancellation) =>
                {
                    Console.WriteLine(code.Message);
                    return Task.FromResult(0);
                },
            };
            // https://learn.microsoft.com/dotnet/api/azure.identity.devicecodecredential
            var deviceCodeCredential = new DeviceCodeCredential(options);

            var graphClient = new GraphServiceClient(deviceCodeCredential, scopes);


            var requestBody = new Microsoft.Graph.Models.User
            {
                PasswordProfile = new PasswordProfile
                {
                    ForceChangePasswordNextSignIn = true,
                },
            };
            // To initialize your graphClient, see https://learn.microsoft.com/en-us/graph/sdks/create-client?from=snippets&tabs=csharp
            var result = await graphClient.Users["{user-id}"].PatchAsync(requestBody);*/
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

        private void ExitButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void GraphResultsTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
