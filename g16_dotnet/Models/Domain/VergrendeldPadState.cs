using System;

namespace g16_dotnet.Models.Domain
{
    public class VergrendeldPadState : PadState
    {
        public VergrendeldPadState(string name) : base(name)
        {

        }

        public override void Ontgrendel(Pad pad)
        {
            pad.PadState = new OpdrachtPadState("Opdracht");
            pad.HuidigeOpdracht.AantalPogingen = 0;
        }

        public override void Vergrendel(Pad pad)
        {
            throw new InvalidOperationException("Pad is al vergrendeld!");
        }
    }
}
