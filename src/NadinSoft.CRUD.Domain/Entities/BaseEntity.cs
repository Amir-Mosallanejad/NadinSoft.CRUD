namespace NadinSoft.CRUD.Domain.Entities;

/// <summary>
/// Represents the base class for all entities in the domain.
/// Provides a unique identifier for each entity.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    /// <value>
    /// A globally unique identifier (GUID) that distinguishes this entity from all others.
    /// </value>
    public Guid Id { get; set; } = Guid.NewGuid();
}