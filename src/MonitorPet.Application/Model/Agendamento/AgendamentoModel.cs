namespace MonitorPet.Application.Model.Agendamento;

public class AgendamentoModel
{
    public int Id { get; set; }
    public Guid IdDosador { get; set; }
    public int DiaSemana { get; set; }
    public TimeSpan HoraAgendada { get; set; }
    public double QtdeLiberadaGr { get; set; }
    public bool Ativado { get; set; }
}
