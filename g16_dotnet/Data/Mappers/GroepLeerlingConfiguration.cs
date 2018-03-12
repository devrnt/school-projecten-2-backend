using g16_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace g16_dotnet.Data.Mappers
{
    public class GroepLeerlingConfiguration : IEntityTypeConfiguration<GroepLeerling>
    {
        public void Configure(EntityTypeBuilder<GroepLeerling> builder)
        {
            builder.ToTable("GroepLeerling");

            builder.HasKey(gl => new { gl.GroepId, gl.LeerlingId });

            builder.HasOne(gl => gl.Groep)
                .WithMany()
                .HasForeignKey(gl => gl.GroepId);

            builder.HasOne(gl => gl.Leerling)
                .WithMany()
                .HasForeignKey(gl => gl.LeerlingId);
        }
    }
}
