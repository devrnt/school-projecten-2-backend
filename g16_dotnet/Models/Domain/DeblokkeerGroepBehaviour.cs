using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Models.Domain
{
    public class DeblokkeerGroepBehaviour : IGroepBehaviour
    {
        public IEnumerable<Groep> VoerUit(IEnumerable<Groep> groepen, int groepId)
        {
            if (groepId == 0)
                groepen.All(g => { g.DeblokkeerPad(); return true; });
            else
                groepen.Single(g => g.GroepId == groepId).DeblokkeerPad();
            return groepen;
        }
    }
}
