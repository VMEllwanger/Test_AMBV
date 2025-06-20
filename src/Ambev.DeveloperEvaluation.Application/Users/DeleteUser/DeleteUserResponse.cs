using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.Application.Users.DeleteUser;

/// <summary>
/// Response model for DeleteUser operation
/// </summary>
/// 
[ExcludeFromCodeCoverage]
public class DeleteUserResponse
{
    /// <summary>
    /// Indicates whether the deletion was successful
    /// </summary>
    public bool Success { get; set; }
}
