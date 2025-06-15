using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUser;

/// <summary>
/// Profile for mapping between User entity and GetUserResponse
/// </summary>
/// 
[ExcludeFromCodeCoverage]
public class GetUserProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetUser operation
    /// </summary>
    public GetUserProfile()
    {
        CreateMap<User, GetUserResult>();
    }
}
