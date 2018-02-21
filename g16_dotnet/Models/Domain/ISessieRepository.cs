using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Models.Domain
{
    public interface ISessieRepository
    {
        Sessie GetSessie(int sessieCode);
    
    }
}
