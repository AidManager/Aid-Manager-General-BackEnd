FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar el archivo de proyecto primero
COPY ["AidManager.API.csproj", "."]
RUN dotnet restore "AidManager.API.csproj"

# Copiar todo el código fuente
COPY . .
RUN dotnet build "AidManager.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AidManager.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Usar el puerto de la variable de entorno PORT para Render
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "AidManager.API.dll"]