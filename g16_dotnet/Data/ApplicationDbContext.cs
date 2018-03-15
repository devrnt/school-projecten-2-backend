using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using g16_dotnet.Models;
using g16_dotnet.Data.Mappers;
using g16_dotnet.Models.SessieViewModel;
using g16_dotnet.Models.Domain;

namespace g16_dotnet.Data
{
    public class ApplicationDbContext : IdentityDbContext<Models.ApplicationUser>
    {
        public DbSet<Sessie> Sessies { get; set; }
        public DbSet<Pad> Paden { get; set; }
        public DbSet<Opdracht> Opdrachten { get; set; }
        public DbSet<Actie> Acties { get; set; }
        public DbSet<Groep> Groepen { get; set; }
        public DbSet<Leerkracht> Leerkrachten { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            //.\SQLEXPRESS
            var connectionstring = @"Server=LAPTOP-JONAS;Database=BreakoutBox;Integrated Security=True;";
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
            modelBuilder.ApplyConfiguration(new SessieConfiguration());
            modelBuilder.ApplyConfiguration(new LeerlingConfiguration());
            modelBuilder.ApplyConfiguration(new KlasConfiguration());
            modelBuilder.ApplyConfiguration(new GroepLeerlingConfiguration());
            modelBuilder.ApplyConfiguration(new LeerkrachtConfiguration());
            modelBuilder.Ignore<SessieLijstViewModel>();
            modelBuilder.Ignore<PadState>();
            modelBuilder.Ignore<ActiePadState>();
            modelBuilder.Ignore<GeblokkeerdPadState>();
            modelBuilder.Ignore<SchatkistPadState>();
            modelBuilder.Ignore<OpdrachtPadState>();
            modelBuilder.Ignore<SessieDetailViewModel>();
        }
    }
}
