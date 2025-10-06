using Asclepius.Auth.Domain.UserObject;

namespace Asclepius.Auth.Domain;

public class User
{
    private readonly List<Role> _roles = new();

    private User(string email, string password, string firstName, string lastName)
    {
        Id = Guid.NewGuid().ToString();
        Email = new Email(email);
        Password = new Password(password);
        FirstName = firstName;
        LastName = lastName;
    }

    [Obsolete]
    private User()
    {
        Id = Guid.NewGuid().ToString();
        Email = new Email(string.Empty);
        Password = new Password(string.Empty);
        FirstName = string.Empty;
        LastName = string.Empty;
    }

    public string Id { get; init; }
    public Email Email { get; private set; }
    public Password Password { get; private set; }

    public IReadOnlyList<Role> Roles => _roles;

    public string FirstName { get; }
    public string LastName { get; }
    public string FullName => $"{FirstName} {LastName}";

    public void AddRole(Role role)
    {
        _roles.Add(role);
    }

    public static User Create(string email, string password, string firstName, string lastName)
    {
        return new User(email, password, firstName, lastName);
    }
}