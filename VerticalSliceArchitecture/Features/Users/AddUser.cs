using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using VerticalSliceArchitecture.Common.Contracts;
using VerticalSliceArchitecture.Common.Endpoints;
using VerticalSliceArchitecture.Domain;
using VerticalSliceArchitecture.Infrastructure;

namespace VerticalSliceArchitecture.Features.Users;

public static class AddUser
{
    public record Request(string Name, string Email);
    public record Response(Guid Value) : IResponse<Guid>;

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/users", Handle)
                .WithName("AddUser")
                .WithTags("Users")
                .WithSummary("Adds a new user.")
                .WithDescription("Creates a user from the submitted payload.")
                .Accepts<Request>(MediaTypeNames.Application.Json)
                .Produces(StatusCodes.Status204NoContent)
                .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
                .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
                .WithOpenApi();
        }
    }

    public static async Task<IResult> Handle(
        [FromBody] Request request,
        [FromServices] IValidator<Request> validator,
        [FromServices] AppDbContext dbContext,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var exists = await dbContext.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);

        if (exists)
        {
            return Results.Problem(
                detail: $"A user with email '{request.Email}' already exists.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        var user = User.Create(request.Name, request.Email);

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Created($"/users/{user.Id}", new Response(user.Id.Value));
    }
}
