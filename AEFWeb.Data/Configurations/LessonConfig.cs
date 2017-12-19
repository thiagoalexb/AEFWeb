using AEFWeb.Data.Configurations.Interfaces;
using AEFWeb.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AEFWeb.Data.Configurations
{
    class LessonConfig : IEntityTypeConfiguration<Lesson>, IConfiguring
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.Property(c => c.Id);

            builder.Property(c => c.Schedule)
                .IsRequired();

            builder.Property(c => c.Title)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.SubTitle)
               .HasColumnType("varchar(100)")
               .HasMaxLength(100);

            builder.HasOne(x => x.Event)
                .WithMany(x => x.Lessons)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
