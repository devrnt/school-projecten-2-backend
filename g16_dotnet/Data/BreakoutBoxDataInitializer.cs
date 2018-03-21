using g16_dotnet.Models;
using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace g16_dotnet.Data
{
    public class BreakoutBoxDataInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BreakoutBoxDataInitializer(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            _context = applicationDbContext;
            _userManager = userManager;
        }

        public async Task InitializeData()
        {
            _context.Database.EnsureDeleted();
            if (_context.Database.EnsureCreated())
            {
                // Oefeningen
                var oefening1 = new Oefening("opgave1", 100);
                var oefening2 = new Oefening("opgave2", 200);
                var oefening3 = new Oefening("opgave3", 300);

                // GroepsBewerkingen
                var groepsBewerking1 = new GroepsBewerking("Vermenigvuldig bovenstaande met 3", 3, Operator.vermeningvuldigen);
                var groepsBewerking2 = new GroepsBewerking("Trek hier 4 van af.", 4, Operator.aftrekken);
                var groepsBewerking3 = new GroepsBewerking("Deel door 3", 3, Operator.delen);

                // Opdrachten
                var opdracht1 = new Opdracht("nugget", oefening1, groepsBewerking1);
                var opdracht2 = new Opdracht("gebouw", oefening2, groepsBewerking2);
                var opdracht3 = new Opdracht("ballon", oefening3, groepsBewerking3);
                var opdrachten = new List<Opdracht> { opdracht1, opdracht2, opdracht3 };
                _context.Opdrachten.AddRange(opdrachten);

                // Acties
                var actie1 = new Actie("Ga naar de McDonalds en koop McNuggets");
                var actie2 = new Actie("Ga naar Gebouw B");
                var actie3 = new Actie("Neem de groene ballon");
                var acties = new List<Actie> { actie1, actie2, actie3 };
                _context.Acties.AddRange(acties);

                // Pad
                var pad = new Pad();
                pad.AddOpdracht(opdracht1, 1);
                pad.AddOpdracht(opdracht2, 2);
                pad.AddOpdracht(opdracht3, 3);
                pad.AddActie(actie1, 1);
                pad.AddActie(actie2, 2);
                pad.AddActie(actie3, 3);
                var pad2 = new Pad();
                pad2.AddOpdracht(opdracht1, 1);
                pad2.AddOpdracht(opdracht2, 2);
                pad2.AddOpdracht(opdracht3, 3);
                pad2.AddActie(actie1, 1);
                pad2.AddActie(actie2, 2);
                pad2.AddActie(actie3, 3);
                var pad3 = new Pad();
                pad3.AddOpdracht(opdracht1, 1);
                pad3.AddOpdracht(opdracht2, 2);
                pad3.AddOpdracht(opdracht3, 3);
                pad3.AddActie(actie1, 1);
                pad3.AddActie(actie2, 2);
                pad3.AddActie(actie3, 3);
                var pad4 = new Pad();
                pad4.AddOpdracht(opdracht1, 1);
                pad4.AddOpdracht(opdracht2, 2);
                pad4.AddOpdracht(opdracht3, 3);
                pad4.AddActie(actie1, 1);
                pad4.AddActie(actie2, 2);
                pad4.AddActie(actie3, 3);
                var paden = new List<Pad> { pad, pad2, pad3, pad4 };
                foreach (var item in paden)
                    item.PadState = new OpdrachtPadState("Opdracht");
                _context.Paden.AddRange(paden);

                // Klas
                var klas = new Klas("2A");
                var klas2 = new Klas("2B");

                // Leerling

                Leerling[] leerlingen1 =
                { new Leerling("Balthazar", "Boma"),
                    new Leerling("Costermans", "Fernand"),
                    new Leerling("Crucke", "Bieke"),
                    new Leerling("De Backer", "Pascale"),
                    new Leerling("De Praetere","Maurice"),
                    new Leerling("De Tremmerie","pol"),
                    new Leerling("Van Hoeck","Doortje"),
                   new Leerling("Vertongen", "marc"),
                   new Leerling("Waterslaeghers", "Carmen")


            };

                Leerling[] leerlingen2 =
                {
                    new Leerling("Halpert", "Jim"),
                    new Leerling("Beesley", "Pam"),
                    new Leerling("Schrute", "Dwight"),
                    new Leerling("Howard", "Ryan"),
                     new Leerling("Vandam", "Alain"),
                    new Leerling("Pallemans", "Guido"),
                    new Leerling("Drets", "Michel"),
                    new Leerling("Loosveld", "Franky")
                };

                foreach (var leerling in leerlingen1)
                    klas.Leerlingen.Add(leerling);
                foreach (var leerling in leerlingen2)
                    klas2.Leerlingen.Add(leerling);

                // Groep
                var groep = new Groep("Het Eiland") { Pad = pad };
                var groep2 = new Groep("The Office") { Pad = pad2 };
                var groep3 = new Groep("2B1") { Pad = pad3 };
                var groep4 = new Groep("2B2") { Pad = pad4 };
                foreach (var leerling in klas.Leerlingen.ToList())
                {
                    if (groep.Leerlingen.Count<4)
                    {
                        groep.Leerlingen.Add(leerling);
                    } else if (groep2.Leerlingen.Count < 3)
                    {
                        groep2.Leerlingen.Add(leerling);
                    }
                }
                foreach (var leerling in klas2.Leerlingen.ToList())
                {
                    if (groep3.Leerlingen.Count<4)
                    {
                        groep3.Leerlingen.Add(leerling);
                    } else if (groep4.Leerlingen.Count < 3)
                    {
                        groep4.Leerlingen.Add(leerling);
                    }
                }
               

                Groep[] groepen = { groep, groep2, groep3, groep4 };

                _context.Groepen.AddRange(groepen);
                _context.SaveChanges();


                // Sessie
                var sessie = new Sessie(123, "2A : Hoofdrekenen", "Enkel een pen en papier dienen gebruikt te worden", new List<Groep> { groep, groep2 }, klas);
                var sessie2 = new Sessie(321, "2B : Hoofdrekenen", "Enkel een pen en papier dienen gebruikt te worden", new List<Groep> { groep3, groep4 }, klas2);
                Sessie[] sessies = { sessie, sessie2 };

                _context.Sessies.AddRange(sessies);
                _context.SaveChanges();

                // Leerkracht
                var leerkracht = new Leerkracht("Protut", "Lydia", "lydia.protut@synalco.be") { Sessies = new List<Sessie> { sessie, sessie2 } };
                _context.Leerkrachten.Add(leerkracht);
                ApplicationUser user = new ApplicationUser { UserName = "lydia.protut@synalco.be", Email = "lydia.protut@synalco.be" };
                await _userManager.CreateAsync(user, "P@ssword1");
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Leerkracht"));
                _context.SaveChanges();
            }
            _context.SaveChanges();
        }
    }
}
