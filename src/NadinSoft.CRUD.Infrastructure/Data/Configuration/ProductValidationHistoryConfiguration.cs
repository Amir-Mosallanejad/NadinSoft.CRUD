using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NadinSoft.CRUD.Domain.Entities;

namespace NadinSoft.CRUD.Infrastructure.Data.Configuration;

/// <summary>
/// Configures the entity mapping for <see cref="ProductValidationHistory"/>.
/// </summary>
public class ProductValidationHistoryConfiguration : IEntityTypeConfiguration<ProductValidationHistory>
{
    /// <summary>
    /// Configures the schema needed for the <see cref="ProductValidationHistory"/> entity.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<ProductValidationHistory> builder)
    {
        builder.HasKey(pvh => pvh.Id);

        builder.Property(pvh => pvh.OldValue)
            .IsRequired(false);

        builder.Property(pvh => pvh.NewValue)
            .IsRequired();

        builder.Property(pvh => pvh.ModifyDate)
            .IsRequired();

        builder.HasOne(pvh => pvh.User)
            .WithMany(u => u.ProductValidationHistories)
            .HasForeignKey(pvh => pvh.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pvh => pvh.Product)
            .WithMany(p => p.ValidationHistories)
            .HasForeignKey(pvh => pvh.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}