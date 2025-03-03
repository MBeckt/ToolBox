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
using System.Data;
using System.IO.Compression;

namespace MsalExample
{
    public partial class MainWindow : Form
    {
        private readonly HttpClient _httpClient = new();


        // In order to take advantage of token caching, your MSAL client singleton must
        // have a lifecycle that at least matches the lifecycle of the user's session in
        // the application. In this sample, the lifecycle of the MSAL client is tied to
        // the lifecycle of this form instance, which is the whole of the application.
        public IPublicClientApplication msalPublicClientApp;

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
        private string Staging = "2fdbaf70-405c-420d-81e6-0d5391cd6245";
        private string Production = "7fad452f-bb21-4814-9756-a7c7c9bbb90c";
        private void Button3_Click(object sender, EventArgs e)
        {
            //var IssueEnv = "'ReferallStaging.onmicrosoft'";
            TenantID.Text = "2fdbaf70-405c-420d-81e6-0d5391cd6245";
            ClientId.Text = "1be0f404-8ead-476c-bc75-72a6bd2ac06d";
            //LookupUser.Enabled = true;
        }
        private void Button4_Click(object sender, EventArgs e)
        {
            TenantID.Text = "7fad452f-bb21-4814-9756-a7c7c9bbb90c";
            ClientId.Text = "2a798ec2-15e3-4dff-bfaa-edb924c1fc91";
            //LookupUser.Enabled = true;
        }



        // <summary>
        // Handle the "Sign In" button click. This will acquire an access token scoped to
        // Microsoft Graph, either from the cache or from an interactive session. It will
        // then use that access token in an HTTP request to Microsoft Graph and display
        // the results.
        // </summary>

        private async void ExpirePassword_Click(object sender, EventArgs e)
        {

            if (TenantID.Text != msalPublicClientApp.AppConfig.TenantId)
            {
                msalPublicClientApp = PublicClientApplicationBuilder.CreateWithApplicationOptions(new PublicClientApplicationOptions
                {
                    // Enter the tenant ID obtained from the Microsoft Entra admin center
                    TenantId = TenantID.Text,

                    // Enter the client ID obtained from the Microsoft Entra admin center
                    ClientId = ClientId.Text
                })
                .WithDefaultRedirectUri() // http://localhost
                .Build();
            }
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

            if (checkBox1.Checked == true && checkBox2.Checked == false)
            {
                var usersRequestUrl = "https://graph.microsoft.com/v1.0/users?$select=id,displayName,mail,identities,otherMails";
                var users = new List<JsonElement>();

                while (!string.IsNullOrEmpty(usersRequestUrl))
                {
                    var usersRequest = new HttpRequestMessage(HttpMethod.Get, usersRequestUrl);
                    usersRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", msalAuthenticationResult.AccessToken);
                    var usersResponse = await _httpClient.SendAsync(usersRequest);
                    usersResponse.EnsureSuccessStatusCode();
                    var usersJson = await usersResponse.Content.ReadAsStringAsync();
                    var usersDocument = JsonDocument.Parse(usersJson);
                    users.AddRange(usersDocument.RootElement.GetProperty("value").EnumerateArray());

                    usersRequestUrl = usersDocument.RootElement.TryGetProperty("@odata.nextLink", out var nextLink) ? nextLink.GetString() : null;
                }

                var payload = new { passwordProfile = new { forceChangePasswordNextSignIn = true } };
                var payloadJSON = System.Text.Json.JsonSerializer.Serialize(payload);
                var patchContent = new StringContent(payloadJSON, Encoding.UTF8, "application/json");

                var dataTable = new DataTable();
                dataTable.Columns.Add("B2C");
                dataTable.Columns.Add("DisplayName");
                dataTable.Columns.Add("Email");
                dataTable.Columns.Add("UserStatus");

                GraphResultsDataGridView.DataSource = dataTable;
                SignInCallToActionLabel.Hide();
                GraphResultsPanel.Show();

                foreach (var user in users)
                {
                    var userId = user.GetProperty("id").GetString();
                    var graphRequest = new HttpRequestMessage(HttpMethod.Patch, $"https://graph.microsoft.com/v1.0/users/{userId}?$select=id,displayName,mail,identities,otherMails")
                    {//Does this work? FK
                        Content = patchContent
                    };
                    graphRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", msalAuthenticationResult.AccessToken);
                    var graphResponseMessage = await _httpClient.SendAsync(graphRequest);

                    if (graphResponseMessage.StatusCode == HttpStatusCode.NoContent)
                    {
                        GraphResultsTextBox.Text += $"User {userId}: Password Successfully Expired\r\n";
                        var UserStatus = $"User {userId}: Password Successfully Expired";
                        var B2C = user.GetProperty("id").GetString();
                        var displayName = user.GetProperty("displayName").GetString();
                        var mail = user.GetProperty("mail").GetString();
                        var identities = user.GetProperty("identities").EnumerateArray();

                        foreach (var identity in identities)
                        {
                            if (identity.GetProperty("signInType").GetString() == "emailAddress")
                            {
                                var issuerAssignedId = identity.GetProperty("issuerAssignedId").GetString();
                                dataTable.Rows.Add(B2C, displayName, issuerAssignedId, UserStatus); //mail, 
                            }
                        }
                    }
                    else
                    {
                        using var graphResponseJson = JsonDocument.Parse(await graphResponseMessage.Content.ReadAsStreamAsync());
                        GraphResultsTextBox.Text += $"User {userId}: " + System.Text.Json.JsonSerializer.Serialize(graphResponseJson, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }) + "\n";
                    }
                    var tokenWasFromCache = TokenSource.Cache == msalAuthenticationResult.AuthenticationResultMetadata.TokenSource;
                    AccessTokenSourceLabel.Text = $"{(tokenWasFromCache ? "Cached" : "Newly Acquired")} (Expires: {msalAuthenticationResult.ExpiresOn:R})";
                }

                SignInCallToActionLabel.Hide();
                GraphResultsPanel.Show();
            }
            

