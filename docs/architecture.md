# Architecture

This document elaborates on the root [ARCHITECTURE.md](../ARCHITECTURE.md) with implementation-level detail.

## Architectural style

NuciText.Grammar follows a **layered library** pattern with a minimal core and extensibility points:

```
Core Interfaces → Base Classes → Concrete Implementations → Consumer Extensions
```

### Layers

| Layer | Components | Responsibility |
|-------|------------|----------------|
| Core Interfaces | `IGrammarRule`, `IGrammarRuleSet`, `IGrammarCorrector` | Define public contracts; stable, versioned |
| Base Classes | `GrammarRule`, `GrammarRuleSet` | Provide default implementations; reduce boilerplate |
| Concrete Implementations | `GrammarCorrector`, `TrimWhitespaceRule`, `SpacingRule`, `PatternReplacementRuleBase` | Ready-to-use implementations |
| Consumer Extensions | Consumer-implemented `IGrammarRule`, `IGrammarRuleSet` | Custom logic for specific languages/scenarios |

## Runtime flow

```
Consumer calls Correct(inputText)
    ↓
GrammarCorrector iterates ruleSet.Rules in order
    ↓
For each rule:
    → rule.CanApply(currentText) → if true: rule.Apply(currentText)
    → output becomes input for next rule
    ↓
Return final corrected text
```

## Components

### Core interfaces

| Interface | File | Key members |
|-----------|------|-------------|
| `IGrammarRule` | `IGrammarRule.cs` | `Id`, `Description`, `CanApply(string)`, `Apply(string)` |
| `IGrammarRuleSet` | `IGrammarRuleSet.cs` | `LanguageCode`, `Rules` (IReadOnlyList<IGrammarRule>) |
| `IGrammarCorrector` | `IGrammarCorrector.cs` | `Correct(string)` |

### Base classes

| Class | File | Purpose |
|-------|------|---------|
| `GrammarRule` | `GrammarRule.cs` | Abstract base; default `CanApply` compares `Apply` result to input |
| `GrammarRuleSet` | `GrammarRuleSet.cs` | Abstract base; requires `LanguageCode` and `Rules` implementation |

### Concrete implementations

| Class | File | Purpose |
|-------|------|---------|
| `GrammarCorrector` | `GrammarCorrector.cs` | Sealed; applies rules sequentially; validates non-null inputs |
| `GrammarRuleException` | `GrammarRuleException.cs` | Exception with `RuleId` property; three constructor overloads |
| `TrimWhitespaceRule` | `Rules/TrimWhitespaceRule.cs` | Sealed; trims and collapses whitespace via regex |
| `SpacingRule` | `Rules/SpacingRule.cs` | Internal sealed; normalises spacing around punctuation/emoticons |
| `PatternReplacementRuleBase` | `Rules/PatternReplacementRuleBase.cs` | Abstract base for regex-based word replacements |

## Dependency direction

```
Consumer Application
    ↓ references
NuciText.Grammar (Core + Base + Concrete)
    ↓ no external dependencies
```

- No external dependencies (dependency-free)
- Interfaces are stable and versioned
- Base classes are extensible but not sealed
- Rules depend only on core interfaces and base classes

## Data architecture

Stateless in-memory string processing:

```
Input Text (string) → Correction Pipeline → Output Text (string)
```

No persistence, no caching, no external resources.

## Cross-cutting concerns

### Security and privacy

- In-memory string transformations only
- No file system, network, or external resource access
- No logging, caching, or retention of input/output text
- No telemetry or data collection
- Consumer controls all data lifetime

### Error handling

- `GrammarRuleException` wraps rule execution failures
- `GrammarCorrector` propagates exceptions from rules
- `CanApply` returning `false` is not an error; rule is skipped
- Consumers should catch `GrammarRuleException` for rule failures

### Observability

- No built-in logging, metrics, or tracing