namespace SmartScannerPro.Tests.Unit.UI;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Discovery;
using SmartScannerPro.Scanner.Abstractions.Models.Drivers;
using SmartScannerPro.Scanner.Abstractions.Models.Jobs;
using SmartScannerPro.Scanner.Abstractions.Models.Sessions;
using SmartScannerPro.UI.ViewModels;
using Xunit;

/// <summary>
/// Contains unit tests for the WorkspaceViewModel.
/// </summary>
public sealed class WorkspaceViewModelTests
{
    private readonly Mock<IScannerEngine> mockEngine;
    private readonly Mock<IScannerDiscoveryService> mockDiscovery;
    private readonly Mock<IScannerFactory> mockFactory;
    private readonly Mock<IScannerSession> mockSession;
    private readonly Mock<IScannerDevice> mockDevice;
    private readonly Mock<IScannerCapabilities> mockCapabilities;
    private readonly Mock<IScanJob> mockJob;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkspaceViewModelTests"/> class.
    /// </summary>
    public WorkspaceViewModelTests()
    {
        this.mockEngine = new Mock<IScannerEngine>();
        this.mockDiscovery = new Mock<IScannerDiscoveryService>();
        this.mockFactory = new Mock<IScannerFactory>();
        this.mockSession = new Mock<IScannerSession>();
        this.mockDevice = new Mock<IScannerDevice>();
        this.mockCapabilities = new Mock<IScannerCapabilities>();
        this.mockJob = new Mock<IScanJob>();

        this.mockEngine.Setup(e => e.Discovery).Returns(this.mockDiscovery.Object);
        this.mockEngine.Setup(e => e.Factory).Returns(this.mockFactory.Object);
        this.mockSession.Setup(s => s.Device).Returns(this.mockDevice.Object);
        this.mockDevice.Setup(d => d.Capabilities).Returns(this.mockCapabilities.Object);
    }

