FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Asclepius.AuthService.Api/Asclepius.AuthService.Api.csproj", "Asclepius.AuthService.Api/"]
RUN dotnet restore "Asclepius.AuthService.Api/Asclepius.AuthService.Api.csproj"
COPY . .
WORKDIR "/src/Asclepius.AuthService.Api"
RUN dotnet build "Asclepius.AuthService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Asclepius.AuthService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Asclepius.AuthService.Api.dll"]
