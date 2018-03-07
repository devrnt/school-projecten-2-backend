using System.Linq;

namespace g16_dotnet.Models.Domain
{
    public class OpdrachtPadState : PadState
    {
        public OpdrachtPadState(string name) : base(name)
        {

        }

        public override bool ControleerAntwoord(Pad pad, int antwoord)
        {
            bool juist = pad.HuidigeOpdracht.ControleerAntwoord(antwoord);
            if (juist)
            {
                pad.HuidigeOpdracht.IsVoltooid = true ;
                pad.PadState = new ActiePadState("Actie");
            }
            if (pad.Opdrachten.All(o => o.Opdracht.IsVoltooid))
                pad.PadState = new SchatkistPadState("Schatkist");
            return juist;
        }
    }
}
