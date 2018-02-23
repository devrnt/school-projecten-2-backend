using g16_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace g16_dotnet.Data.Mappers
{
    public class ActieConfiguration : IEntityTypeConfiguration<Actie>
    {
        public void Configure(EntityTypeBuilder<Actie> builder)
        {
            builder.ToTable("Actie");

            builder.Property(a => a.Omschrijving)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(a => a.GelinkteOpdracht)
                .WithMany();
        }
    }
}
