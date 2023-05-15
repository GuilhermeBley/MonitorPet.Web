using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using MonitorPet.Application.StorageRepositories;
using MonitorPet.Infrastructure.Options;

namespace MonitorPet.Infrastructure.StorageRepositories;

internal class ImgRepository : IImgRepository
{
    const string CONTAINER_IMG = "files";
    private readonly BlobContainerClient _container;

    public ImgRepository(IOptions<MpStorageAccountOptions> options)
    {
        var client = new BlobServiceClient(options.Value.ConnectionString);
        _container = client.GetBlobContainerClient(CONTAINER_IMG);
    }

    public async Task<Uri> AddImageAsync(string fileName, Stream file)
    {
        await _container.UploadBlobAsync(fileName, file);

        return GetUri(fileName);
    }

    public async Task RemoveImageAsync(string fileName)
    {
        await _container.DeleteBlobAsync(fileName);
    }

    public Uri GetUri(string fileName)
    {
        var url = _container.Uri.ToString();
        return new Uri(url+"/"+fileName);
    }
}
