using Asclepius.Auth.Domain;
using Asclepius.Auth.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Asclepius.Auth.Data.Repo;

public class RoleRepo(ApplicationContext context) : IRole
{
    public async Task<Role?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var query = (from role in context.Roles
            where role.Id == id
            select role).AsNoTracking();
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public void Add(Role entity)
    {
        context.Roles.Add(entity);
    }

    public void Update(Role entity)
    {
        context.Roles.Update(entity);
    }

    public void Delete(Role entity)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Roles.AsNoTracking().ToListAsync(cancellationToken);
    }
}