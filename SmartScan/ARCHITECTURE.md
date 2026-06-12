# Smart Scanner Pro – Enterprise Software Architecture (Phase 0.5)

This document defines the complete, enterprise-grade software architecture for Smart Scanner Pro. It strictly adheres to the principles of Clean Architecture, SOLID, and the Smart Scanner Pro AI Constitution.

---

## 1. Complete Solution Structure

The solution (`SmartScannerPro.sln`) is modularized into distinct projects to enforce strict boundary separation and dependency direction.

- `SmartScannerPro.Domain` *(Class Library)*: Contains enterprise logic, domain models, and exception types.
- `SmartScannerPro.Application` *(Class Library)*: Contains application business rules, use cases, interfaces, and DTOs.
- `SmartScannerPro.Infrastructure` *(Class Library)*: Concrete implementations for I/O, file system, logging, and OS integrations.
- `SmartScannerPro.Scanner` *(Class Library)*: The core scanning engine abstraction and workflow orchestrator.
- `SmartScannerPro.Scanner.Drivers` *(Class Library)*: Low-level hardware communication (TWAIN, WIA, eSCL).
- `SmartScannerPro.ImageProcessing` *(Class Library)*: Concrete implementations for image manipulation engines (OpenCV, ImageSharp).
- `SmartScannerPro.OCR` *(Class Library)*: Concrete implementations for OCR engines (Tesseract, Windows OCR).
- `SmartScannerPro.PDF` *(Class Library)*: Concrete implementations for PDF generation and manipulation.
- `SmartScannerPro.Plugins` *(Class Library)*: Core plugin framework, definitions, and loader mechanisms.
- `SmartScannerPro.Settings` *(Class Library)*: User preferences, profiles, and application configuration management.
- `SmartScannerPro.Diagnostics` *(Class Library)*: Crash reporting, telemetry (if opt-in), and performance metrics.
- `SmartScannerPro.Localization` *(Class Library)*: Translation resources and language switching logic.
- `SmartScannerPro.UI` *(WPF Application)*: The entry point (Composition Root), DI container configuration, ViewModels, and WPF Views.
- `SmartScannerPro.Shared` *(Class Library)*: Common primitives, enums, `Result<T>` types, and extensions used across projects.
- `SmartScannerPro.Tests.Unit` *(xUnit)*: Unit tests for Domain and Application logic.
- `SmartScannerPro.Tests.Integration` *(xUnit)*: Integration tests for Infrastructure, Drivers, and Engines.
- `SmartScannerPro.Tests.UI` *(xUnit)*: UI automation tests.

---

## 2. Dependency Graph

Dependencies flow strictly inward toward the Domain and Application layers. No circular dependencies exist.

`SmartScannerPro.UI`
↓
`SmartScannerPro.Application` & `SmartScannerPro.Infrastructure` & `SmartScannerPro.Plugins` & `SmartScannerPro.Scanner`

`SmartScannerPro.Infrastructure`
↓
`SmartScannerPro.Application` & `SmartScannerPro.Domain`

`SmartScannerPro.Scanner`
↓
`SmartScannerPro.Application` & `SmartScannerPro.Domain`

`SmartScannerPro.Scanner.Drivers`
↓
`SmartScannerPro.Scanner` & `SmartScannerPro.Domain`

`SmartScannerPro.ImageProcessing`, `SmartScannerPro.OCR`, `SmartScannerPro.PDF`
↓
`SmartScannerPro.Application` & `SmartScannerPro.Domain`

`SmartScannerPro.Application`
↓
`SmartScannerPro.Domain`

`SmartScannerPro.Domain`
*(No Dependencies, except `SmartScannerPro.Shared` and standard .NET libraries)*

**Explanation:**
- `Domain` is the center. It knows nothing about anything else.
- `Application` defines the interfaces (e.g., `IScannerEngine`) that `Infrastructure` and `Scanner` projects will implement.
- `UI` serves as the Composition Root. It references everything to configure the `IServiceCollection` but only interacts with `Application` interfaces at runtime.

---

## 3. Folder Structure

### `SmartScannerPro.Application`
- `/Commands` (CQRS Write operations)
- `/Queries` (CQRS Read operations)
- `/Services` (Application-level orchestrators)
- `/Interfaces` (Ports for Infrastructure to implement)
- `/Validators` (FluentValidation classes)
- `/Events` (Domain/Application event models)
- `/DTOs` (Data Transfer Objects)

