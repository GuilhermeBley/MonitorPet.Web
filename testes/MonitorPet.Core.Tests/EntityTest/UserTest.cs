using MonitorPet.Core.Entity;
using MonitorPet.Core.Exceptions;

namespace MonitorPet.Core.Tests.EntityTest;

public class UserTest
{
    [Fact]
    public void Create_TryCreateUser_Success()
    {
        Assert.NotNull(
            CreateValidUser()
        );
    }

    [Fact]
    public void Create_TryCreateUserWithInvalidEmail_Failed()
    {
        Assert.ThrowsAny<Core.Exceptions.CommonCoreException>(
            () => CreateValidUser(email: "invalidemail")
        );
    }

    [Fact]
    public void Create_TryCreateUserWithEmptyEmail_Failed()
    {
        Assert.ThrowsAny<CommonCoreException>(
            () => CreateValidUser(email: string.Empty)
        );
    }

    [Fact]
    public void Create_TryCreateUserWithInvalidPassword_Failed()
    {
        Assert.ThrowsAny<CommonCoreException>(
            () => CreateValidUser(password: "invalidpassword")
        );
    }

    [Fact]
    public void Equal_CheckTwoNewsEquals_Failed()
    {
        var user1 = CreateValidUser();
        var user2 = CreateValidUser();

        Assert.NotEqual(user1, user2);
    }

    [Fact]
    public void Equal_CheckSame_Success()
    {
        var user = CreateValidUser();

        Assert.Equal(user, user);
    }

    [Fact]
    public void TryResetAcessFailedAccount_CheckIfIsTrue_Success()
    {
        var user = CreateValidUser();

        user.AddAcessFailedAccount();
        user.AddAcessFailedAccount();
        user.AddAcessFailedAccount();

        Assert.True(user.TryResetAcessFailedAccount());
    }
    
    [Fact]
    public void TryResetAcessFailedAccount_CheckIfIsZero_Success()
    {
        var user = CreateValidUser();

        user.AddAcessFailedAccount();
        user.AddAcessFailedAccount();
        user.AddAcessFailedAccount();

        user.TryResetAcessFailedAccount();

        Assert.Equal(0, user.AccessFailedCount);
        Assert.Null(user.LockOutEnd);
    }

    [Fact]
    public void AddAcessFailedAccount_CheckIfIsZero_Failed()
    {
        var user = CreateValidUser();

        for (int i = 0; i < User.MAX_COUNT_ACCESS_FAILED; i++)
            user.AddAcessFailedAccount();

        Assert.NotEqual(0, user.AccessFailedCount);
        Assert.NotNull(user.LockOutEnd);
    }

    [Fact]
    public void AddAcessFailedAccount_CheckLockoutEndBeforeMaxCountAccess_Failed()
    {
        var user = CreateValidUser();

        int beforeMaxCountAccess = User.MAX_COUNT_ACCESS_FAILED - 1;
        for (int i = 0; i < beforeMaxCountAccess; i++)
            user.AddAcessFailedAccount();

        Assert.Null(user.LockOutEnd);
    }

    private static User CreateValidUser(
        string login = "valid@email.com",
        string email = "valid@email.com",
        bool emailConfirmed = true,
        DateTime? lockOutEnd = null,
        int accessFailedCount = 0,
        string name = "valid name",
        string? nickName = "nickName",
        string password = "valid@123",
        string passwordHash = "",
        string passwordSalt = "")
        => User.Create(login, email, emailConfirmed, lockOutEnd, accessFailedCount, 
            name, nickName, password, passwordHash, passwordSalt);
}