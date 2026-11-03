# Quick Start

## Prerequisites

- .NET 10 SDK or later
- Git (for cloning)

## Installation

### From NuGet

```bash
dotnet add package NuciText.Grammar
```

### From source

```bash
git clone https://github.com/nucitext/nucitext.grammar.git
cd nucitext.grammar
dotnet restore
dotnet build
```

## First correction

```csharp
using NuciText.Grammar;
using NuciText.Grammar.Rules;

// Create a rule set with built-in rules
var ruleSet = new MyRuleSet(); // Implement IGrammarRuleSet

// Create corrector
var corrector = new GrammarCorrector(ruleSet);

// Correct text
string input = "  Hello   world !  How are you ?  ";
string output = corrector.Correct(input);
// Output: "Hello world! How are you?"
```

## Minimal rule set implementation

```csharp
using NuciText.Grammar;
using NuciText.Grammar.Rules;

public sealed class MyRuleSet : GrammarRuleSet
{
    public override string LanguageCode => "en";

    public override IReadOnlyList<IGrammarRule> Rules { get; } = new IGrammarRule[]
    {
        new TrimWhitespaceRule(),
        // Add custom rules here
    };
}
```

## Run tests

```bash
dotnet test
```

## Next steps

- Read [API usage examples](api-usage-examples.md) for more patterns
- See [components/core.md](components/core.md) for full API reference
- Check [behaviour/text-correction.md](behaviour/text-correction.md) for correction behaviors
- Review [extensibility](faq.md#extensibility) for custom rules