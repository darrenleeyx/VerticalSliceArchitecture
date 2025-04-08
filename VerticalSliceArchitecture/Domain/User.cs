namespace VerticalSliceArchitecture.Domain;

public record UserId(Guid Value) : IEntityId<Guid>;

public class User
{
    public UserId Id { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;

    public void ChangeEmail(string newEmail)
    {
        Email = newEmail;
    }

    public static User Create(string name, string email)
    {
        return new User
        {
            Id = new UserId(Guid.NewGuid()),
            Name = name,
            Email = email
        };
    }

    private User() { }
}