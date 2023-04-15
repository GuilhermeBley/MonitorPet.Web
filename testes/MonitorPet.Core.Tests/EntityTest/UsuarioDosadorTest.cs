using MonitorPet.Core.Entity;
using MonitorPet.Core.Exceptions;

namespace MonitorPet.Core.Tests.EntityTest;

public class UsuarioDosadorTest
{
    [Fact]
    public void Create_TryCreate_Success()
    {
        Assert.NotNull(
            Create()
        );
    }

    [Fact]
    public void Create_TryCreateInvalidDosador_FailedToCreate()
    {
        Assert.ThrowsAny<CommonCoreException>(
            () => Create(idDosador: Guid.Empty)
        );
    }
    
    [Fact]
    public void Create_TryCreateInvalidUser_FailedToCreate()
    {
        Assert.ThrowsAny<CommonCoreException>(
            () => Create(idUser: -1)
        );
    }

    private static UsuarioDosador Create(Guid? idDosador = null, int idUser = 1)
    {
        if (idDosador is null)
            idDosador = Guid.NewGuid();

        return UsuarioDosador.Create(idDosador.Value, idUser);
    }
}