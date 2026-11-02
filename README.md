[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/funding)
[![Latest Release](https://img.shields.io/github/v/release/hmlendea/nucitext.grammar)](https://github.com/hmlendea/nucitext.grammar/releases/latest)
[![Build Status](https://github.com/hmlendea/nucitext.grammar/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/nucitext.grammar/actions/workflows/dotnet.yml)
[![NuGet](https://img.shields.io/nuget/v/NuciText.Grammar)](https://nuget.org/packages/NuciText.Grammar)
[![License](https://img.shields.io/github/license/hmlendea/nucitext.grammar)](https://github.com/hmlendea/nucitext.grammar/blob/master/LICENSE)

# NuciText.Grammar

`NuciText.Grammar` is a lightweight, dependency-free .NET library for rule-based grammar and text normalisation.

## 📑 Table of Contents

- [Capabilities](#-capabilities)
- [Usage](#-usage)
- [Installation](#-installation)
- [Requirements](#-system-requirements)
- [Public API](#-public-api)
- [Built-in Rule](#-built-in-rule)
- [Known Limitations](#-known-limitations)
- [Contributing](#-contributing)
- [Security](#-security)
- [Architecture](#-architecture)
- [License](#-license)

## ✨ Capabilities

- A small core API for defining grammar rules and rule sets
- A concrete `GrammarCorrector` that applies rules in order
- Reusable base classes for building language-specific correction packages
- A bundled `TrimWhitespaceRule` for whitespace normalisation

## 🚀 Usage

```csharp
using NuciText.Grammar;
using NuciText.Grammar.Rules;

public sealed class BasicRuleSet : GrammarRuleSet
{
    public override string LanguageCode => "und";

    public override IReadOnlyList<IGrammarRule> Rules { get; } = new IGrammarRule[]
    {
        new TrimWhitespaceRule()
    };
}

IGrammarRuleSet ruleSet = new BasicRuleSet();
IGrammarCorrector corrector = new GrammarCorrector(ruleSet);

string corrected = corrector.Correct("  Hello    world  ");
// "Hello world"
```

### How it works

`GrammarCorrector` walks through the rules exposed by an `IGrammarRuleSet` and applies each rule in order.

- Rules run sequentially
- Each rule receives the output of the previous rule
- A rule is only applied when `CanApply` returns `true`
- Rule ordering matters when multiple rules can affect the same text

This makes the package suitable for deterministic, rule-based cleanup pipelines such as punctuation spacing, whitespace normalisation, and language-specific corrections.

## 📦 Installation

[![Obtain it from NuGet](https://raw.githubusercontent.com/hmlendea/readme-assets/master/badges/stores/nuget.png)](https://nuget.org/packages/NuciText.Grammar)

### Package Manager Installation

```bash
dotnet add package NuciText.Grammar
```

Or, via the `Package Manager Console`:
```powershell
Install-Package NuciText.Grammar
```

## 🖥️ System Requirements

| Component | Minimum | Recommended |
|-----------|---------|-------------|
| .NET SDK/runtime | net10.0 | net10.0 |

## 📚 Public API

| Type | Kind | Purpose |
|------|------|---------|
| `IGrammarRule` | Interface | Defines a single correction rule |
| `IGrammarRuleSet` | Interface | Defines an ordered list of rules for one language or scenario |
| `IGrammarCorrector` | Interface | Defines the text correction entry point |
| `GrammarRule` | Abstract class | Base class for custom rules with default applicability logic |
| `GrammarRuleSet` | Abstract class | Base class for rule set implementations |
| `GrammarCorrector` | Class | Applies all rules in a rule set to a piece of text |
| `GrammarRuleException` | Exception | Exception type available for rule execution failures |
| `TrimWhitespaceRule` | Rule | Trims leading and trailing whitespace and collapses internal whitespace |

## 🔧 Built-in Rule

### `TrimWhitespaceRule`

Normalises whitespace by:

- Trimming leading whitespace
- Trimming trailing whitespace
- Collapsing consecutive whitespace characters to a single space

Example:

```csharp
var rule = new TrimWhitespaceRule();
string result = rule.Apply("\t salut\n\n lume \t");
// "salut lume"
```

## ⚠️ Known Limitations

- Only provides a single built-in rule (`TrimWhitespaceRule`); language-specific rules must be implemented by consumers
- No built-in natural language processing or machine learning capabilities
- Rule ordering is manual and must be managed by the rule set implementation

## 🤝 Contributing

See [CONTRIBUTING.md](./CONTRIBUTING.md) for contribution guidelines.

## 🔒 Security

Report security vulnerabilities per the [Security Policy](./SECURITY.md).

## 🏗️ Architecture

See [ARCHITECTURE.md](./ARCHITECTURE.md) for the system context, principal components, runtime flows, ownership boundaries, dependencies, constraints, and extension points.

## 📄 License

Licensed under the [GNU General Public License v3.0 or later](./LICENSE).
