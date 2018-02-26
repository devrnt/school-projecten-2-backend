using g16_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Data.Repositories
{
    public class LeerkrachtRepository : ILeerkrachtRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Leerkracht> _leerkrachten;

        public LeerkrachtRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
            _leerkrachten = _context.Leerkrachten;
        }

        public void Add(Leerkracht leerkracht)
        {
            _leerkrachten.Add(leerkracht);
        }

        public void Delete(Leerkracht leerkracht)
        {
            _leerkrachten.Remove(leerkracht);
        }

        public IEnumerable<Leerkracht> GetAll()
        {
            return _leerkrachten.ToList();
        }

        public Leerkracht GetById(int id)
        {
            return _leerkrachten.Include(l => l.Sessies)
                .ThenInclude(s => s.Groepen)
                .ThenInclude(g => g.Leerlingen)
                .SingleOrDefault(l => l.LeerkrachtId == id);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
