namespace SmartScannerPro.Application.Exceptions;

using System;
using System.Collections.Generic;
using FluentValidation.Results;

/// <summary>
/// Exception thrown when one or more validation errors occur in the application layer.
/// </summary>
public class ApplicationValidationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationValidationException"/> class.
    /// </summary>
    public ApplicationValidationException()
        : base("One or more validation failures have occurred.")
    {
        this.Errors = new Dictionary<string, string[]>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationValidationException"/> class with a specified collection of validation failures.
    /// </summary>
    /// <param name="failures">The collection of validation failures.</param>
    public ApplicationValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        var failureGroups = new Dictionary<string, List<string>>();

        foreach (var failure in failures)
        {
            if (!failureGroups.ContainsKey(failure.PropertyName))
            {
                failureGroups[failure.PropertyName] = new List<string>();
            }

            failureGroups[failure.PropertyName].Add(failure.ErrorMessage);
        }

        var errors = new Dictionary<string, string[]>();
        foreach (var group in failureGroups)
        {
            errors.Add(group.Key, group.Value.ToArray());
        }

        this.Errors = errors;
    }

    /// <summary>
    /// Gets the validation errors, keyed by property name.
    /// </summary>
    public IDictionary<string, string[]> Errors { get; }
}
