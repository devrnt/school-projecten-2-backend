﻿using g16_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace g16_dotnet.Data.Repositories
{
    public class PadRepository : IPadRepository {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Pad> _paden;
        public PadRepository(ApplicationDbContext context) {
            _context = context;
            _paden = _context.Paden;

        }
        public Pad GetById(int id) {
            var pad =  _paden
                .Include(p => p.Opdrachten)
                    .ThenInclude(po => po.Opdracht)
                    .ThenInclude(o => o.GroepsBewerking)
                .Include(p => p.Opdrachten)
                    .ThenInclude(po => po.Opdracht)
                    .ThenInclude(opdracht => opdracht.Oefening)
                .Include(p => p.Acties)
                    .ThenInclude(pa => pa.Actie)
                .SingleOrDefault(p => p.PadId == id);

            pad.Opdrachten = pad.Opdrachten.OrderBy(po => po.Order).ToList();
            pad.Acties.OrderBy(pa => pa.Order);

            switch (pad.State)
            {
                case States.Geblokkeerd:
                    pad.PadState = new GeblokkeerdPadState();
                    break;
                case States.Opdracht:
                    pad.PadState = new OpdrachtPadState();
                    break;
                case States.Actie:
                    pad.PadState = new ActiePadState();
                    break;
                case States.Vergrendeld:
                    pad.PadState = new VergrendeldPadState();
                    break;
                case States.Schatkist:
                    pad.PadState = new SchatkistPadState();
                    break;
            }

            return pad;
        }

        public IEnumerable<Pad> GetAll() {
            return _paden
                .Include(p => p.Opdrachten)
                    .ThenInclude(po => po.Opdracht)
                    .ThenInclude(opdracht => opdracht.GroepsBewerking)
               .Include(p => p.Opdrachten)
                    .ThenInclude(po => po.Opdracht)
                    .ThenInclude(opdracht => opdracht.Oefening)
                .Include(p => p.Acties)
                    .ThenInclude(pa => pa.Actie)
               .ToList();
        }

        public void SaveChanges() {
            _context.SaveChanges();
        }
    }
}
