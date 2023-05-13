using MonitorPet.Application.Model.Dosador;
using MonitorPet.Application.Tests.ModelDb;

namespace MonitorPet.Application.Tests.Mocks;

internal static class DosadorMock
{
    public static Guid DefaultIdDosador1 { get; } = Guid.NewGuid();
    public static Guid DefaultIdDosador2 { get; } = Guid.NewGuid();
    public static DosadorModel DosadorExample1 =>
        new DosadorModel
        {
            IdDosador = DefaultIdDosador1,
            Nome = "Default name",
            ImgUrl = string.Empty
        };
    public static DosadorModel DosadorExample2 =>
        new DosadorModel
        {
            IdDosador = DefaultIdDosador2,
            Nome = "Default name",
            ImgUrl = string.Empty
        };

    public static DosadorModel CreateNewDosador()
        => new DosadorModel
        {
            IdDosador = Guid.NewGuid(),
            Nome = "Default name",
            ImgUrl = string.Empty
        };
}