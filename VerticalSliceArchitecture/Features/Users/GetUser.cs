using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VerticalSliceArchitecture.Common.Contracts;
using VerticalSliceArchitecture.Common.Endpoints;
using VerticalSliceArchitecture.Infrastructure;

namespace VerticalSliceArchitecture.Features.Users;

public static class GetUser
{
    public record Response(UserDto Value) : IResponse<UserDto>;
    public record UserDto(string Name, string Email);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/users/{id:guid}", Handle)
                .WithName("GetUser")
                .WithTags("Users")
                .WithSummary("Gets a user by id.")
                .WithDescription("Returns a single user from the system.")
                .Produces<Response>(StatusCodes.Status200OK)
                .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
                .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
                .WithOpenApi();
        }

    }

    public static async Task<IResult> Handle(
        [FromRoute] Guid id,
        [FromServices] AppDbContext dbContext,
        CancellationToken cancellationToken = default)
    {
        var userDto = await dbContext.Users
            .AsNoTracking()
            .Where(x => x.Id == new Domain.UserId(id))
            .Select(x => new UserDto(x.Name, x.Email))
            .FirstOrDefaultAsync(cancellationToken);

        return userDto is null
            ? Results.Problem(
                detail: $"User with id 'id' was not found.",
                statusCode: StatusCodes.Status404NotFound)
            : Results.Ok(new Response(userDto));
    }
}
