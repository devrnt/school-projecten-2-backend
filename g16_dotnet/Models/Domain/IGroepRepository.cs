using System.Collections.Generic;

namespace g16_dotnet.Models.Domain
{
    public interface IGroepRepository
    {
        Groep GetById(int id);
        IEnumerable<Groep> GetAll();
        void Add(Groep groep);
        void Delete(Groep groep);
        void SaveChanges();
    }
}
