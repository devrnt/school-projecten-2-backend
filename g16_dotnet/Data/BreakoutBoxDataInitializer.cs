using g16_dotnet.Models.Domain;
using System.Collections.Generic;

namespace g16_dotnet.Data
{
    public static class BreakoutBoxDataInitializer
    {

        public static void InitializeData(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            if (context.Database.EnsureCreated()) {
                // Paden toevoegen aan de DbSet<Pad>
                // Oefeningen
                var oefening1 = new Oefening("Opgave 1", "antwoord1");
                var oefening2 = new Oefening("Opgave 2", "antwoord2");
                var oefening3 = new Oefening("Opgave 3", "antwoord3");

                // GroepsBewerkingen
                var groepsBewerking1 = new GroepsBewerking("Groepsbewerking 1 - vb. Vermenigvuldig bovenstaande met 3");
                var groepsBewerking2 = new GroepsBewerking("Groepsbewerking 2 - vb. Trek hier 4 van af.");
                var groepsBewerking3 = new GroepsBewerking("Groepsbewerking 3 - vb. Deel door 3");

                // Opdrachten
                var opdracht1 = new Opdracht("code1", oefening1, groepsBewerking1);
                var opdracht2 = new Opdracht("code2", oefening2, groepsBewerking2);
                var opdracht3 = new Opdracht("code3", oefening3, groepsBewerking3);

                Opdracht[] opdrachten = { opdracht1, opdracht2, opdracht3 };
                context.Opdrachten.AddRange(opdrachten);
                context.SaveChanges();

                // Acties
                var actie1 = new Actie("Ga naar de mcDonalds en koop chicken nuggets");
                var actie2 = new Actie("Ga naar gebouw B");
                var actie3 = new Actie("Neem de groene ballon");

                Actie[] acties = { actie1, actie2, actie3 };
                context.Acties.AddRange(acties);
                context.SaveChanges();

                // Pad
                var pad = new Pad(opdrachten, acties);
                context.Paden.Add(pad);
                context.SaveChanges();

                // Klas
                var klas = new Klas("Klas1");

                // Leerling
                var leerling = new Leerling("Derpson", "Derp");
                var leerling2 = new Leerling("McDerp", "Derpie");
                var leerling3 = new Leerling("Derp", "Herpie");
                var leerling4 = new Leerling("Peeters", "Peter");
                klas.Leerlingen.Add(leerling);
                klas.Leerlingen.Add(leerling2);
                klas.Leerlingen.Add(leerling3);
                klas.Leerlingen.Add(leerling4);

                // Groep
                var groep = new Groep("Groep1") { Pad = pad, DeelnameBevestigd = true };
                var groep2 = new Groep("Groep2") { Pad = pad, DeelnameBevestigd = false };
                Groep[] groepen = { groep, groep2 };
                groep.Leerlingen.Add(leerling);
                groep.Leerlingen.Add(leerling2);
                groep2.Leerlingen.Add(leerling3);
                groep2.Leerlingen.Add(leerling4);

                context.Groepen.AddRange(groepen);
                context.SaveChanges();


                // Sessie
                var sessie = new Sessie(123, "Sessie1", "Dit is sessie 1", new List<Groep> { groep }, klas);
                var sessie2 = new Sessie(321, "Sessie2", "Dit is sessie 2", new List<Groep> { groep2 }, klas);
                Sessie[] sessies = { sessie, sessie2 };

                context.Sessies.AddRange(sessies);
                context.SaveChanges();

                //// Leerkracht
                var leerkracht = new Leerkracht("Ipsum", "Lorem") { Sessies = new List<Sessie> { sessie, sessie2 } };
                context.Leerkrachten.Add(leerkracht);
                context.SaveChanges();
            }
        }
    }
}
