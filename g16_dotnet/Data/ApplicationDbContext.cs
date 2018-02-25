using g16_dotnet.Data.Mappers;
using g16_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace g16_dotnet.Data
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<Sessie> Sessies { get; set; }
        public DbSet<Pad> Paden { get; set; }
        public DbSet<Opdracht> Opdrachten { get; set; }
        public DbSet<Actie> Acties { get; set; }
        public DbSet<Groep> Groepen { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            //.\SQLEXPRESS
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
            modelBuilder.ApplyConfiguration(new GroepConfiguration());
            modelBuilder.Ignore<Klas>();
            modelBuilder.Ignore<Leerling>();
            modelBuilder.Ignore<Sessie>();
        }

    }
}
