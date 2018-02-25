using g16_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace g16_dotnet.Data.Mappers
{
    public class LeerlingConfiguration : IEntityTypeConfiguration<Leerling>
    {
        public void Configure(EntityTypeBuilder<Leerling> builder)
        {
            builder.ToTable("Leerling");

            builder.Property(l => l.Naam)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(l => l.Voornaam)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
