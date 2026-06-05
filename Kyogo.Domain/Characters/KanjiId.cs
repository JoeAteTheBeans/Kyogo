namespace Kyogo.Domain.Characters;

public readonly record struct KanjiId
{
    private readonly string _value;
    
    public KanjiId(string value) : this()
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("KanjiId cannot be whitespace or null.");
        if (value.EnumerateRunes().Count() != 1)
            throw new ArgumentException("KanjiId must be exactly one Unicode scalar value.");
        _value = value;
    }
    
    public override string ToString() 
        => _value;
}