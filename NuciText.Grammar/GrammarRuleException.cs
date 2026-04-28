using System;

namespace NuciText.Grammar;

/// <summary>
/// The exception thrown when a <see cref="IGrammarRule"/> fails to apply its correction.
/// </summary>
public class GrammarRuleException : Exception
{
    /// <summary>
    /// Gets the ID of the rule that failed.
    /// </summary>
    public string RuleId { get; }

    /// <summary>
    /// Initialises a new <see cref="GrammarRuleException"/> for the given rule.
    /// </summary>
    public GrammarRuleException(string ruleId)
        : base($"Grammar rule '{ruleId}' failed to apply.")
    {
        RuleId = ruleId;
    }

    /// <summary>
    /// Initialises a new <see cref="GrammarRuleException"/> for the given rule with a custom message.
    /// </summary>
    public GrammarRuleException(string ruleId, string message)
        : base(message)
    {
        RuleId = ruleId;
    }

    /// <summary>
    /// Initialises a new <see cref="GrammarRuleException"/> for the given rule with a custom message and inner exception.
    /// </summary>
    public GrammarRuleException(string ruleId, string message, Exception innerException)
        : base(message, innerException)
    {
        RuleId = ruleId;
    }
}
