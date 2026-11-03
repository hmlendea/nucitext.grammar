# Security

## Security posture

NuciText.Grammar is designed with a **minimal attack surface**:

- **No I/O operations** — No file, network, or database access
- **No external dependencies** — Zero NuGet dependencies in the main library
- **No reflection/serialization** — No dynamic code execution
- **No unsafe code** — Pure managed C#
- **No native interop** — No P/Invoke or COM
- **In-memory only** — All processing in managed memory

## Threat model

### Assets

- Input text (processed in memory)
- Output text (returned to caller)
- Rule logic (compiled code)

### Threats mitigated

| Threat | Mitigation |
|--------|------------|
| Code injection | No dynamic code execution, no script evaluation |
| Data exfiltration | No network I/O, no logging, no telemetry |
| Denial of service | No unbounded allocations, no recursion, O(n) processing |
| Supply chain | Zero external dependencies |
| Deserialization attacks | No serialization/deserialization |

### Residual risks

| Risk | Likelihood | Impact | Notes |
|------|------------|--------|-------|
| Malicious rule implementation | Medium | High | Consumer controls rule code |
| Large input OOM | Low | Medium | Consumer should validate input size |
| Regex DoS (ReDoS) | Low | Medium | Built-in rules use simple patterns; custom rules may introduce risk |

## Input validation

The library validates:

- `null` inputs → `ArgumentNullException`
- No length limits enforced (consumer responsibility)
- No content validation (consumer responsibility)

## Data handling

| Data type | Handling |
|-----------|----------|
| Input text | Read-only, processed in memory, returned as new string |
| Output text | New string allocated, no persistence |
| Rule configuration | Compile-time only (constructor parameters) |
| Exception data | `GrammarRuleException` contains rule ID and message only |

## No sensitive data processing

The library does not:
- Process credentials, tokens, or secrets
- Handle PII specially
- Encrypt or decrypt data
- Store any data

## Dependency security

### Main library (NuciText.Grammar)

- **Zero external dependencies**
- Target framework: `net10.0`
- No transitive dependencies

### Test project (NuciText.Grammar.UnitTests)

| Package | Version | Purpose |
|---------|---------|---------|
| NUnit | 4.5.1 | Test framework |
| Moq | 4.20.72 | Mocking |
| Microsoft.NET.Test.Sdk | 18.3.0 | Test runner |
| NUnit3TestAdapter | 6.2.0 | VS integration |
| NuciAPI.Middleware | 2.0.1 | Internal test utilities |

## Secure development practices

- All code reviewed via PR
- CI runs on every push (`dotnet.yml`)
- No secrets in repository
- GPL-3.0-or-later license (copyleft)

## Consumer security responsibilities

1. **Validate input size** — Prevent OOM from huge texts
2. **Audit custom rules** — Review rule logic for ReDoS, infinite loops
3. **Handle exceptions** — Catch `ArgumentNullException` and `GrammarRuleException`
4. **Dependency updates** — Monitor for .NET runtime security updates
5. **License compliance** — GPL-3.0-or-later obligations

## Reporting security issues

See [SECURITY.md](../SECURITY.md) for vulnerability disclosure process.

## Compliance

| Standard | Status |
|----------|--------|
| SOC 2 | Not certified |
| ISO 27001 | Not certified |
| GDPR | No personal data processed |
| HIPAA | No health data processed |
| PCI DSS | No payment data processed |

## Summary

| Aspect | Rating |
|--------|--------|
| Attack surface | Minimal |
| External dependencies | None |
| Data persistence | None |
| Network access | None |
| Code execution | Static only |
| Consumer risk | Low (if rules audited) |