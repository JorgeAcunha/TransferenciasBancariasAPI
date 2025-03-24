#!/bin/bash
set -e

echo "Esperando a que PostgreSQL esté listo..."

until pg_isready -h postgres_transferencias -p 5432 -U "$POSTGRES_USER"; do
  >&2 echo "PostgreSQL no disponible, esperando..."
  sleep 2
done

echo "PostgreSQL está listo, iniciando la API..."

exec "$@"
