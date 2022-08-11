# Password Purgatory for .NET

This is Troy Hunt's [Password Purgatory](https://www.troyhunt.com/building-password-purgatory-with-cloudflare-pages-and-workers/), rewritten as a .NET set of applications published to Azure. It is not a 1:1 conversation of Troy's code, but maintains its spirit.

Build Statuses

| Project           | Status     |
|-------------------|------------|
| API               | [![Publish Azure Function](https://github.com/LanceMcCarthy/dotnet-password-purgatory/actions/workflows/publish-azure-functions.yml/badge.svg)](https://github.com/LanceMcCarthy/dotnet-password-purgatory/actions/workflows/publish-azure-functions.yml) |
| Blazor App: [https://partnership.dvlup.com](https://partnership.dvlup.com) |  [![Publish Azure Web App](https://github.com/LanceMcCarthy/dotnet-password-purgatory/actions/workflows/publish-azure-webapp.yml/badge.svg)](https://github.com/LanceMcCarthy/dotnet-password-purgatory/actions/workflows/publish-azure-webapp.yml)   |

## Workflow

In my Junk email folder in O365, I have a subfolder named "Spammer Hell". Using [Microsoft Flow](https://flow.microsoft.com), I created a rule that will automatically reply to any email that was just moved to the folder with the following message:

![image](https://user-images.githubusercontent.com/3520532/184170349-108a3665-594b-4673-ba1e-dbc8c05c892b.png)