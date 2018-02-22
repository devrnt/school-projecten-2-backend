using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Models.Domain
{
    public class Sessie
    {
        #region Properties
        public int SessionId { get; set; }
        public string Omschrijving { get; set; }
        public int Code { get; set; }

        public ICollection<Groep> Groepen { get; private set; }

        public Klas Klas { get; set; }
        #endregion

        #region Constructor
        public Sessie(int code, Klas klas)
        {
            Code = code;
            Klas = klas;
            Groepen = new List<Groep>();
        }
        public Sessie(int code, Klas klas, ICollection<Groep> groepen)
        {
            Code = code;
            Klas = klas;
            Groepen = groepen;
        }
        #endregion
    }
}
