using System.Collections.Generic;
using System.Linq;

namespace g16_dotnet.Models.Domain
{
    public class BlokkeerGroepBehaviour : IGroepBehaviour
    {
        public IEnumerable<Groep> VoerUit(IEnumerable<Groep> groepen, int groepId)
        {
            if (groepId == 0)
                groepen.All(g => { if (g.Pad.State != States.Schatkist) g.BlokkeerPad(); return true; });
            else
                groepen.Single(g => g.GroepId == groepId).BlokkeerPad();
            return groepen;
        }
    }
}
