using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonitorPet.Application.Model.User;
using MonitorPet.Application.Services.Interfaces;
using MonitorPet.Ui.Server.Context;
using MonitorPet.Ui.Server.Security;
using MonitorPet.Ui.Shared.Model.Exceptions;
using MonitorPet.Ui.Shared.Model.User;
using MySqlX.XDevAPI.Common;

namespace MonitorPet.Ui.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _map;
    private readonly IApiToken _tokenService;
    private readonly Application.Security.ContextClaim _contextClaim;

    public LoginController(IUserService userService, IMapper map, IApiToken tokenService,
        Application.Security.ContextClaim contextClaim)
    {
        _userService = userService;
        _map = map;
        _tokenService = tokenService;
        _contextClaim = contextClaim;
    }

    [HttpPost]
    public async Task<ActionResult<ResultAccessAccountViewModel>> Login([FromBody] LoginUserViewModel loginUserModel)
    {
        try
        {
            var result = await _userService.Login(loginUserModel.Login, loginUserModel.Password);
            return Ok(new ResultAccessAccountViewModel
            {
                Token = _tokenService.GenerateToken(result.Claims.ToArray()),
                User = _map.Map<QueryUserViewModel>(result.UserModel)
            });
        }
        catch (Application.Exceptions.BlockedUserCoreException e)
        {
            return BadRequest(new ErrorViewModel(e.StatusCode, $"Usuário bloqueado até {e.LockoutEnd}."));
        }
    }

    [Authorize(AuthenticationSchemes = Security.JwtBearerConfigurationExtension.JwtBearerConfiguration.SCHEME_EMAIL_TOKEN)]
    [HttpPost("confirmaccount")]
    public async Task<ActionResult<ResultAccessAccountViewModel>> ConfirmAccount([FromBody] ConfirmEmailViewModel confirmEmailViewModel)
    {
        var result = await _userService.ConfirmEmail(confirmEmailViewModel.Email);

        return Ok(new ResultAccessAccountViewModel
        {
            Token = _tokenService.GenerateToken(result.Claims.ToArray()),
            User = _map.Map<QueryUserViewModel>(result.UserModel)
        });
    }

    [HttpPost("sendemailconfirmaccount")]
    public async Task<IActionResult> SendAgainEmailConfirmAccount([FromBody] ConfirmEmailViewModel confirmEmailViewModel)
    {
        var confirmEmailModel = _map.Map<ConfirmEmailViewModel>(confirmEmailViewModel);

        await _userService.SendAgainEmailConfirmAccount(confirmEmailModel.Email);

        return Ok();
    }

    [HttpPost("SendEmailForgotPassword")]
    public async Task<IActionResult> SendEmailForgotPassword([FromBody] ForgotPasswordViewModel forgotPasswordViewModel)
    {
        await _userService.SendEmailChangePassword(forgotPasswordViewModel.EmailAddress);

        return Ok();
    }

    [Authorize(AuthenticationSchemes = Security.JwtBearerConfigurationExtension.JwtBearerConfiguration.SCHEME_EMAIL_TOKEN,
        Roles = "ChangePassword")]
    [HttpPost("ConfirmForgotPassword")]
    public async Task<IActionResult> ConfirmForgotPassword([FromBody] ChangePasswordViewModel changePasswordViewModel)
    {
        var context = await _contextClaim.GetRequiredCurrentClaim();

        var resultClaim =
            await _userService.ConfirmChangePassoword(context.RequiredEmail, changePasswordViewModel.Password);

        return Ok(new ResultAccessAccountViewModel
        {
            Token = _tokenService.GenerateToken(resultClaim.Claims.ToArray()),
            User = _map.Map<QueryUserViewModel>(resultClaim.UserModel)
        });
    }
}