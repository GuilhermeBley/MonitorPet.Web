using MonitorPet.Core.Entity;

namespace MonitorPet.Core.Tests.EntityTest;

public class AgendamentoTest
{
    [Fact]
    public void Create_TryCreate_Success()
    {
        Assert.NotNull(
            Create()
        );
    }

    [Fact]
    public void Create_TryCreateInvalidIdDosador_FailedToCreate()
    {
        Assert.ThrowsAny<Core.Exceptions.CommonCoreException>(
            () => Create(idDosador: Guid.Empty)
        );
    }

    [Fact]
    public void Create_TryCreateInvalidQuantity_FailedToCreate()
    {
        Assert.ThrowsAny<Core.Exceptions.CommonCoreException>(
            () => Create(quantity: 0.9)
        );
    }

    private static Agendamento Create(
        Guid? idDosador = null,
        DayOfWeek dayOfWeek = default,
        TimeOnly hour = default,
        double quantity = 1)
    {
        if (idDosador is null)
            idDosador = Guid.NewGuid();
        return Agendamento.CreateActivated(idDosador.Value, dayOfWeek, hour, quantity);
    }
}
