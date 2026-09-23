using DistriMosane.Api.Domain;

namespace DistriMosane.Api.Data;

public static class DatabaseSeeder
{
    private const int GeneratorSeed = 20240115;

    private const double DaysPerMonth = 30.44;

    public static void Seed(AppDbContext context)
    {
        if (context.Customers.Any())
        {
            return;
        }

        var today = DateTime.UtcNow.Date;
        var random = new Random(GeneratorSeed);

        var customers = Blueprints
            .Select(blueprint => BuildCustomer(blueprint, today, random))
            .ToList();

        context.Customers.AddRange(customers);
        context.SaveChanges();
    }

    private static Customer BuildCustomer(CustomerBlueprint blueprint, DateTime today, Random random)
    {
        var orderDates = BuildOrderDates(blueprint, today, random);

        var createdAt = orderDates.Count > 0
            ? orderDates[^1].AddDays(-random.Next(20, 190))
            : today.AddDays(-random.Next(30, 320));

        var customer = new Customer
        {
            CompanyName = blueprint.CompanyName,
            VatNumber = blueprint.VatNumber,
            Email = blueprint.Email,
            City = blueprint.City,
            CreatedAt = createdAt,
            IsArchived = blueprint.IsArchived,
            Orders = orderDates
                .Select(date => BuildOrder(date, today, random))
                .OrderBy(order => order.OrderDate)
                .ToList()
        };

        return customer;
    }

    private static List<DateTime> BuildOrderDates(CustomerBlueprint blueprint, DateTime today, Random random)
    {
        var dates = new List<DateTime>();

        for (var index = 0; index < blueprint.OrderCount; index++)
        {
            var progress = blueprint.OrderCount == 1
                ? 0d
                : (double)index / (blueprint.OrderCount - 1);

            var monthsAgo = blueprint.LastOrderMonthsAgo
                + ((blueprint.FirstOrderMonthsAgo - blueprint.LastOrderMonthsAgo) * progress);

            var offset = (int)Math.Round(monthsAgo * DaysPerMonth) + random.Next(-9, 10);
            var date = today.AddDays(-Math.Max(offset, 2));

            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                date = date.AddDays(-1);
            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                date = date.AddDays(-2);
            }

            dates.Add(date);
        }

