using g16_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace g16_dotnet.Data.Mappers
{
    public class OefeningConfiguration : IEntityTypeConfiguration<Oefening>
    {
        public void Configure(EntityTypeBuilder<Oefening> builder)
        {
            builder.ToTable("Oefening");

            builder.Property(o => o.Opgave)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.GroepsAntwoord)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
