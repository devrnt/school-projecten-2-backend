using g16_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Data.Mappers
{
    public class PadConfiguration : IEntityTypeConfiguration<Pad>
    {
        public void Configure(EntityTypeBuilder<Pad> builder)
        {
            builder.ToTable("Pad");

            builder.Ignore(p => p.AantalOpdrachten);
            builder.Ignore(p => p.Voortgang);

        }
    }
}
