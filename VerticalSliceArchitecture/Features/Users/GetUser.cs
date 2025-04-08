using Microsoft.AspNetCore.Mvc;
using VerticalSliceArchitecture.Common.Abstractions.Repositories;
using VerticalSliceArchitecture.Common.Contracts;
using VerticalSliceArchitecture.Common.Contracts.Users;
using VerticalSliceArchitecture.Common.Endpoints;
using VerticalSliceArchitecture.Domain.Users;

namespace VerticalSliceArchitecture.Features.Users;

public static class GetUser
{
    public record Response(UserDto Value) : IResponse<UserDto>;

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
        [FromServices] IReadOnlyUserRepository userRepository,
        CancellationToken cancellationToken = default)
    {
        var userDto = await userRepository.GetDtoByIdAsync(new UserId(id), cancellationToken);

        return userDto is null
            ? Results.Problem(
                detail: $"User with id 'id' was not found.",
                statusCode: StatusCodes.Status404NotFound)
            : Results.Ok(new Response(userDto));
    }
}
