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
                var oefening1 = new Oefening("Opgave 1", "abc");
                var oefening2 = new Oefening("Opgave 2", "def");
                var oefening3 = new Oefening("Opgave 3", "ghi");

                var groepsBewerking1 = new GroepsBewerking("Groepsbewerking 1 - vb. Vermenigvuldig bovenstaande met 3");
                var groepsBewerking2 = new GroepsBewerking("Groepsbewerking 2 - vb. Trek hier 4 van af.");
                var groepsBewerking3 = new GroepsBewerking("Groepsbewerking 3 - vb. Deel door 3");

                var opdracht1 = new Opdracht("toegangscode", oefening1, groepsBewerking1);
                var opdracht2 = new Opdracht("toegangscode789", oefening2, groepsBewerking2);
                var opdracht3 = new Opdracht("toegangscode5", oefening3, groepsBewerking3);

                Opdracht[] opdrachten = { opdracht1, opdracht2, opdracht3};               

                var actie1 = new Actie("Ga naar de mcDonalds en koop chicken ngts");
                var actie2 = new Actie("GA naar gebouw B");
                var actie3 = new Actie("Neem de groene ballon");

                Actie[] acties = { actie1, actie2, actie3 };

                var pad = new Pad(opdrachten, acties);

                context.Paden.Add(pad);
                context.SaveChanges();
            }
        }
    }
}
