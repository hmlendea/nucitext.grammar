# FAQ

## General

### What is NuciText.Grammar?

A lightweight, dependency-free .NET library for rule-based grammar and text normalisation. It provides a minimal core API for defining grammar correction rules and rule sets, along with a concrete `GrammarCorrector` that applies rules in order.

### What can it do?

- Whitespace normalisation (trim, collapse internal whitespace)
- Punctuation spacing normalisation
- Emoticon spacing
- Custom rule-based text transformations
- Language-specific correction pipelines

### What can't it do?

- Natural language parsing or understanding
- Machine learning-based corrections
- Spell checking
- Grammar checking beyond rule-based patterns
- Streaming/large document processing (loads entire text into memory)

## Usage

### How do I use it?

```csharp
IGrammarRuleSet ruleSet = new MyRuleSet();
IGrammarCorrector corrector = new GrammarCorrector(ruleSet);
string corrected = corrector.Correct(inputText);
```

### How do I create a custom rule?

Inherit from `GrammarRule` and implement `Id`, `Description`, and `DoApply`:

```csharp
public sealed class MyRule : GrammarRule
{
    public override string Id => "my-rule";
    public override string Description => "Does something.";

    protected override string DoApply(string text)
    {
        // Your correction logic
        return correctedText;
    }
}
```

### How do I create a rule set?

Inherit from `GrammarRuleSet` and implement `LanguageCode` and `Rules`:

```csharp
public sealed class MyRuleSet : GrammarRuleSet
{
    public override string LanguageCode => "en";

    public override IReadOnlyList<IGrammarRule> Rules { get; } = new IGrammarRule[]
    {
        new TrimWhitespaceRule(),
        new MyRule()
    };
}
```

### Can I use multiple rule sets?

Yes, instantiate multiple `GrammarCorrector` instances with different rule sets, or create a composite rule set that combines rules from multiple sources.

## Built-in rules

### What built-in rules are included?

- `TrimWhitespaceRule` — Trims and collapses whitespace (public)
- `SpacingRule` — Normalises punctuation/emoticon spacing (internal)

### Why is SpacingRule internal?

It's considered an implementation detail not yet stabilized for public API. It may be made public in a future version.

## Extensibility

### Can I use regex in my rules?

Yes, use `PatternReplacementRuleBase` which provides:
- `ApplyRegexReplacements` — direct regex replacements
- `ApplyWordReplacements` — whole-word replacements with boundary handling
- `ReplaceWord` — single word replacement with terminator handling

### Can rules have configuration?

The library has no built-in configuration. Pass configuration to your custom rule/rule set constructors.

### Can I implement IGrammarRule directly?

Yes, implement all four members: `Id`, `Description`, `CanApply`, `Apply`. This gives full control but requires more boilerplate.

## Performance

### Is it fast?

The library is designed for typical text correction scenarios. No benchmarks are published. For very large texts, consider that the entire text is loaded into memory and processed sequentially.

### Is it thread-safe?

Yes, all operations are pure functions with no shared state. Multiple threads can safely use the same `GrammarCorrector` instance concurrently.

## Licensing

### What license?

GPL-3.0-or-later. This is a copyleft license — derivative works must also be open source under compatible terms.

### Can I use it in a commercial product?

Yes, but you must comply with GPL-3.0-or-later terms, including making your source code available under compatible license.

## Contributing

### How do I contribute?

See [CONTRIBUTING.md](../CONTRIBUTING.md) for guidelines on reporting issues, suggesting enhancements, and submitting code changes.

### What's the code style?

Follow the project's C# coding conventions. Run `dotnet format` before committing.

## Troubleshooting

### My rule isn't being applied

Check:
1. Rule is in the rule set's `Rules` list
2. `CanApply` returns `true` for your input
3. Rule order — earlier rules may transform text so later rules don't match

### Getting ArgumentNullException

Ensure you're not passing `null` to:
- `GrammarCorrector` constructor (ruleSet)
- `GrammarCorrector.Correct` (text)
- `GrammarRule.CanApply` (text)
- `GrammarRule.Apply` (text)

### Getting GrammarRuleException

A rule's `DoApply` threw an exception. Check the `RuleId` property to identify which rule failed, and the message/inner exception for details.