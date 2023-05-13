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
    public void CreateWithoutImg_TryCreate_Success()
    {
        Assert.NotNull(
            CreateWithoutImg()
        );
    }

    [Fact]
    public void Create_TryCreateInvalidName_FailedToCreate()
    {
        Assert.ThrowsAny<Core.Exceptions.CommonCoreException>(
            () => Create(nome: string.Empty)
        );
    }

    private static Dosador Create(string nome = "Valid name", string? imgUrl = "img.jpg")
        => Dosador.Create(nome, imgUrl);

    private static Dosador CreateWithoutImg(string nome = "Valid name")
        => Dosador.CreateWithoutImg(nome);
}