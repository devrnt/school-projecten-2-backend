using g16_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace g16_dotnet.Data.Mappers
{
    public class SessieConfiguration : IEntityTypeConfiguration<Sessie>
    {
        public void Configure(EntityTypeBuilder<Sessie> builder)
        {
            builder.ToTable("Sessie");

            builder.HasKey(s => s.SessieCode);

            builder.Property(s => s.Naam)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Omschrijving)
                .HasMaxLength(100);

            builder.HasOne(s => s.Klas)
                .WithMany();

            builder.HasMany(s => s.Groepen)
                .WithOne();
        }
    }
}
