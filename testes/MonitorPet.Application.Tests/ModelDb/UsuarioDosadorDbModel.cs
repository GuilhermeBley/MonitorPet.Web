using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitorPet.Application.Tests.ModelDb;

[Table("UsuarioDosador")]
public class UsuarioDosadorDbModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int IdUsuario { get; set; }
    public UserDbModel User { get; set; } = null!;
    public Guid IdDosador { get; set; }
    public DosadorDbModel Dosador { get; set; } = null!;
}