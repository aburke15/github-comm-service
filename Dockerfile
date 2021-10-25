# syntax=docker/dockerfile:1 Learn more about the "FROM" Dockerfile command.
FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build-env
WORKDIR /app

# Copy csproj, config, and restore packages as distinct layers
COPY *.csproj *.config ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal
WORKDIR /app
COPY --from=build-env /app/out .
ENV CONNNECTION_STRING=""
ENTRYPOINT ["dotnet", "GitHubCommunicationService.dll", "$CONNNECTION_STRING"]