# Invariants

## System-wide invariants

### 1. Null safety

- **Invariant**: No public method accepts or returns null for string parameters/results
- **Enforcement**: `ArgumentNullException` thrown at entry points
- **Scope**: `GrammarCorrector.Correct`, `GrammarRule.CanApply`, `GrammarRule.Apply`, `GrammarCorrector` constructor

### 2. Rule application order

- **Invariant**: Rules are applied in the exact order returned by `IGrammarRuleSet.Rules`
- **Enforcement**: `GrammarCorrector.Correct` iterates `ruleSet.Rules` sequentially
- **Scope**: All rule sets, all corrector instances

### 3. CanApply/Apply consistency

- **Invariant**: If `CanApply(text)` returns `true`, then `Apply(text)` will return a different string
- **Enforcement**: Default `GrammarRule.CheckApplicability` compares `DoApply(text) != text`
- **Scope**: All rules inheriting from `GrammarRule` (unless overridden)

### 4. Idempotency of non-matching rules

- **Invariant**: If `CanApply(text)` returns `false`, the text is unchanged
- **Enforcement**: `GrammarCorrector` only calls `Apply` when `CanApply` returns `true`
- **Scope**: All rule applications

### 5. Statelessness

- **Invariant**: No instance state affects correction results
- **Enforcement**: All methods are pure functions; no instance fields in `GrammarCorrector` except readonly `ruleSet`
- **Scope**: `GrammarCorrector`, all built-in rules

### 6. Rule identity

- **Invariant**: Each rule has a unique, stable `Id` string
- **Enforcement**: `IGrammarRule.Id` is abstract/required; `GrammarRuleException` carries `RuleId`
- **Scope**: All rule implementations

### 7. Language code format

- **Invariant**: `IGrammarRuleSet.LanguageCode` returns a valid IETF language tag
- **Enforcement**: Convention only (not validated by library)
- **Scope**: All rule set implementations

### 8. Exception hierarchy

- **Invariant**: Rule execution failures are wrapped in `GrammarRuleException`
- **Enforcement**: Convention; built-in rules don't throw other exceptions
- **Scope**: Consumer-implemented rules should follow this pattern

### 9. No side effects

- **Invariant**: Correction operations have no observable side effects
- **Enforcement**: No I/O, no logging, no static state mutation, no external calls
- **Scope**: All library code

### 10. Deterministic output

- **Invariant**: Same input + same rule set = same output
- **Enforcement**: Pure functions, no randomness, no time-dependent logic
- **Scope**: All correction operations

## Component invariants

### GrammarCorrector

| Invariant | Description |
|-----------|-------------|
| Constructor validates ruleSet | Throws `ArgumentNullException` if null |
| Correct validates text | Throws `ArgumentNullException` if null |
| Returns non-null string | Never returns null |
| Preserves rule order | Applies rules in `ruleSet.Rules` order |
| Propagates exceptions | Does not catch rule exceptions |

### GrammarRule (base class)

| Invariant | Description |
|-----------|-------------|
| CanApply validates text | Throws `ArgumentNullException` if null |
| Apply validates text | Throws `ArgumentNullException` if null |
| Default CanApply matches Apply change | `CheckApplicability` returns `DoApply(text) != text` |
| DoApply is pure | No side effects, deterministic |

### GrammarRuleSet (base class)

| Invariant | Description |
|-----------|-------------|
| LanguageCode is non-null | Abstract, must return string |
| Rules is non-null | Abstract, must return `IReadOnlyList<IGrammarRule>` |
| Rules order is significant | Consumer-defined priority |

### TrimWhitespaceRule

| Invariant | Description |
|-----------|-------------|
| Id = "trim-whitespace" | Stable identifier |
| Output has no leading/trailing whitespace | `text.Trim()` applied |
| Internal whitespace collapsed | `\\s+` → single space |
| Empty input → empty output | `""` → `""` |

### SpacingRule

| Invariant | Description |
|-----------|-------------|
| Id = "spacing" | Stable identifier |
| No spaces before commas/periods | `[ ]*([,.])` → `$1` |
| Space after punctuation before letters | `([,.:;])([a-zA-Z][a-zA-Z])` → `$1 $2` |
| Space before emoticons | Pattern matches common emoticons |

## Contract invariants

### IGrammarRule

- `Id` — non-null, non-empty, stable
- `Description` — non-null, human-readable
- `CanApply(string)` — non-null input, returns bool, no side effects
- `Apply(string)` — non-null input, returns non-null string, may throw `GrammarRuleException`

### IGrammarRuleSet

- `LanguageCode` — non-null, IETF language tag format
- `Rules` — non-null, ordered, each element non-null

### IGrammarCorrector

- `Correct(string)` — non-null input, returns non-null string, throws `ArgumentNullException` for null, propagates rule exceptions

## Test invariants

### Unit test requirements

- Every public method has tests for null input
- Every rule has tests for matching and non-matching cases
- Rule ordering tests verify sequence matters
- Exception tests verify correct exception types and properties