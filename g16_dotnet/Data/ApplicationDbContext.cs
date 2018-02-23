using g16_dotnet.Data.Mappers;
using g16_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace g16_dotnet.Data
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<Sessie> Sessies { get; set; }
        public DbSet<Pad> Paden { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            var connectionstring = @"Server=.\SQLEXPRESS;Database=BreakoutBox;Integrated Security=True;";
            optionsBuilder.UseSqlServer(connectionstring);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new OefeningConfiguration());
            modelBuilder.ApplyConfiguration(new PadActieConfiguration());
            modelBuilder.ApplyConfiguration(new PadConfiguration());
            modelBuilder.ApplyConfiguration(new GroepsBewerkingConfiguration());
            modelBuilder.ApplyConfiguration(new OpdrachtConfiguration());
            modelBuilder.ApplyConfiguration(new PadOpdrachtConfiguration());
            modelBuilder.ApplyConfiguration(new ActieConfiguration());
            modelBuilder.Ignore<Groep>();
            modelBuilder.Ignore<Klas>();
            modelBuilder.Ignore<Leerling>();
            modelBuilder.Ignore<Sessie>();
        }

    }
}
