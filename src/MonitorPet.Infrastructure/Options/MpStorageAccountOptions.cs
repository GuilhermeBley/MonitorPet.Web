namespace MonitorPet.Infrastructure.Options;

public class MpStorageAccountOptions
{
    public const string SECTION = "MpStorageAccount";
    public string ConnectionString { get; set; } = string.Empty;
}
