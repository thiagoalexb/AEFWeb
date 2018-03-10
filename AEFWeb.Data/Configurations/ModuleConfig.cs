using AEFWeb.Data.Configurations.Interfaces;
using AEFWeb.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AEFWeb.Data.Configurations
{
    public class ModuleConfig : IEntityTypeConfiguration<Module>, IConfiguring
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.Property(c => c.Id);

            builder.Property(c => c.Name)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(c => c.Description)
               .HasColumnType("varchar(5000)")
               .HasMaxLength(100);

            builder.HasMany(x => x.Lessons)
                .WithOne(x => x.Module);

            builder.HasOne(x => x.Fase)
                .WithMany(x => x.Modules);
        }
    }
}
