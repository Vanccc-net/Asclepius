using Asclepius.DTO.Auth;
using MediatR;

namespace Asclepius.Auth.Api.MediatR.Commands;

public record AddRoleCommand(string RoleId, string UserId) : IRequest<JwtResponse>;