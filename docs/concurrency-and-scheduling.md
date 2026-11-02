# Concurrency and Scheduling

## Concurrency model

NuciText.Grammar is **fully thread-safe** by design:

- All operations are pure functions with no shared mutable state
- `GrammarCorrector` holds only a readonly `IGrammarRuleSet` reference
- Rules are stateless (no instance fields affecting behavior)
- Multiple threads can safely call `Correct` on the same instance concurrently

## Thread safety guarantees

| Component | Thread-safe | Reason |
|-----------|-------------|--------|
| `GrammarCorrector` | ✅ Yes | Immutable after construction |
| `GrammarRule` (base) | ✅ Yes | No instance state |
| `GrammarRuleSet` (base) | ✅ Yes | No instance state |
| `TrimWhitespaceRule` | ✅ Yes | Stateless |
| `SpacingRule` | ✅ Yes | Stateless |
| Custom rules | ⚠️ Consumer responsibility | Must be stateless |

## No async operations

The library has **no asynchronous APIs**:

- `IGrammarCorrector.Correct` is synchronous
- `IGrammarRule.CanApply` is synchronous
- `IGrammarRule.Apply` is synchronous
- No `Task`, `ValueTask`, or `async`/`await` anywhere

## No scheduling

The library has **no scheduling infrastructure**:

- No background workers
- No timers
- No thread pools
- No cancellation tokens
- No `IAsyncEnumerable` or streaming

## Execution model

```
Caller thread
    │
    ▼
GrammarCorrector.Correct(text)
    │
    ├──► ruleSet.Rules (enumeration)
    │       │
    │       ├──► rule.CanApply(text) ──► bool
    │       │
    │       └──► rule.Apply(text) ──► string (if CanApply true)
    │
    ▼
Returns corrected string
```

All execution happens on the caller's thread, sequentially.

## Consumer concurrency patterns

### Parallel text processing

```csharp
// Safe: same corrector, different texts
var texts = GetTexts();
var results = texts.AsParallel()
    .Select(t => corrector.Correct(t))
    .ToArray();
```

### Multiple correctors

```csharp
// Safe: different correctors, same or different threads
var enCorrector = new GrammarCorrector(new EnglishRuleSet());
var deCorrector = new GrammarCorrector(new GermanRuleSet());

// Can be used concurrently
```

### Custom rule thread safety

```csharp
// ✅ Safe: stateless
public sealed class UppercaseRule : GrammarRule
{
    protected override string DoApply(string text) => text.ToUpperInvariant();
}

// ❌ Unsafe: mutable state
public sealed class CountingRule : GrammarRule
{
    int count = 0; // Shared mutable state!
    protected override string DoApply(string text) => $"{count++}: {text}";
}
```

## Performance characteristics

| Aspect | Behavior |
|--------|----------|
| Lock contention | None (no locks) |
| Memory allocation | Per-call string allocations only |
| Scalability | Linear with core count (embarrassingly parallel) |
| Latency | Deterministic, no GC pauses from library |

## Cancellation

No built-in cancellation support. Consumers can implement their own:

```csharp
public string CorrectWithCancellation(string text, CancellationToken ct)
{
    ct.ThrowIfCancellationRequested();
    return corrector.Correct(text);
}
```

## Summary

| Feature | Status |
|---------|--------|
| Thread-safe | ✅ Yes |
| Async APIs | ❌ None |
| Scheduling | ❌ None |
| Cancellation | ❌ None (consumer) |
| Parallelism | ✅ Embarrassingly parallel |
| Lock-free | ✅ Yes |