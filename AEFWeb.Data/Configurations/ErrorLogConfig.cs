using AEFWeb.Data.Configurations.Interfaces;
using AEFWeb.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AEFWeb.Data.Configurations
{
    public class ErrorLogConfig : IEntityTypeConfiguration<ErrorLog>, IConfiguring
    {
        public void Configure(EntityTypeBuilder<ErrorLog> builder)
        {
            builder.Property(c => c.Id);

            builder.Property(c => c.Message)
                .HasColumnType("varchar(5000)")
                .HasMaxLength(5000);

            builder.Property(c => c.ExceptionString)
                .HasColumnType("varchar(5000)")
                .HasMaxLength(5000);

            builder.Property(c => c.Date)
                .IsRequired();
        }
    }
}
