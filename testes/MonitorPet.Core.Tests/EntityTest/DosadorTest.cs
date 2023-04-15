using MonitorPet.Core.Entity;
using MonitorPet.Core.Exceptions;

namespace MonitorPet.Core.Tests.EntityTest;

public class DosadorTest
{
    [Fact]
    public void Create_TryCreate_Success()
    {
        Assert.NotNull(
            Create()
        );
    }

    [Fact]
    public void CreatePesoZero_TryCreate_Success()
    {
        Assert.NotNull(
            CreatePesoZero()
        );
    }

    [Fact]
    public void Create_TryCreateInvalidName_FailedToCreate()
    {
        Assert.ThrowsAny<Core.Exceptions.CommonCoreException>(
            () => Create(nome: string.Empty)
        );
    }
    
    [Fact]
    public void Create_TryCreateInvalidPeso_FailedToCreate()
    {
        Assert.ThrowsAny<Core.Exceptions.CommonCoreException>(
            () => Create(pesoMax: -1)
        );
    }

    private static Dosador Create(string nome = "Valid name", double pesoMax = 1.00)
        => Dosador.Create(nome, pesoMax);

    private static Dosador CreatePesoZero(string nome = "Valid name")
        => Dosador.CreatePesoZero(nome);
}