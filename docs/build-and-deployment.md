# Build and deployment

## Build pipeline

### CI workflow (`.github/workflows/dotnet.yml`)

Triggers:
- Push to `master` branch
- Pull requests to `master` branch

Steps:
1. Checkout repository
2. Setup .NET 10.0.x
3. Restore dependencies (`dotnet restore`)
4. Build (`dotnet build --no-restore`)
5. Test (`dotnet test --no-build --verbosity normal`)

### Release workflows

#### GitHub Release (`.github/workflows/github-release.yml`)

Trigger: Published GitHub release

Steps:
1. Checkout with full history
2. Setup .NET 10.0.x
3. Extract version from Git tag (strips `v` prefix)
4. Restore dependencies
5. Pack NuGet package (`dotnet pack -c Release --output ./nupkg -p:Version=<version>`)
6. Calculate SHA256 checksum
7. Upload package to GitHub Release
8. Append SHA256 to release notes

#### NuGet.org Release (`.github/workflows/nuget-release.yml`)

Trigger: Published GitHub release

Steps:
1. Checkout with full history
2. Setup .NET 10.0.x
3. Extract version from Git tag
4. Restore dependencies
5. Pack NuGet package
6. NuGet login via OIDC
7. Push to NuGet.org (`dotnet nuget push`)

## Local development

### Prerequisites

- .NET SDK 10.0 or newer

### Commands

```bash
# Clone and setup
git clone https://github.com/hmlendea/nucitext.grammar.git
cd nucitext.grammar

# Restore dependencies
dotnet restore

# Build
dotnet build

# Run tests
dotnet test

# Format code
dotnet format

# Verify formatting
dotnet format --verify-no-changes

# Create NuGet package
dotnet pack -c Release --output ./nupkg
```

## Project configuration

### NuciText.Grammar.csproj

```xml
<TargetFramework>net10.0</TargetFramework>
<RootNamespace>NuciText.Grammar</RootNamespace>
<AssemblyName>NuciText.Grammar</AssemblyName>
<Version>1.0.0</Version>
<IsPackable>true</IsPackable>
<GeneratePackageOnBuild>false</GeneratePackageOnBuild>
<GenerateDocumentationFile>true</GenerateDocumentationFile>
<ImplicitUsings>disable</ImplicitUsings>
<Nullable>enable</Nullable>
<LangVersion>latest</LangVersion>
```

### NuciText.Grammar.UnitTests.csproj

```xml
<TargetFramework>net10.0</TargetFramework>
<IsPackable>false</IsPackable>
<IsTestProject>true</IsTestProject>
<ImplicitUsings>disable</ImplicitUsings>
<Nullable>enable</Nullable>
```

## Package metadata

| Property | Value |
|----------|-------|
| Package ID | `NuciText.Grammar` |
| Authors | Horațiu Mlendea |
| License | GPL-3.0-or-later |
| Repository | https://github.com/hmlendea/nucitext.grammar |
| Tags | text;grammar;correction;nlp;nucitext |
| Target framework | net10.0 |

## Deployment artifacts

- **NuGet package**: `NuciText.Grammar.{version}.nupkg`
- **Symbols package**: `NuciText.Grammar.{version}.snupkg` (if enabled)
- **Documentation**: XML documentation file included in package

## Versioning

- Version extracted from Git tag (e.g., `v1.0.0` → `1.0.0`)
- Semantic versioning expected
- Package version set via `-p:Version` during pack