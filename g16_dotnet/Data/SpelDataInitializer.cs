using g16_dotnet.Models.Domain;
using System.Collections.Generic;

namespace g16_dotnet.Data
{
    public static class SpelDataInitializer
    {

        public static void InitializeData(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            if (context.Database.EnsureCreated()) {
                // Paden toevoegen aan de DbSet<Pad>
                var oefening1 = new Oefening("Opgave 1", "antwoord1");
                var oefening2 = new Oefening("Opgave 2", "antwoord2");
                var oefening3 = new Oefening("Opgave 3", "antwoord3");

                var groepsBewerking1 = new GroepsBewerking("Groepsbewerking 1 - vb. Vermenigvuldig bovenstaande met 3");
                var groepsBewerking2 = new GroepsBewerking("Groepsbewerking 2 - vb. Trek hier 4 van af.");
                var groepsBewerking3 = new GroepsBewerking("Groepsbewerking 3 - vb. Deel door 3");

                var opdracht1 = new Opdracht("code1", oefening1, groepsBewerking1);
                var opdracht2 = new Opdracht("code2", oefening2, groepsBewerking2);
                var opdracht3 = new Opdracht("code3", oefening3, groepsBewerking3);

                Opdracht[] opdrachten = { opdracht1, opdracht2, opdracht3};
                context.Opdrachten.AddRange(opdrachten);

                var actie1 = new Actie("Ga naar de mcDonalds en koop chicken nuggets");
                var actie2 = new Actie("Ga naar gebouw B");
                var actie3 = new Actie("Neem de groene ballon");

                Actie[] acties = { actie1, actie2, actie3 };
                context.Acties.AddRange(acties);

                var pad = new Pad(opdrachten, acties);
                context.Paden.Add(pad);

                Groep groep = new Groep("Groep1") { Pad = pad };

                context.Groepen.Add(groep);
                context.SaveChanges();
            }
        }
    }
}
