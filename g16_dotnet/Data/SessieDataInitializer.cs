using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Data
{
    public class SessieDataInitializer
    {
        private readonly ApplicationDbContext _context;

        public SessieDataInitializer(ApplicationDbContext context) {
            _context = context;
        }

        // tijdelijk database maken, als er nood is aan een db
        // kunnen we hier sessies toevoegen en saveChanges aanspreken in de ApplicationDbContext
        public void InitializeData() {
            // data here...
        }
    }
}