### `SmartScannerPro.Scanner`
- `/Providers` (Scanner hardware providers)
- `/Factories` (Instantiating scanner sessions)
- `/Profiles` (Scan profile handling)
- `/Capabilities` (Querying device hardware limits)
- `/Sessions` (Active scan job management)
- `/Discovery` (Network and local scanner discovery)
- `/Enums` (Scanner specific enumerations)

### `SmartScannerPro.UI`
- `/App` (Startup, DI configuration)
- `/ViewModels` (MVVM logic)
- `/Views` (WPF XAML files)
- `/Controls` (Reusable custom UI controls)
- `/Converters` (IValueConverter implementations)
- `/Behaviors` (Attached behaviors for XAML)
- `/Resources` (Styles, Themes, Templates)

---

## 4. Interface Design

Major abstractions defining the system boundaries (No implementations):

**Core Scanning:**
- `IScannerEngine`: High-level orchestrator for starting and stopping jobs.
- `IScannerProvider`: Factory for discovering and creating device connections.
- `IScannerDevice`: Represents a physical scanner and its properties.
- `IScanJob`: Represents a specific executing task.
- `IScannerCapabilities`: Exposes what a specific device can do (e.g., Max DPI, Duplex support).
- `IScannerSession`: Represents an active connection to a device.

**Engines:**
- `IImageProcessor`: Abstraction for cropping, rotating, deskewing.
- `IOcrProvider`: Abstraction for submitting an image and receiving text/bounding boxes.
- `IPdfEngine`: Abstraction for building, encrypting, and saving PDFs.

**Infrastructure & Services:**
- `ILogService`: Generic logging interface.
- `ISettingsService`: Read/write application settings.
- `IProfileManager`: Read/write scan profiles.
- `ILocalizationService`: Fetch localized strings dynamically.
- `IUpdateService`: Check for updates securely.
- `IDiagnosticsService`: Monitor memory and report crashes.

**Plugins:**
- `IPlugin`: The base interface every plugin must implement (Initialize, Shutdown).
- `IPluginHost`: Provided to plugins so they can register their own services.

---

## 5. Domain Model

Core entities governing the business rules (No implementations):

- `ScannerDevice`: ID, Name, ConnectionType (USB, Network), Status.
- `ScanProfile`: Name, DPI, ColorMode, PaperSource, OutputFormat, FileNamingPattern.
- `ScanDocument`: An aggregate root containing multiple `ScanPage` entities.
- `ScanPage`: Represents a single physical page, holds an image buffer reference and metadata.
- `OCRResult`: Bounding boxes, text content, confidence score, language.
- `PdfDocument`: Metadata, page list, security settings.
- `PluginInfo`: Author, Version, Name, PublicKey, Status.
- `Settings`: Global user preferences.
- `Language`: Locale code, Display Name.
- `Theme`: Light/Dark, Accent Color.

---

## 6. Package Selection

- **`CommunityToolkit.Mvvm`**: Official, highly optimized, source-generator based MVVM framework. Zero boilerplate.
- **`Serilog` & `Serilog.Sinks.File`**: Standard for structured logging. Highly performant and extensible.
- **`Microsoft.Extensions.Hosting` & `Microsoft.Extensions.DependencyInjection`**: Enterprise-standard DI and application lifecycle management.
- **`ImageSharp`**: Managed, cross-platform image processing. Safe, no native dependencies.
- **`OpenCvSharp4`**: For advanced image manipulation (deskew, blank detection, barcode) where ImageSharp is too slow or lacks advanced math.
- **`Tesseract` (or `NAPS2.Tesseract.Binaries`)**: Best open-source OCR engine.
- **`QuestPDF`**: Modern, highly performant, code-first PDF generation. Better layout engine than PdfPig (which is better for extraction).
- **`FluentValidation`**: Clean, rule-based validation for Settings, Profiles, and DTOs.
- **`xUnit`**: Standard, modern testing framework. Avoids shared state issues of NUnit.
- **`FluentAssertions`**: Enhances test readability dramatically.
- **`Moq`**: Industry standard for mocking interfaces in tests.
- **`StyleCop.Analyzers` & `Roslynator.Analyzers`**: Enforces strict coding standards and catches code smells.
- **`BenchmarkDotNet`**: For testing memory and speed of image processing and PDF generation.

