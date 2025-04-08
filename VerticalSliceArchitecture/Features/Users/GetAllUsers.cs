using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VerticalSliceArchitecture.Common.Contracts;
using VerticalSliceArchitecture.Common.Endpoints;
using VerticalSliceArchitecture.Infrastructure;

namespace VerticalSliceArchitecture.Features.Users;

public static class GetAllUsers
{
    public record Response(List<UserDto> Value) : IResponse<List<UserDto>>;
    public record UserDto(string Name, string Email);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/users", Handle)
                .WithName("GetAllUsers")
                .WithTags("Users")
                .WithSummary("Gets a list of all users.")
                .WithDescription("Returns a full list of users from the system.")
                .Produces<Response>(StatusCodes.Status200OK)
                .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
                .WithOpenApi();
        }

    }

    public static async Task<IResult> Handle(
        [FromServices] AppDbContext dbContext,
        CancellationToken cancellationToken = default)
    {
        var userDtos = await dbContext.Users
            .AsNoTracking()
            .Select(x => new UserDto(x.Name, x.Email))
            .ToListAsync(cancellationToken);

        Response response = new(userDtos);

        return Results.Ok(response);
    }
}
