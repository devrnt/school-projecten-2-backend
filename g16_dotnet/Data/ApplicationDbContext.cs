using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Data
{
    public class ApplicationDbContext:DbContext
    {
        public IList<Sessie> sessies { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            SessieDataInitializer.InitializeData(this);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            // no connectionstring needed atm.
        }
        
    }
}
