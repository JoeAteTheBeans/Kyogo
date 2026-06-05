namespace Kyogo.Domain.Users;

public readonly record struct Token(string Value)
{
    public override string ToString() => Value;
}