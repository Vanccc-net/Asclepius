using Asclepius.Auth.Domain;
using Asclepius.Auth.Domain.Interfaces;
using MediatR;

namespace Asclepius.Auth.Api.MediatR.Queries;

public class RoleListQueriesHandler(IRole roleRepo) : IRequestHandler<RoleListQueries, IEnumerable<Role>>
{
    public async Task<IEnumerable<Role>> Handle(RoleListQueries request, CancellationToken cancellationToken)
    {
        var roles = await roleRepo.GetAllAsync(cancellationToken);
        return roles;
    }
}