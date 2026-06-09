namespace Kyogo.Api.Contracts.Authentication;

public sealed record TokenExchangeResponse(string RawToken, string RawRefreshToken);