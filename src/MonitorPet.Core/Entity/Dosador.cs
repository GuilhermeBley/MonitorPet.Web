using MonitorPet.Core.Exceptions;

namespace MonitorPet.Core.Entity;

public class Dosador : Entity
{
    public Guid IdDosador { get; private set; }
    public override Guid IdEntity => IdDosador;
    public string Nome { get; private set; }
    public string? ImgUrl { get; private set; }

    private Dosador(Guid idDosador, string nome, string? imgUrl)
    {
        IdDosador = idDosador;
        Nome = nome;
        ImgUrl = imgUrl;
    }
    
    public override bool Equals(object? obj)
    {
        if (!base.Equals(obj))
            return false;
        
        var dosador = obj as Dosador;
        if (dosador is null)
            return false;

        if (!this.Nome.Equals(dosador.Nome) ||
            ImgUrl != dosador.ImgUrl)
            return false;

        return true;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode() * 1414164564;
    }

    public static Dosador Create(string nome, string? imgUrl)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new CommonCoreException("Nome não deve estar vazio.");

        return new Dosador(Guid.NewGuid(), nome, imgUrl);
    }

    public static Dosador CreateWithoutImg(string nome)
        => Create(nome, null);
}