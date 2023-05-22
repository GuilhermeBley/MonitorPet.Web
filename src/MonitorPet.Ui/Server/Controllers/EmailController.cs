using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonitorPet.Application.Model.Email;
using MonitorPet.Application.Security;
using MonitorPet.Application.Services.Interfaces;
using MonitorPet.Ui.Shared.Model.Email;

namespace MonitorPet.Ui.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmailController : ControllerBase
{
    private readonly IEmailUserService _emailUserService;
    private readonly IMapper _map;
    private readonly Application.Security.ContextClaim _contextClaim;

    public EmailController(IEmailUserService emailUserService, IMapper map, ContextClaim contextClaim)
    {
        _emailUserService = emailUserService;
        _map = map;
        _contextClaim = contextClaim;
    }

    [HttpGet("Availables")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<EmailTypeViewModel>>> GetAvailables(CancellationToken cancellationToken)
    {
        var emailsAvailables = await _emailUserService.GetAvailables(cancellationToken: cancellationToken);

        if (!emailsAvailables.Any())
            return NoContent();

        return Ok(_map.Map<IEnumerable<EmailTypeViewModel>>(emailsAvailables));
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<QueryRoleEmailUserViewModel>>> GetFromUser(CancellationToken cancellationToken)
    {
        var ctx = await _contextClaim.GetRequiredCurrentClaim();

        var emailsUser = await _emailUserService.GetByIdUser(ctx.RequiredIdUser, cancellationToken: cancellationToken);

        if (!emailsUser.Any())
            return NoContent();

        return Ok(_map.Map<IEnumerable<QueryRoleEmailUserViewModel>>(emailsUser));
    }

    [HttpPatch]
    [Authorize]
    public async Task<ActionResult<IEnumerable<QueryRoleEmailUserViewModel>>> PatchFromUser(
        CreateOrUpdateRoleEmailUserModel createOrUpdateRoleEmailUserModel,
        CancellationToken cancellationToken)
    {
        _ = await _contextClaim.GetRequiredCurrentClaim();

        var emailsUserModified = await _emailUserService.CreateOrUpdate(createOrUpdateRoleEmailUserModel, cancellationToken: cancellationToken);

       return Ok(_map.Map<IEnumerable<QueryRoleEmailUserViewModel>>(emailsUserModified));
    }
}
