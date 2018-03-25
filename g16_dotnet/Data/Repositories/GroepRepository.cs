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
            return _groepen.Include(g => g.Pad)
                .Include(g => g.Leerlingen)
                .ToList();
        }

        public Groep GetById(int id)
        {
            var groep =  _groepen
                .Include(g => g.Pad)
                .Include(p => p.Leerlingen)
                .SingleOrDefault(g => g.GroepId == id);

            switch (groep.Pad.State)
            {
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

            return groep;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
