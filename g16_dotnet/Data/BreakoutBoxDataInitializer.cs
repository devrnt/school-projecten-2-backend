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
                // Oefeningen
                var oefening1 = new Oefening("opgave1", 100);
                var oefening2 = new Oefening("opgave2", 200);
                var oefening3 = new Oefening("opgave3", 300);

                // GroepsBewerkingen
                var groepsBewerking1 = new GroepsBewerking("Vermenigvuldig bovenstaande met 3", 3, Operator.vermeningvuldigen);
                var groepsBewerking2 = new GroepsBewerking("Trek hier 4 van af.", 4, Operator.aftrekken);
                var groepsBewerking3 = new GroepsBewerking("Deel door 3", 3, Operator.delen);

                // Opdrachten
                var opdracht1 = new Opdracht("code1", oefening1, groepsBewerking1);
                var opdracht2 = new Opdracht("code2", oefening2, groepsBewerking2);
                var opdracht3 = new Opdracht("code3", oefening3, groepsBewerking3);

                var opdrachten = new List<Opdracht>{ opdracht1, opdracht2, opdracht3 };
                context.Opdrachten.AddRange(opdrachten);

                // Acties
                var actie1 = new Actie("Ga naar McDonalds en koop chicken nuggets");
                var actie2 = new Actie("Ga naar gebouw B");
                var actie3 = new Actie("Neem de groene ballon");

                var acties = new List<Actie>{ actie1, actie2, actie3 };
                context.Acties.AddRange(acties);

                // Pad
                var pad = new Pad();               
                pad.AddOpdracht(opdracht1);
                pad.AddOpdracht(opdracht2);
                pad.AddOpdracht(opdracht3);
                pad.AddActie(actie1);
                pad.AddActie(actie2);
                pad.AddActie(actie3);
                var pad2 = new Pad();
                pad2.AddOpdracht(opdracht1);
                pad2.AddOpdracht(opdracht3);
                pad2.AddOpdracht(opdracht2);
                pad2.AddActie(actie1);
                pad2.AddActie(actie3);
                pad2.AddActie(actie2);
                var pad3 = new Pad();
                pad3.AddOpdracht(opdracht2);
                pad3.AddOpdracht(opdracht1);
                pad3.AddOpdracht(opdracht3);
                pad3.AddActie(actie2);
                pad3.AddActie(actie1);
                pad3.AddActie(actie3);
                var pad4 = new Pad();
                pad4.AddOpdracht(opdracht3);
                pad4.AddOpdracht(opdracht2);
                pad4.AddOpdracht(opdracht1);
                pad4.AddActie(actie3);
                pad4.AddActie(actie2);
                pad4.AddActie(actie1);
                var paden = new List<Pad>{ pad, pad2, pad3, pad4 };
                context.Paden.AddRange(paden);

                // Klas
                var klas = new Klas("Het Eiland");
                var klas2 = new Klas("The Office");

                // Leerling
                
                Leerling[] leerlingen1 = 
                {
                    new Leerling("Vandam", "Alain"),
                    new Leerling("Pallemans", "Guido"),
                    new Leerling("Drets", "Michel"),
                    new Leerling("Loosveld", "Franky")
                };
                Leerling[] leerlingen2 =
                {
                    new Leerling("Halpert", "Jim"),
                    new Leerling("Beesley", "Pam"),
                    new Leerling("Schrute", "Dwight"),
                    new Leerling("Howard", "Ryan")
                };
                foreach (var leerling in leerlingen1)
                    klas.Leerlingen.Add(leerling);
                foreach (var leerling in leerlingen2)
                    klas2.Leerlingen.Add(leerling);

                // Groep
                var groep = new Groep("Groep 1") { Pad = pad };
                var groep2 = new Groep("Groep 2") { Pad = pad2 };
                var groep3 = new Groep("Groep 3") { Pad = pad3 };
                var groep4 = new Groep("Groep 4") { Pad = pad4 };
                Groep[] groepen1 = { groep, groep2 };
                Groep[] groepen2 = { groep3, groep4 };
                for (int i = 0; i < 2; i++)
                    groep.Leerlingen.Add(leerlingen1[i]);
                for (int i = 2; i < 4; i++)
                    groep2.Leerlingen.Add(leerlingen1[i]);
                for (int i = 0; i < 2; i++)
                    groep3.Leerlingen.Add(leerlingen2[i]);
                for (int i = 2; i < 4; i++)
                    groep4.Leerlingen.Add(leerlingen2[i]);

                context.Groepen.AddRange(groepen1);
                context.SaveChanges();

                context.Groepen.AddRange(groepen2);
                context.SaveChanges();


                // Sessie
                var sessie = new Sessie(123, "Sessie1", "Dit is sessie 1", groepen1, klas);
                var sessie2 = new Sessie(321, "Sessie2", "Dit is sessie 2", groepen2, klas2);
                Sessie[] sessies = { sessie, sessie2 };

                context.Sessies.AddRange(sessies);
                context.SaveChanges();

                //// Leerkracht
                var leerkracht = new Leerkracht("Protut", "Lydia") { Sessies = new List<Sessie> { sessie, sessie2 } };
                context.Leerkrachten.Add(leerkracht);
                context.SaveChanges();
            }
        }
    }
}
