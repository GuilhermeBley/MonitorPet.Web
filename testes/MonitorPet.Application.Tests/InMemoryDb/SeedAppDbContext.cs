using MonitorPet.Application.Tests.Mocks;
using MonitorPet.Application.Tests.ModelDb;

namespace MonitorPet.Application.Tests.InMemoryDb;

internal class SeedAppDbContext
{
    public static async Task AddDosadores(AppDbContext context)
    {
        await context.Dosadores.AddRangeAsync(
            new DosadorDbModel[] {
                new DosadorDbModel
                {
                    IdDosador = Mocks.DosadorMock.DosadorExample1.IdDosador,
                    Nome = Mocks.DosadorMock.DosadorExample1.Nome,
                    ImgUrl = Mocks.DosadorMock.DosadorExample1.ImgUrl
                },
                new DosadorDbModel
                {
                    IdDosador = Mocks.DosadorMock.DosadorExample2.IdDosador,
                    Nome = Mocks.DosadorMock.DosadorExample2.Nome,
                    ImgUrl = Mocks.DosadorMock.DosadorExample2.ImgUrl
                }
            }
        );

        await context.SaveChangesAsync();
    }
}