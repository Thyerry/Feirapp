FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Feirapp.API/Feirapp.API.csproj", "Feirapp.API/"]
COPY ["Feirapp.Domain/Feirapp.Domain.csproj", "Feirapp.Domain/"]
COPY ["Feirapp.Infrastructure/Feirapp.Infrastructure.csproj", "Feirapp.Infrastructure/"]
COPY ["Feirapp.Entities/Feirapp.DocumentModels.csproj", "Feirapp.DocumentModels/"]
RUN dotnet restore "Feirapp.API/Feirapp.API.csproj"
COPY . .
WORKDIR "/src/Feirapp.API"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Feirapp.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "Feirapp.API.dll"]