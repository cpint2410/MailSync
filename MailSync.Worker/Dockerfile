FROM mcr.microsoft.com/dotnet/sdk:11.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish MailSync.Worker/MailSync.Worker.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:11.0 AS base
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MailSync.Worker.dll"]