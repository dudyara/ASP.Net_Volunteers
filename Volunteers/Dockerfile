# SDK image used to build and publish the application
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

ARG Configuration=Release
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true \
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true

WORKDIR /src

COPY Volunteers.sln ./Volunteers.sln
COPY Volunteers.API/Volunteers.API.csproj ./Volunteers.API/Volunteers.API.csproj
COPY Volunteers.DB/Volunteers.DB.csproj ./Volunteers.DB/Volunteers.DB.csproj
COPY Volunteers.Entities/Volunteers.Entities.csproj ./Volunteers.Entities/Volunteers.Entities.csproj
COPY Volunteers.Services/Volunteers.Services.csproj ./Volunteers.Services/Volunteers.Services.csproj
COPY Volunteers.Tests/Volunteers.Tests.csproj ./Volunteers.Tests/Volunteers.Tests.csproj
RUN dotnet restore
COPY . .
RUN dotnet build --configuration $Configuration --no-restore /p:TreatWarningsAsErrors=True
RUN dotnet publish "Volunteers.API/Volunteers.API.csproj" --configuration $Configuration --no-build --output /app

# Runtime image used to run the application
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS runtime
WORKDIR /app
COPY --from=build /app ./
ENV ASPNETCORE_URLS=http://*:8080
ENTRYPOINT ["dotnet", "Volunteers.API.dll"]
