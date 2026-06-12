# Smart Scanner Pro Documentation

Welcome to the Smart Scanner Pro developer documentation! This folder contains all the technical guides necessary for contributing to the core engine, UI, or building external plugins.

## Architecture & Core
- [Architecture Deep Dive](../ARCHITECTURE.md) - The master design document governing the entire repository.
- [Coding Standards](coding-standards.md) - Rules for formatting, naming, and analyzing code.
- [Developer Setup](developer-setup.md) - Instructions for configuring your IDE, installing SDKs, and compiling the app.
- [API Reference](api.md) - High-level overview of the `IScannerEngine` and other core domain interfaces.

## Engines & Drivers
- [Scanner Driver Guide](scanner-driver-guide.md) - How to implement or modify TWAIN, WIA, and eSCL providers.
- [Image Engine Guide](image-engine-guide.md) - Working with ImageSharp and OpenCvSharp4 for document enhancement.
- [OCR Guide](ocr-guide.md) - Adding or modifying OCR providers (Tesseract, Windows OCR).
- [PDF Guide](pdf-guide.md) - Working with QuestPDF for generating searchable, encrypted PDFs.

## Extensions & Maintenance
- [Plugin Development](plugin-development.md) - A step-by-step guide to writing safe, isolated plugins using `AssemblyLoadContext`.
- [Diagnostics Guide](diagnostics-guide.md) - How to use Serilog, read crash reports, and analyze memory dumps.
- [Performance Guide](performance-guide.md) - Best practices for keeping the UI responsive and memory allocations low.
- [Security Guide](security-guide.md) - Secure coding practices, signing, and updating.

## Repository & Release Management
- [GitHub Management](github-management.md) - Explains our use of Labels, Projects, Milestones, and Issue Templates.
- [Release Process](release-process.md) - The CI/CD pipeline and how we publish artifacts.
- [Versioning Strategy](versioning-strategy.md) - How we use Semantic Versioning.
- [Future Roadmap](roadmap.md) - Planned features for versions 1.0 through 5.0.
