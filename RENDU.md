# Rendu

## Ce qui est fait

_Ce qui fonctionne et comment le vérifier._

Côté Frontend : 

- Les chiffres affichées de la réponse JSON de l'API correspond bien aux chiffres afficher à l'écran (Vérification dans dans l'outils de développement du navigateur (F12)) --> Piste - Problème potentiellement au niveau du backend.


Côté backend : 

- Vérification dans le controlleur quelle service est appelé pour la commande --> CustomerService --> Appel de la fonction toListItem().


- On va voir dans la base de données les différents status : dans le fichier et dans la classe DatabaseSeader on retrouve : Pending, Shipped, Delivered et Cancelled
- Dans la fonction toListItem(), on remarque que on fait la somme de toute les commandes. Et donc on va implémenter des filtres.



## Ce qui n'est pas fait

_Ce que vous n'avez pas traité, et pourquoi._

## Choix faits

_Les décisions que vous avez prises et ce qui les a motivées._



## Questions ouvertes

_Ce sur quoi vous auriez eu besoin d'une réponse pour aller plus loin._

## Usage de l'IA et des ressources en ligne

_Ce que vous avez utilisé, pour quoi faire, et ce que vous en avez retenu ou écarté._

Utilisation de Gemini : 
- Pour comprendre au niveau de dotnet où trouver les choses.

- Demande de raccourcis pour des recherches avancées

