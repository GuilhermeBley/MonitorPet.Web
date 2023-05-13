namespace MonitorPet.Application.StorageRepositories;

public interface IImgRepository
{
    Task<Uri> AddImageAsync(string fileName, Stream file);
    Task RemoveImageAsync(string fileName);
}
