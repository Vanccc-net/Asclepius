namespace Asclepius.Auth.Domain.Interfaces;

public interface IRole : IRepository<Role>
{
    Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default);
}