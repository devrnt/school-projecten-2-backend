using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Models.Domain
{
    public class Oefening
    {
        public string Opgave { get; private set; }
        public string GroepsAntwoord { get; private set; }

        public Oefening(string opgave, string groepsAntwoord)
        {
            Opgave = opgave;
            GroepsAntwoord = groepsAntwoord;
        }

        public bool ControleerAntwoord(string antwoord)
        {
            return antwoord == GroepsAntwoord;
        }
    }
}
