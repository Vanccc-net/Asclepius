using Asclepius.Auth.Domain.Interfaces;

namespace Asclepius.Auth.Data;

public class Repository<T>(ApplicationContext context) : IRepository<T> where T : class
{
    public Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Add(T entity)
    {
        throw new NotImplementedException();
    }

    public void Update(T entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(T entity)
    {
        throw new NotImplementedException();
    }
}