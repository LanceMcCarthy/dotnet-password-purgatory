FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN --mount=type=secret,id=telerik_key \
  dotnet nuget add source 'https://nuget.telerik.com/v3/index.json' -n "TelerikNuGetServer" -u "api-key" -p $(cat /run/secrets/telerik_key) --store-password-in-clear-text
RUN dotnet restore "./PasswordPurgatory.Web.csproj"
RUN dotnet build "./PasswordPurgatory.Web.csproj" -c Release -o /app/build --no-restore


FROM build AS publish
RUN dotnet publish "./PasswordPurgatory.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore --self-contained false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PasswordPurgatory.Web.dll"]