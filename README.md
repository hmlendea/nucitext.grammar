[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/fund.html)
[![Build Status](https://github.com/hmlendea/nucitext.grammar/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/nucitext.grammar/actions/workflows/dotnet.yml)

# NuciText.Grammar

Base NuGet package providing the interfaces and abstract classes for building language-specific grammar correction packages.

## Overview

`NuciText.Grammar` is a framework-agnostic, dependency-free base library. It defines the contracts that all language-specific grammar correction packages must implement. Each language pack depends on this package and contributes only its own rule set — no shared logic is duplicated.

## Abstractions

| Type | Kind | Purpose |
|------|------|---------|
| `IGrammarRule` | Interface | A single correction rule: detects a pattern and transforms text |
| `IGrammarRuleSet` | Interface | An ordered collection of rules for a specific language |
| `IGrammarCorrector` | Interface | Applies a rule set to produce corrected text |
| `GrammarRule` | Abstract class | Base for custom rules; provides a default `CanApply` implementation |
| `GrammarRuleSet` | Abstract class | Base for language rule sets |
| `GrammarCorrector` | Sealed class | Concrete corrector; iterates through a rule set and applies each matching rule |
| `GrammarRuleException` | Exception | Thrown when a rule fails to apply |

## Creating a language pack

1. Add a NuGet reference to `NuciText.Grammar`.
2. Implement your rules by extending `GrammarRule`:

```csharp
public sealed class NoDoubleSpaceRule : GrammarRule
{
    public override string Id => "no-double-space";
    public override string Description => "Replaces consecutive spaces with a single space.";
    public override string Apply(string text) => Regex.Replace(text, " {2,}", " ");
}
```

3. Collect your rules in a `GrammarRuleSet`:

```csharp
public sealed class EnglishGrammarRuleSet : GrammarRuleSet
{
    public override string LanguageCode => "en";

    public override IReadOnlyList<IGrammarRule> Rules { get; } = new IGrammarRule[]
    {
        new NoDoubleSpaceRule(),
        // … more rules
    };
}
```

4. Use `GrammarCorrector` to apply corrections:

```csharp
IGrammarRuleSet ruleSet = new EnglishGrammarRuleSet();
IGrammarCorrector corrector = new GrammarCorrector(ruleSet);

string corrected = corrector.Correct(inputText);
```

## License

GPL-3.0-or-later — see [LICENSE](LICENSE).