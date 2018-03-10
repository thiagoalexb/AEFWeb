using AEFWeb.Data.Configurations.Interfaces;
using AEFWeb.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AEFWeb.Data.Configurations
{
    public class SpecialWeekConfig : IEntityTypeConfiguration<SpecialWeek>, IConfiguring
    {
        public void Configure(EntityTypeBuilder<SpecialWeek> builder)
        {
            builder.Property(c => c.Id);

            builder.Property(c => c.Title)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Description)
               .HasColumnType("varchar(5000)")
               .HasMaxLength(100)
               .IsRequired();

            builder.HasMany(x => x.Lessons)
                .WithOne(x => x.SpecialWeek);
        }
    }
}
