# Use the official .NET SDK image as a build environment
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory inside the container
WORKDIR /src

COPY ["/PDFService/PDFService.csproj", "PDFService/"]

RUN dotnet restore "PDFService/PDFService.csproj"

COPY . .

WORKDIR /src/PDFService
RUN dotnet build "PDFService.csproj" -c Release -o /app/build

RUN dotnet publish "PDFService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "PDFService.dll"]