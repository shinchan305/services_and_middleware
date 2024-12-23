# Use the official .NET SDK image as a build environment
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory inside the container
WORKDIR /src

COPY ["/NotificationService2/NotificationService2.csproj", "NotificationService2/"]
COPY ["/Accounts.CrossCutting/Accounts.CrossCutting.csproj", "Accounts.CrossCutting/"]

RUN dotnet restore "NotificationService2/NotificationService2.csproj"

COPY . .

WORKDIR /src/NotificationService2
RUN dotnet build "NotificationService2.csproj" -c Release -o /app/build

RUN dotnet publish "NotificationService2.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "NotificationService2.dll"]