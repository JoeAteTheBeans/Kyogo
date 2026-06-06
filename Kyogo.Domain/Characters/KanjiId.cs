using System.Diagnostics.CodeAnalysis;

namespace Kyogo.Domain.Characters;

public readonly record struct KanjiId
{
    public string Value { get; init; }
    
    public KanjiId(string value) : this()
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("KanjiId cannot be whitespace or null.");
        if (value.EnumerateRunes().Count() != 1)
            throw new ArgumentException("KanjiId must be exactly one Unicode scalar value.");
        Value = value;
    }
    
    public override string ToString() 
        => Value;
}