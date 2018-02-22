using g16_dotnet.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Data
{
    public class SessieDataInitializer
    {
        public SessieDataInitializer()
        {
        }
        // tijdelijk database maken, als er nood is aan een db
        // kunnen we hier sessies toevoegen en saveChanges aanspreken in de ApplicationDbContext
        public static void InitializeData(ApplicationDbContext context) {
            context.sessies = new List<Sessie>();
            ICollection<Groep> s1 = new List<Groep>();
            s1.Add(new Groep("Groep1"));
            s1.Add(new Groep("Groep2"));
            context.sessies.Add(new Sessie(123, new Klas(), s1));
            ICollection<Groep> s2 = new List<Groep>();
            s2.Add(new Groep("Groep1"));
            s2.Add(new Groep("Groep2"));
            s2.Add(new Groep("Groep3"));
            context.sessies.Add(new Sessie(12345, new Klas(), s2));

            // data here...
        }
    }
}
