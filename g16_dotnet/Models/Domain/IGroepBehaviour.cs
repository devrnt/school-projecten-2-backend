using System.Collections.Generic;

namespace g16_dotnet.Models.Domain
{
    public interface IGroepBehaviour
    {
        IEnumerable<Groep> VoerUit(IEnumerable<Groep> groepen, int groepId);
    }
}
