using System.Security.Claims;

namespace MonitorPet.Application.Model.User;

public class ResultUserLoginModel
{
    public QueryUserModel UserModel { get; set; } = new();
    public IEnumerable<Claim> Claims { get; set; } = Enumerable.Empty<Claim>();
}