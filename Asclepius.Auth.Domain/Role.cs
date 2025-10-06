namespace Asclepius.Auth.Domain;

public class Role
{
    private readonly List<User> _users = new();

    private Role(string name)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
    }

    [Obsolete]
    private Role()
    {
        Id = Guid.NewGuid().ToString();
        Name = string.Empty;
    }

    public string Id { get; init; }
    public string Name { get; private set; }
    public IReadOnlyCollection<User> Users => _users.AsReadOnly();

    public static Role Create(string name)
    {
        return new Role(name);
    }
}