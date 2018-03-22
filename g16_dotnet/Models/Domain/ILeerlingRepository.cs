using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Models.Domain
{
    public interface ILeerlingRepository
    {
        Leerling GetById(int id);

        IEnumerable<Leerling> GetAll();

        Leerling GetByEmail(string email);
    }
}
