using AEFWeb.Data.Configurations.Interfaces;
using AEFWeb.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AEFWeb.Data.Configurations
{
    class EventConfig : IEntityTypeConfiguration<Event>, IConfiguring
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.Property(c => c.Id);

            builder.Property(c => c.Date)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasMany(c => c.Lessons);
        }
    }
}
