namespace NadinSoft.CRUD.Application.Common.ResourceKeys;

/// <summary>
/// Contains keys for API message resources used for localization.
/// </summary>
public static class ApiMessageResourceKey
{
    /// <summary>
    /// Key for the message indicating that the email already exists.
    /// </summary>
    public const string EmailAlreadyExists = "EmailAlreadyExists";

    /// <summary>
    /// Key for the message indicating invalid login credentials.
    /// </summary>
    public const string Invalidcredentials = "Invalidcredentials";

    /// <summary>
    /// Key for the message indicating the user is not the owner of the product when attempting deletion.
    /// </summary>
    public const string NotOwnerOfProductDelete = "NotOwnerOfProductDelete";

    /// <summary>
    /// Key for the message indicating the user is not the owner of the product when attempting an update.
    /// </summary>
    public const string NotOwnerOfProductUpdate = "NotOwnerOfProductUpdate";

    /// <summary>
    /// Key for the message indicating that the product already exists.
    /// </summary>
    public const string ProductAlreadyExists = "ProductAlreadyExists";

    /// <summary>
    /// Key for the message indicating that the product was not found.
    /// </summary>
    public const string ProductNotFound = "ProductNotFound";

    /// <summary>
    /// Key for the message indicating an unexpected error occurred.
    /// </summary>
    public const string UnexpectedError = "UnexpectedError";

    /// <summary>
    /// Key for the message indicating the user is unauthorized.
    /// </summary>
    public const string UserUnauthorized = "UserUnauthorized";
}