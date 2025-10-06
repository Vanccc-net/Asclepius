using Asclepius.Auth.Api.MediatR.Commands;
using Asclepius.Auth.Api.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asclepius.Auth.Api.Controllers;

/// <summary>
///     Контроллер для работы с RBAC. Требует прав Админа
/// </summary>
/// <param name="mediator"></param>
[Route("api/roles/[action]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class RoleController(IMediator mediator) : ControllerBase
{
    /// <summary>
    ///     Получить список всех ролей
    /// </summary>
    /// <returns>Список Role - Id и Название</returns>
    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var response = await mediator.Send(new RoleListQueries()).ConfigureAwait(false);
        return Ok(response);
    }

    /// <summary>
    ///     Добавить роль к пользователю
    /// </summary>
    /// <param name="request">ID роли, ID пользователя, к котору добавляем роль</param>
    /// <returns>Вернёт JwtResponse с новым accessToken и новым RefreshToken</returns>
    [HttpPost]
    public async Task<IActionResult> AddRole(AddRoleCommand request)
    {
        var response = await mediator.Send(request).ConfigureAwait(false);
        return Ok(response);
    }
}