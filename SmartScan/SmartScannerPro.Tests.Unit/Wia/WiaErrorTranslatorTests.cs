namespace SmartScannerPro.Tests.Unit.Wia;

using System;
using System.Runtime.InteropServices;
using FluentAssertions;
using SmartScannerPro.Scanner.Abstractions.Models.Jobs;
using SmartScannerPro.Scanner.WIA.Helpers;
using Xunit;

/// <summary>
/// Verifies the behavior of WiaErrorTranslator.
/// </summary>
public sealed class WiaErrorTranslatorTests
{
    /// <summary>
    /// Verifies mapping of WIA COM HRESULTs to FailureReason values.
    /// </summary>
    /// <param name="hResult">The HRESULT error code.</param>
    /// <param name="expectedReason">The expected SDK failure reason.</param>
    [Theory]
    [InlineData(0x80210005, FailureReason.DeviceOffline)] // WIA_ERROR_OFFLINE
    [InlineData(0x8021000B, FailureReason.DeviceOffline)] // WIA_ERROR_DEVICE_COMMUNICATION
    [InlineData(0x80210002, FailureReason.PaperJam)] // WIA_ERROR_PAPER_JAM
    [InlineData(0x80210003, FailureReason.OutOfPaper)] // WIA_ERROR_PAPER_EMPTY
    [InlineData(0x80210006, FailureReason.DriverError)] // WIA_ERROR_BUSY
    [InlineData(0x80210007, FailureReason.DriverError)] // WIA_ERROR_WARMING_UP
    [InlineData(0x80210008, FailureReason.DriverError)] // WIA_ERROR_USER_INTERVENTION
    [InlineData(0x8021000D, FailureReason.DriverError)] // WIA_ERROR_DEVICE_LOCKED
    [InlineData(0x80210001, FailureReason.HardwareError)] // WIA_ERROR_GENERAL_ERROR
    [InlineData(0x80210004, FailureReason.HardwareError)] // WIA_ERROR_PAPER_PROBLEM
    public void Translate_ComException_MapsToCorrectFailureReason(uint hResult, FailureReason expectedReason)
    {
        var exception = new COMException("WIA COM Error", (int)hResult);

        var reason = WiaErrorTranslator.Translate(exception);

        reason.Should().Be(expectedReason);
    }

    /// <summary>
    /// Verifies mapping of TimeoutException.
    /// </summary>
    [Fact]
    public void Translate_TimeoutException_MapsToTimeoutReason()
    {
        var exception = new TimeoutException("Operation timed out");

        var reason = WiaErrorTranslator.Translate(exception);

        reason.Should().Be(FailureReason.Timeout);
    }

    /// <summary>
    /// Verifies mapping of OperationCanceledException.
    /// </summary>
    [Fact]
    public void Translate_OperationCanceledException_MapsToNone()
    {
        var exception = new OperationCanceledException();

        var reason = WiaErrorTranslator.Translate(exception);

        reason.Should().Be(FailureReason.None);
    }

    /// <summary>
    /// Verifies mapping of general exceptions.
    /// </summary>
    [Fact]
    public void Translate_GeneralException_MapsToUnknownError()
    {
        var exception = new InvalidOperationException();

        var reason = WiaErrorTranslator.Translate(exception);

        reason.Should().Be(FailureReason.UnknownError);
    }

    /// <summary>
    /// Verifies mapping of null exception.
    /// </summary>
    [Fact]
    public void Translate_NullException_MapsToNone()
    {
        var reason = WiaErrorTranslator.Translate(null!);

        reason.Should().Be(FailureReason.None);
    }
}
