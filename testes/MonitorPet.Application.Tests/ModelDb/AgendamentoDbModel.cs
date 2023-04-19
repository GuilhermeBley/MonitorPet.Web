using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitorPet.Application.Tests.ModelDb;

[Table("Agendamento")]
public class AgendamentoDbModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public Guid IdDosador { get; set; }
    public DosadorDbModel Dosador { get; set; } = null!;
    public int DiaSemana { get; set; }
    public TimeOnly HoraAgendada { get; set; }
    public double QtdeLiberadaGr { get; set; }
    public bool Ativado { get; set; }
}
