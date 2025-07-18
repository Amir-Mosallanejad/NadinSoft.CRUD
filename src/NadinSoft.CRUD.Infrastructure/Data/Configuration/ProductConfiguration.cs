using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NadinSoft.CRUD.Domain.Entities;

namespace NadinSoft.CRUD.Infrastructure.Data.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasIndex(p => new { p.ManufactureEmail, p.ProduceDate })
            .IsUnique();

        builder.HasOne(p => p.CreatedByUser)
            .WithMany(u => u.Products)
            .HasForeignKey(p => p.CreatedByUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.ProduceDate)
            .IsRequired();

        builder.Property(p => p.ManufacturePhone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.ManufactureEmail)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.IsAvailable)
            .IsRequired();
    }
}