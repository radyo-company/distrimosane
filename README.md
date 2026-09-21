# Distri'Mosane - Suivi commercial

Application interne utilisée par l'équipe commerciale de Distri'Mosane pour suivre ses clients
professionnels et leurs commandes.

| Dossier | Contenu |
| --- | --- |
| `src/DistriMosane.Api` | API REST ASP.NET Core (.NET 10), EF Core sur SQLite |
| `tests/DistriMosane.Api.Tests` | Tests unitaires xUnit |
| `frontend` | Application Angular (composants standalone) |
| `samples` | Fichiers d'exemple transmis par le client |

## Prérequis

- .NET SDK 10.0 ou supérieur
- Node.js 20.19 ou supérieur, et npm

## Lancer l'application

```bash
./start.sh
```

Sous Windows : `start.cmd`.

Le script installe les dépendances du frontend au premier lancement, démarre l'API sur
<http://localhost:5080> puis le frontend sur <http://localhost:4200>.

### Lancer les deux parties séparément

```bash
dotnet run --project src/DistriMosane.Api
```

```bash
cd frontend
npm install
npm start
```

Le serveur de développement Angular relaie les appels `/api` vers l'API
(voir `frontend/proxy.conf.json`), il n'y a donc rien à configurer côté navigateur.

## Tests

```bash
dotnet test
```

## Base de données

SQLite, dans le fichier `src/DistriMosane.Api/distrimosane.db`. Il est créé, migré et alimenté
avec un jeu de données de démonstration au premier démarrage de l'API. Pour repartir d'une base
propre, supprimez le fichier et relancez l'API.

Les migrations sont gérées avec EF Core, via l'outil local déclaré dans `dotnet-tools.json` :

```bash
dotnet tool restore
dotnet dotnet-ef migrations add NomDeLaMigration --project src/DistriMosane.Api --output-dir Data/Migrations
```

## API

| Méthode | Route | Description |
| --- | --- | --- |
| GET | `/api/customers?search=` | Liste des clients, filtrable sur le nom de société |
| GET | `/api/customers/{id}` | Fiche d'un client et ses commandes |
| GET | `/api/orders/{id}` | Détail d'une commande et ses lignes |

La spécification OpenAPI est exposée sur `/openapi/v1.json` en environnement de développement.

## Organisation du code

L'API suit un découpage classique : les `Controllers` exposent les routes, les `Services`
portent la logique et les accès EF Core, les `Dtos` décrivent les contrats renvoyés au frontend
et le dossier `Mapping` contient les méthodes d'extension qui convertissent les entités en DTO.
Le dossier `Data` regroupe le `DbContext`, les migrations et le jeu de données de démonstration.
