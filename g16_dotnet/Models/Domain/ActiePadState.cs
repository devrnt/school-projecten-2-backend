using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Models.Domain
{
    public class ActiePadState : PadState
    {
        public ActiePadState() : base("Actie")
        {

        }

        public override bool ControleerToegangsCode(Pad pad, string toegangscode)
        {
            bool juist = pad.HuidigeOpdracht.Opdracht.ControleerToegangsCode(toegangscode);
            if (juist)
                pad.PadState = new OpdrachtPadState();
            return juist;
        }
    }
}
