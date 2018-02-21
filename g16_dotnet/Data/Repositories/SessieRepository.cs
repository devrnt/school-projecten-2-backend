using g16_dotnet.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Data.Repositories
{
    public class SessieRepository : ISessieRepository{
        public Sessie GetSessie(int sessieCode) {
            // vraag aan dbCOntext of deze sessie met deze code bestaat
            // tijdelijk dit returnen
            if (sessieCode == 1234) {
                return new Sessie(1234, new Klas());
            } else {
                return null;
            }
        }
    }
}
