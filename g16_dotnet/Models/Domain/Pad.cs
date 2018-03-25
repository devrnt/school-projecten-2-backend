using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace g16_dotnet.Models.Domain
{
    public class Pad
    {
        private PadState _padState;
        #region Fields and Properties
        public int PadId { get; set; }
        public int? AantalOpdrachten { get { return Opdrachten?.Count(); } }
        public int? Voortgang { get { return Opdrachten?.Where(po => po.IsVoltooid).Count(); } }
        public PadOpdracht HuidigeOpdracht { get { return Opdrachten?.FirstOrDefault(po => !po.IsVoltooid); } }
        public PadState PadState { get { return _padState; }
            set
            {
                switch (value.StateName)
                {
                    case "Geblokkeerd":
                        State = States.Geblokkeerd;
                        break;
                    case "Opdracht":
                        State = States.Opdracht;
                        break;
                    case "Actie":
                        State = States.Actie;
                        break;
                    case "Vergrendeld":
                        State = States.Vergrendeld;
                        break;
                    case "Schatkist":
                        State = States.Schatkist;
                        break;
                }
                _padState = value;
            }
        }
        // Voor persistentie
        public States State { get; set; }
        public PadActie HuidigeActie { get { return Acties?.FirstOrDefault(pa => !pa.IsUitgevoerd); } }
        public IList<PadOpdracht> Opdrachten { get; set; }
        public IList<PadActie> Acties { get; set; }
        #endregion

        #region Constructors
        public Pad(IList<PadOpdracht> opdrachten, IList<PadActie> acties)
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
        public void AddOpdracht(Opdracht opdracht, int order)
        {
            Opdrachten.Add(new PadOpdracht(this, opdracht, order));
        }

        public void AddActie(Actie actie, int order)
        {
            Acties.Add(new PadActie(this, actie, order));
        }

        public bool ControleerAntwoord(int antwoord)
        {
            return PadState.ControleerAntwoord(this, antwoord);
        }

        public bool ControleerToegangsCode(string code)
        {
            return PadState.ControleerToegangsCode(this, code);
        }

        public void Ontgrendel()
        {
            PadState.Ontgrendel(this);
        }

        public void Vergrendel()
        {
            PadState.Vergrendel(this);
        }

        public void Blokkeer()
        {
            PadState.Blokkeer(this);
        }

        public void DeBlokkeer()
        {
            PadState.Deblokkeer(this);
        }
        #endregion
    }
}