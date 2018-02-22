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
        public DbSet<Sessie> Sessies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            var connectionstring = @"Server=.\SQLEXPRESS;Database=BreakoutBox;Integrated Security=True;";
            optionsBuilder.UseSqlServer(connectionstring);
        }


        
    }
}
