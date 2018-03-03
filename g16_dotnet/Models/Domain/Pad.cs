using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace g16_dotnet.Models.Domain
{
    public class Pad
    {
        #region Fields and Properties
        public int PadId { get; set; }
        public int? AantalOpdrachten { get { return Opdrachten?.Count(); } }
        public int? Voortgang { get { return Opdrachten?.Where(po => po.Opdracht.IsVoltooid).Count(); } }
        public Opdracht HuidigeOpdracht { get { return Opdrachten?.FirstOrDefault(po => !po.Opdracht.IsVoltooid)?.Opdracht; } }
        public Actie HuidigeActie { get { return Acties?.FirstOrDefault(pa => !pa.Actie.IsUitgevoerd)?.Actie; } }
        public ICollection<PadOpdracht> Opdrachten { get; set; }
        public ICollection<PadActie> Acties { get; set; }
        public bool IsGeblokkeerd { get; set; }
        #endregion

        #region Constructors
        public Pad(ICollection<PadOpdracht> opdrachten, ICollection<PadActie> acties)
        {
            Opdrachten = opdrachten;
            Acties = acties;
            IsGeblokkeerd = false;
        }

        public Pad()
        {
            Opdrachten = new List<PadOpdracht>();
            Acties = new List<PadActie>();
            IsGeblokkeerd = false;
        }
        #endregion

        #region Methods
        public void AddOpdracht(Opdracht opdracht)
        {
            Opdrachten.Add(new PadOpdracht(this, opdracht));
        }

        public void AddActie(Actie actie)
        {
            Acties.Add(new PadActie(this, actie));
        }
        #endregion
    }
}
