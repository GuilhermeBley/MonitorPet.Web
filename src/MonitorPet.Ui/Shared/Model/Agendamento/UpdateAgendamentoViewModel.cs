using System.ComponentModel.DataAnnotations;

namespace MonitorPet.Ui.Shared.Model.Agendamento;

public class UpdateAgendamentoViewModel
{
    [Required(ErrorMessage = "Dia da semana é obrigatório.")]
    [Range(1, 6, ErrorMessage = "Dia de semana inválido.")]
    public int DiaSemana { get; set; }

    [Required(ErrorMessage = "Hora agendada é obrigatória.")]
    public TimeOnly? HoraAgendada { get; set; }

    [Required(ErrorMessage = "Quantidade de ração é obrigatória.")]
    [Range(0.1, 10000, ErrorMessage = "Quantidade de ração inválida.")]
    public double? QtdeLiberadaGr { get; set; }

    public bool Ativado { get; set; }
}
