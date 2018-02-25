using System.Collections.Generic;

namespace g16_dotnet.Models.Domain
{
    public interface IOpdrachtRepository
    {
        Opdracht GetById(int id);
        IEnumerable<Opdracht> GetAll();
        void Add(Opdracht opdracht);
        void Delete(Opdracht opdracht);
        void SaveChanges();
    }
}