---

## 7. Configuration Strategy

Configuration files are stored in `Environment.SpecialFolder.ApplicationData` (`%APPDATA%\SmartScannerPro`).

- `settings.json`: Global user preferences (Theme, Default Scanner, Output Paths).
- `profiles.json`: Array of saved `ScanProfile` objects.
- `/themes`: Custom user-provided XAML or JSON theme definitions.
- `/plugins`: Subfolders for loaded `.dll` plugins.
- `/languages`: `.json` or `.resx` files for translations.
- `/cache`: Temporary image buffers for active scan sessions to prevent RAM exhaustion.
- `/logs`: Rolling log files managed by Serilog.

---

## 8. Plugin Architecture

The system uses `System.Runtime.Loader.AssemblyLoadContext` to isolate plugin dependencies.

- **Loading Strategy**: On startup, `SmartScannerPro.Plugins` scans the `%APPDATA%\SmartScannerPro\plugins` directory. Validates the signature, creates a separate `AssemblyLoadContext` for each plugin to prevent DLL hell, and invokes the `IPlugin.Initialize(IPluginHost)` method.
- **Dependency Isolation**: Plugins do not share dependencies with the Core unless explicitly exposed via `IPluginHost`.
- **Extension Points**: Plugins can register themselves as an `IOcrProvider`, `IImageProcessor`, `IScannerProvider`, or `IExportProvider` within the DI container.
- **Future AI Plugins**: Extensible interfaces allow future plugins to inject `IAiEnhancementProvider` for upscaling or smart cropping.

---

## 9. Driver Architecture

- **TWAIN**: Primary fallback for legacy and professional scanners. Operates in a separate process or thread to prevent 32-bit/64-bit conflicts and UI freezes.
- **WIA**: Native Windows fallback. Often less feature-rich but highly stable.
- **eSCL (AirScan)**: Network protocol. Driverless scanning for modern network printers.
- **TWAIN Direct**: Future-proof cloud and local driverless standard.

**Selection & Fallback Strategy**:
The `IScannerEngine` queries all registered `IScannerProvider`s.
1. Engine attempts connection via preferred protocol (TWAIN).
2. If connection fails or timeouts, it automatically falls back to eSCL, then WIA.
3. The successful provider is cached in `settings.json` for that specific hardware ID to optimize future connections.

---

## 10. Logging Architecture

Powered by **Serilog**.
- **Sinks**: Console (Debug only), File (Rolling daily logs), InMemory (for viewing in a "Diagnostics" UI tab).
- **Format**: Structured JSON logging to allow easy parsing of Crash Reports.
- **Rotation**: 10 MB file size limit, keep last 30 days.
- **Crash Reports**: Unhandled exceptions trigger a fatal log write and generate a `crash_report.zip` containing logs and system specs (no PII).
- **Performance Logs**: Specific timings for hardware initialization and PDF generation are logged using `ILogger.LogInformation` with explicit correlation IDs.

---

## 11. Exception Architecture

Exceptions are reserved ONLY for truly exceptional circumstances (e.g., OutOfMemory, Hardware Disconnect).
Normal control flow uses a custom `Result<T>` or `Result<T, Error>` pattern.

- `Error`: A record containing an ErrorCode and Message.
- `Result<T>`: Encapsulates success state and the value, or failure state and the `Error`.
- **Custom Exceptions**:
  - `DomainException`: Business rule violations (e.g., Invalid Profile configuration).
  - `InfrastructureException`: File system or OS level failures.
  - `ScannerException`: Hardware faults (e.g., Paper Jam, Lid Open).
  - `OCRException`: Missing language models.
  - `PDFException`: Corrupted output or write lock.

---

## 12. Async Strategy

Every long-running operation in `SmartScannerPro.Application` and `SmartScannerPro.Scanner` is asynchronous.
- **`CancellationToken`**: Mandatory parameter on all `Async` methods to support user abortion.
- **`IProgress<T>`**: Mandatory parameter for operations like PDF generation and OCR to report percentage and status strings back to the UI thread.
- **Timeouts**: Wrapped using `CancellationTokenSource(TimeSpan)` to prevent deadlocks when drivers hang.
- **Background Tasks**: The UI utilizes `Task.Run` for CPU-bound work (like Image Processing) to strictly preserve 60FPS UI responsiveness.

