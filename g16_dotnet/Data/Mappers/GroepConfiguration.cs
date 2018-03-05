using g16_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace g16_dotnet.Data.Mappers
{
    public class GroepConfiguration : IEntityTypeConfiguration<Groep>
    {
        public void Configure(EntityTypeBuilder<Groep> builder)
        {
            builder.ToTable("Groep");

            builder.Property(g => g.Groepsnaam)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(g => g.Pad)
                .WithOne();

            
        }
    }
}
