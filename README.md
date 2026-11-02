[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/fund.html)
[![Latest Release](https://img.shields.io/github/v/release/hmlendea/nucitext.grammar)](https://github.com/hmlendea/nucitext.grammar/releases/latest)
[![Build Status](https://github.com/hmlendea/nucitext.grammar/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/nucitext.grammar/actions/workflows/dotnet.yml)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://gnu.org/licenses/gpl-3.0)

# NuciText.Grammar

`NuciText.Grammar` is a lightweight, dependency-free .NET library for rule-based grammar and text normalisation.

It provides:

- A small core API for defining grammar rules and rule sets
- A concrete `GrammarCorrector` that applies rules in order
- Reusable base classes for building language-specific correction packages
- A bundled `TrimWhitespaceRule` for whitespace normalisation


## Installation

[![Get it from NuGet](https://raw.githubusercontent.com/hmlendea/readme-assets/master/badges/stores/nuget.png)](https://nuget.org/packages/NuciText.Grammar)

### .NET CLI

```bash
dotnet add package NuciText.Grammar
```

### Package Manager

```powershell
Install-Package NuciText.Grammar
```

## Requirements

- .NET SDK/runtime with support for `net10.0`

## Quick start

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

## How it works

`GrammarCorrector` walks through the rules exposed by an `IGrammarRuleSet` and applies each rule in order.

- Rules run sequentially
- Each rule receives the output of the previous rule
- A rule is only applied when `CanApply` returns `true`
- Rule ordering matters when multiple rules can affect the same text

This makes the package suitable for deterministic, rule-based cleanup pipelines such as punctuation spacing, whitespace normalisation, and language-specific corrections.

## Public API

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

## Built-in rule

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

## Creating custom rules

Derive from `GrammarRule` and implement `Id`, `Description`, and `DoApply`.

```csharp
using System.Text.RegularExpressions;
using NuciText.Grammar;

public sealed class NoDoubleSpaceRule : GrammarRule
{
    public override string Id => "no-double-space";
    public override string Description => "Replaces consecutive spaces with a single space.";

    protected override string DoApply(string text)
        => Regex.Replace(text, " {2,}", " ", RegexOptions.CultureInvariant);
}
```

By default, `GrammarRule.CanApply` checks whether `DoApply` would change the text. Override `CanApply` or `CheckApplicability` if you need a faster pre-check.

## Creating a language-specific rule set

```csharp
using NuciText.Grammar;

public sealed class EnglishGrammarRuleSet : GrammarRuleSet
{
    public override string LanguageCode => "en";

    public override IReadOnlyList<IGrammarRule> Rules { get; } = new IGrammarRule[]
    {
        new TrimWhitespaceRule(),
        new NoDoubleSpaceRule()
    };
}
```

You can then plug the rule set into `GrammarCorrector`:

```csharp
IGrammarRuleSet ruleSet = new EnglishGrammarRuleSet();
IGrammarCorrector corrector = new GrammarCorrector(ruleSet);

string corrected = corrector.Correct(inputText);
```

## Development

### Build

```bash
dotnet build NuciText.Grammar.sln
```

### Test

```bash
dotnet test
```

### Package

```bash
dotnet pack -c Release
```

## 🤝 Contributing

You are welcome to submit any suggestion, feedback, or modification to this project.

When doing so, please:
- Maintain cross-platform compatibility
- Preserve the existing public contract unless a breaking change is intentional
- Submit focused pull requests that conform to the existing code style
- Maintain your branch synchronised with `master`

See [CONTRIBUTING.md](./CONTRIBUTING.md) for detailed guidelines.

## Related Projects

- [NuciText.Grammar](https://github.com/hmlendea/nucitext.grammar)
- [NuciText.Grammar.English](https://github.com/hmlendea/nucitext.grammar.english)
- [NuciText.Grammar.Romanian](https://github.com/hmlendea/nucitext.grammar.romanian)

## License

Licensed under the GNU General Public License v3.0 or later.
See [LICENSE](./LICENSE) for details.

## 🛡️ Privacy and Data

NuciText.Grammar operates entirely in-process on text provided by the calling application and does not collect, transmit, store, or process any personal data on its own behalf. See [PRIVACY.md](./PRIVACY.md) for the full data-handling description.

## 📜 Code of Conduct

This project follows the [Code of Conduct](./CODE_OF_CONDUCT.md).
