namespace MonitorPet.Ui.Shared.Model.Agendamento;

public class AgendamentoViewModel
{
    public int Id { get; set; }
    public Guid IdDosador { get; set; }
    public int DiaSemana { get; set; }
    public TimeOnly HoraAgendada { get; set; }
    public double QtdeLiberadaGr { get; set; }
    public bool Ativado { get; set; }
}
