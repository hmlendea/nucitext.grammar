# Repository overview

NuciText.Grammar is a lightweight, dependency-free .NET library for rule-based grammar and text normalisation. It provides a minimal core API for defining grammar correction rules and rule sets, along with a concrete `GrammarCorrector` that applies rules in order.

## Purpose

The library enables deterministic, rule-based text cleanup pipelines such as:
- Whitespace normalisation
- Punctuation spacing
- Language-specific corrections

## Scope

The repository contains:
- **NuciText.Grammar** — Main library project with core interfaces, base classes, and built-in rules
- **NuciText.Grammar.UnitTests** — Unit test project using NUnit 4.x and Moq

## Entry points

- `IGrammarCorrector.Correct(string text)` — Primary entry point for text correction
- `IGrammarRuleSet` — Defines ordered rules for a language or scenario
- `IGrammarRule` — Defines a single correction rule

## Key capabilities

- Small core API with no external dependencies
- Extensible rule system via `IGrammarRule` and `IGrammarRuleSet` interfaces
- Built-in `TrimWhitespaceRule` for whitespace normalisation
- `PatternReplacementRuleBase` for regex-based word replacements
- Target framework: .NET 10.0
- Licensed under GPL-3.0-or-later