namespace Asclepius.Auth.Domain.Interfaces;

public interface IRefreshToken : IRepository<RefreshToken>
{
    Task<RefreshToken> Create(string userId, CancellationToken cancellationToken);
    Task<RefreshToken?> GetByKey(string key);
    Task DeleteByKey(string key);
}