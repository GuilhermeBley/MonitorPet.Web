namespace MonitorPet.Application.Model.Agendamento;

public class CreateAgendamentoModel
{
    public Guid IdDosador { get; set; }
    public int DiaSemana { get; set; }
    public TimeSpan HoraAgendada { get; set; }
    public double QtdeLiberadaGr { get; set; }
}
