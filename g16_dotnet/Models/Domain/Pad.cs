using Newtonsoft.Json;
using System;
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
        public PadState PadState { get; set; }
        public bool IsVergrendeld { get { return HuidigeOpdracht !=null && HuidigeOpdracht.AantalPogingen > 2 ? true : false; } }


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
        }

        public Pad()
        {
            Opdrachten = new List<PadOpdracht>();
            Acties = new List<PadActie>();
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

        public bool ControleerAntwoord(int antwoord)
        {
            return PadState.ControleerAntwoord(this, antwoord);
        }

        public bool ControleerToegangsCode(string code)
        {           
            return PadState.ControleerToegangsCode(this, code);
        }
        #endregion
    }
}