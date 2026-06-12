namespace SmartScannerPro.Application.Events.Notifications;

using System;
using MediatR;
using SmartScannerPro.Domain.ValueObjects;

/// <summary>
/// Notification triggered when application settings are changed.
/// </summary>
public record SettingsChangedNotification : INotification;

/// <summary>
/// Notification triggered when a profile is successfully imported.
/// </summary>
/// <param name="profileId">The ID of the imported profile.</param>
public record ProfileImportedNotification(ProfileId profileId) : INotification;

/// <summary>
/// Notification triggered when a profile is successfully exported.
/// </summary>
/// <param name="profileId">The ID of the exported profile.</param>
/// <param name="destination">The destination URI where the profile was exported.</param>
public record ProfileExportedNotification(ProfileId profileId, StorageUri destination) : INotification;

/// <summary>
/// Notification triggered when the application theme is changed.
/// </summary>
/// <param name="newTheme">The new theme name.</param>
public record ThemeChangedNotification(string newTheme) : INotification;

/// <summary>
/// Notification triggered when the application language is changed.
/// </summary>
/// <param name="newLanguageCode">The new language code.</param>
public record LanguageChangedNotification(LanguageCode newLanguageCode) : INotification;
