using g16_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace g16_dotnet.Data.Mappers
{
    public class PadActieConfiguration : IEntityTypeConfiguration<PadActie>
    {
        public void Configure(EntityTypeBuilder<PadActie> builder)
        {
            builder.ToTable("PadActie");

            builder.HasKey(pa => new { pa.PadId, pa.ActieId });

            builder.HasOne(pa => pa.Pad)
                .WithMany()
                .HasForeignKey(pa => pa.PadId);

            builder.HasOne(pa => pa.Actie)
                .WithMany()
                .HasForeignKey(pa => pa.ActieId);
        }
    }
}
