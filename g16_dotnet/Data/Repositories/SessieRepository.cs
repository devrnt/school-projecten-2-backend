using g16_dotnet.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Data.Repositories
{
    public class SessieRepository : ISessieRepository
    {
        private ApplicationDbContext _context;
        public SessieRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Sessie> getAll()
        {
            return _context.sessies;
        }

        public Sessie getById(int inputCode)
        {
            Sessie sessie = null;

            foreach (Sessie ses in _context.sessies)
            {
                if (ses.Code == inputCode)
                {
                    sessie = ses;
                }
            }
            return sessie;
        }

        public Groep getByName(Sessie correcteSessie, string groepsnaam)
        {
           
            Groep groep = null;
            //foreach (Groep gr in correcteSessie.Groepen)
            //{
            //    if (gr.Groepsnaam == groepsnaam) {
            //        groep = gr;
            //    }
            //}
            return groep;
        }

        public Sessie valideerCode(int inputCode)
        {
            Sessie sessie = null;

            foreach (Sessie ses in _context.sessies)
            {
                if (ses.Code == inputCode)
                {
                    sessie = ses;
                }
            }
            return sessie;
        }
    }
}
