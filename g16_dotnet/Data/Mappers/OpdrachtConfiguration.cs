using g16_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace g16_dotnet.Data.Mappers
{
    public class OpdrachtConfiguration : IEntityTypeConfiguration<Opdracht>
    {
        public void Configure(EntityTypeBuilder<Opdracht> builder)
        {
            builder.ToTable("Opdracht");

            builder.HasKey(o => o.VolgNr);

            builder.Property(o => o.ToegangsCode)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(o => o.Oefening)
                .WithMany();

            builder.HasOne(o => o.GroepsBewerking)
                .WithMany();
        }
    }
}
