# ToDo
- [x] Make Invocable Client Tenant ID strings for building connection arg.<br>
- [x] Remove 'Domain' until it can be used on B2C tenants <br>
- [ ] Add Main nonb2c tenant.<br>
- [x] Enter IDs prior to creating PublicClientApplicationBuilder<br>
- [x] Make Application 'Safer'

# .NET | Windows Forms | user sign-in, protected web API access (Microsoft Graph) | Microsoft identity platform

This .NET Windows Forms application authenticates a user and then makes a request to the Graph API as the authenticated user. The response to the request is presented to the user.

![A screenshot of a Windows Forms application displaying a response from Microsoft Graph.](./app.png)

## Prerequisites

- Microsoft Entra tenant and the permissions or role required for managing app registrations in the tenant.
- Visual Studio 2022, [configured for the .NET 8 desktop development workload](https://docs.microsoft.com/dotnet/desktop/winforms/get-started/create-app-visual-studio?view=netdesktop-8.0#prerequisites)

## Setup

### 1. Register the app

First, complete the steps in [Register an application with the Microsoft identity platform](https://docs.microsoft.com/azure/active-directory/develop/quickstart-register-app) to register the application.

Use these settings in your app registration.

| App registration <br/> setting  | Value for this sample app                                           | Notes                                                                           |
|--------------------------------:|:--------------------------------------------------------------------|:--------------------------------------------------------------------------------|
| **Name**                        | `dotnet-winforms`                                                   | Suggested value for this sample. <br/> You can change the app name at any time. |
| **Supported account types**     | **Accounts in this organizational directory only (Single tenant)**  | Suggested value for this sample.                                                |
| **Platform type**               | **Mobile and desktop applications**                                 | Required value for this sample                                                  |
| **Redirect URIs**               | `http://localhost`                                                  | Required value for this sample                                                  |

> :information_source: **Bold text** in the tables above matches (or is similar to) a UI element in the Microsoft Entra admin center, while `code formatting` indicates a value you enter into a text box in the Microsoft Entra admin center.

### 2. Open the project in Visual Studio

Next, open the _ReferAll Graph Toolbox.csproj_ project in Visual Studio.

### 3. Update code sample in _MainWindow.cs_ with app registration values

Finally, set the following values in _MainWindow.cs_.

<!--```csharp
// Enter the tenant ID obtained from the Microsoft Entra admin center
TenantId = "Enter the client ID obtained from the Microsoft Entra admin center",

// Enter the client ID obtained from the Microsoft Entra admin center
ClientId = "Enter the tenant ID obtained from the Microsoft Entra admin center"
```-->

## Run the application

Run the application by pressing **F5** in Visual Studio.

Enter both the ClientID & TenantID in the fields, where TenantID is the B2C / AD tenant and the Client is the object ID of the application you registered.

The appliction will open allowing you to click the **Reset Passwords** button to use the authentication flow.

![A screenshot of a Windows Forms application guiding the user to click the "Expire Passwords" button.](./app-launch.png)

## About the code

This .NET 8 Windows Forms application presents a button that initiates an authentication flow using the Microsoft Authentication Library (MSAL). The user completes this flow in their default web browser. Upon successful authentication, an HTTP GET request to the Microsoft Graph /me endpoint is issued with the user's access token in the HTTP header. The response from the GET request is then displayed to the user. The MSAL client first looks to its token cache, refreshing if necessary, before acquiring a new access token.
