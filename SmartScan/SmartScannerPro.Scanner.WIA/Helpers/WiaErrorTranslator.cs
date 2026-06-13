namespace SmartScannerPro.Scanner.WIA.Helpers;

using System;
using System.Runtime.InteropServices;
using SmartScannerPro.Scanner.Abstractions.Models.Jobs;

/// <summary>
/// Translates WIA COM HRESULT exceptions into domain-level <see cref="FailureReason"/> values.
/// This prevents raw COM exceptions from bubbling up to the presentation layer.
/// </summary>
public static class WiaErrorTranslator
{
    private const uint WiaErrorGeneralError = 0x80210001;
    private const uint WiaErrorPaperJam = 0x80210002;
    private const uint WiaErrorPaperEmpty = 0x80210003;
    private const uint WiaErrorPaperProblem = 0x80210004;
    private const uint WiaErrorOffline = 0x80210005;
    private const uint WiaErrorBusy = 0x80210006;
    private const uint WiaErrorWarmingUp = 0x80210007;
    private const uint WiaErrorUserIntervention = 0x80210008;
    private const uint WiaErrorIncorrectSetting = 0x8021000A;
    private const uint WiaErrorDeviceCommunication = 0x8021000B;
    private const uint WiaErrorInvalidCommand = 0x8021000C;
    private const uint WiaErrorDeviceLocked = 0x8021000D;
    private const uint WiaErrorExceptionInDriver = 0x8021000E;
    private const uint WiaErrorInvalidDriverResponse = 0x8021000F;

    /// <summary>
    /// Translates the specified exception into a <see cref="FailureReason"/>.
    /// </summary>
    /// <param name="exception">The exception to translate.</param>
    /// <returns>The corresponding <see cref="FailureReason"/>.</returns>
    public static FailureReason Translate(Exception exception)
    {
        if (exception == null)
        {
            return FailureReason.None;
        }

        if (exception is COMException comEx)
        {
            var hResult = (uint)comEx.HResult;
            switch (hResult)
            {
                case WiaErrorOffline:
                case WiaErrorDeviceCommunication:
                    return FailureReason.DeviceOffline;

                case WiaErrorPaperJam:
                    return FailureReason.PaperJam;

                case WiaErrorPaperEmpty:
                    return FailureReason.OutOfPaper;

                case WiaErrorBusy:
                case WiaErrorWarmingUp:
                case WiaErrorUserIntervention:
                case WiaErrorDeviceLocked:
                case WiaErrorIncorrectSetting:
                case WiaErrorInvalidCommand:
                case WiaErrorExceptionInDriver:
                case WiaErrorInvalidDriverResponse:
                    return FailureReason.DriverError;

                case WiaErrorGeneralError:
                case WiaErrorPaperProblem:
                    return FailureReason.HardwareError;

                default:
                    return FailureReason.UnknownError;
            }
        }

        if (exception is TimeoutException)
        {
            return FailureReason.Timeout;
        }

        if (exception is OperationCanceledException)
        {
            return FailureReason.None;
        }

        return FailureReason.UnknownError;
    }
}
