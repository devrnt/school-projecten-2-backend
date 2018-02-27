using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace g16_dotnet.Models.Domain
{
    public class Sessie
    {
        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SessieCode { get; set; }
        public string Naam { get; set; }
        public string Omschrijving { get; set; }
        public bool IsActief { get; set; }
        public IEnumerable<Groep> Groepen { get; set; }
        public Klas Klas { get; set; }
        #endregion

        #region Constructor
        public Sessie(int code, string naam, string omschrijving, IEnumerable<Groep> groepen, Klas klas)
        {
            SessieCode = code;
            Naam = naam;
            Omschrijving = omschrijving;
            Groepen = groepen;
            Klas = klas;
        }

        public Sessie()
        {
            Groepen = new List<Groep>();
        }
        #endregion

        #region Methods
        //public bool ControleerSessieCode(int code)
        //{
        //    if (code < 0)
        //        throw new ArgumentException("SessieCode kan niet negatief zijn. ");
        //    return code == SessieCode;
        //}

        public void ActiveerSessie()
        {
            if (Groepen.Any(g => !g.DeelnameBevestigd))
                throw new InvalidOperationException("Alle groepen moeten eerst hun deelname bevestigd hebben!");
            IsActief = true;
        }
        #endregion
    }
}
