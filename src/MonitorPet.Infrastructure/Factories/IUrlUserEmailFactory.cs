using System.Security.Claims;
using MonitorPet.Application.Model.User;

namespace MonitorPet.Infrastructure.Factories;

public interface IUrlUserEmailFactory
{
    Task<Uri> Create(UserModel userToCreateUrl, IEnumerable<Claim> claims);
}