using Asclepius.Auth.Api.MediatR.Commands;
using Asclepius.Auth.Api.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Asclepius.Auth.Api.Controllers;

[Route("api/auth/[action]")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// </summary>
    /// <param name="userCommand"></param>
    /// <param name="useCookie">Сохранение токена в куки</param>
    /// <returns></returns>
    [HttpPost("{useCookie}")]
    public async Task<IActionResult> Register(RegisterUserCommand userCommand, bool useCookie = false)
    {
        var res = await mediator.Send(userCommand).ConfigureAwait(false);

        if (useCookie)
            Response.Cookies.Append("refreshToken", res.RefreshToken);

        return Ok(res);
    }

    /// <summary>
    /// </summary>
    /// <param name="userCommand"></param>
    /// <param name="useCookie">Сохранение токена в куки</param>
    /// <returns></returns>
    [HttpPost("{useCookie}")]
    public async Task<IActionResult> Login(LoginUserQueries userCommand, bool useCookie = false)
    {
        var res = await mediator.Send(userCommand).ConfigureAwait(false);

        if (useCookie)
            Response.Cookies.Append("refreshToken", res.RefreshToken);

        return Ok(res);
    }
}