            if (checkBox1.Checked == false && checkBox2.Checked == false)
            {
                if (!string.IsNullOrEmpty(textBox3.Text))
                {
                    var existingText = textBox3.Text;
                    //existingText = textBox4.Text;
                    var IssueEnv = "'ReferallProduction.onmicrosoft'";
                    if (TenantID.Text == "2fdbaf70-405c-420d-81e6-0d5391cd6245")
                    {
                        IssueEnv = "'StagingReferall.onmicrosoft'";
                    }
                    var usersRequest = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/users?$filter=identities/any(id:id/issuerAssignedId eq " + "'" + existingText + "'" + " and id/issuer eq " + IssueEnv + ")&$select=id,displayName,mail,identities,otherMails");
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
                        // Parsing HTTP response code var httpResponseCode = (int)graphResponseMessage.StatusCode; HttpResponseCodeLabel.Text = $"HTTP Response Code: {httpResponseCode}"
                        // Hide the call to action and show the results.
                        SignInCallToActionLabel.Hide();
                        GraphResultsPanel.Show();
                    }
                    //NEW SHIT ATTEMPT // FUCKING MAKE THIS USE SAME REQUEST AS PRIOR !!!?????!???!? // EXPIREY ATTEMPT 2
                    usersResponse.EnsureSuccessStatusCode();

                    // Create a DataTable to hold the user data
                    var dataTable = new DataTable();
                    dataTable.Columns.Add("UserId");
                    dataTable.Columns.Add("DisplayName");
                    dataTable.Columns.Add("Mail");

                    // Iterate through all users and add them to the DataTable
                    foreach (var user in users.EnumerateArray())
                    {
                        var userId = user.GetProperty("id").GetString();
                        var displayName = user.GetProperty("displayName").GetString();
                        var mail = user.GetProperty("mail").GetString();
                        dataTable.Rows.Add(userId, displayName, mail);

                        // Define the payload for the patch request if expiring passwrods
                    }
                    // Bind the DataTable to the DataGridView
                    GraphResultsDataGridView.DataSource = dataTable;
                    SignInCallToActionLabel.Hide();
                    GraphResultsPanel.Show();
                }
                if (!string.IsNullOrEmpty(textBox4.Text))
                {
                    var existingText = textBox4.Text;
                    //existingText = textBox4.Text;
                    var usersRequest = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/users/" + existingText + "&$select=id,displayName,mail,identities,otherMails");
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
                        // Parsing HTTP response code var httpResponseCode = (int)graphResponseMessage.StatusCode; HttpResponseCodeLabel.Text = $"HTTP Response Code: {httpResponseCode}"
                        // Hide the call to action and show the results.
                        SignInCallToActionLabel.Hide();
                        GraphResultsPanel.Show();
                    }
                    //NEW SHIT ATTEMPT // FUCKING MAKE THIS USE SAME REQUEST AS PRIOR !!!?????!???!? // EXPIREY ATTEMPT 2
                    usersResponse.EnsureSuccessStatusCode();

