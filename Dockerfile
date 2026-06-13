FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/EnviosRapidosGT.Api/EnviosRapidosGT.Api.csproj", "src/EnviosRapidosGT.Api/"]
RUN dotnet restore "src/EnviosRapidosGT.Api/EnviosRapidosGT.Api.csproj"

COPY . .
RUN dotnet publish "src/EnviosRapidosGT.Api/EnviosRapidosGT.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EnviosRapidosGT.Api.dll"]
