using Asclepius.Auth.Domain;
using MediatR;

namespace Asclepius.Auth.Api.MediatR.Queries;

public record RoleListQueries : IRequest<IEnumerable<Role>>;