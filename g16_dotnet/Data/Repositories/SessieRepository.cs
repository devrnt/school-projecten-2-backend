using g16_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace g16_dotnet.Data.Repositories
{
    public class SessieRepository : ISessieRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly DbSet<Sessie> _sessies;

        public SessieRepository(ApplicationDbContext context)
        {
            _context = context;
            _sessies = context.Sessies;
        }

        public void Add(Sessie sessie)
        {
            _sessies.Add(sessie);
        }

        public void Delete(Sessie sessie)
        {
            _sessies.Remove(sessie);
        }

        public IEnumerable<Sessie> GetAll()
        {
            return _sessies.Include(s => s.Groepen)
                .ThenInclude(g => g.Leerlingen)
                .Include(s => s.Klas)
                .ToList();
        }

        public Sessie GetById(int id)
        {
            return _sessies
                .Include(s => s.Klas)
                .Include(s => s.Groepen)
                .ThenInclude(g => g.Pad)
                .SingleOrDefault(s => s.SessieCode == id);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}