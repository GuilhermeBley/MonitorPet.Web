namespace MonitorPet.Ui.Shared.Model.Agendamento;

public class CreateAgendamentoViewModel
{
    public Guid IdDosador { get; set; }
    public int DiaSemana { get; set; }
    public TimeOnly HoraAgendada { get; set; }
    public double QtdeLiberadaGr { get; set; }
}
