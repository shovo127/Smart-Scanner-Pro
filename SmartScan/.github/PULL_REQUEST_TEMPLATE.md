## Description
<!-- Please include a summary of the changes and the related issue. -->
<!-- Please also include relevant motivation and context. -->

Fixes # (issue)

## Type of change
- [ ] Bug fix (non-breaking change which fixes an issue)
- [ ] New feature (non-breaking change which adds functionality)
- [ ] Breaking change (fix or feature that would cause existing functionality to not work as expected)
- [ ] Documentation update
- [ ] Architectural refactoring

## Checklist:
- [ ] My code strictly follows the Clean Architecture boundaries (Domain -> Application <- Infrastructure).
- [ ] I have performed a self-review of my own code.
- [ ] I have commented my code, particularly in hard-to-understand areas, and added XML Documentation.
- [ ] I have made corresponding changes to the documentation in `/docs`.
- [ ] My changes generate no new warnings or StyleCop violations.
- [ ] I have added tests that prove my fix is effective or that my feature works.
- [ ] New and existing unit tests pass locally with my changes.
- [ ] Any dependent changes have been merged and published in downstream modules.

## Performance & Security
- [ ] I have verified this change does not block the UI thread.
- [ ] Long running operations accept a `CancellationToken`.
- [ ] I have validated that memory allocations are minimal.
