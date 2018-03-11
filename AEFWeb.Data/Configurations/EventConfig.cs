using AEFWeb.Data.Configurations.Interfaces;
using AEFWeb.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AEFWeb.Data.Configurations
{
    class EventConfig : IEntityTypeConfiguration<Event>, IConfiguring
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.Property(c => c.Id);

            builder.Property(c => c.StartDate)
               .IsRequired();

            builder.Property(c => c.EndDate)
               .IsRequired();

            builder.HasOne(c => c.Lesson)
                .WithMany(c => c.Events);

            builder.Property(c => c.Deleted);
        }
    }
}
