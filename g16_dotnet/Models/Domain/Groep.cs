using System.Collections.Generic;

namespace g16_dotnet.Models.Domain
{
    public class Groep {
        #region Properties
        public int GroepId { get; set; }
        public string Groepsnaam { get; set; }
        public ICollection<Leerling> Leerlingen { get; set; }
        public bool IsVergrendeld { get; set; }
        public Pad Pad { get; set; }
        #endregion

        #region Constructor
        public Groep() {
            Leerlingen = new List<Leerling>();
            IsVergrendeld = false;
        }

        public Groep(string naam) : this()
        {
            Groepsnaam = naam;
        }
        #endregion
    }
}
