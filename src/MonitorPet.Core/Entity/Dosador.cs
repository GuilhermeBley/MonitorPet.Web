using MonitorPet.Core.Exceptions;

namespace MonitorPet.Core.Entity;

public class Dosador : Entity
{
    public Guid IdDosador { get; private set; }
    public override Guid IdEntity => IdDosador;
    public string Nome { get; private set; }
    public double PesoMax { get; private set; }

    private Dosador(Guid idDosador, string nome, double pesoMax)
    {
        IdDosador = idDosador;
        Nome = nome;
        PesoMax = pesoMax;
    }
    
    public override bool Equals(object? obj)
    {
        if (!base.Equals(obj))
            return false;
        
        var dosador = obj as Dosador;
        if (dosador is null)
            return false;

        if (!this.Nome.Equals(dosador.Nome) ||
            !this.PesoMax.Equals(dosador.PesoMax))
            return false;

        return true;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode() * 1414164564;
    }

    public static Dosador Create(string nome, double pesoMax)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new CommonCoreException("Nome não deve estar vazio.");

        if (pesoMax < 0)
            throw new CommonCoreException("Peso máximo não pode ser menor que zero.");

        return new Dosador(Guid.NewGuid(), nome, pesoMax);
    }

    public static Dosador CreatePesoZero(string nome)
        => Create(nome, 0);
}