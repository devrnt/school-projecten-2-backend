using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Models.Domain {
    public class Groep {
        #region Properties
        public string Groepsnaam { get; set; }
        public List<Leerling> Leerlingen { get; set; }
        public bool IsVergrendeld { get; set; }
        #endregion

        #region Constructor
        public Groep() {
            Leerlingen = new List<Leerling>();
            Groepsnaam = "NogGeenGroepsNaam";
            IsVergrendeld = false;
        }
        public Groep(string naam)
        {
            Groepsnaam = naam; 
        }
        #endregion
    }
}
