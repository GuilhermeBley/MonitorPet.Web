using MonitorPet.Application.StorageRepositories;

namespace MonitorPet.Application.Tests.StorageRepositories;

internal class ImgRepository : IImgRepository
{
    public async Task<Uri> AddImageAsync(string fileName, Stream file)
        => await Task.FromResult(new Uri($"https://blob/container/{fileName}"));
    

    public async Task RemoveImageAsync(string fileName)
    {
        await Task.CompletedTask;
    }
}
