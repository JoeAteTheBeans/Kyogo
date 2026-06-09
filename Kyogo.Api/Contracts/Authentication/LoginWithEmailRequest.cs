namespace Kyogo.Api.Contracts.Authentication;

public sealed record LoginWithEmailRequest(string Email, string Password);