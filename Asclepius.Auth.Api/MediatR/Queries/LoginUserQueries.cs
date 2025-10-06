using Asclepius.DTO.Auth;
using MediatR;

namespace Asclepius.Auth.Api.MediatR.Queries;

public record LoginUserQueries(string Email, string Password) : IRequest<JwtResponse>;