---

## 13. Performance Strategy

- **Memory Management**: Scanned pages are rarely kept entirely in RAM. The `ScannerSession` streams raw bitmaps from the driver directly to a temporary `/cache` folder on disk using memory-mapped files or standard FileStreams.
- **Image Streaming**: UI thumbnails load low-resolution proxies. The full-resolution image is only loaded into memory when actively viewed or processed.
- **1000-Page Documents**: PDF generation occurs incrementally. `QuestPDF` or `PdfSharp` streams pages to disk rather than building a 1000-page DOM in memory.
- **Resource Disposal**: `IDisposable` and `IAsyncDisposable` are strictly enforced for unmanaged memory, OpenCV matrices, and file handles using `using` declarations.

---

## 14. Security

- **Code Signing**: All executables and DLLs must be Authenticode signed using an EV certificate.
- **Plugin Validation**: `IPluginHost` verifies the public key hash of plugin DLLs against a whitelist or requires user consent for unsigned plugins.
- **Network**: The app operates 100% offline. The only network request is the optional Update Checker (via HTTPS), which verifies the SHA256 hash of the downloaded payload before execution.
- **Dependencies**: GitHub Dependabot is enforced. CodeQL runs on every PR to scan for injection vulnerabilities.

---

## 15. Testing Strategy

- **Unit Tests**: Business logic in `Domain` and `Application` is tested purely in memory. Fast execution (< 5 seconds for the whole suite).
- **Integration Tests**: Tests the `Infrastructure` project against actual file systems and mocked OS calls.
- **Scanner Simulation Tests**: Custom `IScannerProvider` mock that simulates hardware behavior, paper jams, and timeouts to ensure the `ScannerEngine` gracefully recovers.
- **UI Tests**: BDD-style testing using tools like FlaUI or Appium to click through the WPF interface.
- **Performance Tests**: `BenchmarkDotNet` tests on OCR and Image processing algorithms to catch regressions.

---

## 16. GitHub Structure

- **Issue Templates**: Bug Report, Feature Request, New Scanner Support.
- **Pull Request Templates**: Requires checklist (Tests added? Docs updated? Architecture adhered to?).
- **Labels**: `bug`, `enhancement`, `hardware-compatibility`, `good first issue`, `needs-triage`.
- **Projects**: Kanban boards for Roadmap versions.
- **Milestones**: Tied to specific semantic version releases.
- **GitHub Actions**:
  1. *PR Check*: Build -> Run Tests -> Check Formatting (dotnet format) -> CodeQL.
  2. *Release*: Build Release -> Obfuscate/Trim (if applicable) -> Create Installer (InnoSetup/WiX) -> Publish GitHub Release.

---

## 17. Coding Standards

- **Naming**: PascalCase for public members, camelCase for variables, `_camelCase` for private fields. `I` prefix for interfaces.
- **Namespaces**: File-scoped namespaces (`namespace SmartScannerPro.Application;`).
- **Comments**: XML Documentation (`///`) required for all `public` types and members. Explain *why*, not *what*.
- **Nullable**: `<Nullable>enable</Nullable>` strictly enforced globally.
- **Formatting**: Enforced by `.editorconfig` via `dotnet format`.
- **Analyzers**: Warnings are treated as errors (`<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`).

---

## 18. Future Roadmap

### Version 1.0 (Foundation)
- Clean Architecture implementation.
- Basic TWAIN/WIA support.
- Simplex scanning.
- Save to PDF/Image.
- Fluent UI Base.

### Version 2.0 (Productivity)
- Manual Duplex Wizard.
- OCR Integration (Tesseract).
- Searchable PDF generation.
- Scan Profiles and Settings UI.

### Version 3.0 (Advanced Processing)
- Advanced Image Engine (Auto-Deskew, Blank Page Removal).
- eSCL (AirScan) network support.
- Plugin Architecture Foundation.

### Version 4.0 (Enterprise)
- Barcode/QR Code separation.
- Digital Signatures for PDFs.
- Active Directory / Group Policy configurations.

### Version 5.0 (Intelligence)
- TWAIN Direct support.
- AI-based document classification and smart cropping.
- Cloud Export Plugins (OneDrive, Google Drive, SharePoint).
