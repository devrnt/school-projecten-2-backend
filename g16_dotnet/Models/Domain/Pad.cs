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
        public int? Voortgang { get { return Opdrachten?.Where(po => po.Opdracht.IsVoltooid).Count(); } }
        public Opdracht HuidigeOpdracht { get { return Opdrachten?.FirstOrDefault(po => !po.Opdracht.IsVoltooid)?.Opdracht; } }
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
        public Actie HuidigeActie { get { return Acties?.FirstOrDefault(pa => !pa.Actie.IsUitgevoerd)?.Actie; } }
        public ICollection<PadOpdracht> Opdrachten { get; set; }
        public ICollection<PadActie> Acties { get; set; }
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

            // Stelt de PadState in adhv de State die is opgeslagen in de databank
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
            PadState.DeBlokkeer(this);
        }
        #endregion
    }
}