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
        }
    }
}
