namespace Kyogo.Api.Contracts.Authentication;

public sealed record LoginWithUsernameRequest(string Username, string Password);