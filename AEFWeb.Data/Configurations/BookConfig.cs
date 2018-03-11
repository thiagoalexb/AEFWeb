using AEFWeb.Data.Configurations.Interfaces;
using AEFWeb.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AEFWeb.Data.Configurations
{
    public class BookConfig : IEntityTypeConfiguration<Book>, IConfiguring
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(c => c.Id);

            builder.Property(c => c.PublishingCompany)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(c => c.Edition)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(c => c.Author)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Title)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.IsSale);

            builder.Property(c => c.Value);

            builder.Property(c => c.Deleted);
        }
    }
}
