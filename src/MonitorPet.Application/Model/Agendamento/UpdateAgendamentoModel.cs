namespace MonitorPet.Application.Model.Agendamento;

public class UpdateAgendamentoModel
{
    public Guid IdDosador { get; set; }
    public int DiaSemana { get; set; }
    public TimeOnly HoraAgendada { get; set; }
    public double QtdeLiberadaGr { get; set; }
    public bool Ativado { get; set; }
}
