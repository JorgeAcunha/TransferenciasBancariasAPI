# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

#  Instala herramientas PostgreSQL
RUN apt-get update && apt-get install -y postgresql-client

# Cambiar el entrypoint
ENTRYPOINT ["dotnet", "TransferenciasBancarias.dll"]
