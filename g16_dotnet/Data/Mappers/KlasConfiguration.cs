using g16_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace g16_dotnet.Data.Mappers
{
    public class KlasConfiguration : IEntityTypeConfiguration<Klas>
    {
        public void Configure(EntityTypeBuilder<Klas> builder)
        {
            builder.ToTable("Klas");

            builder.Property(k => k.Naam)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(k => k.Leerlingen)
                .WithOne();
        }
    }
}
