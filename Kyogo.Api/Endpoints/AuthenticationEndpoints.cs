using System.Diagnostics;
using Kyogo.Api.Contracts.Authentication;
using Kyogo.Application.Authentication;
using Kyogo.Application.Authentication.Tokens;

namespace Kyogo.Api.Endpoints;

public static class AuthenticationEndpoints
{
    public static IEndpointRouteBuilder MapAuthenticationEndpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("/api/auth");
        
        //Register
        group.MapPost("/register", async (
            RegisterRequest request,
            AuthenticationService authenticationService,
            CancellationToken cancellationToken = default) =>
        {
            RegisterResult result = await authenticationService.RegisterAsync(request.Username, request.Email, request.Password, cancellationToken);
            return result.State switch
            {
                RegisterResultState.UsernameTaken => Results.Problem("That username is already taken.", statusCode: StatusCodes.Status409Conflict),
                RegisterResultState.EmailInUse => Results.Problem("An account with that email address already exists.", statusCode: StatusCodes.Status409Conflict),
                RegisterResultState.Success => Results.Ok(new RegisterResponse(
                    result.Token?.Value ?? throw new InvalidOperationException($"{nameof(AuthenticationService)} has returned a successful {nameof(RegisterResultState)} without a token"),
                    result.RefreshToken?.RawToken ?? throw new InvalidOperationException($"{nameof(AuthenticationService)} has returned a successful {nameof(RegisterResultState)} without a refresh token"))),
                _ => throw new UnreachableException($"Unhandled {nameof(RegisterResultState)} value: {result.State}")
            };
        });
        
        //LoginWithEmail
        group.MapPost("/login/email", async (
            LoginWithEmailRequest request,
            AuthenticationService authenticationService,
            CancellationToken cancellationToken = default) =>
        {
            LoginResult result = await authenticationService.LoginWithEmailAsync(request.Email, request.Password, cancellationToken);
            return result.State switch
            {
                LoginResultState.EmailDoesNotExist or LoginResultState.IncorrectPassword => Results.Problem("Invalid credentials.", statusCode: StatusCodes.Status401Unauthorized),
                LoginResultState.Success => Results.Ok(new LoginResponse(
                    result.Token?.Value ?? throw new InvalidOperationException($"{nameof(AuthenticationService)} has returned a successful {nameof(LoginResultState)} without a token"),
                    result.RefreshToken?.RawToken ?? throw new InvalidOperationException($"{nameof(AuthenticationService)} has returned a successful {nameof(LoginResultState)} without a refresh token"))),
                _ => throw new UnreachableException($"Unhandled {nameof(LoginResultState)} value: {result.State}")
            };
        });
        
        //LoginWithUsername
        group.MapPost("/login/username", async (
            LoginWithUsernameRequest request,
            AuthenticationService authenticationService,
            CancellationToken cancellationToken = default) =>
        {
            LoginResult result = await authenticationService.LoginWithUsernameAsync(request.Username, request.Password, cancellationToken);
            return result.State switch
            {
                LoginResultState.UsernameDoesNotExist or LoginResultState.IncorrectPassword => Results.Problem("Invalid credentials.", statusCode: StatusCodes.Status401Unauthorized),
                LoginResultState.Success => Results.Ok(new LoginResponse(
                    result.Token?.Value ?? throw new InvalidOperationException($"{nameof(AuthenticationService)} has returned a successful {nameof(LoginResultState)} without a token"),
                    result.RefreshToken?.RawToken ?? throw new InvalidOperationException($"{nameof(AuthenticationService)} has returned a successful {nameof(LoginResultState)} without a refresh token"))),
                _ => throw new UnreachableException($"Unhandled {nameof(LoginResultState)} value: {result.State}")
            };
        });
        
        //TokenExchange
        group.MapPost("tokenExchange", async (
            TokenExchangeRequest request,
            TokenExchangeService tokenExchangeService,
            CancellationToken cancellationToken = default) =>
        {
            TokenExchangeResult result = await tokenExchangeService.ExchangeByRawTokenAsync(request.RawRefreshToken, cancellationToken);
            return result.State switch
            {
                TokenExchangeResultState.RefreshTokenNotFound or TokenExchangeResultState.Revoked or TokenExchangeResultState.Expired or TokenExchangeResultState.UserNotFound
                    => Results.Problem("Refresh token invalid.", statusCode: StatusCodes.Status401Unauthorized),
                TokenExchangeResultState.Exchanged => Results.Ok(new TokenExchangeResponse(
                    result.Token?.Value ?? throw new InvalidOperationException($"{nameof(TokenExchangeService)} has returned a successful {nameof(TokenExchangeResult)} without a token"),
                    result.RefreshToken?.RawToken ?? throw new InvalidOperationException($"{nameof(TokenExchangeService)} has returned a successful {nameof(TokenExchangeResult)} without a refresh token"))),
                _ => throw new UnreachableException($"Unhandled {nameof(TokenExchangeResult)} value: {result.State}")
            };
        });
        
        return builder;
    }
}