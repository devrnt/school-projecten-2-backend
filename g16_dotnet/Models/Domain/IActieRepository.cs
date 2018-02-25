using System.Collections.Generic;

namespace g16_dotnet.Models.Domain
{
    public interface IActieRepository
    {
        Actie GetById(int id);
        IEnumerable<Actie> GetAll();
        void Add(Actie actie);
        void Delete(Actie actie);
        void SaveChanges();
    }
}
