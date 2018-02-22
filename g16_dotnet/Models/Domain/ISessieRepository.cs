using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Models.Domain
{
    public interface ISessieRepository
    {
        Sessie GetById(int sessieCode);
        IEnumerable<Sessie> GetAll();
        void Delete(Sessie sessie);
        void Add(Sessie sessie);
        void SaveChanges();
    }
}
