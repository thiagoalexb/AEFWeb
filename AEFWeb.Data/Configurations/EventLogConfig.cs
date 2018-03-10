using AEFWeb.Data.Configurations.Interfaces;
using AEFWeb.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AEFWeb.Data.Configurations
{
    public class EventLogConfig : IEntityTypeConfiguration<EventLog>, IConfiguring
    {
        public void Configure(EntityTypeBuilder<EventLog> builder)
        {
            builder.Property(c => c.Id);

            builder.Property(c => c.Action)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(c => c.CreationDate);

            builder.Property(c => c.CreatorUserId);

            builder.Property(c => c.Data)
                .HasColumnType("varchar(5000)")
                .HasMaxLength(5000)
                .IsRequired();

            builder.Property(c => c.Type)
               .HasColumnType("varchar(100)")
               .HasMaxLength(100)
               .IsRequired();

            builder.Property(c => c.UpdateDate);

            builder.Property(c => c.UpdatedUserId);
        }
    }
}

