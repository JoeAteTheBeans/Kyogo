namespace Kyogo.Application.Authentication.Tokens;

public readonly record struct Token(string Value)
{
    public override string ToString() => Value;
}