# Repository structure

## Source tree layout

```
nucitext.grammar/
├── .github/
│   └── workflows/
│       ├── dotnet.yml              # CI: build, test
│       ├── github-release.yml      # GitHub Release: pack, upload, checksum
│       └── nuget-release.yml       # NuGet.org: pack, OIDC login, push
├── NuciText.Grammar/               # Main library project
│   ├── Properties/
│   │   └── AssemblyInfo.cs         # InternalsVisibleTo for tests
│   ├── Rules/
│   │   ├── PatternReplacementRuleBase.cs  # Regex/word replacement base
│   │   ├── SpacingRule.cs                  # Punctuation/emoticon spacing (internal)
│   │   └── TrimWhitespaceRule.cs           # Whitespace normalisation (public)
│   ├── GrammarCorrector.cs         # Concrete corrector implementation
│   ├── GrammarRule.cs              # Abstract base for rules
│   ├── GrammarRuleException.cs     # Rule failure exception
│   ├── GrammarRuleSet.cs           # Abstract base for rule sets
│   ├── IGrammarCorrector.cs        # Corrector interface
│   ├── IGrammarRule.cs             # Rule interface
│   ├── IGrammarRuleSet.cs          # Rule set interface
│   └── NuciText.Grammar.csproj     # Project config (net10.0, packable)
├── NuciText.Grammar.UnitTests/     # Test project
│   ├── Rules/
│   │   ├── SpacingRule.cs          # SpacingRule tests
│   │   └── TrimWhitespaceRule.cs   # TrimWhitespaceRule tests
│   ├── GrammarCorrectorTests.cs    # Corrector behavior tests
│   ├── GrammarRuleTests.cs         # Base rule behavior tests
│   └── NuciText.Grammar.UnitTests.csproj  # Test config (NUnit, Moq)
├── docs/                           # Generated documentation
│   ├── api-reference/
│   │   └── INDEX.md
│   ├── behaviour/
│   │   └── text-correction.md
│   ├── components/
│   │   └── core.md
│   ├── flows/
│   │   └── text-correction.md
│   ├── INDEX.md
│   └── ... (23 total files)
├── ARCHITECTURE.md                 # Root architecture doc
├── CODE_OF_CONDUCT.md
├── CONTRIBUTING.md
├── LICENSE
├── NuciText.Grammar.sln            # Solution file
├── PRIVACY.md
├── README.md
└── SECURITY.md
```

## Module organisation

| Module | Path | Purpose |
|--------|------|---------|
| Core API | `NuciText.Grammar/` | Interfaces, base classes, concrete implementations |
| Rules | `NuciText.Grammar/Rules/` | Built-in rule implementations |
| Tests | `NuciText.Grammar.UnitTests/` | Unit tests for all components |
| Documentation | `docs/` | Comprehensive documentation |
| CI/CD | `.github/workflows/` | Build, test, release pipelines |

## Project configuration

### NuciText.Grammar.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <LangVersion>latest</LangVersion>
    <Nullable>enable</Nullable>
    <ImplicitUsings>disable</ImplicitUsings>
    <IsPackable>true</IsPackable>
    <Version>1.0.0</Version>
    <PackageLicenseExpression>GPL-3.0-or-later</PackageLicenseExpression>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
    <PackageTags>text;grammar;correction;nlp;nucitext</PackageTags>
  </PropertyGroup>
</Project>
```

### NuciText.Grammar.UnitTests.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <IsPackable>false</IsPackable>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="18.3.0" />
    <PackageReference Include="NUnit" Version="4.5.1" />
    <PackageReference Include="NUnit3TestAdapter" Version="6.2.0" />
    <PackageReference Include="Moq" Version="4.20.72" />
    <PackageReference Include="NuciAPI.Middleware" Version="2.0.1" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\NuciText.Grammar\NuciText.Grammar.csproj" />
  </ItemGroup>
</Project>
```

## Build outputs

| Project | Output | Location |
|---------|--------|----------|
| NuciText.Grammar | `NuciText.Grammar.dll` | `bin/Debug/net10.0/` |
| NuciText.Grammar | `NuciText.Grammar.xml` (docs) | `bin/Debug/net10.0/` |
| NuciText.Grammar.UnitTests | `NuciText.Grammar.UnitTests.dll` | `bin/Debug/net10.0/` |

## Key files by category

### Public API surface
- `IGrammarRule.cs`
- `IGrammarRuleSet.cs`
- `IGrammarCorrector.cs`
- `GrammarRule.cs`
- `GrammarRuleSet.cs`
- `GrammarCorrector.cs`
- `GrammarRuleException.cs`
- `Rules/TrimWhitespaceRule.cs`
- `Rules/PatternReplacementRuleBase.cs`
- `Rules/RegexReplacement.cs` (record struct)

### Internal API
- `Rules/SpacingRule.cs` (internal sealed)

### Test files
- `GrammarCorrectorTests.cs`
- `GrammarRuleTests.cs`
- `Rules/SpacingRule.cs`
- `Rules/TrimWhitespaceRule.cs`

### Configuration
- `NuciText.Grammar.csproj`
- `NuciText.Grammar.UnitTests.csproj`
- `NuciText.Grammar.sln`
- `.github/workflows/*.yml`