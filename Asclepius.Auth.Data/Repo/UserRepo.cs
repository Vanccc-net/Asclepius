using Asclepius.Auth.Domain;
using Asclepius.Auth.Domain.Interfaces;
using Asclepius.Auth.Domain.UserObject;
using Microsoft.EntityFrameworkCore;

namespace Asclepius.Auth.Data.Repo;

public class UserRepo(ApplicationContext context) : IUser
{
    public async Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var query = from user in context.Users
            where user.Id == id
            select user;

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

        var query = from user in context.Users
            where user.Email == emailRecord
            select user;

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetByEmailWithRolesAsync(string email, CancellationToken cancellationToken)
    {
        var emailRecord = new Email(email);

        var query = (from user in context.Users
            where user.Email == emailRecord
            select user).Include(user => user.Roles);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> EmailExistAsync(string email, CancellationToken cancellationToken)
    {
        var emailRecord = new Email(email);

        return await context.Users.AnyAsync(u => u.Email == emailRecord, cancellationToken);
    }
}