# Ambiguities and open questions

## Known gaps

### 1. SpacingRule visibility
- **Issue**: `SpacingRule` is marked `internal` but provides useful functionality
- **Question**: Should it be made `public` for consumer use?
- **Impact**: Consumers cannot directly use or test `SpacingRule` without reflection

### 2. Language-specific rule sets
- **Issue**: No built-in language-specific rule sets (e.g., `EnglishRuleSet`, `RomanianRuleSet`)
- **Question**: Should the library provide opinionated rule sets for common languages?
- **Impact**: Consumers must compose their own rule sets

### 3. Rule ordering guidance
- **Issue**: No documented best practices for rule ordering
- **Question**: Should the library provide guidance or validation for rule ordering?
- **Impact**: Incorrect ordering can produce unexpected results

### 4. Performance characteristics
- **Issue**: No documented performance benchmarks or complexity guarantees
- **Question**: Should performance characteristics be documented?
- **Impact**: Consumers cannot reason about performance for large texts

### 5. Unicode/Internationalization
- **Issue**: Built-in rules use ASCII-focused patterns (e.g., `[a-zA-Z]`)
- **Question**: How should the library handle non-Latin scripts?
- **Impact**: May not work correctly for languages with different scripts

### 6. Rule composition patterns
- **Issue**: No documented patterns for composing rules from multiple sources
- **Question**: Should there be a `CompositeRuleSet` or similar utility?
- **Impact**: Consumers must manually merge rule lists

### 7. Streaming/large text support
- **Issue**: API only accepts `string` (entire text in memory)
- **Question**: Should there be streaming support for large documents?
- **Impact**: Memory pressure for very large texts

### 8. Configuration/options
- **Issue**: No configuration options for rules (e.g., custom emoticon lists)
- **Question**: Should rules be configurable?
- **Impact**: Consumers must subclass to customize behavior

## TODOs

- [ ] Document rule ordering best practices
- [ ] Add performance benchmarks
- [ ] Consider making `SpacingRule` public
- [ ] Evaluate Unicode support in built-in rules
- [ ] Consider providing language-specific rule sets
- [ ] Document extensibility patterns more thoroughly