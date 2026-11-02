# Logging

## Logging framework

NuciText.Grammar has **no built-in logging framework**. The library performs no logging, metrics, or tracing of any kind.

## Log levels

Not applicable — no logging performed.

## Structured fields

Not applicable — no log entries generated.

## Correlation

Not applicable — no request tracing or correlation IDs.

## Sinks/destinations

Not applicable — no log output.

## Redaction/masking

Not applicable — no logging of input or output text.

## Retention

Not applicable — no logs retained.

## Sampling

Not applicable — no log sampling.

## Performance considerations

- **Zero logging overhead** — No logging code paths, no allocations for logging
- **No async logging** — No logging at all
- **No throughput impact** — Pure string processing only

## Error logging

The library does not log exceptions. Exceptions propagate to the consumer:

- `ArgumentNullException` — for null input validation
- `GrammarRuleException` — for rule execution failures

Consumers should catch and log these exceptions as appropriate for their application.

## Audit logging

No audit logging performed. The library does not track:
- Input text
- Output text
- Rule applications
- Timing
- Success/failure rates

## Configuration

No logging configuration exists. Consumers configure logging in their host application.

## Consumer guidance

### Adding logging to rule execution

```csharp
public sealed class LoggingCorrector : IGrammarCorrector
{
    readonly IGrammarCorrector inner;
    readonly ILogger<LoggingCorrector> logger;

    public LoggingCorrector(IGrammarCorrector inner, ILogger<LoggingCorrector> logger)
    {
        this.inner = inner;
        this.logger = logger;
    }

    public string Correct(string text)
    {
        logger.LogDebug("Correcting text (length: {Length})", text.Length);
        try
        {
            string result = inner.Correct(text);
            logger.LogDebug("Correction complete (length: {Length})", result.Length);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Correction failed");
            throw;
        }
    }
}
```

### Adding logging to custom rules

```csharp
public sealed class LoggingRule : GrammarRule
{
    readonly IGrammarRule inner;
    readonly ILogger logger;

    public LoggingRule(IGrammarRule inner, ILogger logger)
    {
        this.inner = inner;
        this.logger = logger;
    }

    public override string Id => inner.Id;
    public override string Description => inner.Description;

    public override bool CanApply(string text)
    {
        bool canApply = inner.CanApply(text);
        logger.LogDebug("Rule {RuleId} CanApply: {CanApply}", Id, canApply);
        return canApply;
    }

    public override string Apply(string text)
    {
        logger.LogDebug("Rule {RuleId} applying", Id);
        string result = inner.Apply(text);
        logger.LogDebug("Rule {RuleId} applied (changed: {Changed})", Id, result != text);
        return result;
    }
}
```

### Recommended logging for consumers

| Event | Level | Fields |
|-------|-------|--------|
| Correction started | Debug | Input length, rule set language |
| Correction completed | Debug | Output length, duration |
| Rule applied | Trace | Rule ID, input/output length |
| Rule skipped | Trace | Rule ID |
| ArgumentNullException | Warning | Parameter name, call site |
| GrammarRuleException | Error | Rule ID, message, stack trace |

## Summary

| Aspect | Status |
|--------|--------|
| Built-in logging | ❌ None |
| Logging dependencies | ❌ None |
| Log output | ❌ None |
| Consumer responsibility | ✅ Full |
| Thread-safe logging | ✅ N/A (no logging) |