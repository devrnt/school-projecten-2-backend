using System.Collections.Generic;

namespace g16_dotnet.Models.Domain
{
    public class Klas {
        #region Fields and properties
        public int KlasId { get; set; }
        public ICollection<Leerling> Leerlingen { get; set; }
        public string Naam { get; set; }
        #endregion

        #region Constructor
        public Klas() {
            Leerlingen = new List<Leerling>();
        }

        public Klas(string naam) : this()
        {
            Naam = naam;
        }

        public Klas(string naam, ICollection<Leerling> leerlingen)
        {
            Naam = naam;
            Leerlingen = leerlingen;
        }
        #endregion

    }
}