                    // Create a DataTable to hold the user data
                    var dataTable = new DataTable();
                    dataTable.Columns.Add("UserId");
                    dataTable.Columns.Add("DisplayName");
                    dataTable.Columns.Add("Mail");

                    // Iterate through all users and add them to the DataTable
                    foreach (var user in users.EnumerateArray())
                    {
                        var userId = user.GetProperty("id").GetString();
                        var displayName = user.GetProperty("displayName").GetString();
                        var mail = user.GetProperty("mail").GetString();
                        dataTable.Rows.Add(userId, displayName, mail);

                        // Define the payload for the patch request if expiring passwrods
                    }
                    // Bind the DataTable to the DataGridView
                    GraphResultsDataGridView.DataSource = dataTable;
                    SignInCallToActionLabel.Hide();
                    GraphResultsPanel.Show();
                }
            }
            if (checkBox1.Checked == false && checkBox2.Checked == true)
            {
                var usersRequest = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me?$select=id,displayName,mail,identities,otherMails");
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
                    // Parsing HTTP response code var httpResponseCode = (int)graphResponseMessage.StatusCode; HttpResponseCodeLabel.Text = $"HTTP Response Code: {httpResponseCode}"
                    // Hide the call to action and show the results.
                    SignInCallToActionLabel.Hide();
                    GraphResultsPanel.Show();

                }
                /*
                //NEW SHIT ATTEMPT // FUCKING MAKE THIS USE SAME REQUEST AS PRIOR !!!?????!???!? // EXPIREY ATTEMPT 2
                usersResponse.EnsureSuccessStatusCode();

                // Create a DataTable to hold the user data
                var dataTable = new DataTable();
                dataTable.Columns.Add("UserId");
                dataTable.Columns.Add("DisplayName");
                dataTable.Columns.Add("Mail");

                // Iterate through all users and add them to the DataTable
                foreach (var user in users.EnumerateArray())
                {
                    var userId = user.GetProperty("id").GetString();
                    var displayName = user.GetProperty("displayName").GetString();
                    var mail = user.GetProperty("mail").GetString();
                    dataTable.Rows.Add(userId, displayName, mail);

                    // Define the payload for the patch request if expiring passwrods
                }
                // Bind the DataTable to the DataGridView
                GraphResultsDataGridView.DataSource = dataTable;*/
                SignInCallToActionLabel.Hide();
                GraphResultsPanel.Show();
            }
        }
        // Find USERS by EMAIL
        private async void FindUser_Click(object sender, EventArgs e)
        {
            if (TenantID.Text != msalPublicClientApp.AppConfig.TenantId)
            {
                msalPublicClientApp = PublicClientApplicationBuilder.CreateWithApplicationOptions(new PublicClientApplicationOptions
                {
                    // Enter the tenant ID obtained from the Microsoft Entra admin center
                    TenantId = TenantID.Text,

                    // Enter the client ID obtained from the Microsoft Entra admin center
                    ClientId = ClientId.Text
                })
                .WithDefaultRedirectUri() // http://localhost
                .Build();
            }
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

            if (checkBox1.Checked == true && checkBox2.Checked == false)
            {
                var usersRequestUrl = "https://graph.microsoft.com/v1.0/users?$select=id,displayName,mail,identities,otherMails";
                var users = new List<JsonElement>();

                while (!string.IsNullOrEmpty(usersRequestUrl))
                {
                    var usersRequest = new HttpRequestMessage(HttpMethod.Get, usersRequestUrl);
                    usersRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", msalAuthenticationResult.AccessToken);
                    var usersResponse = await _httpClient.SendAsync(usersRequest);
                    usersResponse.EnsureSuccessStatusCode();
                    var usersJson = await usersResponse.Content.ReadAsStringAsync();
                    var usersDocument = JsonDocument.Parse(usersJson);
                    users.AddRange(usersDocument.RootElement.GetProperty("value").EnumerateArray());

                    usersRequestUrl = usersDocument.RootElement.TryGetProperty("@odata.nextLink", out var nextLink) ? nextLink.GetString() : null;

                }

                var dataTable = new DataTable();
                dataTable.Columns.Add("UserId");
                dataTable.Columns.Add("DisplayName");
                //dataTable.Columns.Add("Mail");
                dataTable.Columns.Add("Email");

                GraphResultsDataGridView.DataSource = dataTable;
                SignInCallToActionLabel.Hide();
                GraphResultsPanel.Show();

                foreach (var user in users)
                {
                    var userId = user.GetProperty("id").GetString();
                    var displayName = user.GetProperty("displayName").GetString();
                    var mail = user.GetProperty("mail").GetString();
                    var identities = user.GetProperty("identities").EnumerateArray();
                    
                    foreach (var identity in identities)
                    {
                        if (identity.GetProperty("signInType").GetString() == "emailAddress")
                        {
                            var issuerAssignedId = identity.GetProperty("issuerAssignedId").GetString();
                            dataTable.Rows.Add(userId, displayName, issuerAssignedId); //mail, 
                        }
                    }

                    var graphRequest = new HttpRequestMessage(HttpMethod.Get, $"https://graph.microsoft.com/v1.0/users/{userId}");
                    graphRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", msalAuthenticationResult.AccessToken);
                    var graphResponseMessage = await _httpClient.SendAsync(graphRequest);

                    if (graphResponseMessage.StatusCode == HttpStatusCode.NoContent)
                    {
                        GraphResultsTextBox.Text += userId;
                    }
                    else
                    {
                        using var graphResponseJson = JsonDocument.Parse(await graphResponseMessage.Content.ReadAsStreamAsync());
                        GraphResultsTextBox.Text += userId + System.Text.Json.JsonSerializer.Serialize(graphResponseJson, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }) + "\n";
                    }
                    var tokenWasFromCache = TokenSource.Cache == msalAuthenticationResult.AuthenticationResultMetadata.TokenSource;
                    AccessTokenSourceLabel.Text = $"{(tokenWasFromCache ? "Cached" : "Newly Acquired")} (Expires: {msalAuthenticationResult.ExpiresOn:R})";
                }
                SignInCallToActionLabel.Hide();
                GraphResultsPanel.Show();
            }

            //NEW SHIT ATTEMPT // FUCKING MAKE THIS USE SAME REQUEST AS PRIOR !!!?????!???!? || THISS WORKS BUT EVERY OTHER REQUEST FUCKING DIES AAAAAAAAAA
            if (checkBox1.Checked == false && checkBox2.Checked == false)
            {
                var IssueEnv = "'ReferallProduction.onmicrosoft'";
                if (TenantID.Text == "2fdbaf70-405c-420d-81e6-0d5391cd6245")
                {
                    IssueEnv = "'StagingReferall.onmicrosoft'";
                }
                if (!string.IsNullOrEmpty(textBox3.Text))
                {
                    var email = textBox3.Text;
                    // Call Microsoft Graph using the access token acquired above.
                    using var graphRequest = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/users?$filter=identities/any(id:id/issuerAssignedId eq " + "'" + email + "'" + " and id/issuer eq " + IssueEnv + ")&$select=id,displayName,mail,identities,otherMails");// + email);
                                                                                                                                                                                                                                                                                           //https://graph.microsoft.com/beta/tenant.onmicrosoft.com/users?$filter=(identities/any(i:i/issuer eq 'tenant.onmicrosoft.com' and i/issuerAssignedId eq 'johnsmith'))
                    graphRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", msalAuthenticationResult.AccessToken);
                    var graphResponseMessage = await _httpClient.SendAsync(graphRequest);
                    graphResponseMessage.EnsureSuccessStatusCode();
                    //graphResponseMessage.EnsureSuccessStatusCode();

                    // Present the results to the user (formatting the json for readability)
                    using var graphResponseJson = JsonDocument.Parse(await graphResponseMessage.Content.ReadAsStreamAsync());
                    GraphResultsTextBox.Text += System.Text.Json.JsonSerializer.Serialize(graphResponseJson, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                    var tokenWasFromCache = TokenSource.Cache == msalAuthenticationResult.AuthenticationResultMetadata.TokenSource;
                    AccessTokenSourceLabel.Text = $"{(tokenWasFromCache ? "Cached" : "Newly Acquired")} (Expires: {msalAuthenticationResult.ExpiresOn:R})";

                    //NEW SHIT ATTEMPT // FUCKING MAKE THIS USE SAME REQUEST AS PRIOR !!!?????!???!?
                    graphResponseMessage.EnsureSuccessStatusCode();
                    var usersJson = await graphResponseMessage.Content.ReadAsStringAsync();
                    var users = JsonDocument.Parse(usersJson).RootElement.GetProperty("value");


                    // Create a DataTable to hold the user data
                    var dataTable = new DataTable();
                    dataTable.Columns.Add("UserId");
                    dataTable.Columns.Add("DisplayName");
                    dataTable.Columns.Add("Mail");

                    // Iterate through all users and add them to the DataTable
                    foreach (var user in users.EnumerateArray())
                    {
                        var userId = user.GetProperty("id").GetString();
                        var displayName = user.GetProperty("displayName").GetString();
                        var mail = user.GetProperty("mail").GetString();
                        dataTable.Rows.Add(userId, displayName, mail);

                        // Define the payload for the patch request if expiring passwrods
                    }
                    // Bind the DataTable to the DataGridView
                    GraphResultsDataGridView.DataSource = dataTable;
                    SignInCallToActionLabel.Hide();
                    GraphResultsPanel.Show();

                }
                if (!string.IsNullOrEmpty(textBox4.Text))
                {
                    var objectID = textBox4.Text;
                    // Call Microsoft Graph using the access token acquired above.
                    using var graphRequest = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/users/" + objectID + "?$select=id,displayName,mail,identities,otherMails");
                    //https://graph.microsoft.com/beta/tenant.onmicrosoft.com/users?$filter=(identities/any(i:i/issuer eq 'tenant.onmicrosoft.com' and i/issuerAssignedId eq 'johnsmith'))
                    graphRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", msalAuthenticationResult.AccessToken);
                    var graphResponseMessage = await _httpClient.SendAsync(graphRequest);
                    //graphResponseMessage.EnsureSuccessStatusCode();

                    // Present the results to the user (formatting the json for readability)
                    using var graphResponseJson = JsonDocument.Parse(await graphResponseMessage.Content.ReadAsStreamAsync());
                    GraphResultsTextBox.Text += System.Text.Json.JsonSerializer.Serialize(graphResponseJson, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                    var tokenWasFromCache = TokenSource.Cache == msalAuthenticationResult.AuthenticationResultMetadata.TokenSource;
                    AccessTokenSourceLabel.Text = $"{(tokenWasFromCache ? "Cached" : "Newly Acquired")} (Expires: {msalAuthenticationResult.ExpiresOn:R})";

                    //NEW SHIT ATTEMPT // FUCKING MAKE THIS USE SAME REQUEST AS PRIOR !!!?????!???!?
                    graphResponseMessage.EnsureSuccessStatusCode();
                    //var usersJson = await graphResponseMessage.Content.ReadAsStringAsync();
                    var users = graphResponseJson.RootElement; // FUCK COLLECTIONS :)

                    // Create a DataTable to hold the user data
                    var dataTable = new DataTable();
                    dataTable.Columns.Add("UserId");
                    dataTable.Columns.Add("DisplayName");
                    dataTable.Columns.Add("Mail");

                        var userId = users.GetProperty("id").GetString();
                        var displayName = users.GetProperty("displayName").GetString();
                        var mail = users.GetProperty("mail").GetString();
                        dataTable.Rows.Add(userId, displayName, mail);

                    // Bind the DataTable to the DataGridView
                    GraphResultsDataGridView.DataSource = dataTable;
                    SignInCallToActionLabel.Hide();
                    GraphResultsPanel.Show();
                }
            }
            if (checkBox1.Checked == false && checkBox2.Checked == true)
            {
                // Call Microsoft Graph using the access token acquired above.
                using var graphRequest = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me");// + email);
                                                                                                                       //https://graph.microsoft.com/beta/tenant.onmicrosoft.com/users?$filter=(identities/any(i:i/issuer eq 'tenant.onmicrosoft.com' and i/issuerAssignedId eq 'johnsmith'))
                graphRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", msalAuthenticationResult.AccessToken);
                var graphResponseMessage = await _httpClient.SendAsync(graphRequest);
                //graphResponseMessage.EnsureSuccessStatusCode();

                // Present the results to the user (formatting the json for readability)
                using var graphResponseJson = JsonDocument.Parse(await graphResponseMessage.Content.ReadAsStreamAsync());
                GraphResultsTextBox.Text = System.Text.Json.JsonSerializer.Serialize(graphResponseJson, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                var tokenWasFromCache = TokenSource.Cache == msalAuthenticationResult.AuthenticationResultMetadata.TokenSource;
                AccessTokenSourceLabel.Text = $"{(tokenWasFromCache ? "Cached" : "Newly Acquired")} (Expires: {msalAuthenticationResult.ExpiresOn:R})";

                //NEW SHIT ATTEMPT // FUCKING MAKE THIS USE SAME REQUEST AS PRIOR !!!?????!???!?
                /* graphResponseMessage.EnsureSuccessStatusCode();
                var usersJson = await graphResponseMessage.Content.ReadAsStringAsync();
                var users = JsonDocument.Parse(usersJson).RootElement.GetProperty("value");


                // Create a DataTable to hold the user data
                var dataTable = new DataTable();
                dataTable.Columns.Add("UserId");
                dataTable.Columns.Add("DisplayName");
                dataTable.Columns.Add("Mail");

                // Iterate through all users and add them to the DataTable
                foreach (var user in users.EnumerateArray())
                {
                    var userId = user.GetProperty("id").GetString();
                    var displayName = user.GetProperty("displayName").GetString();
                    var mail = user.GetProperty("mail").GetString();
                    dataTable.Rows.Add(userId, displayName, mail);

                    // Define the payload for the patch request if expiring passwrods
                }
                // Bind the DataTable to the DataGridView
                GraphResultsDataGridView.DataSource = dataTable;*/
                SignInCallToActionLabel.Hide();
                GraphResultsPanel.Show();
            }


            if (checkBox1.Checked == false && checkBox2.Checked == false && !string.IsNullOrEmpty(textBox5.Text))
            {
                // https://developer.microsoft.com/en-us/graph/graph-explorer https://graph.microsoft.com/v1.0/users?$count=true&$search="displayName:room"&$filter=endsWith(mail,'microsoft.com')&$orderBy=displayName&$select=id,displayName,mail
                using var graphRequest = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/beta/users?$count=true&$filter=endsWith(mail,'" + textBox5.Text + "')&$orderBy=displayName&$select=id,displayName,mail");

                graphRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", msalAuthenticationResult.AccessToken);
                var graphResponseMessage = await _httpClient.SendAsync(graphRequest);
                //graphResponseMessage.EnsureSuccessStatusCode();

                // Present the results to the user (formatting the json for readability)
                using var graphResponseJson = JsonDocument.Parse(await graphResponseMessage.Content.ReadAsStreamAsync());
                GraphResultsTextBox.Text = System.Text.Json.JsonSerializer.Serialize(graphResponseJson, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                var tokenWasFromCache = TokenSource.Cache == msalAuthenticationResult.AuthenticationResultMetadata.TokenSource;
                AccessTokenSourceLabel.Text = $"{(tokenWasFromCache ? "Cached" : "Newly Acquired")} (Expires: {msalAuthenticationResult.ExpiresOn:R})";


                //NEW SHIT ATTEMPT // FUCKING MAKE THIS USE SAME REQUEST AS PRIOR !!!?????!???!?
                graphResponseMessage.EnsureSuccessStatusCode();
                var usersJson = await graphResponseMessage.Content.ReadAsStringAsync();
                var users = JsonDocument.Parse(usersJson).RootElement.GetProperty("value");


                // Create a DataTable to hold the user data
                var dataTable = new DataTable();
                dataTable.Columns.Add("UserId");
                dataTable.Columns.Add("DisplayName");
                dataTable.Columns.Add("Mail");

                // Iterate through all users and add them to the DataTable
                foreach (var user in users.EnumerateArray())
                {
                    var userId = user.GetProperty("id").GetString();
                    var displayName = user.GetProperty("displayName").GetString();
                    var mail = user.GetProperty("mail").GetString();
                    dataTable.Rows.Add(userId, displayName, mail);

                    // Define the payload for the patch request if expiring passwrods
                }
                // Bind the DataTable to the DataGridView
                GraphResultsDataGridView.DataSource = dataTable;
                SignInCallToActionLabel.Hide();
                GraphResultsPanel.Show();
            }
            SignInCallToActionLabel.Hide();
            GraphResultsPanel.Show();


        }

        public async void Delete_click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                GraphResultsTextBox.Text = "For our Safety I have disabled the ability to delete ALL users in one of our tenants.";
            }
            if (checkBox2.Checked == true)
            {
                GraphResultsTextBox.Text = "For Your Safety I have disabled the ability to delete-suicide";
            }
            if (!string.IsNullOrEmpty(textBox5.Text))
            {
                GraphResultsTextBox.Text = "I have disabled bulk deletion by domain until it can be made safer";
            }
            AuthenticationResult? msalAuthenticationResult = null;
            var accounts = await msalPublicClientApp.GetAccountsAsync();
            if (accounts.Any())
            {
                try
                {
                    msalAuthenticationResult = await msalPublicClientApp.AcquireTokenSilent(
                        new[] { "https://graph.microsoft.com/User.Read" },
                        accounts.First())
                        .ExecuteAsync();
                }
                catch (MsalUiRequiredException)
                {
                }
            }

            if (msalAuthenticationResult == null)
            {
                msalAuthenticationResult = await msalPublicClientApp.AcquireTokenInteractive(
                    new[] { "https://graph.microsoft.com/User.Read" })
                    .ExecuteAsync();
            }
            if (!string.IsNullOrEmpty(textBox4.Text)) // USE RECURSIVE PATCHING WHEN IT COMES TO FIXING
            {
                using var graphRequest = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/beta/users/" + textBox4.Text);

                graphRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", msalAuthenticationResult.AccessToken);
                var graphResponseMessage = await _httpClient.SendAsync(graphRequest);

                using var graphResponseJson = JsonDocument.Parse(await graphResponseMessage.Content.ReadAsStreamAsync());
                GraphResultsTextBox.Text = System.Text.Json.JsonSerializer.Serialize(graphResponseJson, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                var tokenWasFromCache = TokenSource.Cache == msalAuthenticationResult.AuthenticationResultMetadata.TokenSource;
                AccessTokenSourceLabel.Text = $"{(tokenWasFromCache ? "Cached" : "Newly Acquired")} (Expires: {msalAuthenticationResult.ExpiresOn:R})";

                //NEW SHIT ATTEMPT // FUCKING MAKE THIS USE SAME REQUEST AS PRIOR !!!?????!???!?
                graphResponseMessage.EnsureSuccessStatusCode();
                var usersJson = await graphResponseMessage.Content.ReadAsStringAsync();
                var users = JsonDocument.Parse(usersJson).RootElement.GetProperty("value");


                // Create a DataTable to hold the user data
                var dataTable = new DataTable();
                dataTable.Columns.Add("UserId");
                dataTable.Columns.Add("DisplayName");
                dataTable.Columns.Add("Mail");

                // Iterate through all users and add them to the DataTable
                foreach (var user in users.EnumerateArray())
                {
                    var userId = user.GetProperty("id").GetString();
                    var displayName = user.GetProperty("displayName").GetString();
                    var mail = user.GetProperty("mail").GetString();
                    dataTable.Rows.Add(userId, displayName, mail);

                    // Define the payload for the patch request if expiring passwrods
                }
                // Bind the DataTable to the DataGridView
                GraphResultsDataGridView.DataSource = dataTable;
                SignInCallToActionLabel.Hide();
                GraphResultsPanel.Show();

            }
            if (!string.IsNullOrEmpty(textBox3.Text))
            {
                using var graphRequest = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/beta/users/" + textBox3.Text);

                graphRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", msalAuthenticationResult.AccessToken);
                var graphResponseMessage = await _httpClient.SendAsync(graphRequest);

                using var graphResponseJson = JsonDocument.Parse(await graphResponseMessage.Content.ReadAsStreamAsync());
                GraphResultsTextBox.Text = System.Text.Json.JsonSerializer.Serialize(graphResponseJson, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                var tokenWasFromCache = TokenSource.Cache == msalAuthenticationResult.AuthenticationResultMetadata.TokenSource;
                AccessTokenSourceLabel.Text = $"{(tokenWasFromCache ? "Cached" : "Newly Acquired")} (Expires: {msalAuthenticationResult.ExpiresOn:R})";

                //NEW SHIT ATTEMPT // FUCKING MAKE THIS USE SAME REQUEST AS PRIOR !!!?????!???!?
                graphResponseMessage.EnsureSuccessStatusCode();
                var usersJson = await graphResponseMessage.Content.ReadAsStringAsync();
                var users = JsonDocument.Parse(usersJson).RootElement.GetProperty("value");


                // Create a DataTable to hold the user data
                var dataTable = new DataTable();
                dataTable.Columns.Add("UserId");
                dataTable.Columns.Add("DisplayName");
                dataTable.Columns.Add("Mail");

                // Iterate through all users and add them to the DataTable
                foreach (var user in users.EnumerateArray())
                {
                    var userId = user.GetProperty("id").GetString();
                    var displayName = user.GetProperty("displayName").GetString();
                    var mail = user.GetProperty("mail").GetString();
                    dataTable.Rows.Add(userId, displayName, mail);

                    // Define the payload for the patch request if expiring passwrods
                }
                // Bind the DataTable to the DataGridView
                GraphResultsDataGridView.DataSource = dataTable;
                SignInCallToActionLabel.Hide();
                GraphResultsPanel.Show();
            }
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
            applicationOptions.TenantId = TenantID.Text;
        }

        private void checkbox_click(object sender, EventArgs e) // THIS IS STATE CHANGED NOT CLICK YOU RETARD // REWRITE ALL RULES, DOUBLING LOGIC FOR NO REASON - IDIOT!
        {
            if (checkBox1.Checked == true)
            {
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                checkBox2.Checked = false;
            }
            if (checkBox2.Checked == false && checkBox1.Checked == false)
            {
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                //textBox5.Enabled = true;
            }
            if (checkBox2.Checked == true && checkBox1.Checked == false) // Hides Self Password Expiry on Production.
            {
                if (TenantID.Text == Production) // || TenantID.Text == Staging
                {
                    ExpirePasswords.Enabled = false;
                }
            }
            if (TenantID.Text == Staging)
            {
                ExpirePasswords.Enabled = true;
            }
            if (TenantID.Text == Production && checkBox1.Checked == true)
            {
                //ExpirePasswords.Enabled = false; // Remove this line to Expire All Passwords
            }
            if (TenantID.Text == Production && checkBox1.Checked == false && checkBox2.Checked == false)
            {
                ExpirePasswords.Enabled = true;
            }
        }

        private void checkbox2_click(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                checkBox1.Checked = false;
                //ExpirePasswords.Enabled = true;
            }
            if (checkBox2.Checked == false && checkBox1.Checked == false)
            {
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                //ExpirePasswords.Enabled = true;
                //textBox5.Enabled = true;
            }
            if (TenantID.Text == Production && checkBox2.Checked == true)
            {
                ExpirePasswords.Enabled = false;
            }
            if (TenantID.Text == Production && checkBox2.Checked == false)
            {
                ExpirePasswords.Enabled = true;
            }
            if (TenantID.Text == Staging)
            {
                ExpirePasswords.Enabled = true;
            }
            if (TenantID.Text == Production && checkBox2.Checked == true)
            {
                ExpirePasswords.Enabled = false;
            }
        }

        private void button2_Click(ApplicationOptions applicationOptions)
        {
            applicationOptions.ClientId = ClientId.Text;
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
            LookupUser.Enabled = true;
            ExpirePasswords.Enabled = true;
            if (TenantID.Text == Production && checkBox1.Checked == true)
            {
                //ExpirePasswords.Enabled = false; // DISABLE FOR DANGEROUS BUILD
            }
            if (TenantID.Text == Production && checkBox2.Checked == true)
            {
                ExpirePasswords.Enabled = false;
            }
            if (checkBox1.Checked == true && checkBox2.Checked == true) // ????? ANY POINT? NON POSSIBLE STATE!
            {
                ExpirePasswords.Enabled = false;
            }
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

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void Safety_CheckedChanged(object sender, EventArgs e)
        {
            ExpirePasswords.Enabled = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }

}
