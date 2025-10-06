namespace Asclepius.Auth.Domain.Interfaces;

public interface IUser : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> GetByEmailWithRolesAsync(string email, CancellationToken cancellationToken);
    Task<bool> EmailExistAsync(string email, CancellationToken cancellationToken);
}