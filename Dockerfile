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

# 👉 Instalar herramientas PostgreSQL SOLO ESTO AGREGA
RUN apt-get update && apt-get install -y postgresql-client

# Copiar script de espera
COPY wait-for-postgres.sh /wait-for-postgres.sh
RUN chmod +x /wait-for-postgres.sh

# Cambiar el entrypoint
ENTRYPOINT ["/wait-for-postgres.sh", "dotnet", "TransferenciasBancarias.dll"]