        return dates;
    }

    private static Order BuildOrder(DateTime orderDate, DateTime today, Random random)
    {
        var lineCount = random.Next(4, 9);

        var lines = new List<OrderLine>();
        var picked = new HashSet<int>();

        while (lines.Count < lineCount)
        {
            var productIndex = random.Next(Catalog.Length);
            if (!picked.Add(productIndex))
            {
                continue;
            }

            var (label, price) = Catalog[productIndex];
            var quantity = random.Next(15, 61);

            lines.Add(new OrderLine
            {
                ProductLabel = label,
                Quantity = quantity,
                UnitPrice = price
            });
        }

        return new Order
        {
            OrderDate = orderDate,
            Status = PickStatus(orderDate, today, random),
            Lines = lines
        };
    }

    private static OrderStatus PickStatus(DateTime orderDate, DateTime today, Random random)
    {
        if (random.Next(100) < 13)
        {
            return OrderStatus.Cancelled;
        }

        var age = (today - orderDate).TotalDays;

        return age switch
        {
            < 12 => OrderStatus.Pending,
            < 40 => OrderStatus.Shipped,
            _ => OrderStatus.Delivered
        };
    }

    private sealed record CustomerBlueprint(
        string CompanyName,
        string City,
        string VatNumber,
        string Email,
        bool IsArchived,
        int OrderCount,
        int FirstOrderMonthsAgo,
        int LastOrderMonthsAgo);

    private static readonly (string Label, decimal Price)[] Catalog =
    [
        ("Ramette papier A4 80g (carton de 5)", 22.90m),
        ("Ramette papier A3 80g (carton de 5)", 41.50m),
        ("Stylo bille bleu (boîte de 50)", 18.75m),
        ("Marqueur permanent noir (lot de 10)", 12.40m),
        ("Surligneur fluo assorti (lot de 6)", 7.95m),
        ("Classeur à levier A4 dos 8 cm", 4.35m),
        ("Chemise cartonnée kraft (lot de 100)", 16.80m),
        ("Enveloppes C4 blanches (boîte de 250)", 34.60m),
        ("Enveloppes C5 à fenêtre (boîte de 500)", 48.20m),
        ("Bloc-notes quadrillé A5 (lot de 10)", 11.25m),
        ("Cahier spirale A4 (lot de 5)", 9.60m),
        ("Post-it 76x76 mm (lot de 12)", 14.30m),
        ("Ruban adhésif transparent (lot de 8)", 6.85m),
        ("Agrafeuse métal 25 feuilles", 19.90m),
        ("Boîte d'agrafes 24/6 (5000 pièces)", 3.45m),
        ("Perforateur 2 trous 30 feuilles", 21.50m),
        ("Cartouche toner laser noir 3000 pages", 89.90m),
        ("Cartouche toner laser couleur 2500 pages", 124.50m),
        ("Cartouche jet d'encre noire XL", 32.70m),
        ("Étiquettes autocollantes 105x37 (boîte)", 27.40m),
        ("Tableau blanc émaillé 90x60 cm", 74.50m),
        ("Paperboard mobile avec bloc", 158.00m),
        ("Destructeur de documents coupe croisée", 189.90m),
        ("Calculatrice de bureau 12 chiffres", 26.80m),
        ("Corbeille à papier 18 L", 8.90m),
        ("Trieur 12 compartiments A4", 13.70m),
        ("Porte-documents plastifié A4 (lot de 100)", 23.60m),
        ("Rouleau papier thermique 80 mm (lot de 10)", 15.40m),
        ("Gel hydroalcoolique 500 ml (lot de 6)", 29.50m),
        ("Essuie-mains papier pliés (carton de 20)", 38.90m)
    ];

    private static readonly CustomerBlueprint[] Blueprints =
    [
        new("Bureau Comptable Lemaire", "Namur", "BE0529739665", "contact@comptable-lemaire.be", false, 14, 34, 0),
        new("Étude Notariale Dethier", "Jambes", "BE0647133520", "etude@notaire-dethier.be", false, 9, 27, 1),
        new("Clinique Saint-Luc Mosane", "Namur", "BE0764527472", "achats@cliniquemosane.be", false, 16, 41, 0),
        new("Architectes Verhoeven & Associés", "Gembloux", "BE0881921327", "info@verhoeven-archi.be", false, 8, 24, 2),
        new("Garage Dupuis SA", "Andenne", "BE0999315279", "administration@garagedupuis.be", false, 5, 33, 17),
        new("Imprimerie Colson", "Namur", "BE0216709183", "commandes@imprimerie-colson.be", false, 15, 38, 0),
        new("Cabinet Dentaire Mercier", "Wépion", "BE0334103038", "secretariat@dentaire-mercier.be", false, 7, 22, 1),
        new("Transports Genot SPRL", "Sambreville", "BE0451496990", "bureau@transports-genot.be", false, 11, 31, 0),
        new("École Libre Sainte-Marie", "Namur", "BE0568890845", "economat@stemarie-namur.be", false, 13, 36, 1),
        new("Assurances Piret & Fils", "Ciney", "BE0686284797", "contact@assurances-piret.be", false, 10, 29, 2),
        new("Menuiserie Halloy", "Assesse", "BE0803678652", "atelier@menuiserie-halloy.be", false, 4, 28, 14),
        new("Fiduciaire Namuroise", "Namur", "BE0921072507", "info@fiduciaire-namuroise.be", false, 12, 30, 0),
        new("Pharmacie du Grognon", "Namur", "BE0138466411", "pharmacie@grognon.be", true, 6, 21, 1),
        new("Société Immobilière Delvaux", "Jambes", "BE0255860363", "agence@immo-delvaux.be", false, 9, 26, 1),
        new("Atelier Graphique Wauters", "Namur", "BE0373254218", "studio@wauters-graphic.be", false, 7, 19, 2),
        new("Brasserie de la Meuse", "Dinant", "BE0490648170", "commandes@brasserie-meuse.be", false, 12, 35, 0),
        new("Cabinet Vétérinaire Hoyoux", "Profondeville", "BE0608042025", "accueil@veto-hoyoux.be", false, 6, 32, 15),
        new("Électricité Générale Dandoy", "Floreffe", "BE0725435977", "bureau@dandoy-elec.be", false, 8, 23, 2),
        new("Centre Sportif de Belgrade", "Namur", "BE0842829832", "secretariat@csbelgrade.be", true, 7, 30, 19),
        new("Librairie Papeterie Renard", "Fosses-la-Ville", "BE0960223784", "contact@librairie-renard.be", false, 3, 25, 16),
        new("Consultance RH Bastin", "Namur", "BE0177617688", "info@bastin-rh.be", false, 6, 18, 3),
        new("Carrosserie Marchal", "Eghezée", "BE0295011543", "atelier@carrosserie-marchal.be", false, 0, 0, 0),
        new("Maison de Repos Les Tilleuls", "La Bruyère", "BE0412405495", "direction@lestilleuls.be", false, 7, 36, 15),
        new("Bureau d'Études Thonon", "Namur", "BE0529799350", "etudes@thonon-be.be", false, 10, 28, 1),
        new("Traiteur Grégoire", "Yvoir", "BE0647193205", "commandes@traiteur-gregoire.be", true, 5, 17, 1),
        new("Cabinet d'Avocats Renson", "Namur", "BE0764587157", "greffe@avocats-renson.be", false, 11, 32, 0),
        new("Horticulture Delhaye", "Rochefort", "BE0881981012", "serres@horti-delhaye.be", false, 4, 29, 21),
        new("Agence de Voyages Évasion", "Namur", "BE0999374964", "agence@evasion-voyages.be", false, 0, 0, 0),
        new("Toitures Servais SPRL", "Andenne", "BE0216768868", "bureau@toitures-servais.be", false, 8, 24, 3),
        new("Laboratoire d'Analyses Warnant", "Namur", "BE0334162723", "labo@warnant-analyses.be", false, 13, 37, 0),
        new("Auto-École Trajectoire", "Jambes", "BE0451556675", "secretariat@autoecole-trajectoire.be", true, 4, 26, 22),
        new("Boulangerie Industrielle Jadot", "Sambreville", "BE0568950530", "production@jadot-boulangerie.be", false, 6, 31, 18),
        new("Chauffage Sanitaire Body", "Ciney", "BE0686344482", "depannage@body-chauffage.be", false, 9, 25, 2),
        new("Crèche Les Petits Mosans", "Namur", "BE0803738337", "direction@petitsmosans.be", false, 0, 0, 0),
        new("Syndic Immobilier Gérard", "Namur", "BE0921132289", "syndic@gerard-immo.be", false, 10, 27, 1),
        new("Distribution Alimentaire Nihoul", "Gembloux", "BE0138526193", "achats@nihoul-distri.be", false, 14, 39, 0),
        new("Cabinet Kiné Lefèvre", "Wépion", "BE0255920048", "rendezvous@kine-lefevre.be", false, 0, 0, 0),
        new("Entreprise Générale Paquet", "Marche-en-Famenne", "BE0373313903", "bureau@paquet-construction.be", false, 5, 30, 24),
        new("Photographe Studio Lumière", "Namur", "BE0490707855", "studio@studio-lumiere.be", true, 5, 20, 8),
        new("Nettoyage Industriel Servico", "Namur", "BE0608101710", "planning@servico-nettoyage.be", false, 12, 33, 0)
    ];
}
