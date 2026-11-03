# Data model

NuciText.Grammar is a stateless text processing library. It does not define domain entities, persistence models, or complex data structures. The data model consists entirely of in-memory string transformations.

## Core data types

| Type | Representation | Purpose |
|------|----------------|---------|
| Input text | `string` | Text provided by consumer to `Correct()` |
| Intermediate text | `string` | Output of each rule, becomes input to next rule |
| Output text | `string` | Final corrected text returned to consumer |
| Rule identifier | `string` | Unique ID for each rule (`IGrammarRule.Id`) |
| Rule description | `string` | Human-readable description (`IGrammarRule.Description`) |
| Language code | `string` | IETF language tag (`IGrammarRuleSet.LanguageCode`) |

## Rule data flow

```
string input → [Rule 1] → string → [Rule 2] → string → ... → [Rule N] → string output
```

Each rule receives a `string` and returns a `string`. The `GrammarCorrector` chains them sequentially.

## Rule set composition

`IGrammarRuleSet` exposes:
- `LanguageCode` — IETF language tag (e.g., "en", "ro", "de")
- `Rules` — `IReadOnlyList<IGrammarRule>` ordered by application priority

## Built-in rule data

### TrimWhitespaceRule
- **Id**: `"trim-whitespace"`
- **Description**: "Normalises whitespace by trimming and collapsing internal whitespace."
- **Transformation**: `text.Trim()` then `Regex.Replace(text, "\\s+", " ")`

### SpacingRule
- **Id**: `"spacing"`
- **Description**: "Normalises spacing around punctuation and emoticons."
- **Transformations** (applied in order):
  1. Remove spaces before punctuation: `[ ]*([,.])` → `$1`
  2. Add space after punctuation before letters: `([,.:;])([a-zA-Z][a-zA-Z])` → `$1 $2`
  3. Add space before emoticons: `([a-z])([Oo][7/]|[:;=][DdOoPpXx*)]|[Xx][Dd])` → `$1 $2`

### PatternReplacementRuleBase
- **RegexReplacement** record: `Pattern` (string), `Replacement` (string)
- **WordTerminators**: `[' ', '?', '!', '.', '-']`
- **Methods**:
  - `ApplyWordReplacements` — whole-word replacements with terminator handling
  - `ApplyRegexReplacements` — direct regex replacements
  - `ReplaceWord` — replaces whole words with boundary checking

## Exception data

`GrammarRuleException` carries:
- `RuleId` — Identifier of the failed rule
- Standard `Exception` properties: `Message`, `InnerException`, `StackTrace`

## No persistence

- No database, file system, or external storage
- No caching of input/output
- No serialization formats defined by the library
- Consumer controls all data lifetime