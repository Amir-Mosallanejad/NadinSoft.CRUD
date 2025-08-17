namespace NadinSoft.CRUD.Application.Common.Interfaces;

/// <summary>
/// Provides information about the currently authenticated user.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the unique identifier of the currently authenticated user.
    /// </summary>
    /// <value>
    /// The user's ID as a string, or <c>null</c> if no user is authenticated.
    /// </value>
    string? UserId { get; }
}