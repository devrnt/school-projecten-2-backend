using System;
using System.Linq;

namespace g16_dotnet.Models.Domain
{
    public class GeblokkeerdPadState : PadState
    {
        public GeblokkeerdPadState(string name) : base(name)
        {

        }

        public override void DeBlokkeer(Pad pad)
        {
            if (pad.Voortgang <= pad.Acties.Count(a => a.Actie.IsUitgevoerd))
                pad.PadState = new OpdrachtPadState("Opdracht");
            else
                pad.PadState = new ActiePadState("Actie");
        }
    }
}
