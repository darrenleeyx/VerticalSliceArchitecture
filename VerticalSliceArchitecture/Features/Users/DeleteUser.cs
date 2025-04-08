using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VerticalSliceArchitecture.Common.Endpoints;
using VerticalSliceArchitecture.Infrastructure;

namespace VerticalSliceArchitecture.Features.Users;

public static class DeleteUser
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("/users/{id:guid}", Handle)
                .WithName("DeleteUser")
                .WithTags("Users")
                .WithSummary("Deletes an existing user.")
                .WithDescription("Deletes a user by id.")
                .Produces(StatusCodes.Status204NoContent)
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
        var user = await dbContext.Users
            .SingleOrDefaultAsync(x => x.Id == new Domain.UserId(id), cancellationToken);

        if (user is null)
        {
            return Results.Problem(
                detail: $"User with id 'id' was not found.",
                statusCode: StatusCodes.Status404NotFound);
        }

        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.NoContent();
    }
}
