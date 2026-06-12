namespace SmartScannerPro.Tests.Unit.Application.Behaviors;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using SmartScannerPro.Application.Exceptions;
using SmartScannerPro.Application.Pipelines.Behaviors;
using Xunit;

/// <summary>
/// Unit tests for the ValidationBehavior.
/// </summary>
public class ValidationBehaviorTests
{
    private readonly Mock<IValidator<TestRequest>> validatorMock;
    private readonly ValidationBehavior<TestRequest, TestResponse> behavior;
    private readonly RequestHandlerDelegate<TestResponse> nextDelegate;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationBehaviorTests"/> class.
    /// </summary>
    public ValidationBehaviorTests()
    {
        this.validatorMock = new Mock<IValidator<TestRequest>>();
        this.behavior = new ValidationBehavior<TestRequest, TestResponse>(new[] { this.validatorMock.Object });
        this.nextDelegate = () => Task.FromResult(new TestResponse());
    }

    /// <summary>
    /// Tests that the behavior calls next when validation passes.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task Handle_WithValidRequest_CallsNext()
    {
        // Arrange
        var request = new TestRequest();
        this.validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        var response = await this.behavior.Handle(request, this.nextDelegate, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
    }

    /// <summary>
    /// Tests that the behavior throws ApplicationValidationException when validation fails.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task Handle_WithInvalidRequest_ThrowsApplicationValidationException()
    {
        // Arrange
        var request = new TestRequest();
        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Property1", "Error message"),
        };
        this.validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationFailures));

        // Act & Assert
        var act = async () => await this.behavior.Handle(request, this.nextDelegate, CancellationToken.None);
        await act.Should().ThrowAsync<ApplicationValidationException>()
            .WithMessage("One or more validation failures have occurred.");
    }

    /// <summary>
    /// A test request.
    /// </summary>
    public class TestRequest : IRequest<TestResponse>
    {
    }

    /// <summary>
    /// A test response.
    /// </summary>
    public class TestResponse
    {
    }
}
