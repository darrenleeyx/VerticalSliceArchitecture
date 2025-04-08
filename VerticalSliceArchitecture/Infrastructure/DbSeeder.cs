using VerticalSliceArchitecture.Domain.Users;

namespace VerticalSliceArchitecture.Infrastructure;

public static class DbSeeder
{
    public static void Seed(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (db.Users.Any()) return;

        var users = new[]
        {
            User.Create("Alice", "alice@example.com"),
            User.Create("Bob", "bob@example.com"),
            User.Create("Charlie", "charlie@example.com")
        };

        db.Users.AddRange(users);
        db.SaveChanges();
    }
}