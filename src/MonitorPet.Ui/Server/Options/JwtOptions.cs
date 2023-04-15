using System.ComponentModel.DataAnnotations;

namespace MonitorPet.Ui.Server.Options;

public class JwtOptions
{
    public const string SECTION = "Jwt";

    [Required]
    [MinLength(32)]
    public string Key { get; set; } = string.Empty;

    [Required]
    [MinLength(32)]
    public string KeyEmail { get; set; } = string.Empty;
}