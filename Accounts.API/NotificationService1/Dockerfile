# Use the official .NET SDK image as a build environment
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory inside the container
WORKDIR /src

COPY ["/NotificationService1/NotificationService1.csproj", "NotificationService1/"]
COPY ["/Accounts.CrossCutting/Accounts.CrossCutting.csproj", "Accounts.CrossCutting/"]

RUN dotnet restore "NotificationService1/NotificationService1.csproj"

COPY . .

WORKDIR /src/NotificationService1
RUN dotnet build "NotificationService1.csproj" -c Release -o /app/build

RUN dotnet publish "NotificationService1.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "NotificationService1.dll"]