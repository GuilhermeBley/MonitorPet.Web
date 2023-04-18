using MonitorPet.Core.Exceptions;

namespace MonitorPet.Core.Entity;

public class Agendamento : Entity
{
    public int Id { get; private set; } 
    public Guid IdDosador { get; private set; }
    public int DiaSemana { get; private set; }
    public TimeOnly HoraAgendada { get; private set; }
    public double QtdeLiberadaGr { get; private set; } 
    public bool Ativado { get; private set; }

    private Agendamento(int id, Guid idDosador, int diaSemana, TimeOnly horaAgendada, double qtdeLiberadaGr, bool ativado)
    {
        Id = id;
        IdDosador = idDosador;
        DiaSemana = diaSemana;
        HoraAgendada = horaAgendada;
        QtdeLiberadaGr = qtdeLiberadaGr;
        Ativado = ativado;
    }

    public override bool Equals(object? obj)
    {
        if (!base.Equals(obj))
            return false;

        var agendamento = obj as Agendamento;
        if (agendamento is null)
            return false;

        if (!this.Id.Equals(agendamento.Id) ||
            !this.IdDosador.Equals(agendamento.IdDosador) ||
            !this.DiaSemana.Equals(agendamento.DiaSemana) ||
            !this.HoraAgendada.Equals(agendamento.HoraAgendada) ||
            !this.QtdeLiberadaGr.Equals(agendamento.QtdeLiberadaGr) ||
            !this.Ativado.Equals(agendamento.Ativado))
            return false;

        return true;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode() * 45976549;
    }

    public void TurnOff()
        => Ativado = false;

    public void TurnOn()
        => Ativado = true;

    public static Agendamento CreateActivated(Guid idDosador, DayOfWeek diaSemana, TimeOnly horaAgendada, double qtdeLiberadaGr)
    {
        if (idDosador == default)
            throw new CommonCoreException("Dosador inválido.");

        if (qtdeLiberadaGr < 1)
            throw new CommonCoreException("Quantidade de ração inválida.");

        return new Agendamento(0, idDosador, (int)diaSemana, horaAgendada, qtdeLiberadaGr, true);
    }
}
