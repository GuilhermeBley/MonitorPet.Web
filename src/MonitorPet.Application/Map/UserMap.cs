using MonitorPet.Application.Model.User;
using MonitorPet.Core.Entity;

namespace MonitorPet.Application.Map;

public static class UserMap
{
    public static QueryUserModel MapQueryUserModel(UserModel userModel)
        => new QueryUserModel
        {
            Id = userModel.Id,
            Email = userModel.Email,
            Name = userModel.Name,
            NickName = userModel.NickName
        };

    public static User MapUserModelWithOutPassword(UserModel userModel)
        => User.CreateWithOutPassword(
            userModel.Email,
            userModel.Email,
            userModel.EmailConfirmed,
            userModel.LockOutEnd,
            userModel.AccessFailedCount,
            userModel.Name,
            userModel.NickName
        );
}