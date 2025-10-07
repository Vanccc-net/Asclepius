using Asclepius.Auth.Domain;
using Asclepius.Auth.Domain.Interfaces;
using Asclepius.Auth.Domain.UserObject;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Asclepius.Auth.Data.Repo;

public class UserRepo(ApplicationContext context, IConnectionMultiplexer redis) : IUser
{
    
    private readonly IDatabase _redis = redis.GetDatabase();
    
    public async Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var query = (from user in context.Users
            where user.Id == id
            select user).AsNoTracking();

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public void Add(User entity)
    {
        context.Users.Add(entity);
    }

    public void Update(User entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(User entity)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var emailRecord = new Email(email);

        var query = (from user in context.Users
            where user.Email == emailRecord
            select user).AsNoTracking();

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetByEmailWithRolesAsync(string email, CancellationToken cancellationToken)
    {
        var emailRecord = new Email(email);

        var query = (from user in context.Users
            where user.Email == emailRecord
            select user).Include(user => user.Roles).AsNoTracking();

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> EmailExistAsync(string email, CancellationToken cancellationToken)
    {
        
        var cache = await _redis.StringGetAsync($"email_exist_{email}").ConfigureAwait(false);
        if (cache.IsNullOrEmpty)
        {
            var emailRecord = new Email(email);   
            var response = await context.Users.AsNoTracking().AnyAsync(u => u.Email == emailRecord, cancellationToken);
            if (response)
            {
                var expiry = TimeSpan.FromDays(30);
                await _redis.StringSetAsync($"email_exist_{email}", emailRecord.Value, expiry).ConfigureAwait(false);
            }
            return response;
        }
        return true;
    }

    public async Task MarkEmailAsExistsAsync(string email, CancellationToken cancellationToken)
    {
        var expiry = TimeSpan.FromDays(30);
        await _redis.StringSetAsync($"email_exist_{email}", email, expiry).ConfigureAwait(false);
    }
}