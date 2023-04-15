using Microsoft.Extensions.DependencyInjection;
using MonitorPet.Application.Services.Interfaces;

namespace MonitorPet.Application.Tests.Tests;

public class DosadorTest : TestBase
{

    [Fact]
    public async Task GetDosadoresByIdUser_ListDosadores_Success()
    {
        var dosadorService = ServiceProvider.GetRequiredService<IDosadorService>();
        var tupleUserCreated = await CreateAndLoginUser();

        using var contextClaim = CreateContext(tupleUserCreated.Claims);

        var newDosador = await CreateDosador();

        await dosadorService
                .AddDosadorToUser(contextClaim.ClaimModel.RequiredIdUser, newDosador.IdDosador.ToString());

        Assert.NotEmpty(
            await dosadorService.GetDosadoresByIdUser(contextClaim.ClaimModel.RequiredIdUser)
        );
    }

    [Fact]
    public async Task AddDosadorToUser_TryAdd_Success()
    {
        var dosadorService = ServiceProvider.GetRequiredService<IDosadorService>();
        var tupleUserCreated = await CreateAndLoginUser();

        using var contextClaim = CreateContext(tupleUserCreated.Claims);

        var newDosador = await CreateDosador();

        Assert.NotNull(
            await dosadorService
                .AddDosadorToUser(contextClaim.ClaimModel.RequiredIdUser, newDosador.IdDosador.ToString()));
    }

    [Fact]
    public async Task RemoveDosadorFromUser_TryRemove_Success()
    {
        var dosadorService = ServiceProvider.GetRequiredService<IDosadorService>();
        var tupleUserCreated = await CreateAndLoginUser();

        using var contextClaim = CreateContext(tupleUserCreated.Claims);

        var newDosador = await CreateDosador();

        await dosadorService
                .AddDosadorToUser(contextClaim.ClaimModel.RequiredIdUser, newDosador.IdDosador.ToString());

        Assert.NotNull(
            await dosadorService
                .RemoveDosadorFromUser(contextClaim.ClaimModel.RequiredIdUser, newDosador.IdDosador));
    }
}
