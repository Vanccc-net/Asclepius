using Asclepius.Auth.Business;
using Asclepius.Auth.Data.Exceptions;
using Asclepius.Auth.Domain;
using Asclepius.Auth.Domain.Interfaces;
using Asclepius.DTO;
using Asclepius.DTO.Auth;
using Asclepius.DTO.ProfileObject;
using MassTransit;
using MediatR;

namespace Asclepius.Auth.Api.MediatR.Commands;

public class RegisterUserCommandHandler(
    ILogger<RegisterUserCommandHandler> logger,
    JwtGenerator jwtGenerator,
    IRefreshToken refreshTokenRepo,
    IUser userRepo,
    IUnitOfWork unitOfWork,
    IPublishEndpoint publishEndpoint)
    : IRequestHandler<RegisterUserCommand, JwtResponse>
{
    public async Task<JwtResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        
        if (await userRepo.EmailExistAsync(request.Email, cancellationToken))
            throw new UserAlreadyExistsException("User with this email already exists");
        
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var user = User.Create(request.Email, request.Password, request.FirstName, request.LastName);
            userRepo.Add(user);
            
            var profile = new Profile(user.Id, request.DateOfBirth, request.Gender, BloodType.AbNegative);

            await publishEndpoint.Publish(profile, cancellationToken);
            await publishEndpoint.Publish(Notification.Create(user.Id, user.Email.Value,
                $"Добро пожаловать, {user.LastName}!"), cancellationToken);

            await unitOfWork.CommitTransactionAsync(cancellationToken);
            await userRepo.MarkEmailAsExistsAsync(request.Email, cancellationToken);
            logger.LogInformation("Успешная регистрация {UserId}", user.Id);
            var accessToken = jwtGenerator.GenerateJwtToken(user);
            var refreshToken = await refreshTokenRepo.Create(user.Id, cancellationToken);
            return new JwtResponse(accessToken, refreshToken.Value);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}