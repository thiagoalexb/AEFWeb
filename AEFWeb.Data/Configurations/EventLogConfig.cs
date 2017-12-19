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
        }
    }
}

