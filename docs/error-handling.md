# Error handling

## Error taxonomy

### ArgumentNullException

**Thrown when**: Required argument is null

| Location | Parameter | Condition |
|----------|-----------|-----------|
| `GrammarCorrector` constructor | `ruleSet` | `ruleSet is null` |
| `GrammarCorrector.Correct` | `text` | `text is null` |
| `GrammarRule.CanApply` | `text` | `text is null` |
| `GrammarRule.Apply` | `text` | `text is null` |

**Message**: Standard .NET `ArgumentNullException` message with parameter name

**Consumer handling**: Validate inputs before calling, or catch and handle

### GrammarRuleException

**Thrown when**: A grammar rule fails to apply its correction

| Property | Type | Description |
|----------|------|-------------|
| `RuleId` | `string` | Identifier of the failed rule |
| `Message` | `string` | Error message |
| `InnerException` | `Exception` | Optional inner exception |

**Constructors**:
1. `GrammarRuleException(string ruleId)` — Default message: "Grammar rule '{ruleId}' failed to apply."
2. `GrammarRuleException(string ruleId, string message)` — Custom message
3. `GrammarRuleException(string ruleId, string message, Exception innerException)` — Custom message with inner exception

**Consumer handling**: Catch `GrammarRuleException` to handle rule-specific failures; inspect `RuleId` for diagnostics

## Error handling patterns

### Library behavior

```
GrammarCorrector.Correct(text)
    ↓
Validate text is not null → ArgumentNullException if null
    ↓
For each rule in ruleSet.Rules:
    ↓
    rule.CanApply(currentText)
        ↓
        Validate text is not null → ArgumentNullException if null
        ↓
        Return bool
    ↓
    If CanApply returned true:
        rule.Apply(currentText)
            ↓
            Validate text is not null → ArgumentNullException if null
            ↓
            DoApply(text)
                ↓
                Rule-specific logic
                ↓
                May throw GrammarRuleException (rule's responsibility)
            ↓
            Return corrected text
    ↓
    Update currentText
    ↓
Return final currentText
```

### Exception propagation

- `ArgumentNullException` — thrown by library, propagates to consumer
- `GrammarRuleException` — thrown by rules, propagates through `GrammarCorrector` to consumer
- Other exceptions — propagate from rules to consumer

### Non-error conditions

- `CanApply` returning `false` — normal behavior, rule skipped
- Empty rule set — returns input text unchanged
- Empty input text — returns empty string (after trim if TrimWhitespaceRule applied)

## Consumer guidance

### Recommended pattern

```csharp
try
{
    string corrected = corrector.Correct(inputText);
}
catch (ArgumentNullException ex)
{
    // Programming error: null input
    // Log and fix calling code
}
catch (GrammarRuleException ex)
{
    // Rule execution failure
    // Log rule ID and message
    // Optionally fall back to original text
}
```

### Defensive pattern

```csharp
string corrected;
try
{
    corrected = corrector.Correct(inputText);
}
catch (Exception ex)
{
    // Log exception
    corrected = inputText; // Fall back to original
}
```

## Testing error paths

### Unit tests for null handling

```csharp
[Test]
public void Correct_NullText_ThrowsArgumentNullException()
{
    IGrammarRuleSet ruleSet = new EmptyRuleSet();
    IGrammarCorrector corrector = new GrammarCorrector(ruleSet);

    Assert.Throws<ArgumentNullException>(() => corrector.Correct(null!));
}
```

### Unit tests for rule failures

```csharp
sealed class FailingRule : GrammarRule
{
    public override string Id => "failing";
    public override string Description => "Always fails.";

    protected override string DoApply(string text)
        => throw new GrammarRuleException(Id, "Simulated failure");
}

[Test]
public void Correct_FailingRule_ThrowsGrammarRuleException()
{
    IGrammarRuleSet ruleSet = new SingleRuleSet(new FailingRule());
    IGrammarCorrector corrector = new GrammarCorrector(ruleSet);

    Assert.Throws<GrammarRuleException>(() => corrector.Correct("test"));
}
```

## Recovery strategies

| Strategy | When to use | Implementation |
|----------|-------------|----------------|
| Fail fast | Critical text processing | Let exception propagate |
| Fallback to original | Non-critical enhancement | Catch `GrammarRuleException`, return input |
| Partial correction | Best-effort processing | Apply rules individually with try/catch |
| Logging and continue | Diagnostics needed | Catch, log, continue with next rule |

## Invariants

- Null input always throws `ArgumentNullException` (never returns null)
- Rule failures propagate as exceptions (never silently swallowed)
- `CanApply` returning false never throws (normal skip behavior)
- Empty rule set returns input unchanged (no-op)