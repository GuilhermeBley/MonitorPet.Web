using MonitorPet.Application.Model.Dosador;
using MonitorPet.Application.Model.User;
using MonitorPet.Application.Tests.ModelDb;

namespace MonitorPet.Application.Tests.Mocks;

internal static class UserMock
{
    public static CreateUserModel CreateNewValidUser(
        string? email = null,
        string name = "Valid Name",
        string nickName = "Valid Nick Name",
        string? password = null,
        string tokenAccessDosador = "access-token"
    )
    {
        if (email is null)
            email = CreateNewValidEmail();

        if (password is null)
            password = ValidPassword();

        return
            new CreateUserModel
            {
                Email = email,
                Name = name,
                NickName = nickName,
                Password = password,
                TokenAccessDosador = tokenAccessDosador
            };
    }

    public static string CreateNewValidEmail()
        => $"{Guid.NewGuid()}@email.com";

    public static string ValidPassword()
        => "validPass123@";
}