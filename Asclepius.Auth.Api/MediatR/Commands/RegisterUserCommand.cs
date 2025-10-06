using Asclepius.DTO.Auth;
using Asclepius.DTO.ProfileObject;
using MediatR;

namespace Asclepius.Auth.Api.MediatR.Commands;

public record RegisterUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    Gender Gender) : IRequest<JwtResponse>;