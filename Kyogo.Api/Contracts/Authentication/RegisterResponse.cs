namespace Kyogo.Api.Contracts.Authentication;

public sealed record RegisterResponse(string RawToken, string RawRefreshToken);