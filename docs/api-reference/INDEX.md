# API reference index

NuciText.Grammar is a class library, not a web API. It exposes no HTTP endpoints, controllers, or REST resources. This index documents the public API surface instead.

## Public API surface

| Type | Kind | Namespace | Purpose |
|------|------|-----------|---------|
| `IGrammarRule` | Interface | `NuciText.Grammar` | Defines a single correction rule |
| `IGrammarRuleSet` | Interface | `NuciText.Grammar` | Defines an ordered list of rules for one language or scenario |
| `IGrammarCorrector` | Interface | `NuciText.Grammar` | Defines the text correction entry point |
| `GrammarRule` | Abstract class | `NuciText.Grammar` | Base class for custom rules with default applicability logic |
| `GrammarRuleSet` | Abstract class | `NuciText.Grammar` | Base class for rule set implementations |
| `GrammarCorrector` | Sealed class | `NuciText.Grammar` | Applies all rules in a rule set to a piece of text |
| `GrammarRuleException` | Exception class | `NuciText.Grammar` | Exception type for rule execution failures |
| `TrimWhitespaceRule` | Sealed class | `NuciText.Grammar.Rules` | Trims and collapses whitespace |
| `PatternReplacementRuleBase` | Abstract class | `NuciText.Grammar.Rules` | Base class for regex-based word replacement rules |
| `RegexReplacement` | Record struct | `NuciText.Grammar.Rules` | Pattern/replacement pair for regex rules |

## Internal API surface

| Type | Kind | Namespace | Purpose |
|------|------|-----------|---------|
| `SpacingRule` | Sealed class | `NuciText.Grammar.Rules` | Normalises spacing around punctuation and emoticons |

## Package information

- **Package**: `NuciText.Grammar`
- **Version**: 1.0.0
- **Target framework**: net10.0
- **License**: GPL-3.0-or-later
- **Repository**: https://github.com/hmlendea/nucitext.grammar
- **NuGet**: https://nuget.org/packages/NuciText.Grammar

## Usage example

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

## Related documentation

- [Architecture](../architecture.md)
- [Components](../components/core.md)
- [Flows](../flows/text-correction.md)
- [API usage examples](../api-usage-examples.md)