using Microsoft.AspNetCore.Components.Authorization;
using MonitorPet.Ui.Client.Model;
using System.Security.Claims;

namespace MonitorPet.Ui.Client.Services;

internal class UserInfoService
{
    private readonly AuthenticationStateProvider _auth;

    public UserInfoService(AuthenticationStateProvider auth)
    {
        _auth = auth;
    }

    public async Task<UserInfo?> GetUserInfo()
    {
        var user = (await _auth.GetAuthenticationStateAsync())?.User;

        if (user is null)
            return null;


        return new UserInfo
        (
            id: TryGetIdUser(user)   
        );
    }

    private int TryGetIdUser(ClaimsPrincipal identity)
    {
        var valueFound = identity.FindFirst("Id")?.Value;

        int.TryParse(valueFound, out int value);

        return value;
    }
}
