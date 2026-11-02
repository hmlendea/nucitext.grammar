# State and Persistence

## State management

NuciText.Grammar is **completely stateless**:

- No instance state in `GrammarCorrector` (only readonly `ruleSet`)
- No instance state in `GrammarRule` base class
- No instance state in `GrammarRuleSet` base class
- No instance state in built-in rules (`TrimWhitespaceRule`, `SpacingRule`)
- No static state anywhere in the library

## Persistence

**No persistence of any kind**:

- No file I/O
- No database access
- No serialization
- No caching
- No configuration files
- No user preferences

## Data flow

```
Input string (parameter)
        │
        ▼
GrammarCorrector.Correct()
        │
        ├──► Rule 1: CanApply → Apply (if true)
        │
        ├──► Rule 2: CanApply → Apply (if true)
        │
        └──► Rule N: CanApply → Apply (if true)
        │
        ▼
Output string (return value)
```

All data flows through method parameters and return values. No data is retained between calls.

## No migrations

Since there is no persistence, there are:
- No schema migrations
- No data migrations
- No version upgrades for stored data
- No backward compatibility concerns for persisted state

## Consumer state management

Consumers may choose to maintain state externally:

```csharp
// Consumer-managed state example
public class CorrectionSession
{
    readonly IGrammarCorrector corrector;
    int correctionsApplied = 0;

    public CorrectionSession(IGrammarCorrector corrector)
    {
        this.corrector = corrector;
    }

    public string Correct(string text)
    {
        string result = corrector.Correct(text);
        if (result != text) correctionsApplied++;
        return result;
    }

    public int CorrectionsApplied => correctionsApplied;
}
```

## Thread safety and state

Since the library has no state:
- No thread synchronization needed
- No race conditions possible
- No memory visibility issues
- Safe for concurrent use without locks

## Testing implications

- No test setup/teardown for state
- Tests are naturally isolated
- No need for database transactions or rollbacks
- Parallel test execution safe by default

## Summary

| Aspect | Status |
|--------|--------|
| Instance state | ❌ None |
| Static state | ❌ None |
| Persistence | ❌ None |
| Serialization | ❌ None |
| Configuration files | ❌ None |
| Migrations | ❌ N/A |
| Consumer state | ✅ External only |