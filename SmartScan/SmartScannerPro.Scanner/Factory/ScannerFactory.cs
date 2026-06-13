namespace SmartScannerPro.Scanner.Factory;

using System;
using System.Threading;
using System.Threading.Tasks;
using SmartScannerPro.Scanner.Abstractions.Interfaces;
using SmartScannerPro.Scanner.Abstractions.Models.Sessions;
using SmartScannerPro.Scanner.Mock.Factory;
using SmartScannerPro.Scanner.WIA.Factory;

/// <summary>
/// Routes scan session creation to the appropriate provider factory based on the HardwareId.
/// </summary>
public sealed class ScannerFactory : IScannerFactory
{
    private readonly MockScannerFactory mockFactory;
    private readonly WiaScannerFactory wiaFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScannerFactory"/> class.
    /// </summary>
    /// <param name="mockFactory">The mock factory.</param>
    /// <param name="wiaFactory">The WIA factory.</param>
    public ScannerFactory(MockScannerFactory mockFactory, WiaScannerFactory wiaFactory)
    {
        this.mockFactory = mockFactory ?? throw new ArgumentNullException(nameof(mockFactory));
        this.wiaFactory = wiaFactory ?? throw new ArgumentNullException(nameof(wiaFactory));
    }

    /// <inheritdoc/>
    public Task<IScannerSession> CreateSessionAsync(ScanSessionOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        if (options.HardwareId.StartsWith("MOCK-", StringComparison.OrdinalIgnoreCase))
        {
            return this.mockFactory.CreateSessionAsync(options, cancellationToken);
        }

        return this.wiaFactory.CreateSessionAsync(options, cancellationToken);
    }
}
