# Dependencies

## External dependencies

### NuciText.Grammar (Main library)

**Direct dependencies**: None

The main library has **zero external dependencies**. It uses only:
- .NET Base Class Library (BCL)
- `System` namespace
- `System.Collections.Generic`
- `System.Text.RegularExpressions`

### NuciText.Grammar.UnitTests (Test project)

| Package | Version | Purpose |
|---------|---------|---------|
| `Microsoft.NET.Test.Sdk` | 18.3.0 | Test SDK |
| `NUnit` | 4.5.1 | Test framework |
| `NUnit3TestAdapter` | 6.2.0 | NUnit adapter for test runner |
| `Moq` | 4.20.72 | Mocking framework |
| `NuciAPI.Middleware` | 2.0.1 | Test utilities (internal Nuci package) |

## Internal dependencies

### Project references

```
NuciText.Grammar.UnitTests → NuciText.Grammar
```

### Namespace dependencies

| Consumer | Provider | Types used |
|----------|----------|------------|
| `NuciText.Grammar.UnitTests` | `NuciText.Grammar` | All public types |
| `NuciText.Grammar.UnitTests` | `NuciText.Grammar.Rules` | `TrimWhitespaceRule`, `SpacingRule` |

## Dependency graph

```
NuciText.Grammar (no external deps)
    ↑
NuciText.Grammar.UnitTests
    ├── Microsoft.NET.Test.Sdk
    ├── NUnit
    ├── NUnit3TestAdapter
    ├── Moq
    └── NuciAPI.Middleware
```

## Version constraints

- **Target framework**: net10.0 (all projects)
- **NuGet package version**: 1.0.0 (set in csproj)
- **Test packages**: Pinned to specific versions in csproj

## No transitive dependencies

Since the main library has no dependencies, consumers of `NuciText.Grammar` get zero transitive dependencies.

## Dependency management

- Package references ordered alphabetically in csproj
- No central package management (Directory.Packages.props)
- No global.json for SDK version pinning
- .NET version specified in GitHub Actions workflows (10.0.x)