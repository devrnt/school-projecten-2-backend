using g16_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Data.Repositories
{
    public class GroepRepository : IGroepRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Groep> _groepen;

        public GroepRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
            _groepen = _context.Groepen;
        }

        public void Add(Groep groep)
        {
            _groepen.Add(groep);
        }

        public void Delete(Groep groep)
        {
            _groepen.Remove(groep);
        }

        public IEnumerable<Groep> GetAll()
        {
            return _groepen.Include(g => g.Pad).ToList();
        }

        public Groep GetById(int id)
        {
            return _groepen.Include(g => g.Pad).SingleOrDefault(g => g.GroepId == id);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
