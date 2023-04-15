using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MonitorPet.Application.Model.User;
using MonitorPet.Application.Services.Interfaces;
using MonitorPet.Ui.Server.Security;
using MonitorPet.Ui.Shared.Model.User;

namespace MonitorPet.Ui.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IMapper _map;
    private readonly IApiToken _tokenService;
    private readonly Application.Security.ContextClaim _contextClaim;

    public UserController(IUserService userService, IMapper map, IApiToken tokenService,
        Application.Security.ContextClaim contextClaim)
    {
        _userService = userService;
        _map = map;
        _tokenService = tokenService;
        _contextClaim = contextClaim;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserViewModel createUserViewModel)
    {
        var userModel = _map.Map<CreateUserModel>(createUserViewModel);

        await _userService.Create(userModel);

        return Ok();
    }

    [HttpPatch]
    public async Task<IActionResult> Patch([FromBody] UpdateUserViewModel updateUserViewModel)
    {
        var user = await _contextClaim.GetRequiredCurrentClaim();

        var userModel = _map.Map<UpdateUserModel>(updateUserViewModel);

        await _userService.Update(user.RequiredIdUser, userModel);

        return Ok();
    }

    [HttpPatch("password")]
    public async Task<IActionResult> PatchPassword([FromBody] UpdatePasswordViewModel updatePasswordViewModel)
    {
        var user = await _contextClaim.GetRequiredCurrentClaim();

        await _userService.ChangePassword(user.RequiredIdUser, 
            updatePasswordViewModel.OldPassword, updatePasswordViewModel.NewPassword);

        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<QueryUserModel>> Get(int id)
    {
        var userFound = await _userService.GetByIdUser(id);
        
        if (userFound is null)
            return NoContent();

        return Ok(userFound);
    }
}
