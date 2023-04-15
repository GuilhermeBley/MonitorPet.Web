namespace MonitorPet.Infrastructure.Options;

public class DbOptions
{
    public const string SECTION = "DbOptions";
    public string ConnectionString { get; set; } = string.Empty;
}