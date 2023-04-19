using Microsoft.Extensions.DependencyInjection;
using MonitorPet.Application.Model.Agendamento;
using MonitorPet.Application.Services.Interfaces;

namespace MonitorPet.Application.Tests.Tests;

public class AgendamentoTest : TestBase
{
    [Fact]
    public async Task Create_TryCreateAgendamento_Success()
    {
        var agendamentoService = ServiceProvider.GetRequiredService<IAgendamentoService>();

        var tupleUserDosador = await CreateUserWithDosador();

        using var context = CreateContext(tupleUserDosador.Claims);

        var validDosador = Mocks.AgendamentoMock.CreateValidAgendamento(tupleUserDosador.Dosador.IdDosador);

        var agendamentoCreated = await agendamentoService.Create(validDosador);

        Assert.NotNull(agendamentoCreated);
    }

    [Fact]
    public async Task Create_TryCreateWithSameDate_FailedToCreate()
    {
        var agendamentoService = ServiceProvider.GetRequiredService<IAgendamentoService>();

        var tupleUserDosador = await CreateUserWithDosador();

        using var context = CreateContext(tupleUserDosador.Claims);

        var validDosador = Mocks.AgendamentoMock.CreateValidAgendamento(tupleUserDosador.Dosador.IdDosador);

        var agendamentoCreated = await agendamentoService.Create(validDosador);
        
        await Assert.ThrowsAnyAsync<Core.Exceptions.ConflictCoreException>(
            () => agendamentoService.Create(validDosador));
    }

    [Fact]
    public async Task Create_TryCreateWithoutAccess_FailedToCreate()
    {
        var agendamentoService = ServiceProvider.GetRequiredService<IAgendamentoService>();

        var tupleUserDosador = await CreateUserWithDosador();

        using var context = CreateContext(tupleUserDosador.Claims);

        var validDosador = Mocks.AgendamentoMock.CreateValidAgendamento(tupleUserDosador.Dosador.IdDosador);

        await RemoveAccessFromDosador(context.ClaimModel.RequiredIdUser, tupleUserDosador.Dosador.IdDosador);

        await Assert.ThrowsAnyAsync<Core.Exceptions.ForbiddenCoreException>(
            () => agendamentoService.Create(validDosador));
    }

    [Fact]
    public async Task GetByDosador_TryGetAllDosadoresAdded_Success()
    {
        var agendamentoService = ServiceProvider.GetRequiredService<IAgendamentoService>();

        var tupleUserDosador = await CreateUserWithDosador();

        var idDosador = tupleUserDosador.Dosador.IdDosador.ToString();

        using var context = CreateContext(tupleUserDosador.Claims);

        var validDosador1 = Mocks.AgendamentoMock.CreateValidAgendamento(tupleUserDosador.Dosador.IdDosador, weekDay: 2);
        var validDosador2 = Mocks.AgendamentoMock.CreateValidAgendamento(tupleUserDosador.Dosador.IdDosador, weekDay: 1);

        await agendamentoService.Create(validDosador1);
        await agendamentoService.Create(validDosador2);

        var countAgendamentoByDosador = (await agendamentoService.GetByDosador(idDosador)).Count();

        Assert.Equal(2, countAgendamentoByDosador);
    }

    [Fact]
    public async Task GetByDosador_TryGetAllDosadoresWithoutAccess_FailedToGet()
    {
        var agendamentoService = ServiceProvider.GetRequiredService<IAgendamentoService>();

        var tupleUserDosador = await CreateUserWithDosador();

        var idDosador = tupleUserDosador.Dosador.IdDosador.ToString();

        using var context = CreateContext(tupleUserDosador.Claims);

        var validDosador1 = Mocks.AgendamentoMock.CreateValidAgendamento(tupleUserDosador.Dosador.IdDosador, weekDay: 2);
        var validDosador2 = Mocks.AgendamentoMock.CreateValidAgendamento(tupleUserDosador.Dosador.IdDosador, weekDay: 1);

        await agendamentoService.Create(validDosador1);
        await agendamentoService.Create(validDosador2);

        await RemoveAccessFromDosador(context.ClaimModel.RequiredIdUser, tupleUserDosador.Dosador.IdDosador);

        await Assert.ThrowsAnyAsync<Core.Exceptions.ForbiddenCoreException>(
            () => agendamentoService.GetByDosador(idDosador));
    }

    [Fact]
    public async Task UpdateById_TryUpdateAgendamento_Success()
    {
        var agendamentoService = ServiceProvider.GetRequiredService<IAgendamentoService>();

        var tupleUserDosador = await CreateUserWithDosador();

        using var context = CreateContext(tupleUserDosador.Claims);

        var oldWeekDay = 3;

        var validDosador = Mocks.AgendamentoMock.CreateValidAgendamento(tupleUserDosador.Dosador.IdDosador, weekDay: oldWeekDay);

        var agendamentoCreated = await agendamentoService.Create(validDosador);

        var newWeekDay = 4;

        var updateDosador = new UpdateAgendamentoModel
        {
            Ativado = agendamentoCreated.Ativado,
            DiaSemana = newWeekDay,
            HoraAgendada = agendamentoCreated.HoraAgendada,
            IdDosador = agendamentoCreated.IdDosador,
            QtdeLiberadaGr = agendamentoCreated.QtdeLiberadaGr
        };

        await agendamentoService.UpdateById(agendamentoCreated.Id, updateDosador);

        var agendamentoFound = 
            (await agendamentoService.GetByDosador(tupleUserDosador.Dosador.IdDosador.ToString()))
            .FirstOrDefault(a => a.Id == agendamentoCreated.Id)
            ?? throw new ArgumentNullException();

        Assert.NotEqual(newWeekDay, oldWeekDay);
        Assert.Equal(newWeekDay, agendamentoFound.DiaSemana);
    }

