using AEFWeb.Data.Configurations.Interfaces;
using AEFWeb.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AEFWeb.Data.Configurations
{
    public class SystemConfigurationConfig : IEntityTypeConfiguration<SystemConfiguration>, IConfiguring
    {
        public void Configure(EntityTypeBuilder<SystemConfiguration> builder)
        {
            builder.Property(c => c.Id);

            builder.Property(c => c.Key)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Value)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Deleted);
        }
    }
}
