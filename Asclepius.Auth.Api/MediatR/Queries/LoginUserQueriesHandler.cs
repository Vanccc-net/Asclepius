using Asclepius.Auth.Business;
using Asclepius.Auth.Domain.Interfaces;
using Asclepius.DTO;
using Asclepius.DTO.Auth;
using MassTransit;
using MediatR;

namespace Asclepius.Auth.Api.MediatR.Queries;

public class LoginUserQueriesHandler(
    JwtGenerator jwtGenerator,
    IRefreshToken refreshTokenRepo,
    IUser userRepo,
    IPublishEndpoint publishEndpoint) : IRequestHandler<LoginUserQueries, JwtResponse>
{
    public async Task<JwtResponse> Handle(LoginUserQueries request, CancellationToken cancellationToken)
    {
        var user = await userRepo.GetByEmailWithRolesAsync(request.Email, cancellationToken);

        if (user == null) throw new KeyNotFoundException("User not found");

        if (!user.Password.Verify(request.Password)) throw new Exception("Invalid password");

        var accessToken = jwtGenerator.GenerateJwtToken(user);
        var refreshToken = await refreshTokenRepo.Create(user.Id, cancellationToken);

        await publishEndpoint.Publish(Notification.Create(user.Id, user.Email.Value,
            $"Новый вход в аккаунт! {DateTime.UtcNow}"), cancellationToken);

        return new JwtResponse(accessToken, refreshToken.Value);
    }
}