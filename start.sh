#!/usr/bin/env bash
set -euo pipefail

cd "$(dirname "$0")"

if [ ! -d frontend/node_modules ]; then
  echo "Installation des dependances du frontend..."
  (cd frontend && npm install)
fi

echo "Demarrage de l'API sur http://localhost:5080"
dotnet run --project src/DistriMosane.Api &
api_pid=$!

cleanup() {
  kill "$api_pid" 2>/dev/null || true
}
trap cleanup EXIT INT TERM

echo "Demarrage du frontend sur http://localhost:4200"
cd frontend
npm start
