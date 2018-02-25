using g16_dotnet.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace g16_dotnet.Tests.Data
{
    public class DummyApplicationDbContext
    {
        public IEnumerable<Opdracht> Opdrachten { get; }
        public IEnumerable<Actie> Acties { get;  }

        public Pad Pad { get; }

        public Oefening Oef1 { get; }
        public Oefening Oef2 { get; }
        public GroepsBewerking GroepsBewerking1 { get; }
        public GroepsBewerking GroepsBewerking2 { get; }
        public Opdracht Opdracht1 { get; }
        public Opdracht Opdracht2 { get;  }
        public Actie Actie1 { get; }
        public Actie Actie2 { get; }
        public Sessie Sessie1 { get; set; }
        public Groep Groep1 { get; set; }
        public Klas Klas1 { get; set; }
        public Leerling Leerling1 { get; set; }
        public Leerling Leerling2 { get; set; }

        public Opdracht PadOpdracht1 { get; }

        public DummyApplicationDbContext() {
            Oef1 = new Oefening("Opgave oefening 1", "antwoord oefening 1");
            GroepsBewerking1 = new GroepsBewerking("Doe de groepsbewerking1");

            Opdracht1 = new Opdracht("toegangsCode123", Oef1, GroepsBewerking1);


            Oef2 = new Oefening("Opgave oefening 2", "antwoord oefening 2");
            GroepsBewerking2 = new GroepsBewerking("Doe de groepsbewerking2");

            Opdracht2 = new Opdracht("toegangsCode678", Oef2, GroepsBewerking2);

            Opdrachten = new[] { Opdracht1, Opdracht2 };

            Actie1 = new Actie("Actie1");
            Actie2 = new Actie("Actie2");
            Acties = new[] { Actie1, Actie2 };

            Pad = new Pad(Opdrachten, Acties);

            PadOpdracht1 = Pad.Opdrachten.FirstOrDefault();

            Groep1 = new Groep("Groep1") { Pad = Pad };
            Leerling1 = new Leerling("McDerp", "Derp");
            Leerling2 = new Leerling("Cena", "John");
            Klas1 = new Klas("Klas1", new List<Leerling> { Leerling1, Leerling2 });
            Sessie1 = new Sessie("Sessie1", "Dit is sessie1", 123, new List<Groep> { Groep1 }, Klas1); 
        }

    }
}
