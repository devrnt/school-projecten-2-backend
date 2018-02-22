using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Models.Domain
{
    public interface ISessieRepository
    {
        IEnumerable<Sessie> getAll();
        Sessie valideerCode(int inputCode);
        Sessie getById(int inputCode);
        Groep getByName(Sessie correcteSessie, string groepsnaam);
    }
}
