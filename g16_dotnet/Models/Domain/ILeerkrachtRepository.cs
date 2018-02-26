using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Models.Domain
{
    public interface ILeerkrachtRepository
    {
        Leerkracht GetById(int id);
        IEnumerable<Leerkracht> GetAll();
        void Add(Leerkracht leerkracht);
        void Delete(Leerkracht leerkracht);
        void SaveChanges();
    }
}
