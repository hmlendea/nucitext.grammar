# API usage examples

## Basic usage

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
// Result: "Hello world"
```

## Multiple rules

```csharp
public sealed class EnglishRuleSet : GrammarRuleSet
{
    public override string LanguageCode => "en";

    public override IReadOnlyList<IGrammarRule> Rules { get; } = new IGrammarRule[]
    {
        new TrimWhitespaceRule(),
        new SpacingRule()
    };
}

IGrammarRuleSet ruleSet = new EnglishRuleSet();
IGrammarCorrector corrector = new GrammarCorrector(ruleSet);

string corrected = corrector.Correct("  Hello  ,  world  :)  ");
// Result: "Hello, world :)"
```

## Custom rule

```csharp
public sealed class DoubleSpaceRule : GrammarRule
{
    public override string Id => "double-space";
    public override string Description => "Replaces double spaces with a single space.";

    protected override string DoApply(string text)
        => text.Replace("  ", " ");
}

public sealed class CustomRuleSet : GrammarRuleSet
{
    public override string LanguageCode => "en";

    public override IReadOnlyList<IGrammarRule> Rules { get; } = new IGrammarRule[]
    {
        new TrimWhitespaceRule(),
        new DoubleSpaceRule()
    };
}
```

## Pattern-based rule

```csharp
public sealed class AbbreviationExpansionRule : PatternReplacementRuleBase
{
    public override string Id => "abbreviation-expansion";
    public override string Description => "Expands common abbreviations.";

    static readonly RegexReplacement[] Replacements =
    [
        new(@"\betc\b", "et cetera"),
        new(@"\be\.g\.\b", "for example"),
        new(@"\bi\.e\.\b", "that is")
    ];

    protected override string DoApply(string text)
        => ApplyWordReplacements(text, Replacements);
}
```

## Error handling

```csharp
try
{
    string corrected = corrector.Correct(inputText);
}
catch (ArgumentNullException ex)
{
    // Handle null input
}
catch (GrammarRuleException ex)
{
    // Handle rule-specific failure
    Console.WriteLine($"Rule '{ex.RuleId}' failed: {ex.Message}");
}
```

## Using with dependency injection

```csharp
// Register in DI container
services.AddSingleton<IGrammarRuleSet, EnglishRuleSet>();
services.AddSingleton<IGrammarCorrector, GrammarCorrector>();

// Inject and use
public class TextProcessor
{
    readonly IGrammarCorrector corrector;

    public TextProcessor(IGrammarCorrector corrector)
    {
        this.corrector = corrector;
    }

    public string Process(string input) => corrector.Correct(input);
}
```