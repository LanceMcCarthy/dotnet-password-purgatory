# Password Purgatory for .NET

This is Troy Hunt's [Password Purgatory](https://www.troyhunt.com/building-password-purgatory-with-cloudflare-pages-and-workers/), rewritten as a .NET set of applications published to Azure. It is not a 1:1 conversation of Troy's code, but maintains its spirit.

Build Statuses

| Project           | Status     |
|-------------------|------------|
| Blazor App: [https://partnership.dvlup.com](https://partnership.dvlup.com) |  [![Deploy to Azure Container App](https://github.com/LanceMcCarthy/dotnet-password-purgatory/actions/workflows/publish-az-container-app.yml/badge.svg)](https://github.com/LanceMcCarthy/dotnet-password-purgatory/actions/workflows/publish-az-container-app.yml)   |

## User Experience 😈

The spammer lands on the page, after a promise that they're going to get a nice "downpayment" in cash for their services.

![image](https://user-images.githubusercontent.com/3520532/184453421-6170199c-6b07-4cf4-8893-949a8a4f7f26.png)

1. ![image](https://user-images.githubusercontent.com/3520532/184453467-4962c6c2-d955-4270-93d8-8ecb02c26fa8.png)
    * First entry form seems normal, some standard client-side validation to make sure I complete both fields.
2. ![image](https://user-images.githubusercontent.com/3520532/184453247-50578c2f-6fbb-4326-aff2-fb0bebbb2412.png)
    * Okay, so I didn't have a number in my password...
3. ![image](https://user-images.githubusercontent.com/3520532/184453267-eab6d8e0-945b-4cfb-b0b3-7c343711c0dc.png)
    * Whoops, maybe I shouldn't have just jammed a "1" on the end of the password...
5. ![image](https://user-images.githubusercontent.com/3520532/184453272-774a05a3-21ac-4aba-8754-6c4e8e66e3e3.png)
    * Ugh, okay, an upper-case character (starting to feel annoyed).
6. ![image](https://user-images.githubusercontent.com/3520532/184453276-464481ef-e3a7-4347-847f-8d45edfb1921.png)
    * This seems my own fault, nothing lets you use short password anymore.
7. ![image](https://user-images.githubusercontent.com/3520532/184453281-36502559-19f7-464d-8e8f-a4e57a87ed28.png)
    * Wait, what? This is starting to seem like I'm being given the run-around... but adding 'dog' to the end is easy enough...
8. ![image](https://user-images.githubusercontent.com/3520532/184453293-b1e46b2f-7f54-4868-b7c6-13ed8def1ac7.png)
    * Really, cat? Okay, three letters, lets add it and keep going because I want to see where this is going.
9. ![image](https://user-images.githubusercontent.com/3520532/184453301-b28f9136-a510-4c65-a186-7e4aac0f0ec4.png)
    * JFK, really? 
10. If they keep going, it gets _significantly_ worst

They better be up to date on their Simpsons and Family Guy characters, know some Greek and German alphabet, and paid attention in elementary school science (about the solar system). Let's see just how far they're really willing to go, because I am logging every attempt.


## Automatic Triggered Workflow

In my Junk email folder in O365, I have a subfolder named "Spammer Hell". Using [Microsoft Flow](https://flow.microsoft.com), I created a rule that will automatically reply to any email that was just moved to the folder with the following message:

![image](https://user-images.githubusercontent.com/3520532/184170349-108a3665-594b-4673-ba1e-dbc8c05c892b.png)
