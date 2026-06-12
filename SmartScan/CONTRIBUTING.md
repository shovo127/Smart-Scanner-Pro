# Contributing to Smart Scanner Pro

First off, thank you for considering contributing to Smart Scanner Pro! It's people like you that make open source such a fantastic community.

## Our Philosophy
We prioritize **Architecture, Maintainability, and Professionalism** over speed. We strictly adhere to our [AI Constitution](CONSTITUTION.md) and [Architecture Document](ARCHITECTURE.md).

## Branching Strategy
- `main`: Stable release branch.
- `develop`: Active development branch.
- `feature/*`: For new features (branch off `develop`).
- `bugfix/*`: For bug fixes (branch off `develop`).

## Coding Standards
1. **Clean Architecture**: Never violate dependency rules. UI cannot reference Infrastructure directly.
2. **SOLID**: Keep classes small and focused.
3. **Async/Await**: Never use `.Result` or `.Wait()`. All I/O must be async.
4. **Formatting**: We use `.editorconfig`. Run `dotnet format` before committing.
5. **Warnings as Errors**: The build will fail if there are any compiler warnings or StyleCop violations.

## Pull Request Process
1. Ensure any install or build dependencies are removed before the end of the layer when doing a build.
2. Update the README.md with details of changes to the interface, if applicable.
3. Your PR must include unit tests for new business logic.
4. Fill out the PR template completely.
5. You may merge the Pull Request once you have the sign-off of at least one maintainer, or if you do not have permission to do that, you may request the reviewer to merge it for you.

## Issue Reporting
Please use the provided GitHub Issue templates to report bugs or request features. Provide as much detail as possible, including scanner models, connection types, and Serilog logs.
