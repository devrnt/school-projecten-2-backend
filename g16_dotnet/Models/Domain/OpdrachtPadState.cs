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
            bool juist = pad.HuidigeOpdracht.Opdracht.ControleerAntwoord(antwoord);
            if (juist)
            {
                pad.HuidigeOpdracht.IsVoltooid = true ;
                pad.PadState = new ActiePadState("Actie");
            } else
            {
                if (++pad.HuidigeOpdracht.AantalPogingen >= 3)
                {
                    pad.PadState = new VergrendeldPadState("Vergrendeld");
                }
            }
            if (pad.Opdrachten.All(po => po.IsVoltooid))
                pad.PadState = new SchatkistPadState("Schatkist");
            return juist;
        }
    }
}
