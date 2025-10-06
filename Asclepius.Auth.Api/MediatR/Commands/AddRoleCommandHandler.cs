using Asclepius.Auth.Business;
using Asclepius.Auth.Domain.Interfaces;
using Asclepius.DTO.Auth;
using MediatR;

namespace Asclepius.Auth.Api.MediatR.Commands;

public class AddRoleCommandHandler(
    ILogger<AddRoleCommandHandler> logger,
    IRole roleRepo,
    IUnitOfWork unitOfWork,
    IUser userRepo,
    IRefreshToken refreshTokenRepo,
    JwtGenerator jwtGenerator) : IRequestHandler<AddRoleCommand, JwtResponse>
{
    public async Task<JwtResponse> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var user = await userRepo.GetByIdAsync(request.UserId, cancellationToken);

            if (user == null)
                throw new InvalidOperationException($"User with id {request.UserId} not found");

            var role = await roleRepo.GetByIdAsync(request.RoleId, cancellationToken);

            if (role == null)
                throw new InvalidOperationException($"Role with id {request.RoleId} not found");

            user.AddRole(role);

            userRepo.Update(user);

            var refreshToken = await refreshTokenRepo.Create(user.Id, cancellationToken);
            var accessToken = jwtGenerator.GenerateJwtToken(user);

            await unitOfWork.CommitTransactionAsync(cancellationToken);

            logger.LogInformation($"User with id {request.UserId} has been added to role {role.Name}");

            return new JwtResponse(accessToken, refreshToken.Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}