using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Models.Domain
{
    public class Actie
    {
        #region Fields and Properties
        public int ActieId { get; set; }
        public string Omschrijving { get; private set; }
        public Opdracht GelinkteOpdracht { get; private set; }
        #endregion

        #region Constructors
        public Actie(string omschrijving) {
            Omschrijving = omschrijving;
        }
        public Actie(string omschrijving, Opdracht opdracht)
        {
            Omschrijving = omschrijving;
            GelinkteOpdracht = opdracht;
        }

        public Actie()
        {

        }
        #endregion
    }
}
