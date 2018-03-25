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
        public IGroepBehaviour GroepBehaviour { get; set; }
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
            IsActief = true;
        }

        public void WijzigGroepen(int behaviourId, int groepId)
        {
            switch (behaviourId)
            {
                case 0: GroepBehaviour = new BlokkeerGroepBehaviour(); break;
                case 1: GroepBehaviour = new DeblokkeerGroepBehaviour(); break;
                case 2: GroepBehaviour = new OntgrendelGroepBehaviour(); break;
            }

            Groepen = GroepBehaviour.VoerUit(Groepen, groepId);
        }
        #endregion
    }
}
