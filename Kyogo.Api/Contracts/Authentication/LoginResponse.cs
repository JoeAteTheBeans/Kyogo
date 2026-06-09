namespace Kyogo.Api.Contracts.Authentication;

public sealed record LoginResponse(string RawToken, string RawRefreshToken);