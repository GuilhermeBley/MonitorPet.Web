namespace MonitorPet.Infrastructure.Options;

public class EmailOptions
{
    public const string SECTION = "Email";
    public string AddressFrom { get; set; } = string.Empty;
    public string AddressNameFrom { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}