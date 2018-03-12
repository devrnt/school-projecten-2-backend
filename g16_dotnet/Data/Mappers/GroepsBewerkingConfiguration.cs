using g16_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace g16_dotnet.Data.Mappers
{
    public class GroepsBewerkingConfiguration : IEntityTypeConfiguration<GroepsBewerking>
    {
        public void Configure(EntityTypeBuilder<GroepsBewerking> builder)
        {
            builder.ToTable("GroepsBewerking");

            builder.Property(gb => gb.Omschrijving)
                .IsRequired()
                .HasMaxLength(100);
            
        }
    }
}
