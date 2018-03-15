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
        public DoelgroepEnum Doelgroep { get; set; } //nodig voor de custom css per 'doelgroep'
        #endregion

        #region Constructor
        public Sessie(int code, string naam, string omschrijving, IEnumerable<Groep> groepen, Klas klas)
        {
            SessieCode = code;
            Naam = naam;
            Omschrijving = omschrijving;
            Groepen = groepen;
            Klas = klas;
            Doelgroep = DoelgroepEnum.Jongeren;
        }

        public Sessie()
        {
            Groepen = new List<Groep>();
        }
        #endregion

        #region Methods
        public void ActiveerSessie()
        {
            // niet nodig als niet alle groepen geselecteerd moeten zijn
            //if (Groepen.Any(g => !g.DeelnameBevestigd))
            //    throw new InvalidOperationException("Alle groepen moeten eerst hun deelname bevestigd hebben!");
            IsActief = true;
        }

        public void BlokkeerAlleGroepen()
        {
            Groepen.All(g => { g.Pad.IsGeblokkeerd = true; return true; });
        }

        public void DeblokkeerAlleGroepen()
        {
            Groepen.All(g => { g.Pad.IsGeblokkeerd = false; return true; });
        }
        #endregion
    }
}
