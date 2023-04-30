namespace MonitorPet.Ui.Shared.Model.Agendamento;

public class PatchAgendamentoViewModel
{
    public int? DiaSemana { get; set; }
    public TimeSpan? HoraAgendada { get; set; }
    public double? QtdeLiberadaGr { get; set; }
    public bool? Ativado { get; set; }
}
