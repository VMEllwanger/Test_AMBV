using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.Application.Users.CreateUser;

/// <summary>
/// Profile for mapping between User entity and CreateUserResponse
/// </summary>
/// 
[ExcludeFromCodeCoverage]
public class CreateUserProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateUser operation
    /// </summary>
    public CreateUserProfile()
    {
        CreateMap<CreateUserCommand, User>();
        CreateMap<User, CreateUserResult>();
    }
}
