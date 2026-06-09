namespace Kyogo.Api.Contracts.Authentication;

public sealed record RegisterRequest(string Username, string Email, string Password);