using g16_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Data.Repositories {
    public class PadRepository : IPadRepository {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Pad> _paden;
        public PadRepository(ApplicationDbContext context) {
            _context = context;
            _paden = _context.Paden;

        }
        public Pad GetById(int id) {
            return _paden
                .Include(p => p.Opdrachten)
                    .ThenInclude(opdracht => opdracht.GroepsBewerking)
                .Include(p => p.Opdrachten)
                    .ThenInclude(opdracht => opdracht.Oefening)
                .Include(p => p.Acties)
                .SingleOrDefault(p => p.PadId == id);
        }

        public IEnumerable<Pad> GetAll() {
            return _paden
                .Include(p => p.Opdrachten)
                    .ThenInclude(opdracht => opdracht.GroepsBewerking)
               .Include(p => p.Opdrachten)
                    .ThenInclude(opdracht => opdracht.Oefening)
                .Include(p => p.Acties)
               .ToList();
        }

        public void SaveChanges() {
            _context.SaveChanges();
        }
    }
}