    [Fact]
    public async Task UpdateById_TryUpdateWithSameDate_Success()
    {
        var agendamentoService = ServiceProvider.GetRequiredService<IAgendamentoService>();

        var tupleUserDosador = await CreateUserWithDosador();

        using var context = CreateContext(tupleUserDosador.Claims);

        var validDosador = Mocks.AgendamentoMock.CreateValidAgendamento(tupleUserDosador.Dosador.IdDosador);

        var agendamentoCreated = await agendamentoService.Create(validDosador);

        var updateDosador = new UpdateAgendamentoModel
        {
            Ativado = agendamentoCreated.Ativado,
            DiaSemana = agendamentoCreated.DiaSemana,
            HoraAgendada = agendamentoCreated.HoraAgendada,
            IdDosador = agendamentoCreated.IdDosador,
            QtdeLiberadaGr = agendamentoCreated.QtdeLiberadaGr
        };

        Assert.NotNull(
            await agendamentoService.UpdateById(agendamentoCreated.Id, updateDosador));
    }

    [Fact]
    public async Task UpdateById_TryUpdateWithExistingSameDate_Success()
    {
        var agendamentoService = ServiceProvider.GetRequiredService<IAgendamentoService>();

        var tupleUserDosador = await CreateUserWithDosador();

        using var context = CreateContext(tupleUserDosador.Claims);

        var existingWeekDay = 2;

        var validDosador1 = Mocks.AgendamentoMock.CreateValidAgendamento(tupleUserDosador.Dosador.IdDosador, weekDay: 1);
        var validDosador2 = Mocks.AgendamentoMock.CreateValidAgendamento(tupleUserDosador.Dosador.IdDosador, weekDay: existingWeekDay);

        var agendamento1Created = await agendamentoService.Create(validDosador1);
        await agendamentoService.Create(validDosador2);

        var updateDosador = new UpdateAgendamentoModel
        {
            Ativado = agendamento1Created.Ativado,
            DiaSemana = existingWeekDay,
            HoraAgendada = agendamento1Created.HoraAgendada,
            IdDosador = agendamento1Created.IdDosador,
            QtdeLiberadaGr = agendamento1Created.QtdeLiberadaGr
        };

        await Assert.ThrowsAnyAsync<Core.Exceptions.ConflictCoreException>(
            () => agendamentoService.UpdateById(agendamento1Created.Id, updateDosador));
    }

    [Fact]
    public async Task UpdateById_TryUpdateWithoutAccessToDosador_FailedToUpdate()
    {
        var agendamentoService = ServiceProvider.GetRequiredService<IAgendamentoService>();

        var tupleUserDosador = await CreateUserWithDosador();

        using var context = CreateContext(tupleUserDosador.Claims);

        var validDosador = Mocks.AgendamentoMock.CreateValidAgendamento(tupleUserDosador.Dosador.IdDosador);

        var agendamentoCreated = await agendamentoService.Create(validDosador);

        var updateDosador = new UpdateAgendamentoModel
        {
            Ativado = agendamentoCreated.Ativado,
            DiaSemana = agendamentoCreated.DiaSemana,
            HoraAgendada = agendamentoCreated.HoraAgendada,
            IdDosador = agendamentoCreated.IdDosador,
            QtdeLiberadaGr = agendamentoCreated.QtdeLiberadaGr
        };

        await RemoveAccessFromDosador(context.ClaimModel.RequiredIdUser, tupleUserDosador.Dosador.IdDosador);

        await Assert.ThrowsAnyAsync<Core.Exceptions.ForbiddenCoreException>(
            () => agendamentoService.UpdateById(agendamentoCreated.Id, updateDosador));
    }

    [Fact]
    public async Task DeleteById_TryDeleteAgendamento_Success()
    {
        var agendamentoService = ServiceProvider.GetRequiredService<IAgendamentoService>();

        var tupleUserDosador = await CreateUserWithDosador();

        using var context = CreateContext(tupleUserDosador.Claims);

        var validDosador = Mocks.AgendamentoMock.CreateValidAgendamento(tupleUserDosador.Dosador.IdDosador);

        var agendamentoCreated = await agendamentoService.Create(validDosador);

        await agendamentoService.DeleteById(agendamentoCreated.Id);

        var agendamentoFound =
            (await agendamentoService.GetByDosador(tupleUserDosador.Dosador.IdDosador.ToString()))
            .FirstOrDefault(a => a.Id == agendamentoCreated.Id);

        Assert.Null(agendamentoFound);
    }

    [Fact]
    public async Task DeleteById_TryDeleteWithoutAccess_FailedToDelete()
    {
        var agendamentoService = ServiceProvider.GetRequiredService<IAgendamentoService>();

        var tupleUserDosador = await CreateUserWithDosador();

        using var context = CreateContext(tupleUserDosador.Claims);

        var validDosador = Mocks.AgendamentoMock.CreateValidAgendamento(tupleUserDosador.Dosador.IdDosador);

        var agendamentoCreated = await agendamentoService.Create(validDosador);

        await RemoveAccessFromDosador(context.ClaimModel.RequiredIdUser, tupleUserDosador.Dosador.IdDosador);

        await Assert.ThrowsAnyAsync<Core.Exceptions.ForbiddenCoreException>(
            () => agendamentoService.DeleteById(agendamentoCreated.Id));
    }

    private async Task RemoveAccessFromDosador(int idUser, Guid idDosador)
    {
        var dosadorUserService = ServiceProvider.GetRequiredService<IDosadorService>();

        await dosadorUserService.RemoveDosadorFromUser(idUser, idDosador);
    }
}
