using g16_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace g16_dotnet.Data.Repositories {
    public class SessieRepository : ISessieRepository {

        private readonly ApplicationDbContext _context;
        private readonly DbSet<Sessie> _sessies;

        public SessieRepository(ApplicationDbContext context) {
            _context = context;
            _sessies = context.Sessies;
        }

        public void Add(Sessie sessie) {
            _sessies.Add(sessie);
        }

        public void Delete(Sessie sessie) {
            _sessies.Remove(sessie);
        }

        public IEnumerable<Sessie> GetAll() {
            return _sessies.Include(s => s.Groepen)
                .ThenInclude(g => g.Leerlingen)
                .Include(s => s.Klas)
                .ToList();
        }

        public Sessie GetById(int id) {
            var sessie = _sessies
                .Include(s => s.Klas)
                    .ThenInclude(g=>g.Leerlingen)
                .Include(s => s.Groepen)
                    .ThenInclude(g => g.Leerlingen)
                 .Include(s => s.Groepen)
                    .ThenInclude(g => g.Pad)
                        .ThenInclude(po => po.Opdrachten)
                            .ThenInclude(po => po.Opdracht)
                .Include(s => s.Groepen)
                    .ThenInclude(g => g.Pad)
                        .ThenInclude(p => p.Acties)
                            .ThenInclude(pa => pa.Actie)
                .SingleOrDefault(s => s.SessieCode == id);
            if (sessie != null) {
                foreach (var groep in sessie.Groepen) {
                    switch (groep.Pad.State) {
                        case States.Geblokkeerd:
                            groep.Pad.PadState = new GeblokkeerdPadState();
                            break;
                        case States.Opdracht:
                            groep.Pad.PadState = new OpdrachtPadState();
                            break;
                        case States.Actie:
                            groep.Pad.PadState = new ActiePadState();
                            break;
                        case States.Vergrendeld:
                            groep.Pad.PadState = new VergrendeldPadState();
                            break;
                        case States.Schatkist:
                            groep.Pad.PadState = new SchatkistPadState();
                            break;
                    }
                }
                sessie.Groepen = sessie.Groepen.OrderBy(g => g.Groepsnaam).ToList();
            }



            return sessie;
        }

        public void SaveChanges() {
            _context.SaveChanges();
        }
    }
}