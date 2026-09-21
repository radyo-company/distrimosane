@echo off
setlocal

cd /d "%~dp0"

if not exist frontend\node_modules (
  echo Installation des dependances du frontend...
  pushd frontend
  call npm install
  popd
)

echo Demarrage de l'API sur http://localhost:5080
start "Distri'Mosane API" cmd /c "dotnet run --project src\DistriMosane.Api"

echo Demarrage du frontend sur http://localhost:4200
cd frontend
call npm start
