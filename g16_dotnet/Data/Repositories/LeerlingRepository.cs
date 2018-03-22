using g16_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Data.Repositories
{
    public class LeerlingRepository : ILeerlingRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Leerling> _leerlingen;

        public LeerlingRepository(ApplicationDbContext context)
        {
            _context = context;
            _leerlingen = _context.Leerlingen;
        }

        public IEnumerable<Leerling> GetAll()
        {
            return _leerlingen.ToList();
        }

        public Leerling GetByEmail(string email)
        {
            return _leerlingen.SingleOrDefault(l => l.Email == email);
        }

        public Leerling GetById(int id)
        {
            return _leerlingen.SingleOrDefault(l => l.LeerlingId == id);

        }
    }
}
