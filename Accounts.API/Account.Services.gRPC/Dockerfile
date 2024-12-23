# Use the official .NET SDK image as a build environment
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory inside the container
WORKDIR /src

COPY ["/Account.Services.gRPC/Account.Services.gRPC.csproj", "Account.Services.gRPC/"]
COPY ["/Accounts.CrossCutting/Accounts.CrossCutting.csproj", "Accounts.CrossCutting/"]

RUN dotnet restore "Account.Services.gRPC/Account.Services.gRPC.csproj"

COPY . .

# Build the application
WORKDIR /src/Account.Services.gRPC
RUN dotnet build "Account.Services.gRPC.csproj" -c Release -o /app/build

# Publish the application (output is ready to run in the final container)
RUN dotnet publish "Account.Services.gRPC.csproj" -c Release -o /app/publish

# Use the official .NET Runtime image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

# Set the working directory in the container
WORKDIR /app

# Copy the published application from the build environment
COPY --from=build /app/publish .

# Expose the port the app will listen on (default for gRPC is 5001)
EXPOSE 5001

# Define the entry point for the container
ENTRYPOINT ["dotnet", "Account.Services.gRPC.dll"]