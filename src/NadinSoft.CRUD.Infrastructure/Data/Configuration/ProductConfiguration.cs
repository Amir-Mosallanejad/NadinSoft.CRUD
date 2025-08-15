// <copyright file="ProductConfiguration.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NadinSoft.CRUD.Infrastructure.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NadinSoft.CRUD.Domain.Entities;

/// <summary>
/// Configures the entity mapping for the <see cref="Product"/> entity.
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    /// <summary>
    /// Configures the <see cref="Product"/> entity type including keys, properties, relationships, and indexes.
    /// </summary>
    /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Define a unique index on ManufactureEmail and ProduceDate
        builder.HasIndex(p => new { p.ManufactureEmail, p.ProduceDate })
            .IsUnique();

        // Configure the relationship between Product and ApplicationUser
        builder.HasOne(p => p.CreatedByUser)
            .WithMany(u => u.Products)
            .HasForeignKey(p => p.CreatedByUserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Set the primary key
        builder.HasKey(p => p.Id);

        // Configure properties
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