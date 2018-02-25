using g16_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace g16_dotnet.Data.Repositories
{
    public class OpdrachtRepository : IOpdrachtRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Opdracht> _opdrachten;

        public OpdrachtRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
            _opdrachten = _context.Opdrachten;
        }

        public void Add(Opdracht opdracht)
        {
            _opdrachten.Add(opdracht);
        }

        public void Delete(Opdracht opdracht)
        {
            _opdrachten.Remove(opdracht);
        }

        public IEnumerable<Opdracht> GetAll()
        {
            return _opdrachten.Include(o => o.Oefening).Include(o => o.GroepsBewerking).ToList();
        }

        public Opdracht GetById(int id)
        {
            return _opdrachten.Include(o => o.Oefening).Include(o => o.GroepsBewerking).SingleOrDefault(o => o.VolgNr == id);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
