using g16_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace g16_dotnet.Data.Repositories
{
    public class ActieRepository : IActieRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Actie> _acties;

        public ActieRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
            _acties = _context.Acties;
        }

        public void Add(Actie actie)
        {
            _acties.Add(actie);
        }

        public void Delete(Actie actie)
        {
            _acties.Remove(actie);
        }

        public IEnumerable<Actie> GetAll()
        {
            return _acties.ToList();
        }

        public Actie GetById(int id)
        {
            return _acties.SingleOrDefault(a => a.ActieId == id);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
