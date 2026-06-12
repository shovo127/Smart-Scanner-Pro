namespace SmartScannerPro.Tests.Architecture;

using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;
using Xunit;

/// <summary>
/// Architecture dependency validation tests.
/// </summary>
public class DependencyRulesTests
{
    private static readonly Assembly DomainAssembly = typeof(SmartScannerPro.Domain.Exceptions.DomainException).Assembly;
    private static readonly Assembly ApplicationAssembly = typeof(SmartScannerPro.Application.Exceptions.ApplicationExceptionBase).Assembly;
    private static readonly Assembly InfrastructureAssembly = typeof(SmartScannerPro.Infrastructure.DependencyInjection).Assembly;
    private static readonly Assembly UIAssembly = Assembly.Load("SmartScannerPro.UI");

    /// <summary>
    /// Verifies that the Domain layer has no dependencies on other projects.
    /// </summary>
    [Fact]
    public void Domain_ShouldNot_HaveDependenciesOnOtherProjects()
    {
        var result = Types.InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOn("SmartScannerPro.Application")
            .And()
            .HaveDependencyOn("SmartScannerPro.Infrastructure")
            .And()
            .HaveDependencyOn("SmartScannerPro.UI")
            .GetResult();

        result.IsSuccessful.Should().BeTrue("The Domain layer must be completely independent.");
    }

    /// <summary>
    /// Verifies that the Application layer does not depend on Infrastructure or UI.
    /// </summary>
    [Fact]
    public void Application_ShouldNot_HaveDependenciesOnInfrastructureOrUI()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .ShouldNot()
            .HaveDependencyOn("SmartScannerPro.Infrastructure")
            .And()
            .HaveDependencyOn("SmartScannerPro.UI")
            .GetResult();

        result.IsSuccessful.Should().BeTrue("The Application layer must not depend on Infrastructure or UI.");
    }

    /// <summary>
    /// Verifies that the Infrastructure layer does not depend on the Presentation (UI).
    /// </summary>
    [Fact]
    public void Infrastructure_ShouldNot_HaveDependenciesOnPresentation()
    {
        var result = Types.InAssembly(InfrastructureAssembly)
            .ShouldNot()
            .HaveDependencyOn("SmartScannerPro.UI")
            .GetResult();

        result.IsSuccessful.Should().BeTrue("The Infrastructure layer must not depend on the Presentation (UI).");
    }
}
