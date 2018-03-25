using System;
using System.Linq;

namespace g16_dotnet.Models.Domain
{
    public class GeblokkeerdPadState : PadState
    {
        public GeblokkeerdPadState() : base("Geblokkeerd")
        {

        }

        public override void Deblokkeer(Pad pad)
        {
            if (pad.Voortgang <= pad.Acties.Count(a => a.IsUitgevoerd))
                pad.PadState = new OpdrachtPadState();
            else
                pad.PadState = new ActiePadState();
        }
    }
}
