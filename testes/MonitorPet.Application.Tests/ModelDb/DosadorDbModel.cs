using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitorPet.Application.Tests.ModelDb;

[Table("Dosador")]
public class DosadorDbModel
{
    [Key]
    public Guid IdDosador { get; set; }
    public string Nome { get; set; } = string.Empty;
    public double PesoMax { get; set; }
}