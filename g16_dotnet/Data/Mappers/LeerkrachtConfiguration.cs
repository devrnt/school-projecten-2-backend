using g16_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace g16_dotnet.Data.Mappers
{
    public class LeerkrachtConfiguration : IEntityTypeConfiguration<Leerkracht>
    {
        public void Configure(EntityTypeBuilder<Leerkracht> builder)
        {
            builder.ToTable("Leerkracht");

            builder.Property(l => l.Naam)
                .IsRequired()
                .HasMaxLength(50);

            builder.Ignore(l => l.InactieveSessies);

            builder.Property(l => l.Voornaam)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasMany(l => l.Sessies)
                .WithOne();
        }
    }
}
