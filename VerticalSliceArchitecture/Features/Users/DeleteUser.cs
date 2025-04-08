using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using VerticalSliceArchitecture.Common.Abstractions;
using VerticalSliceArchitecture.Common.Abstractions.Repositories;
using VerticalSliceArchitecture.Common.Endpoints;
using VerticalSliceArchitecture.Domain.Users;

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
        [FromServices] IReadWriteUserRepository userRepository,
        [FromServices] IUnitOfWork unitOfWork,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(new UserId(id), track: true, cancellationToken);

        if (user is null)
        {
            return Results.Problem(
                detail: $"User with id 'id' was not found.",
                statusCode: StatusCodes.Status404NotFound);
        }

        userRepository.Remove(user);
        await unitOfWork.CommitChangesAsync(cancellationToken);

        return Results.NoContent();
    }
}
