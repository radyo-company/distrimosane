# Rendu

## Ce qui est fait

_Ce qui fonctionne et comment le vérifier._

Les 2 premières demandes ont été réalisées : correction des chiffres d'affaires incorrects + ne voir que les clients actifs.
On peut le voir via le frontend.
Par exemple, "Bureau Comptable Lemaire" a vu son chiffre d'affaire passer d'environ 12000 euros à environ 8000 euros.
Et on ne retrouve pas "Traiteur Grégoire" dans la liste (client archivé).

## Ce qui n'est pas fait

_Ce que vous n'avez pas traité, et pourquoi._
La 3ème demande, l'import du fichier Excel. Je n'ai aucune idée de comment réaliser ceci. J'ai idée assez précise de la logique à amener, mais
techniquement, je ne sais pas comment le réaliser. Pour moi, il faudrait : ouvrir le fichier, lire ligne par ligne, et mapper chaque colonne au
champ en base de données, créer l'objet CustomerDetailsDto (en ajoutant la date d'ajourd'hui, et une liste de commandes vide), puis insérer en DB.
Et bien sur ajouter un bouton ou autre permettant d'importer le fichier, qui appelerait la méthode d'import quand on valide.

## Choix faits

_Les décisions que vous avez prises et ce qui les a motivées._
J'ai décidé d'ajouter une clause "Where" dans le fichier CustomerMappingExtensions afin de compter uniquement les commandes dont l'état
est "Delivered". Cela inclut également d'afficher, dans le nombre de commande, celles qui sont terminées uniquement. Cela me semblait
plus logique.

Concernant la visualisation des clients actifs uniquement, j'ai rajouté
dans le service Customer cette fois, une clause "Where" ne gardant que ceux
qui n'ont pas le tag "IsArchived".

## Questions ouvertes

_Ce sur quoi vous auriez eu besoin d'une réponse pour aller plus loin._
Comment ouvrir, lire, et mapper des colonnes d'un fichier Excel vers un objet C#.

## Usage de l'IA et des ressources en ligne

_Ce que vous avez utilisé, pour quoi faire, et ce que vous en avez retenu ou écarté._
J'ai utilisé Claude afin d'avoir la syntaxe exacte, l'ordre de certaines méthodes, ainsi que pour le débug lorsque que j'avais
des erreurs.
J'adaptais sa réponse au code disponible.
Je lui ai demandé à plusieurs reprise si mon intuition était bonne ou pas.
J'ai également utilisé la doc internet + Claude pour essayer de comprendre comment importer le fichier Excel.
