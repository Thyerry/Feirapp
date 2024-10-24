FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Feirapp.API/Feirapp.API.csproj", "Feirapp.API/"]
COPY ["Feirapp.Domain/Feirapp.Domain.csproj", "Feirapp.Domain/"]
COPY ["Feirapp.Entities/Feirapp.Entities.csproj", "Feirapp.Entities/"]
COPY ["Feirapp.Infrastructure/Feirapp.Infrastructure.csproj", "Feirapp.Infrastructure/"]
RUN dotnet restore "Feirapp.API/Feirapp.API.csproj"
COPY . .
WORKDIR "/src/Feirapp.API"
RUN dotnet build "Feirapp.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Feirapp.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY Feirapp.API/certificate.pfx ./
ENTRYPOINT ["dotnet", "Feirapp.API.dll"]