    /// <summary>
    /// Verifies that RefreshScanners Command discovers scanners and populates the list.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public Task RefreshScanners_PopulatesScannersList()
    {
        return RunInStaThreadAsync(async () =>
        {
            var scanners = new List<ScannerDescriptor>
            {
                new()
                {
                    HardwareId = "MOCK-1",
                    Name = "Mock Scanner 1",
                    Driver = new DriverInfo
                    {
                        Id = Guid.NewGuid(),
                        Name = "Mock Driver",
                        Version = new DriverVersion { Major = 1, Minor = 0, OriginalString = "1.0" },
                        Type = DriverType.Mock
                    }
                }
            };

            this.mockDiscovery
                .Setup(d => d.DiscoverAsync(It.IsAny<DiscoveryRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DiscoveryResult { Scanners = scanners.AsReadOnly(), Duration = TimeSpan.FromSeconds(1) });

            var viewModel = new WorkspaceViewModel(this.mockEngine.Object);
            
            // Wait for constructor auto-refresh
            await Task.Delay(100);

            viewModel.Scanners.Should().HaveCount(1);
            viewModel.Scanners[0].Name.Should().Be("Mock Scanner 1");
            viewModel.SelectedScanner.Should().NotBeNull();
        });
    }

    /// <summary>
    /// Verifies that running a scan job successfully adds pages to the workspace.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public Task StartScan_ExecutesJob_AndAppendsPages()
    {
        return RunInStaThreadAsync(async () =>
        {
            var selectedScanner = new ScannerDescriptor
            {
                HardwareId = "MOCK-1",
                Name = "Mock Scanner 1",
                Driver = new DriverInfo
                {
                    Id = Guid.NewGuid(),
                    Name = "Mock Driver",
                    Version = new DriverVersion { Major = 1, Minor = 0, OriginalString = "1.0" },
                    Type = DriverType.Mock
                }
            };

            // Setup temporary files to simulate scan results
            var tempFile = Path.Combine(Path.GetTempPath(), $"test_page_{Guid.NewGuid():N}.png");
            File.WriteAllText(tempFile, "Fake Image Data");

            var jobResult = new ScanJobResult
            {
                Status = ScanJobStatus.Completed,
                ScannedFilePaths = new List<string> { tempFile }.AsReadOnly(),
                Statistics = new ScanStatistics { PagesAcquired = 1, PagesProcessed = 1 }
            };

            this.mockDiscovery
                .Setup(d => d.DiscoverAsync(It.IsAny<DiscoveryRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DiscoveryResult { Scanners = new List<ScannerDescriptor> { selectedScanner }.AsReadOnly() });

            this.mockFactory
                .Setup(f => f.CreateSessionAsync(It.IsAny<ScanSessionOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(this.mockSession.Object);

            this.mockSession
                .Setup(s => s.CreateJob(It.IsAny<ScanJobOptions>()))
                .Returns(this.mockJob.Object);

            this.mockJob
                .Setup(j => j.ExecuteAsync(It.IsAny<IProgress<ScanProgress>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(jobResult);

            var viewModel = new WorkspaceViewModel(this.mockEngine.Object);
            await Task.Delay(100); // Wait auto-refresh

            viewModel.SelectedScanner = selectedScanner;
            viewModel.FileNamePattern = "TestScan_####";
            viewModel.OutputFolder = Path.Combine(Path.GetTempPath(), $"MyScans_{Guid.NewGuid():N}");
            
            // Execute scan command
            await viewModel.StartScanCommand.ExecuteAsync(null);

            viewModel.Pages.Should().HaveCount(1, $"ProgressMessage is '{viewModel.ProgressMessage}' and StatusMessage is '{viewModel.StatusMessage}'");
            viewModel.Pages[0].PageNumber.Should().Be(1);
            viewModel.Pages[0].ImagePath.Should().Contain("TestScan_0001");
            viewModel.SelectedPage.Should().Be(viewModel.Pages[0]);

            // Clean up files
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
            if (viewModel.Pages.Count > 0 && File.Exists(viewModel.Pages[0].ImagePath))
            {
                File.Delete(viewModel.Pages[0].ImagePath);
            }
            if (Directory.Exists(viewModel.OutputFolder))
            {
                Directory.Delete(viewModel.OutputFolder, true);
            }
        });
    }

    /// <summary>
    /// Verifies page rotation changes rotation properties.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public Task RotatePage_IncrementsRotationAngle()
    {
        return RunInStaThreadAsync(async () =>
        {
            var viewModel = new WorkspaceViewModel(this.mockEngine.Object);
            var page = new PageViewModel("temp.png", 1);
            viewModel.Pages.Add(page);
            viewModel.SelectedPage = page;

            viewModel.RotatePageCommand.Execute(null);
            page.Rotation.Should().Be(90);

            viewModel.RotatePageCommand.Execute(null);
            page.Rotation.Should().Be(180);

            await Task.CompletedTask;
        });
    }

    /// <summary>
    /// Verifies deleting a page updates queue indexing.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public Task DeletePage_UpdatesQueue_AndReindexes()
    {
        return RunInStaThreadAsync(async () =>
        {
            var viewModel = new WorkspaceViewModel(this.mockEngine.Object);
            var page1 = new PageViewModel("temp1.png", 1);
            var page2 = new PageViewModel("temp2.png", 2);
            viewModel.Pages.Add(page1);
            viewModel.Pages.Add(page2);

            viewModel.SelectedPage = page1;
            viewModel.DeletePageCommand.Execute(null);

            viewModel.Pages.Should().HaveCount(1);
            viewModel.Pages[0].PageNumber.Should().Be(1);
            viewModel.Pages[0].ImagePath.Should().Be("temp2.png");
            viewModel.SelectedPage.Should().Be(page2);

            await Task.CompletedTask;
        });
    }

    private static Task RunInStaThreadAsync(Func<Task> action)
    {
        var tcs = new TaskCompletionSource();
        var thread = new Thread(() =>
        {
            try
            {
                action().GetAwaiter().GetResult();
                tcs.SetResult();
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        return tcs.Task;
    }
}
