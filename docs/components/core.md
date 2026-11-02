# Components

## Core components

### IGrammarRule
**File**: `NuciText.Grammar/IGrammarRule.cs`
**Namespace**: `NuciText.Grammar`

Defines the contract for a single grammar correction rule.

| Member | Type | Description |
|--------|------|-------------|
| `Id` | `string` | Unique identifier for the rule |
| `Description` | `string` | Human-readable description of what the rule corrects |
| `CanApply(string text)` | `bool` | Determines if the rule matches the given text |
| `Apply(string text)` | `string` | Applies the correction to the text |

### IGrammarRuleSet
**File**: `NuciText.Grammar/IGrammarRuleSet.cs`
**Namespace**: `NuciText.Grammar`

Defines an ordered collection of grammar correction rules for a specific language.

| Member | Type | Description |
|--------|------|-------------|
| `LanguageCode` | `string` | IETF language tag (e.g., "en", "ro", "de") |
| `Rules` | `IReadOnlyList<IGrammarRule>` | Ordered list of rules; applied in sequence |

### IGrammarCorrector
**File**: `NuciText.Grammar/IGrammarCorrector.cs`
**Namespace**: `NuciText.Grammar`

Defines the text correction entry point.

| Member | Type | Description |
|--------|------|-------------|
| `Correct(string text)` | `string` | Applies all rules from the configured rule set to the given text |

### GrammarRule
**File**: `NuciText.Grammar/GrammarRule.cs`
**Namespace**: `NuciText.Grammar`

Abstract base class for grammar correction rules with default applicability logic.

| Member | Type | Description |
|--------|------|-------------|
| `Id` | `abstract string` | Must be implemented by derived classes |
| `Description` | `abstract string` | Must be implemented by derived classes |
| `CanApply(string text)` | `virtual bool` | Default: calls `CheckApplicability` |
| `Apply(string text)` | `string` | Validates input, calls `DoApply` |
| `CheckApplicability(string text)` | `protected virtual bool` | Default: `DoApply(text) != text` |
| `DoApply(string text)` | `protected abstract string` | Must be implemented; actual correction logic |

### GrammarRuleSet
**File**: `NuciText.Grammar/GrammarRuleSet.cs`
**Namespace**: `NuciText.Grammar`

Abstract base class for language-specific grammar rule sets.

| Member | Type | Description |
|--------|------|-------------|
| `LanguageCode` | `abstract string` | Must be implemented; IETF language tag |
| `Rules` | `abstract IReadOnlyList<IGrammarRule>` | Must be implemented; ordered rules |

### GrammarCorrector
**File**: `NuciText.Grammar/GrammarCorrector.cs`
**Namespace**: `NuciText.Grammar`

Sealed implementation of `IGrammarCorrector` that applies rules sequentially.

| Member | Type | Description |
|--------|------|-------------|
| `GrammarCorrector(IGrammarRuleSet ruleSet)` | Constructor | Validates non-null ruleSet |
| `Correct(string text)` | `string` | Iterates rules; applies each where `CanApply` returns true |

### GrammarRuleException
**File**: `NuciText.Grammar/GrammarRuleException.cs`
**Namespace**: `NuciText.Grammar`

Exception type for rule execution failures.

| Member | Type | Description |
|--------|------|-------------|
| `RuleId` | `string` | ID of the rule that failed |
| `GrammarRuleException(string ruleId)` | Constructor | Default message |
| `GrammarRuleException(string ruleId, string message)` | Constructor | Custom message |
| `GrammarRuleException(string ruleId, string message, Exception inner)` | Constructor | Custom message with inner exception |

## Rule components

### TrimWhitespaceRule
**File**: `NuciText.Grammar/Rules/TrimWhitespaceRule.cs`
**Namespace**: `NuciText.Grammar.Rules`

Sealed rule that trims and collapses whitespace.

| Property | Value |
|----------|-------|
| `Id` | `"trim-whitespace"` |
| `Description` | "Normalises whitespace by trimming and collapsing internal whitespace." |
| `DoApply` | `Regex.Replace(text.Trim(), "\\s+", " ")` |

### SpacingRule
**File**: `NuciText.Grammar/Rules/SpacingRule.cs`
**Namespace**: `NuciText.Grammar.Rules`

Internal sealed rule that normalises spacing around punctuation and emoticons.

| Property | Value |
|----------|-------|
| `Id` | `"spacing"` |
| `Description` | "Normalises spacing around punctuation and emoticons." |
| `DoApply` | Three regex replacements in sequence |

### PatternReplacementRuleBase
**File**: `NuciText.Grammar/Rules/PatternReplacementRuleBase.cs`
**Namespace**: `NuciText.Grammar.Rules`

Abstract base class for regex-based word replacement rules.

| Member | Type | Description |
|--------|------|-------------|
| `RegexReplacement` | `readonly record struct` | `Pattern` (string), `Replacement` (string) |
| `WordTerminators` | `static readonly char[]` | `[' ', '?', '!', '.', '-']` |
| `ApplyWordReplacements` | `protected static string` | Whole-word replacements with terminator handling |
| `ApplyRegexReplacements` | `protected static string` | Direct regex replacements |
| `ReplaceWord` | `protected static string` | Replaces whole words with boundary checking |