using NetArchTest.Rules;
using Xunit;
using FluentAssertions;
using SmartScannerPro.Application;
using SmartScannerPro.Infrastructure;
using SmartScannerPro.UI;
using SmartScannerPro.Domain;

namespace SmartScannerPro.Tests.Architecture;

/// <summary>
/// Architecture dependency validation tests.
/// </summary>
public class DependencyRulesTests
{
    /// <summary>
    /// Verifies that the Domain layer has no dependencies on other projects.
    /// </summary>
    [Fact]
    public void Domain_ShouldNot_HaveDependenciesOnOtherProjects()
    {
        var result = Types.InAssembly(typeof(SmartScannerPro.Domain.Class1).Assembly) // Placeholder class
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
    public void Application_ShouldNot_HaveDependenciesOnInfrastructure()
    {
        var result = Types.InAssembly(typeof(SmartScannerPro.Application.DependencyInjection).Assembly)
            .ShouldNot()
            .HaveDependencyOn("SmartScannerPro.Infrastructure")
            .And()
            .HaveDependencyOn("SmartScannerPro.UI")
            .GetResult();

        result.IsSuccessful.Should().BeTrue("The Application layer must not depend on Infrastructure or UI.");
    }

    /// <summary>
    /// Verifies that the UI layer does not depend directly on the Domain layer.
    /// </summary>
    [Fact]
    public void UI_ShouldNot_HaveDependenciesOnDomainDirectly()
    {
        var result = Types.InAssembly(typeof(SmartScannerPro.UI.App).Assembly)
            .ShouldNot()
            .HaveDependencyOn("SmartScannerPro.Domain")
            .GetResult();

        result.IsSuccessful.Should().BeTrue("The UI layer should only interact with the Application layer, not Domain entities directly.");
    }
}
