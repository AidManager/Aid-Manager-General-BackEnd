FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# 1. Copia el archivo de proyecto (csproj) desde su subdirectorio 'AidManager.API/'
#    al directorio AidManager.API/ dentro del contenedor.
COPY ["AidManager.API/AidManager.API.csproj", "AidManager.API/"]

# 2. Restaura las dependencias (dentro del subdirectorio AidManager.API/)
RUN dotnet restore "AidManager.API/AidManager.API.csproj"

# 3. Copia todo el código fuente al contenedor
COPY . . 

# 4. Construye el proyecto (usando la ruta completa dentro del contenedor)
RUN dotnet build "AidManager.API/AidManager.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
# Publica el proyecto (usando la ruta completa dentro del contenedor)
RUN dotnet publish "AidManager.API/AidManager.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Usar el puerto de la variable de entorno PORT para Render (o Railway)
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "AidManager.API.dll"]
