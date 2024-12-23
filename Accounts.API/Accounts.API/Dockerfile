# Use the official .NET SDK image as a build environment
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ENV ASPNETCORE_ENVIRONMENT=Development

# Set the working directory inside the container
WORKDIR /src

COPY ["/Accounts.API/Accounts.API.csproj", "Accounts.API/"]
COPY ["/Accounts.CrossCutting/Accounts.CrossCutting.csproj", "Accounts.CrossCutting/"]
COPY ["/Accounts.Services/Accounts.Services.csproj", "Accounts.Services/"]

RUN dotnet restore "Accounts.API/Accounts.API.csproj"

COPY . .

WORKDIR /src/Accounts.API
RUN dotnet build "Accounts.API.csproj" -c Release -o /app/build

RUN dotnet publish "Accounts.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "Accounts.API.dll"]