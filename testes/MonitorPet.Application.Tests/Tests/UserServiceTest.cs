using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using MonitorPet.Application.Model.User;
using MonitorPet.Application.Services.Interfaces;
using MonitorPet.Application.Tests.Email;

namespace MonitorPet.Application.Tests.Tests;

public class UserServiceTest : TestBase
{
    [Fact]
    public async Task Create_TryCreateUserWithoutDosador_FailedUserNeedsDosador()
    {
        var userService =
            ServiceProvider.GetRequiredService<IUserService>();

        var validNewUserWithoutDosador = Mocks.UserMock.CreateNewValidUser();

        await Assert.ThrowsAnyAsync<Core.Exceptions.CommonCoreException>(
            () => userService.Create(validNewUserWithoutDosador));
    }

    [Fact]
    public async Task Create_TryCreateUserWithValidTokenDosador_Success()
    {
        var userService =
            ServiceProvider.GetRequiredService<IUserService>();

        var validNewUser =
            Mocks.UserMock.CreateNewValidUser(tokenAccessDosador: Mocks.DosadorMock.DefaultIdDosador1.ToString());

        await userService.Create(validNewUser);
    }

    [Fact]
    public async Task Create_TryCreateUserTwoTimes_Failed()
    {
        var userService =
            ServiceProvider.GetRequiredService<IUserService>();

        var validNewUser =
            Mocks.UserMock.CreateNewValidUser(tokenAccessDosador: Mocks.DosadorMock.DefaultIdDosador1.ToString());

        await userService.Create(validNewUser);

        await Assert.ThrowsAnyAsync<Core.Exceptions.ConflictCoreException>(
            () => userService.Create(validNewUser));
    }

    [Fact]
    public async Task ConfirmEmail_TryConfirmEmailWithInvalidContext_FailedToConfirm()
    {
        var userService = ServiceProvider.GetRequiredService<IUserService>();
        var tupleUserCreated = await CreateCompleteUser();

        using var contextClaim =
            CreateContext(Enumerable.Empty<Claim>());

        await Assert.ThrowsAnyAsync<Core.Exceptions.UnauthorizedCoreException>(
            () => userService.ConfirmEmail(tupleUserCreated.UserCreated.Email));
    }

    [Fact]
    public async Task ConfirmEmail_TryConfirmEmailWithValidContext_Success()
    {
        var userService = ServiceProvider.GetRequiredService<IUserService>();
        var tupleUserCreated = await CreateCompleteUser();

        using var contextClaim =
            CreateContext(tupleUserCreated.Inbox.TryGetObjectLastInbox<Claim[]>() ?? Enumerable.Empty<Claim>());

        Assert.NotNull(
            await userService.ConfirmEmail(tupleUserCreated.UserCreated.Email));
    }

    [Fact]
    public async Task Login_TryLoginBeforeConfirmEmail_FailedBecauseNotConfirmedYet()
    {
        var userService = ServiceProvider.GetRequiredService<IUserService>();
        var tupleUserCreated = await CreateCompleteUser();

        await Assert.ThrowsAnyAsync<Application.Exceptions.UserEmailNotConfirmedCoreException>(
            () => userService.Login(tupleUserCreated.UserCreated.Email, tupleUserCreated.UserCreated.Password));
    }

    [Fact]
    public async Task Login_Login_Success()
    {
        var userService = ServiceProvider.GetRequiredService<IUserService>();
        var tupleUserCreated = await CreateCompleteUserEmailConfirmed();

        var resultLogin =
            await userService.Login(tupleUserCreated.UserCreated.Email, tupleUserCreated.UserCreated.Password);

        Assert.NotEmpty(resultLogin.Claims);
    }

    [Fact]
    public async Task GetByIdUser_GetOwnUser_Success()
    {
        var userService = ServiceProvider.GetRequiredService<IUserService>();
        var tupleUserCreated = await CreateAndLoginUser();

        using var contextClaim = CreateContext(tupleUserCreated.Claims);
        
        Assert.NotNull(
            await userService.GetByIdUser(contextClaim.ClaimModel.RequiredIdUser)
        );
    }

    [Fact]
    public async Task LoginDosador_LoginLinkedDosador_Success()
    {
        var dosadorService = ServiceProvider.GetRequiredService<IDosadorService>();
        var userService = ServiceProvider.GetRequiredService<IUserService>();
        var tupleUserCreated = await CreateAndLoginUser();

        using var contextClaim = CreateContext(tupleUserCreated.Claims);

        var newDosador = await CreateDosador();

        await dosadorService
                .AddDosadorToUser(contextClaim.ClaimModel.RequiredIdUser, newDosador.IdDosador.ToString());
        
        Assert.NotEmpty(
            await userService.LoginDosador(contextClaim.ClaimModel.RequiredIdUser, newDosador.IdDosador)
        );
    }

    [Fact]
    public async Task LoginDosador_LoginUnlinkedDosador_FailedToLogin()
    {
        var userService = ServiceProvider.GetRequiredService<IUserService>();
        var tupleUserCreated = await CreateAndLoginUser();

        using var contextClaim = CreateContext(tupleUserCreated.Claims);

        await Assert.ThrowsAnyAsync<Core.Exceptions.ForbiddenCoreException>(
            () => userService.LoginDosador(contextClaim.ClaimModel.RequiredIdUser, Guid.NewGuid())
        );
    }

    [Fact]
    public async Task SendEmailChangePassword_SendEmailChangePassword_SuccessSent()
    {
        var scooped = ServiceProvider;
        var userService = scooped.GetRequiredService<IUserService>();
        var tupleUserCreated = await CreateAndLoginUser(scooped);

        await userService.SendEmailChangePassword(tupleUserCreated.UserCreated.Email);
    }

    [Fact]
    public async Task ConfirmChangePassoword_ConfirmNewPasswordWithLogin_Success()
    {
        var scooped = ServiceProvider;
        var userService = scooped.GetRequiredService<IUserService>();
        var inbox = scooped.GetRequiredService<FakeInbox>();
        var tupleUserCreated = await CreateAndLoginUser(scooped);

        await userService.SendEmailChangePassword(tupleUserCreated.UserCreated.Email);

        string newPassword = tupleUserCreated.UserCreated.Password + "123";

        using var context = CreateContext(inbox.TryGetObjectLastInbox<Claim[]>()
            ?? Enumerable.Empty<Claim>());

        await userService.ConfirmChangePassoword(tupleUserCreated.UserCreated.Email, newPassword);

        Assert.NotEmpty(
            (await userService.Login(tupleUserCreated.UserCreated.Email, newPassword)).Claims
        );
    }
}