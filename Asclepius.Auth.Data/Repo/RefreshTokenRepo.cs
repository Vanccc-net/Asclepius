using System.Security.Claims;
using System.Text.Json;
using Asclepius.Auth.Domain;
using Asclepius.Auth.Domain.Interfaces;
using StackExchange.Redis;

namespace Asclepius.Auth.Data.Repo;

public class RefreshTokenRepo : IRefreshToken
{
    private readonly ApplicationContext _applicationContext;

    private readonly IDatabase _redis;

    public RefreshTokenRepo(IConnectionMultiplexer redis, ApplicationContext applicationContext)
    {
        _redis = redis.GetDatabase();
        _applicationContext = applicationContext;
    }

    public async Task<RefreshToken> Create(string userId, CancellationToken cancellationToken)
    {
        var expiry = TimeSpan.FromDays(30);

        var refresh = RefreshToken.Create($"refresh_token:{userId}", [new Claim(ClaimTypes.Sid, userId)]);
        var serializedValue = JsonSerializer.Serialize<RefreshToken>(refresh);
        await _redis.StringSetAsync(refresh.Value, serializedValue, expiry);
        return refresh;
    }

    public async Task<RefreshToken?> GetByKey(string key)
    {
        var refresh = await _redis.StringGetAsync($"refresh_token:{key}");
        return refresh.HasValue ? JsonSerializer.Deserialize<RefreshToken>(refresh!) : null;
    }

    public async Task DeleteByKey(string key)
    {
        await _redis.KeyDeleteAsync(key);
    }

    public Task<RefreshToken?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Add(RefreshToken entity)
    {
        throw new NotImplementedException();
    }

    public void Update(RefreshToken entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(RefreshToken entity)
    {
        throw new NotImplementedException();
    }
}