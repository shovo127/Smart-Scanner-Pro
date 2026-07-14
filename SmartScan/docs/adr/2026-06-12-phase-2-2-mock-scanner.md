# ADR: Phase 2.2 Mock Scanner Provider

## Status

Accepted

## Context

Phase 2.2 requires a production-ready mock scanner provider that behaves like real hardware for UI, OCR, PDF, image processing, integration tests, and CI. The provider must be data-driven, support staged progress, and avoid hardcoded device fleets.

## Decision

The mock scanner provider now loads scanner profiles from JSON files under `SmartScannerPro.Scanner.Mock/Profiles`.

The provider uses:

- `OperationContext` as the shared execution envelope
- `ScanStage` for staged progress reporting
- `CapabilitySet` as the capability model
- `SmartScannerPro.TestAssets` for reusable synthetic document generators and ImageSharp rendering

Failure injection remains composable through profile-driven failure injector names and existing compatibility services.

## Consequences

- Adding a new scanner profile does not require recompilation.
- Integration tests can use the same synthetic documents as the provider.
- The mock provider stays independent of hardware APIs and UI dependencies.
- Legacy mock services continue to compile while the provider is migrated toward the staged architecture.
