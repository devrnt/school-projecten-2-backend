using g16_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace g16_dotnet.Data.Mappers
{
    public class PadOpdrachtConfiguration : IEntityTypeConfiguration<PadOpdracht>
    {
        public void Configure(EntityTypeBuilder<PadOpdracht> builder)
        {
            builder.ToTable("PadOpdracht");

            builder.HasKey(po => new { po.PadId, po.OpdrachtId });

            builder.HasOne(po => po.Opdracht)
                .WithMany()
                .HasForeignKey(po => po.OpdrachtId);

            builder.HasOne(po => po.Pad)
                .WithMany()
                .HasForeignKey(po => po.PadId);
        }
    }
}
