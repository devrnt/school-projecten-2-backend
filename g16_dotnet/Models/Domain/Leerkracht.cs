using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Models.Domain
{
    public class Leerkracht
    {
        #region Fields and Properties
        public int LeerkrachtId { get; set; }
        public string Naam { get; set; }
        public string Voornaam { get; set; }
        public IEnumerable<Sessie> Sessies { get; set; }
        public IEnumerable<Sessie> InactieveSessies { get { return Sessies.Where(s => !s.IsActief); } }
        #endregion

        #region Constructors
        public Leerkracht(string naam, string voornaam) : this()
        {
            Naam = naam;
            Voornaam = voornaam;
        }

        public Leerkracht()
        {
            Sessies = new List<Sessie>();
        }
        #endregion

        #region Methods

        #endregion
    }
}
