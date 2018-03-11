using AEFWeb.Data.Configurations.Interfaces;
using AEFWeb.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AEFWeb.Data.Configurations
{
    public class FaseConfig : IEntityTypeConfiguration<Fase>, IConfiguring
    {
        public void Configure(EntityTypeBuilder<Fase> builder)
        {
            builder.Property(c => c.Id);

            builder.Property(c => c.Name)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.HasMany(x => x.Modules)
                .WithOne(x => x.Fase);

            builder.Property(c => c.Deleted);
        }
    }
}
