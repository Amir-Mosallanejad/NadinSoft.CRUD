// <copyright file="ValidationBehavior.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NadinSoft.CRUD.Application.Common.Behaviors;

using FluentValidation;
using FluentValidation.Results;
using MediatR;

/// <summary>
/// A MediatR pipeline behavior that handles validation for requests using FluentValidation.
/// </summary>
/// <typeparam name="TRequest">
/// The type of request being validated. Must implement <see cref="IRequest{TResponse}"/>.
/// </typeparam>
/// <typeparam name="TResponse">
/// The type of response returned by the request handler.
/// </typeparam>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> validators;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="validators">
    /// A collection of validators for the request type.
    /// </param>
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        this.validators = validators;
    }

    /// <summary>
    /// Handles the request by performing validation before passing it to the next behavior or handler.
    /// </summary>
    /// <param name="request">The incoming request to validate.</param>
    /// <param name="next">The delegate to the next behavior or handler in the pipeline.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains the response from the next behavior or handler.
    /// </returns>
    /// <exception cref="ValidationException">
    /// Thrown when one or more validation failures occur.
    /// </exception>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (this.validators.Any())
        {
            ValidationContext<TRequest> context = new(request);
            List<ValidationFailure> failures = this.validators
                .Select(x => x.Validate(context))
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }
        }

        return await next(cancellationToken);
    }
}