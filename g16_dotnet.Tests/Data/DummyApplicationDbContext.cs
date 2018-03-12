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
        public Pad PadMet1Opdracht { get; set; }

        public Oefening Oef1 { get; }
        public Oefening Oef2 { get; }
        public GroepsBewerking GroepsBewerking1 { get; }
        public GroepsBewerking GroepsBewerking2 { get; }
        public Opdracht Opdracht1 { get; }
        public Opdracht Opdracht2 { get;  }
        public Actie Actie1 { get; }
        public Actie Actie2 { get; }
        public Sessie SessieAlleDeelnamesBevestigd { get; set; }
        public Sessie SessieNogDeelnamesTeBevestigen { get; set; }
        public Groep Groep1 { get; set; }
        public Groep Groep2 { get; set; }
        public Klas Klas1 { get; set; }
        public Leerling Leerling1 { get; set; }
        public Leerling Leerling2 { get; set; }

        public Opdracht PadOpdracht1 { get; }
        public Leerkracht Leerkracht1 { get; set; }

        public DummyApplicationDbContext() {
            Oef1 = new Oefening("Opgave oefening 1", 100);
            GroepsBewerking1 = new GroepsBewerking("Doe de groepsbewerking1", 2, Operator.aftrekken);

            Opdracht1 = new Opdracht("toegangsCode123", Oef1, GroepsBewerking1);


            Oef2 = new Oefening("Opgave oefening 2", 210);
            GroepsBewerking2 = new GroepsBewerking("Doe de groepsbewerking2", 3, Operator.delen);

            Opdracht2 = new Opdracht("toegangsCode678", Oef2, GroepsBewerking2);

            Opdrachten = new[] { Opdracht1, Opdracht2 };

            Actie1 = new Actie("Actie1");
            Actie2 = new Actie("Actie2");
            Acties = new[] { Actie1, Actie2 };

            Pad = new Pad() { PadId = 1 };
            Pad.AddOpdracht(Opdracht1);
            Pad.AddOpdracht(Opdracht2);
            Pad.AddActie(Actie1);
            Pad.AddActie(Actie2);
            Pad.PadState = new OpdrachtPadState("Opdracht");
            PadMet1Opdracht = new Pad() { PadId = 5 };
            PadMet1Opdracht.AddOpdracht(Opdracht1);
            PadMet1Opdracht.AddActie(Actie1);
            PadMet1Opdracht.PadState = new OpdrachtPadState("Opdracht");


            Groep1 = new Groep("Groep1") { GroepId = 1, Pad = Pad, DeelnameBevestigd = true };
            Groep2 = new Groep("Groep2") { Pad = Pad, DeelnameBevestigd = false };
            Leerling1 = new Leerling("McDerp", "Derp");
            Leerling2 = new Leerling("Cena", "John");
            Klas1 = new Klas("Klas1", new List<Leerling> { Leerling1, Leerling2 });
            SessieAlleDeelnamesBevestigd = new Sessie(123, "Sessie1", "Dit is sessie1", new List<Groep> { Groep1 }, Klas1);
            SessieNogDeelnamesTeBevestigen = new Sessie(321, "Sessie2", "Dit is sessie2", new List<Groep> { Groep2 }, Klas1);
            Leerkracht1 = new Leerkracht("VanDam", "Alain", "alain.vandam@synalco.be") { Sessies = new List<Sessie> { SessieAlleDeelnamesBevestigd } };
        }

    }
}
